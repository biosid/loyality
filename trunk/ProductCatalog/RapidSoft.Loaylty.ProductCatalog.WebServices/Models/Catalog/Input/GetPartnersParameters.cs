namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class GetPartnersParameters : CatalogAdminParameters
    {
        [DataMember]
        public int[] PartnerIds { get; set; }
    }
}
