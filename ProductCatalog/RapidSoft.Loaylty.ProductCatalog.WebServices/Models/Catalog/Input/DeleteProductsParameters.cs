namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class DeleteProductsParameters : CatalogAdminParameters
    {
        [DataMember]
        public string[] ProductIds { get; set; }
    }
}
