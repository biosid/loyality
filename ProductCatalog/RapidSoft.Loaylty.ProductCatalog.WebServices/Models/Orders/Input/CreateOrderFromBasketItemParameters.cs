namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders.Input
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class CreateOrderFromBasketItemParameters
    {
        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
        public Dictionary<string, string> ClientContext { get; set; }

        [DataMember]
        public Guid BasketItemId { get; set; }

        [DataMember]
        public DeliveryInfo Delivery { get; set; }
    }
}
