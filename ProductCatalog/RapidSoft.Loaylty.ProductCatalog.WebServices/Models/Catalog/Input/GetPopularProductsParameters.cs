namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class GetPopularProductsParameters
    {
        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
        public Dictionary<string, string> ClientContext { get; set; }

        [DataMember]
        public PopularityTypes PopularityType { get; set; }

        [DataMember]
        public int? Take { get; set; }
    }
}
