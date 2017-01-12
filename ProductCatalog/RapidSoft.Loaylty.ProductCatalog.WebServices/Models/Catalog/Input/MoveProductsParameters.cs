namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class MoveProductsParameters : CatalogAdminParameters
    {
        [DataMember]
        public string[] ProductIds { get; set; }

        [DataMember]
        public int TargetCategoryId { get; set; }
    }
}
