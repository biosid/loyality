namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Output
{
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Output;

    [DataContract]
    public class SubCategoriesResult : PagedResult<ProductCategory>
    {
        [DataMember]
        public int ChildrenCount { get; set; }
    }
}
