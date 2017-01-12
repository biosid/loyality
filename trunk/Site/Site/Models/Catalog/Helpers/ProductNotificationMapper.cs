using Vtb24.Site.Services.GiftShop.Model;

namespace Vtb24.Site.Models.Catalog.Helpers
{
    public static class ProductNotificationMapper
    {
        public static string Map(ProductStatus status)
        {
            switch (status)
            {
                case ProductStatus.Available:
                    return null;
                case ProductStatus.NotAvailable:
                    return "Вознаграждение недоступно для заказа";
                case ProductStatus.DeliveryNotAvailable:
                    return "Доставка в выбранный регион недоступна";
                case ProductStatus.NotInStock:
                    return "Вознаграждение временно недоступно";
                default:
                    return null;
            }
        }
    }
}