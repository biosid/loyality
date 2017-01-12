namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class GetPartnersSettingsParameters : CatalogAdminParameters
    {
        [DataMember]
        public int? PartnerId { get; set; }
    }
}
