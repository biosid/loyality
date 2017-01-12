namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ChangeProductsStatusParameters : CatalogAdminParameters
    {
        [DataMember]
        public string[] ProductIds { get; set; }

        [DataMember]
        public ProductStatuses Status { get; set; }
    }
}
