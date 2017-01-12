namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders;

    [DataContract]
    public class ChangePaymentStatusesParameters : CatalogAdminParameters
    {
        [DataMember]
        public PaymentStatusChange[] Changes { get; set; }
    }
}
