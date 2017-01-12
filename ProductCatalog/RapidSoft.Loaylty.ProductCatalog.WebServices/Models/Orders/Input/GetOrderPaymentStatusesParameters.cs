namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class GetOrderPaymentStatusesParameters
    {
        [DataMember]
        public int[] OrderIds { get; set; }
    }
}
