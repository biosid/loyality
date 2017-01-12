using System;
using Vtb24.Arms.AdminServices.CatalogAdminService;
using Vtb24.Arms.AdminServices.GiftShopManagement.Products.Models;
using Product = Vtb24.Arms.AdminServices.GiftShopManagement.Products.Models.Product;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Products
{
    internal static class MappingsToService
    {
        public static SortTypes ToProductsSort(ProductsSort original)
        {
            switch (original)
            {
                case ProductsSort.ByPriceAscByNameAsc:
                    return SortTypes.ByPriceAscByNameAsc;
                case ProductsSort.ByPriceDescByNameAsc:
                    return SortTypes.ByPriceDescByNameAsc;
            }
            return SortTypes.ByPriceAscByNameAsc;
        }

        public static ProductModerationStatuses? ToProductModerationStatus(ProductModerationStatus? original)
        {
            return original.HasValue
                       ? ToProductModerationStatus(original.Value)
                       : (ProductModerationStatuses?) null;
        }

        public static ProductModerationStatuses ToProductModerationStatus(ProductModerationStatus original)
        {
            switch (original)
            {
                case ProductModerationStatus.Applied:
                    return ProductModerationStatuses.Applied;
                case ProductModerationStatus.Canceled:
                    return ProductModerationStatuses.Canceled;
                case ProductModerationStatus.InModeration:
                    return ProductModerationStatuses.InModeration;
            }
            return ProductModerationStatuses.InModeration;
        }

        public static ProductParam ToProductParameter(Product.Parameter original)
        {
            return new ProductParam
            {
                Name = original.Name,
                Unit = original.Unit,
                Value = original.Value
            };
        }

        public static ProductStatuses ToProductStatuses(ProductStatus original)
        {
            switch (original)
            {
                case ProductStatus.Active:
                    return ProductStatuses.Active;
                case ProductStatus.NotActive:
                    return ProductStatuses.NotActive;
            }

            throw new InvalidOperationException("Неизвестный статус вознаграждения");
        }
    }
}
