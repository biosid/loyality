using System.Linq;
using Vtb24.Arms.AdminServices.GiftShopManagement.Catalog.Models;

namespace Vtb24.Arms.Catalog.Models.PartnerCategories
{
    public class SupplierCategoryModel
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string Title { get; set; }

        public int Depth { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsEmpty { get; set; }

        public bool HasChildren { get; set; }

        public bool NotAvailable { get; set; }

        public string[] Bindings { get; set; }

        public bool SupplierHasAccess { get; set; }

        public SupplierCategoriesPermissionsModel Permissions { get; set; }

        public static SupplierCategoryModel Map(Category category)
        {
            var cat = new SupplierCategoryModel
            {
                Id = category.Id,
                Title = category.Title,
                ParentId = category.ParentId,
                Depth = category.Depth,
                IsEnabled = category.Status == CategoryStatus.Enabled,
                IsEmpty = category.CountedProducts == 0,
                HasChildren = category.CountedSubCategories > 0,
            };

            cat.NotAvailable = !cat.IsEnabled;

            return cat;
        }

        public SupplierCategoryModel SetBindings(CategoryBinding binding)
        {
            Bindings = binding != null ? binding.CategoryPaths.Select(cp => cp.NamePath).ToArray() : null;

            return this;
        }
    }
}