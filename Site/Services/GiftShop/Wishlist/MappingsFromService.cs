using System.Linq;
using Vtb24.Site.Services.GiftShop.Model;
using Vtb24.Site.Services.ProductCatalogWishListService;
using Product = Vtb24.Site.Services.GiftShop.Model.Product;

namespace Vtb24.Site.Services.GiftShop.Wishlist
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

        public static Product ToProduct(ProductCatalogWishListService.Product original)
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
                PriceWithoutDiscount = original.PriceBase
            };
        }

        public static ReservedProductItem ToReservedProductItem(WishListItem original)
        {
            if (original == null)
                return null;

            return new ReservedProductItem
            {
                Product = ToProduct(original.Product), // WTF: Где Id у вишлиста
                ProductStatus = ToProductStatus(original.AvailabilityStatus),
                Quantity = original.ProductsQuantity,
                TotalQuantityPrice = original.TotalPrice,
                ItemPrice = original.ItemPrice,
                AddedDate = original.CreatedDate
            };
        }
    }
}
