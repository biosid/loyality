namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class GetOrderByExternalIdParameters
    {
        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
        public int PartnerId { get; set; }

        [DataMember]
        public string ExternalOrderId { get; set; }
    }
}