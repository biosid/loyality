namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class GetCategoriesPermissionsParameters : CatalogAdminParameters
    {
        [DataMember]
        public int PartnerId { get; set; }
    }
}
