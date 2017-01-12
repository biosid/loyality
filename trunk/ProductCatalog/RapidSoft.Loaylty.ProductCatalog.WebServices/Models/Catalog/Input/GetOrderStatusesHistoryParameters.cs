namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Input;

    [DataContract]
    public class GetOrderStatusesHistoryParameters : CatalogAdminParameters
    {
        [DataMember]
        public int OrderId { get; set; }

        [DataMember]
        public PagingParameters Paging { get; set; }
    }
}
