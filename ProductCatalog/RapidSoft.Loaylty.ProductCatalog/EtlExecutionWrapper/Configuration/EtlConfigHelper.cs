namespace RapidSoft.Loaylty.ProductCatalog.EtlExecutionWrapper.Configuration
{
    using System.Configuration;

    internal static class EtlConfigHelper
    {
        public const string ConnectionStringName = "LoyaltyProductCatalogDB";

        public const string Schema = "dbo";

        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
            }
        }
    }
}
