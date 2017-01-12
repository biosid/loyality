namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class GetPartnerProductCategoryLinksParameters : CatalogAdminParameters
    {
        [DataMember]
        public int PartnerId { get; set; }

        [DataMember]
        public int[] CategoryIds { get; set; }
    }
}
