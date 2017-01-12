namespace Vtb24.Arms.AdminServices.GiftShopManagement.Catalog.Models.Inputs
{
    public class GetCategoriesFilter
    {
        public int? ParentCategoryId { get; set; }

        public int? Depth { get; set; }

        public CategoryType? Type { get; set; }

        public CategoryStatus? Status { get; set; }

        public bool IncludeParent { get; set; }
    }
}