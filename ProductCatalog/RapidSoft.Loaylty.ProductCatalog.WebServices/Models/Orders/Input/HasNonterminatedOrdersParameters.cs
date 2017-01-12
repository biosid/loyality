namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class HasNonterminatedOrdersParameters
    {
        [DataMember]
        public string ClientId { get; set; }
    }
}