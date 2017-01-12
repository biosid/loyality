namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories
{
    internal abstract class BaseRepository
    {
        private readonly string connectionString;

        protected BaseRepository()
        {
            this.connectionString = DataSourceConfig.ConnectionString;
        }

        protected BaseRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected LoyaltyDBEntities DbNewContext(string userId = null)
        {
            return new LoyaltyDBEntities(connectionString)
            {
                UserId = userId
            }; 
        }
    }
}