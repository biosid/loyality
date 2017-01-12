namespace RapidSoft.Loaylty.ProductCatalog.OrdersNotifications.Helpers
{
    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.Entities;

    internal static class OrderNotificationHelpers
    {
        public static string ExtractEmailAddress(this OrderNotification notification)
        {
            EnsureDeliveryInfo(notification);

            return notification.DeliveryInfoObject.ExtractEmailAddress();
        }

        public static string ExtractDeliveryAddress(this OrderNotification notification)
        {
            EnsureDeliveryInfo(notification);

            return notification.DeliveryInfoObject.ExtractDeliveryAddress();
        }

        private static void EnsureDeliveryInfo(OrderNotification notification)
        {
            if (notification.DeliveryInfoObject == null && !string.IsNullOrWhiteSpace(notification.DeliveryInfo))
            {
                notification.DeliveryInfoObject = XmlSerializer.Deserialize<DeliveryInfo>(notification.DeliveryInfo);
            }
        }
    }
}
