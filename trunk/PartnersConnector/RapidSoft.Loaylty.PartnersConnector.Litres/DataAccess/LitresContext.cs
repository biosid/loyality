using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using RapidSoft.Loaylty.PartnersConnector.Litres.DataAccess.Entities;

namespace RapidSoft.Loaylty.PartnersConnector.Litres.DataAccess
{
    public class LitresContext : DbContext
    {
        public const string CONNECTION_STRING = "Litres";

        static LitresContext()
        {
            Database.SetInitializer<LitresContext>(null);
        }

        public LitresContext(string connectionString)
            : base(connectionString)
        {
        }

        public LitresContext()
            : base(CONNECTION_STRING)
        {
        }

        public DbSet<LitresDownloadCode> LitresDownloadCodes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            OnLitresDownloadCodeModelCreating(modelBuilder.Entity<LitresDownloadCode>());
        }

        private static void OnLitresDownloadCodeModelCreating(EntityTypeConfiguration<LitresDownloadCode> entityConfig)
        {
            entityConfig.ToTable("LitresDownloadCodes", "connect");
        }
    }
}
