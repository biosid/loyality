using System.Linq;
using Vtb24.Site.Services.GiftShop.Model;
using Vtb24.Site.Services.ProductCatalogBasketService;
using Product = Vtb24.Site.Services.GiftShop.Model.Product;

namespace Vtb24.Site.Services.GiftShop.Basket
{
    internal static class MappingsFromService
    {
        public static ProductStatus ToProductStatus(ProductAvailabilityStatuses original)
        {
            switch (original)
            {
                case ProductAvailabilityStatuses.Available:
                    return ProductStatus.Available;

                case ProductAvailabilityStatuses.DeliveryRateNotFound:
                    return ProductStatus.DeliveryNotAvailable;

                case ProductAvailabilityStatuses.CategoryIsNotActive:
                case ProductAvailabilityStatuses.CategoryPermissionNotFound:
                case ProductAvailabilityStatuses.ModerationNotApplied:
                case ProductAvailabilityStatuses.PartnerIsNotActive:
                case ProductAvailabilityStatuses.PriceNotFound:
                case ProductAvailabilityStatuses.ProductIsNotActive:
                case ProductAvailabilityStatuses.TargetAudienceNotFound:
                    return ProductStatus.NotInStock;

                default:
                    return ProductStatus.Unknown;
            }
        }

        public static Product ToProduct(ProductCatalogBasketService.Product original)
        {
            if (original == null)
                return null;

            return new Product
            {
                Id = original.ProductId,
                PartnerId = original.PartnerId,
                CategoryId = original.CategoryId,
                CategoryTitle = original.CategoryName,
                Title = original.Name,
                Thumbnail = original.Pictures == null
                                ? null
                                : original.Pictures.FirstOrDefault(),
                Vendor = original.Vendor,
                VendorCode = original.Vendor,
                HasDiscount = original.IsActionPrice,
                AddedToCatalogDate = original.InsertedDate,
                PriceRur = original.PriceRUR,
                Price = original.Price,
                PriceWithoutDiscount = original.PriceBase,
				IsDeliveredByEmail = original.IsDeliveredByEmail
            };
        }

        public static ReservedProductItem ToReservedProductItem(BasketItem original)
        {
            if (original == null)
                return null;

            return new ReservedProductItem
            {
                Id = original.Id.ToString(),
                Product = ToProduct(original.Product),
                ProductStatus = ToProductStatus(original.AvailabilityStatus),
                Quantity = original.ProductsQuantity,
                ItemPrice = original.ItemPrice,
                TotalQuantityPrice = original.TotalPrice,
                TotalQuantityPriceRur = original.TotalPriceRur,
                AddedDate = original.CreatedDate,
                ReservedProductGroupId = original.BasketItemGroupId
            };
        }
    }
}