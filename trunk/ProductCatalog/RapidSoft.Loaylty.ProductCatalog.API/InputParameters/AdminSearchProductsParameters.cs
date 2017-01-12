namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using Entities;

    public class AdminSearchProductsParameters
    {
        public string UserId { get; set; }

        public long? CountToSkip { get; set; }

        public int? CountToTake { get; set; }

        public SortTypes? SortType { get; set; }

        public ProductStatuses? Status { get; set; }

        public ProductModerationStatuses? ModerationStatus { get; set; }

        public bool? IsRecommended { get; set; }

        public string[] ProductIds { get; set; }

        public bool? IncludeSubCategory { get; set; }

        public string SearchTerm { get; set; }

        public int[] PartnerIds { get; set; }

        public int[] ParentCategories { get; set; }
    }
}