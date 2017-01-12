namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class GetProductByIdParameters : IClientContextParameters
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