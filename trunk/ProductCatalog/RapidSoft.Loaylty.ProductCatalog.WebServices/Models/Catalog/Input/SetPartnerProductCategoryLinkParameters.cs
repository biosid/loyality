namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class SetPartnerProductCategoryLinkParameters : CatalogAdminParameters
    {
        [DataMember]
        public PartnerCategoryLink Link { get; set; }
    }
}
