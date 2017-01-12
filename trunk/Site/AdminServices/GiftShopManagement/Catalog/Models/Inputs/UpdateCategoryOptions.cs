namespace Vtb24.Arms.AdminServices.GiftShopManagement.Catalog.Models.Inputs
{
    public class UpdateCategoryOptions
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string OnlineCategoryUrl { get; set; }

        public string NotifyOrderStatusUrl { get; set; }

        public int? OnlineCategoryPartnerId { get; set; }

        public CategoryStatus Status { get; set; }
    }
}