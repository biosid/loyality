namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class GetDeliveryVariantsParameters
    {
        [DataMember]
        public Dictionary<string, string> ClientContext { get; set; }

        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
        public Guid[] BasketItems { get; set; }

        [DataMember]
        public Location Location { get; set; }
    }
}
