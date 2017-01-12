using Vtb24.Site.Services.Buy;
using Vtb24.Site.Services.GiftShop.Orders.Models;

namespace Vtb24.Site.Models.MyOrders.Helpers
{
    public static class OrderStatusTextHelper
    {
        public static string GetStatusText(OrderStatus status, int partnerId)
        {
            switch (status)
            {
                case OrderStatus.Registration:
                    return "Оформляется";
                case OrderStatus.Processing:
                    return partnerId != BuyService.BankProductsPartnerId
                        ? "В обработке"
                        : "В процессе обработки";
                case OrderStatus.DeliveryWaiting:
                    return "Комплектуется";

                case OrderStatus.Delivered:
                    return "Доставлен";
                case OrderStatus.Delivery:
                    return "В процессе доставки";
                case OrderStatus.NotDelivered:
                    return "Не доставлен";
                case OrderStatus.Cancelled:
                    return "Аннулирован";
            }

            return string.Empty;
        }
    }
}