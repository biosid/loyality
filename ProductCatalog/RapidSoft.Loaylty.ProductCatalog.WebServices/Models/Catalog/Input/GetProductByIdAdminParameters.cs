namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class GetProductByIdAdminParameters : CatalogAdminParameters
    {
        [DataMember]
        public string ProductId { get; set; }
    }
}
