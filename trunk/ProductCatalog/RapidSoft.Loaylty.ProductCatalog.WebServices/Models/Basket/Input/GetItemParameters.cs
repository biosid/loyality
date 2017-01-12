namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Basket.Input
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class GetItemParameters
    {
        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
        public string ProductId { get; set; }

        [DataMember]
        public Dictionary<string, string> ClientContext { get; set; }
    }
}
