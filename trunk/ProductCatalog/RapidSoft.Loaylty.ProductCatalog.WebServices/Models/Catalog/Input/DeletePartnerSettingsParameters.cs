namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class DeletePartnerSettingsParameters : CatalogAdminParameters
    {
        [DataMember]
        public int PartnerId { get; set; }

        [DataMember]
        public string[] Settings { get; set; }
    }
}
