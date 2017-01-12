using Vtb24.Site.Services.GiftShop.Orders.Models;

namespace Vtb24.Arms.Security.Models.Users.Helpers
{
    internal static class OrderStatusMapper
    {
        public static string Map(this OrderStatus original)
        {
            switch (original)
            {
                case OrderStatus.Registration:
                    return "Оформляется";
                case OrderStatus.Processing:
                    return "В обработке";
                case OrderStatus.DeliveryWaiting:
                    return "Заказ принят";
                case OrderStatus.Delivery:
                    return "Доставка";
                case OrderStatus.Delivered:
                    return "Доставлен";
                case OrderStatus.NotDelivered:
                    return "Не доставлен";
                case OrderStatus.Cancelled:
                    return "Аннулирован";

                default:
                    return "- неизвестен -";
            }
        }
    }
}
