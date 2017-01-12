namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Input;

    [DataContract]
    public class GetDeliveryLocationHistoryParameters : CatalogAdminParameters
    {
        [DataMember]
        public int PartnerId { get; set; }

        [DataMember]
        public int? LocationId { get; set; }

        [DataMember]
        public PagingParameters Paging { get; set; }
    }
}
