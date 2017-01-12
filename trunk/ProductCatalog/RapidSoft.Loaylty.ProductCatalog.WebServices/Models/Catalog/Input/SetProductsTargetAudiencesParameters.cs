namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class SetProductsTargetAudiencesParameters : CatalogAdminParameters
    {
        [DataMember]
        public string[] ProductIds { get; set; }

        [DataMember]
        public string[] TargetAudienceIds { get; set; }
    }
}
