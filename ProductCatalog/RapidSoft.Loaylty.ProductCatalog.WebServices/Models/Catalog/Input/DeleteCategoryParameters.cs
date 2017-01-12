namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class DeleteCategoryParameters : CatalogAdminParameters
    {
        [DataMember]
        public int CategoryId { get; set; }
    }
}
