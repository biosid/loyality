using Vtb24.Arms.AdminServices.GiftShopManagement.Products.Models;

namespace Vtb24.Arms.Catalog.Models.Gifts.Helpers
{
    internal static class ProductModerationStatusMapper
    {
        public static string Map(ProductModerationStatus status)
        {
            switch (status)
            {
                case ProductModerationStatus.Applied:
                    return "Принято";
                case ProductModerationStatus.Canceled:
                    return "Отклонено";
                case ProductModerationStatus.InModeration:
                    return "Ожидает модерации";
            }
            return null;
        }
    }
}