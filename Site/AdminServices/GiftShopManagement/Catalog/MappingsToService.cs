using System;
using Vtb24.Arms.AdminServices.CatalogAdminService;
using Vtb24.Arms.AdminServices.GiftShopManagement.Catalog.Models;
using Vtb24.Arms.AdminServices.GiftShopManagement.Catalog.Models.Inputs;
using System.Linq;
using CategoryPath = Vtb24.Arms.AdminServices.GiftShopManagement.Catalog.Models.CategoryPath;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Catalog
{
    internal static class MappingsToService
    {
        public static ProductCategoryStatuses ToCategoryStatus(CategoryStatus status)
        {
            switch (status)
            {
                case CategoryStatus.Disabled:
                    return ProductCategoryStatuses.NotActive;
                case CategoryStatus.Enabled:
                    return ProductCategoryStatuses.Active;
            }
            return ProductCategoryStatuses.NotActive;
        }

        public static ProductCategoryTypes ToCategoryType(CategoryType type)
        {
            switch (type)
            {
                case CategoryType.Online:
                    return ProductCategoryTypes.Online;
                case CategoryType.Static:
                    return ProductCategoryTypes.Static;
            }
            return ProductCategoryTypes.Static;
        }

        public static CategoryPositionTypes ToMoveOptions(MoveOptions options)
        {
            switch (options)
            {
                case MoveOptions.Append:
                    return CategoryPositionTypes.Append;
                case MoveOptions.After:
                    return CategoryPositionTypes.After;
                case MoveOptions.Before:
                    return CategoryPositionTypes.Before;
                case MoveOptions.Prepend:
                    return CategoryPositionTypes.Prepend;
            }

            throw new NotSupportedException();
        }

        public static CatalogAdminService.CategoryPath ToCategoryPath(CategoryPath path)
        {
            if (path == null)
                return null;

            return new CatalogAdminService.CategoryPath
            {
                IncludeSubcategories = path.IncludeSubcategories,
                NamePath = path.NamePath,
            };
        }

        public static PartnerProductCategoryLink ToCategoryBinding(CategoryBinding binding)
        {
            if (binding == null)
                return null;

            return new PartnerProductCategoryLink
            {
                PartnerId = binding.SupplierId,
                ProductCategoryId = binding.ProductCategoryId,
                Paths = binding.CategoryPaths.Select(ToCategoryPath).ToArray()
            };
        }
    }
}
