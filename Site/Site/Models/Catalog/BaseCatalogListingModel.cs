using Vtb24.Site.Models.Shared;

namespace Vtb24.Site.Models.Catalog
{
    public class BaseCatalogListingModel
    {
        public BreadCrumbModel[] BreadCrumbs { get; set; }

        public ListProductModel[] Products { get; set; }

        public long Total { get; set; }

        public CatalogQueryModel Query { get; set; }

        public FilterModel Filters { get; set; }

        public int TotalPages { get; set; }

        public int Page { get; set; }
    }
}