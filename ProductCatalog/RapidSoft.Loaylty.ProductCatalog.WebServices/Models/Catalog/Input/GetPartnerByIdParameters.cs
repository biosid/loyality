namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class GetPartnerByIdParameters : CatalogAdminParameters
    {
        [DataMember]
        public int PartnerId { get; set; }
    }
}
