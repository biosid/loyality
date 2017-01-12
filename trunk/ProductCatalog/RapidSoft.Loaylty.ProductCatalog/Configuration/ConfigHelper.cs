namespace RapidSoft.Loaylty.ProductCatalog.Configuration
{
    using System;
    using System.Configuration;

    public static class ConfigHelper
    {
        public static int LoyaltyProgramId
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["LoyaltyProgramId"]);
            }
        }

        public static string OrdersNotificationsRecipientsSettingName
        {
            get
            {
                return ConfigurationManager.AppSettings["OrdersNotificationsRecipientsSettingName"];
            }
        }

        public static string ArmOrderUrlTemplate
        {
            get
            {
                return ConfigurationManager.AppSettings["ArmOrderUrlTemplate"];
            }
        }

        public static int SaveOrderAttemptMinHour
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["SaveOrderAttemptMinHour"]);
            }
        }

        public static int SaveOrderAttemptMaxHour
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["SaveOrderAttemptMaxHour"]);
            }
        }

        public static int ClearOrderAttemptMinHour
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["ClearOrderAttemptMinHour"]);
            }
        }

        public static int ClearOrderAttemptMaxHour
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["ClearOrderAttemptMaxHour"]);
            }
        }

        public static string CustomCommitMethodSettingName
        {
            get
            {
                return ConfigurationManager.AppSettings["CustomCommitMethodSettingName"];
            }
        }

        public static int BankProductsPartnerId
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["BankProductsPartnerId"]);
            }
        }
    }
}
