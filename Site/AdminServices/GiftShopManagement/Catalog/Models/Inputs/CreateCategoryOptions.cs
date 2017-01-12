namespace Vtb24.Arms.AdminServices.GiftShopManagement.Catalog.Models.Inputs
{
    public class CreateCategoryOptions
    {
        public int? ParentId { get; set; }

        public string Title { get; set; }

        public CategoryType Type { get; set; }

        public string OnlineCategoryUrl { get; set; }

        public string NotifyOrderStatusUrl { get; set; }

        public int? OnlineCategoryPartnerId { get; set; }
    }
}