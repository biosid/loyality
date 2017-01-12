namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders;

    [DataContract]
    public class ChangeOrdersStatusesParameters : CatalogAdminParameters
    {
        [DataMember]
        public OrderStatusChange[] Changes { get; set; }
    }
}
