using System.Linq;
using Vtb24.Arms.AdminServices.CatalogAdminService;
using Vtb24.Arms.AdminServices.GiftShopManagement.Products.Models;
using Product = Vtb24.Arms.AdminServices.GiftShopManagement.Products.Models.Product;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Products
{
    internal static class MappingsFromService
    {
        public static ProductStatus ToProductStatus(ProductStatuses original)
        {
            switch (original)
            {
                case ProductStatuses.Active:
                    return ProductStatus.Active;
                case ProductStatuses.NotActive:
                    return ProductStatus.NotActive;
            }

            return ProductStatus.Unknown;
        }

        public static ProductModerationStatus ToProductModerationStatus(ProductModerationStatuses original)
        {
            switch (original)
            {
                case ProductModerationStatuses.Applied:
                    return ProductModerationStatus.Applied;
                case ProductModerationStatuses.Canceled:
                    return ProductModerationStatus.Canceled;
                case ProductModerationStatuses.InModeration:
                    return ProductModerationStatus.InModeration;
            }
            return ProductModerationStatus.InModeration;
        }

        public static Product.Parameter ToProductParameter(ProductParam original)
        {
            return new Product.Parameter
            {
                Name = original.Name,
                Unit = original.Unit,
                Value = original.Value
            };
        }

        public static Product ToProduct(CatalogAdminService.Product original)
        {
            
            return new Product
            {
                Id = original.ProductId,
                SupplierId = original.PartnerId,
                CategoryId = original.CategoryId,
                CategoryPath = original.CategoryNamePath,
                SupplierProductId = original.PartnerProductId,
                Name = original.Name,
                Status = ToProductStatus(original.Status),
                ModerationStatus = ToProductModerationStatus(original.ModerationStatus),
                IsRecommended = original.IsRecommended,
                PriceRUR = original.PriceRUR,
                Segments = original.TargetAudiencesIds,
                Pictures = original.Pictures,
                Description = original.Description,
                Vendor = original.Vendor,
                Weight = original.Weight,
				IsDeliveredByEmail = original.IsDeliveredByEmail,
                BasePriceRUR = original.BasePriceRUR,
                BasePriceRurDate = original.BasePriceRurDate,
                Parameters = original.Param != null ? 
                                original.Param.Select(ToProductParameter).ToArray() : null
            };
        }
    }
}
