namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Input;

    [DataContract]
    public class GetPublicSubCategoriesParameters
    {
        [DataMember]
        public int? ParentId { get; set; }

        [DataMember]
        public bool IncludeParent { get; set; }

        [DataMember]
        public int? NestingLevel { get; set; }

        [DataMember]
        public ProductCategoryTypes? Type { get; set; }

        [DataMember]
        public Dictionary<string, string> ClientContext { get; set; }

        [DataMember]
        public PagingParameters Paging { get; set; }
    }
}
