namespace RapidSoft.Loaylty.ProductCatalog.DataSources
{
    using System.Configuration;
    using System.Data.EntityClient;
    using System.Data.SqlClient;

    public class DataSourceConfig
    {
        public const string LoyaltyproductcatalogDb = "LoyaltyProductCatalogDB";

        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[LoyaltyproductcatalogDb].ConnectionString;
            }
        }

        public static string ProviderName
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[LoyaltyproductcatalogDb].ProviderName;
            }
        }

        public static void CheckProductDBConnection()
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
            }
        }
    }
}