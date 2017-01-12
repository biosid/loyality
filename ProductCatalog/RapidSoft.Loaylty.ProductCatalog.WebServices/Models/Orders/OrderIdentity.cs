namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders
{
    using System.Runtime.Serialization;

    [DataContract]
    public class OrderIdentity
    {
        [DataMember]
        public int OrderId { get; set; }

        [DataMember]
        public string ExternalOrderId { get; set; }
    }
}
