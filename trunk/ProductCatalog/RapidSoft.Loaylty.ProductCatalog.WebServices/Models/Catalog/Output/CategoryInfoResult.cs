namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Output
{
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Output;

    [DataContract]
    public class CategoryInfoResult : ResultBase
    {
        [DataMember]
        public ProductCategory Category { get; set; }

        [DataMember]
        public ProductCategory[] CategoryPath { get; set; }
    }
}
