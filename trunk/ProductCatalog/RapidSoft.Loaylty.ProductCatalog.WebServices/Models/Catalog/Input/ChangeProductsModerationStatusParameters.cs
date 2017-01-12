namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ChangeProductsModerationStatusParameters : CatalogAdminParameters
    {
        [DataMember]
        public string[] ProductIds { get; set; }

        [DataMember]
        public ProductModerationStatuses ModerationStatus { get; set; }
    }
}
