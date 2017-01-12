using System.Configuration;

namespace RapidSoft.Loaylty.PartnersConnector.Litres
{
    public static class Configuration
    {
        private const int DEFAULT_MIN_REMAINING_CODES_COUNT = 5;

        static Configuration()
        {
            int intResult;

            CatalogAdminUserId = ConfigurationManager.AppSettings["VtbSystemUserName"];

            LitresPartnerId = int.TryParse(ConfigurationManager.AppSettings["LitresPartnerId"], out intResult)
                                  ? intResult
                                  : (int?) null;

            MinRemainingCodesCount = int.TryParse(ConfigurationManager.AppSettings["LitresMinRemainingCodesCount"], out intResult)
                                         ? intResult
                                         : DEFAULT_MIN_REMAINING_CODES_COUNT;

            LowCodesCountWarningTo = ConfigurationManager.AppSettings["LitresLowCodesCountWarningTo"];

            LitresRemainingCodesCountReportTo = ConfigurationManager.AppSettings["LitresRemainingCodesCountReportTo"];
        }

        public static string CatalogAdminUserId { get; private set; }

        public static int? LitresPartnerId { get; private set; }

        public static int MinRemainingCodesCount { get; private set; }

        public static string LowCodesCountWarningTo { get; private set; }

        public static string LitresRemainingCodesCountReportTo { get; private set; }
    }
}
