namespace Vtb24.Arms.AdminServices.GiftShopManagement.Products.Models.Inputs
{
    public class ProductsSearchCriteria
    {
        public int[] SupplierIds { get; set; }

        public string[] ProductIds { get; set; }

        public string SearchTerm { get; set; }

        public int[] CategoryIds { get; set; }

        public ProductModerationStatus? ModerationStatus { get; set; }

        public bool? IsRecommended { get; set; }

        public bool IncludeSubCategories { get; set; }

        public ProductsSort Sorting { get; set; }
    }
}
