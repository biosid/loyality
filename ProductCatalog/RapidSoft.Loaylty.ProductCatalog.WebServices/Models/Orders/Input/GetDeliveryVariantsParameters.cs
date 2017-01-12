namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders.Input
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class GetDeliveryVariantsParameters
    {
        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
        public Dictionary<string, string> ClientContext { get; set; }

        [DataMember]
        public Guid[] BasketItemIds { get; set; }

        [DataMember]
        public Location Location { get; set; }
    }
}
