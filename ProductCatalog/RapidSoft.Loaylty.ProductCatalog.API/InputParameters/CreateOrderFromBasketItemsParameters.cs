namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System;
    using System.Collections.Generic;

    public class CreateOrderFromBasketItemsParameters
    {
        public string ClientId { get; set; }

        public Dictionary<string, string> ClientContext { get; set; }

        public Guid[] BasketItems { get; set; }

        public DeliveryDto Delivery { get; set; }

        public decimal TotalAdvance { get; set; }
    }
}