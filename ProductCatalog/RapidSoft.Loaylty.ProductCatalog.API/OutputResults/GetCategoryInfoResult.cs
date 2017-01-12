namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    [DataContract]
    public class GetCategoryInfoResult : ResultBase
    {
        [DataMember]
        public ProductCategory Category { get; set; }

        [DataMember]
        public ProductCategory[] CategoryPath { get; set; }
    }
}