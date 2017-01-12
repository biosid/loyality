using System;
using System.Linq;
using Vtb24.Site.Security.Models;
using Vtb24.Site.Services.GiftShop.Basket.Models.Exceptions;
using Vtb24.Site.Services.GiftShop.Catalog.Models.Exceptions;
using Vtb24.Site.Services.GiftShop.Model;
using Vtb24.Site.Services.Infrastructure;
using Vtb24.Site.Services.Models;
using Vtb24.Site.Services.ProductCatalogBasketService;
using BasketResult = Vtb24.Site.Services.GiftShop.Basket.Models.Outputs.BasketResult;

namespace Vtb24.Site.Services.GiftShop.Basket
{
    public class GiftShopBasket
    {
        public GiftShopBasket(ClientPrincipal principal, IClientService client)
        {
            _principal = principal;
            _client = client;
        }

        private readonly ClientPrincipal _principal;
        private readonly IClientService _client;

        public BasketResult GetBasket(PagingSettings paging)
        {
            using (var service = new BasketServiceClient())
            {
                var attributes = _client.GetMechanicsContext();

                var options = new GetBasketParameters
                {
                    CountToSkip = paging.Skip,
                    CountToTake = paging.Take,
                    CalcTotalCount = true,
                    ClientContext = attributes,
                    ClientId = _principal.ClientId
                };

                var response = service.GetBasket(options);

                AssertResponse(response.ResultCode, response.ResultDescription);

                var items = response.Items.MaybeSelect(MappingsFromService.ToReservedProductItem).MaybeToArray();
                return new BasketResult(items, response.TotalCount ?? 0, paging)
                {
                    TotalPrice = response.TotalPrice
                };
            }
        }       

        public ReservedProductItem GetBasketItem(string productId, string locationKladr)
        {
            using (var service = new BasketServiceClient())
            {
                var attributes = _client.GetMechanicsContext();

                if (!string.IsNullOrEmpty(locationKladr))
                {
                    attributes.ClientLocationKladr = locationKladr;
                }

                var response = service.GetBasketItem(_principal.ClientId, productId, attributes);

                if (response.ResultCode != 600) // игнорируем BasketItemNotFound (в этом случае мы просто вернём null)
                {
                    AssertResponse(response.ResultCode, response.ResultDescription);
                }

                var item = response.Item == null
                               ? null
                               : MappingsFromService.ToReservedProductItem(response.Item);

                return item;
            }
        }

        public ReservedProductItem[] GetBasketItems(string[] productIds, string locationKladr)
        {
            using (var service = new BasketServiceClient())
            {
                var attributes = _client.GetMechanicsContext();

                if (!string.IsNullOrEmpty(locationKladr))
                {
                    attributes.ClientLocationKladr = locationKladr;
                }

                var response = service.GetBasketItems(_principal.ClientId, productIds, attributes);

                if (response.ResultCode != 600) // игнорируем BasketItemNotFound (в этом случае мы просто вернём null)
                {
                    AssertResponse(response.ResultCode, response.ResultDescription);
                }

                var items = response.Items == null
                               ? null
                               : response.Items.Select(i => MappingsFromService.ToReservedProductItem(i)).ToArray();

                return items;
            }
        } 

        public void AddToBasket(string productId, int quantity = 1)
        {
            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException("quantity");
            }

            var attributes = _client.GetMechanicsContext();

            using (var service = new BasketServiceClient())
            {
                var response = service.Add(_principal.ClientId, productId, quantity, attributes);

                AssertResponse(response.ResultCode, response.ResultDescription);
            }
        }

        public void RemoveFromBasket(string productId)
        {
            using (var service = new BasketServiceClient())
            {
                var response = service.Remove(_principal.ClientId, productId);

                AssertResponse(response.ResultCode, response.ResultDescription);
            }
        }

        public void UpdateBasketItemQuantity(string productId, int quantity)
        {
            var attributes = _client.GetMechanicsContext();

            using (var service = new BasketServiceClient())
            {
                var response = service.SetQuantity(_principal.ClientId, productId, quantity, attributes);

                AssertResponse(response.ResultCode, response.ResultDescription);
            }
        }

        private static void AssertResponse(int code, string message)
        {
            switch (code)
            {
                case 0:
                    return;
                case 2:
                    throw new ProductNotFoundException(code, message);
                case 20:
                    throw new BasketItemQuantityOverflowException(code, message);
                case 600:
                    throw new BasketItemNotFoundException(code, message);
                case 610:
                    throw new BasketItemPriceNotFixedException(code, message);
            }
            throw new BasketServiceException(code, message);
        }
    }
}
