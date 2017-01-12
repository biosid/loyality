namespace RapidSoft.VTB24.BankConnector.Extension
{
    using System.Collections.Generic;

    using RapidSoft.Loaylty.ProductCatalog.WsClients.CatalogAdminService;
    using RapidSoft.VTB24.BankConnector.Infrastructure.Configuration;

    public static class PartnerExtensions
    {
        private const string GetDeliveryVariantsKey = "GetDeliveryVariants";

        public static bool IsOnlineDeliveryVariantsSupported(this Partner partner)
        {
            return
                partner != null &&
                partner.Settings != null &&
                !string.IsNullOrWhiteSpace(partner.Settings.GetValue(GetDeliveryVariantsKey));
        }

        public static bool IsAdvancePaymentsSupported(this Partner partner)
        {
            string strValue;

            return
                partner != null &&
                partner.Settings != null &&
                partner.Settings.TryGetValue(ConfigHelper.PaymentSupportPartnerSetting, out strValue) &&
                (strValue == "Delivery" || strValue == "Full");
        }

        public static string UnitellerAdvancePaymentShopId(this Partner partner)
        {
            string value;

            return
                partner != null &&
                partner.Settings != null &&
                partner.Settings.TryGetValue(ConfigHelper.PaymentUnitellerShopIdPartnerSetting, out value)
                    ? value
                    : null;
        }

        private static string GetValue(this IReadOnlyDictionary<string, string> settings, string key)
        {
            string value;
            return settings.TryGetValue(key, out value) ? value : null;
        }
    }
}
