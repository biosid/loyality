namespace RapidSoft.Loaylty.ProductCatalog.OrdersNotifications.Helpers
{
    using RapidSoft.Loaylty.ProductCatalog.Configuration;

    internal static class ArmUrlHelper
    {
        public static string GenerateOrderUrl(this int orderId)
        {
            return string.Format(ConfigHelper.ArmOrderUrlTemplate, orderId);
        }
    }
}
