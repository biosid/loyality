using System.Configuration;

namespace Vtb24.OnlineCategories.Client.Internal
{
    internal class Configuration
    {
        public static string GatewayEndpoint
        {
            get { return String("bonus_gateway::endpoint", string.Empty); }
        }

        public static string PaymentFormUrl
        {
            get { return String("bonus_gateway::payment_form_url", string.Empty); }
        }

        public static string ShopId
        {
            get { return String("bonus_gateway::shop_id", string.Empty); }
        }

        public static string PrivateKeyPem
        {
            get { return String("bonus_gateway::private_key", string.Empty); }
        }

        public static string GatewayPublicKeyPem
        {
            get { return String("bonus_gateway::public_key", string.Empty); }
        }

        private static string String(string key, string defaultValue)
        {
            return ConfigurationManager.AppSettings[key] ?? defaultValue;
        }
    }
}
