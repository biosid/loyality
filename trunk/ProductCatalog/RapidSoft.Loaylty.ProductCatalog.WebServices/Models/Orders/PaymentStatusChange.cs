namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders
{
    using System.Runtime.Serialization;

    [DataContract]
    public class PaymentStatusChange
    {
        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
        public int OrderId { get; set; }

        [DataMember]
        public PaymentStatuses PaymentStatus { get; set; }

    }
}
