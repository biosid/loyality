using Vtb24.Arms.AdminServices.CatalogAdminService;
using Vtb24.Arms.AdminServices.GiftShopManagement.Catalog.Models;
using CategoryPath = Vtb24.Arms.AdminServices.GiftShopManagement.Catalog.Models.CategoryPath;
using System.Linq;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Catalog
{
    internal static class MappingsFromService
    {
        public static CategoryStatus ToCategoryStatus(ProductCategoryStatuses original)
        {
            switch (original)
            {
                case ProductCategoryStatuses.Active:
                    return CategoryStatus.Enabled;
                case ProductCategoryStatuses.NotActive:
                    return CategoryStatus.Disabled;
            }
            return CategoryStatus.Disabled;
        }

        public static CategoryType ToCategoryType(ProductCategoryTypes original)
        {
            switch (original)
            {
                case ProductCategoryTypes.Static:
                    return CategoryType.Static;
                case ProductCategoryTypes.Online:
                    return CategoryType.Online;
            }
            return CategoryType.Static;
        }

        public static Category ToCategory(ProductCategory original)
        {
            if (original == null)
                return null;

            return new Category
            {
                Id = original.Id,
                ParentId = original.ParentId,
                Depth = original.NestingLevel,
                CountedProducts = original.ProductsCount,
                CountedSubCategories = original.SubCategoriesCount,
                Title = original.Name,
                OnlineCategoryUrl = original.OnlineCategoryUrl,
                NotifyOrderStatusUrl = original.NotifyOrderStatusUrl,
                OnlineCategoryPartnerId = original.OnlineCategoryPartnerId,
                Status = ToCategoryStatus(original.Status),
                Type = ToCategoryType(original.Type),
                CategoryPath = original.NamePath
            };
        }

        public static CategoryPath ToCategoryPath(CatalogAdminService.CategoryPath original)
        {
            if (original == null)
                return null;

            return new CategoryPath
            {
                NamePath = original.NamePath,
                IncludeSubcategories = original.IncludeSubcategories
            };
        }

        public static CategoryBinding ToCategoryBinding(PartnerProductCategoryLink original)
        {
            if (original == null)
                return null;

            return new CategoryBinding
            {
                CategoryPaths = original.Paths.Select(ToCategoryPath).ToArray(),
                SupplierId = original.PartnerId,
                ProductCategoryId = original.ProductCategoryId
            };
        }
    }
}
