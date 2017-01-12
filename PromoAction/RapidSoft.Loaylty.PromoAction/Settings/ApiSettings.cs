namespace RapidSoft.Loaylty.PromoAction.Settings
{
    using System.Configuration;

    using RapidSoft.Extensions;

    public class ApiSettings
    {
        public static int MaxResultsCountRules
        {
            get
            {
                return ConfigurationManager.AppSettings.GetIntOrDefault("MaxResultsCountRules", 50);
            }
        }

        public static int MaxResultsCountRuleHistories
        {
            get
            {
                return ConfigurationManager.AppSettings.GetIntOrDefault("MaxResultsCountRuleHistories", 50);
            }
        }

        public static string ClientProfileObjectName
        {
            get
            {
                return ConfigurationManager.AppSettings["ClientProfileObjectName"] ?? "ClientProfile";
            }
        }

        public static string PromoActionPropertyName
        {
            get
            {
                return ConfigurationManager.AppSettings["PromoActionPropertyName"] ?? "Audiences";
            }
        }

        public static string TargetAudienceLiteralPrefix
        {
            get
            {
                return ConfigurationManager.AppSettings["TargetAudienceLiteralPrefix"] ?? "Audience_";
            }
        }
    }
}