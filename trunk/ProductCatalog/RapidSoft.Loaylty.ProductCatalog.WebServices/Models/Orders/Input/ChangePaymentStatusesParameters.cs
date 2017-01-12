namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ChangePaymentStatusesParameters
    {
        [DataMember]
        public PaymentStatusChange[] Changes { get; set; }
    }
}
