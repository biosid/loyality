namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class GetProductByIdParameters
    {
        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
        public string ProductId { get; set; }

        [DataMember]
        public Dictionary<string, string> ClientContext { get; set; }

        [DataMember]
        public bool? RegisterView { get; set; }
    }
}
