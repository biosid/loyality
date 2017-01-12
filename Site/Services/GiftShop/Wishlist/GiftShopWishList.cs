using System;
using System.Linq;
using Vtb24.Site.Security.Models;
using Vtb24.Site.Services.GiftShop.Catalog.Models.Exceptions;
using Vtb24.Site.Services.GiftShop.Model;
using Vtb24.Site.Services.GiftShop.Wishlist.Models.Exceptions;
using Vtb24.Site.Services.Models;
using Vtb24.Site.Services.ProductCatalogWishListService;
using WishListResult = Vtb24.Site.Services.GiftShop.Wishlist.Models.Outputs.WishListResult;

namespace Vtb24.Site.Services.GiftShop.Wishlist
{
    internal class GiftShopWishList
    {
        public GiftShopWishList(ClientPrincipal principal, IClientService client)
        {
            _principal = principal;
            _client = client;
        }

        private readonly ClientPrincipal _principal;
        private readonly IClientService _client;

        public WishListResult GetWishList(PagingSettings paging)
        {
            using (var service = new WishListServiceClient())
            {
                var attributes = _client.GetMechanicsContext();

                var options = new GetWishListParameters
                {
                    SortDirect = SortDirections.Desc,
                    SortType = WishListSortTypes.ByCreateDate,

                    ClientId = _principal.ClientId,
                    ClientContext = attributes,

                    CalcTotalCount = true,
                    CountToTake = paging.Take,
                    CountToSkip = paging.Skip
                };

                var response = service.GetWishList(options);

                AssertResponse(response.ResultCode, response.ResultDescription);

                var result = new WishListResult(
                    response.Items.Select(MappingsFromService.ToReservedProductItem).ToArray(),
                    response.TotalCount ?? 0,
                    paging
                );

                return result;
            }
        }

        public ReservedProductItem GetWishedProduct(string productId)
        {

            using (var service = new WishListServiceClient())
            {
                var attributes = _client.GetMechanicsContext();

                var response = service.GetWishListItem(_principal.ClientId, productId, attributes);

                AssertResponse(response.ResultCode, response.ResultDescription);

                var item = response.Item == null
                               ? null
                               : MappingsFromService.ToReservedProductItem(response.Item);

                return item;
            }

        }

        public void AddToWishList(string productId, int quantity = 1)
        {
            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException("quantity");
            }

            using (var service = new WishListServiceClient())
            {
                var attributes = _client.GetMechanicsContext();

                var response = service.Add(_principal.ClientId, productId, quantity, attributes);

                AssertResponse(response.ResultCode, response.ResultDescription);
            }
        }

        public void RemoveFromWishList(string productId)
        {
            using (var service = new WishListServiceClient())
            {
                var response = service.Remove(_principal.ClientId, productId);

                AssertResponse(response.ResultCode, response.ResultDescription);
            }
        }

        public void UpdateWishListItemQuantity(string productId, int quantity)
        {
            using (var service = new WishListServiceClient())
            {
                var attributes = _client.GetMechanicsContext();

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
                    throw new WishListItemQuantityOverflowException(code, message);
            }
            throw new WishListServiceException(code, message);
        }
    }
}
