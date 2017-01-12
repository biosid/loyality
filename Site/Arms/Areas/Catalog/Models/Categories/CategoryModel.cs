using Vtb24.Arms.AdminServices.GiftShopManagement.Catalog.Models;

namespace Vtb24.Arms.Catalog.Models.Categories
{
    public class CategoryModel
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string Title { get; set; }

        public int Depth { get; set; }

        public long ProductsCount { get; set; }

        public string OnlineCategoryUrl { get; set; }

        public string NotifyOrderStatusUrl { get; set; }

        public int? OnlineCategoryPartnerId { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsEmpty { get; set; }

        public bool HasChildren { get; set; }

        public bool IsOnline { get; set; }

        public bool NotAvailable { get; set; }

        public CategoriesPermissionsModel Permissions { get; set; }

        public static CategoryModel Map(Category original, CategoriesPermissionsModel permissions)
        {
            var cat = new CategoryModel
            {
                Id = original.Id,
                Title = original.Title,
                ParentId = original.ParentId,
                ProductsCount = original.CountedProducts ?? 0,
                Depth = original.Depth,
                IsEnabled = original.Status == CategoryStatus.Enabled,
                IsEmpty = original.CountedProducts == 0,
                HasChildren = original.CountedSubCategories > 0,
                IsOnline = original.Type == CategoryType.Online,
                OnlineCategoryUrl = original.OnlineCategoryUrl,
                NotifyOrderStatusUrl = original.NotifyOrderStatusUrl,
                OnlineCategoryPartnerId = original.OnlineCategoryPartnerId,
                Permissions = permissions
            };

            cat.NotAvailable = !cat.IsEnabled;

            return cat;
        }
    }
}