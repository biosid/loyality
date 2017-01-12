namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input
{
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Input;

    [DataContract]
    public class SearchProductsParameters : CatalogAdminParameters
    {
        [DataMember]
        public ProductsSortTypes? SortType { get; set; }

        [DataMember]
        public ProductStatuses? Status { get; set; }

        [DataMember]
        public ProductModerationStatuses? ModerationStatus { get; set; }

        [DataMember]
        public bool? IsRecommended { get; set; }

        [DataMember]
        public string[] ProductIds { get; set; }

        [DataMember]
        public bool? IncludeSubCategory { get; set; }

        [DataMember]
        public string SearchTerm { get; set; }

        [DataMember]
        public int[] PartnerIds { get; set; }

        [DataMember]
        public int[] ParentCategories { get; set; }

        [DataMember]
        public PagingParameters Paging { get; set; }
    }
}
