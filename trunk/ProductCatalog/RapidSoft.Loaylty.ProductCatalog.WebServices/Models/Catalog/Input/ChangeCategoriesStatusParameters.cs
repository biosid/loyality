namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ChangeCategoriesStatusParameters : CatalogAdminParameters
    {
        [DataMember]
        public int[] CategoryIds { get; set; }

        [DataMember]
        public ProductCategoryStatuses Status { get; set; }
    }
}
