namespace RapidSoft.Loaylty.ProductCatalog.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using API;
    using API.Entities;
    using API.OutputResults;

    using Extensions;

    using Interfaces;

    using RapidSoft.Loaylty.ProductCatalog.Entities;

    using Settings;

    internal class DeliveryOrderBuilder : IOrderBuilder
    {
        private readonly IDeliverySpecification deliverySpecification;
        private IBasketService basketService;

        public DeliveryOrderBuilder(
            IDeliverySpecification deliverySpecification = null, 
            IBasketService basketService = null)
        {
            this.deliverySpecification = deliverySpecification ?? new OrderDeliverySpecification();
            this.basketService = basketService ?? new BasketService();
        }

        public Order CreateOrder(string clientId, Guid[] basketItemIds, DeliveryInfo deliveryInfo, Dictionary<string, string> clientContext)
        {
            var address = deliveryInfo.Address;

            address.ThrowIfNull("deliveryInfo.Address required for DeliveryType Delivery");
            Utils.CheckArgument(string.IsNullOrEmpty, address.CountryCode, "CountryCode");
            Utils.CheckArgument(string.IsNullOrEmpty, address.StreetTitle, "StreetTitle");
            Utils.CheckArgument(string.IsNullOrEmpty, address.House, "House");
            Utils.CheckArgument(o => !string.IsNullOrEmpty(o) && o.Length != 13, address.AddressKladrCode, "deliveryInfo.AddressKladrCode");

            deliverySpecification.FillDeliveryAddress(address);

            var basketItems = this.GetBasketItems(basketItemIds, clientContext);

            foreach (var basketItem in basketItems)
            {
                if (basketItem.AvailabilityStatus != ProductAvailabilityStatuses.Available)
                {
                    if (basketItem.AvailabilityStatus == ProductAvailabilityStatuses.DeliveryRateNotFound)
                    {
                        var error = string.Format(
                            "Товар id:{0} не доставляется КЛАДР:{1}", basketItem.ProductId, address.AddressKladrCode);
                        throw new OperationException(ResultCodes.PRODUCT_NOT_DELIVERED, error);                        
                    }
                }
                else
                {
                    var error = string.Format(
                        "Товар id:{0} имеет не допустимый статус {1}", basketItem.ProductId, basketItem.AvailabilityStatus);
                    throw new OperationException(ResultCodes.PRODUCT_INVALID_STATUS, error);                    
                }
            }

            var items = new ProductsSearcher().GetProductsByIds(basketItems.Select(b => b.ProductId).ToArray(), clientContext);

            var productDeliveryPrices = GetProductDeliveryPrices(basketItems.Select(i => i.Product).ToArray(), deliveryInfo.Address.AddressKladrCode);

            var order = this.CreateDraftOrder(clientId, basketItems, deliveryInfo, productDeliveryPrices, items);

            return order;
        }
        
        private BasketItem[] GetBasketItems(Guid[] basketItemIds, Dictionary<string, string> clientContext)
        {
            if (basketItemIds.Length != 1)
            {
                throw new Exception("Only one basketItemId is supported");
            }

            var basketItemId = basketItemIds.First();

            var res = basketService.GetBasketItem(basketItemId, clientContext);

            if (res == null || res.Item == null || !res.Success)
            {
                throw new OperationException(string.Format("Элемент корзины {0} найти не удалось", basketItemId), ResultCodes.NOT_FOUND);
            }

            return new[] { res.Item };
        }

        private FixedPrice GetFixedPrice(string fixedPriceXml)
        {
            return string.IsNullOrEmpty(fixedPriceXml) ? null : XmlSerializer.Deserialize<FixedPrice>(fixedPriceXml);
        }

        private ProductDeliveryPrice[] GetProductDeliveryPrices(Product[] products, string kladr)
        {
            return products.Select(p =>
                new ProductDeliveryPrice()
                {
                    ProductId = p.ProductId,
                    PriceDelivery = 100,
                    PriceDeliveryRur = 10
                }).ToArray();
        }

        private Order CreateDraftOrder(string clientId, BasketItem[] basketItems, DeliveryInfo deliveryInfo, ProductDeliveryPrice[] deliveryPrices, IEnumerable<GetProductByIdItem> items)
        {
            if (basketItems.Length != 1)
            {
                throw new Exception("Only one orderItem supported");
            }

            var basketItem = basketItems[0];

            var orderItem = new OrderItem
            {
                Amount = basketItem.ProductsQuantity,
                Product = items.Select(i => i.Product).First(p => p.ProductId == basketItem.ProductId),
                BasketItemId = basketItem.Id.ToString(),
                FixedPrice = GetFixedPrice(basketItem.FixedPrice)
            };

            var order = new Order
            {
                ClientId = clientId,
                Status = OrderStatuses.Draft,
                UpdatedUserId = ApiSettings.ClientSiteUserName,
                DeliveryInfo = deliveryInfo,
                PartnerId = orderItem.Product.PartnerId
            };

            if (orderItem.Product.CarrierId.HasValue)
            {
                order.CarrierId = orderItem.Product.CarrierId.Value;
            }

            order.Items = new[] { orderItem };
            order.TotalWeight = order.Items.Sum(i => (i.Product.Weight ?? 0) * i.Amount);

            PriceSpecification.FillOrderPrice(order, deliveryPrices);

            return order;
        }
    }
}