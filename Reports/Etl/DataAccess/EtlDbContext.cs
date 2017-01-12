using System.Data.Entity;
using Rapidsoft.VTB24.Reports.Etl.DataAccess.Models;
using Rapidsoft.VTB24.Reports.Etl.Infrastructure;

namespace Rapidsoft.VTB24.Reports.Etl.DataAccess
{
    public class EtlDbContext : DbContext
    {
        static EtlDbContext()
        {
            Database.SetInitializer<EtlDbContext>(null);
        }

        public EtlDbContext()
            : base(Settings.EtlConnectionString)
        {
            Configuration.ProxyCreationEnabled = false;
            _schemaName = Settings.EtlSchemaName;
        }

        private readonly string _schemaName;

        public DbSet<EtlIncomingFile> IncomingFiles { get; set; }

        public DbSet<EtlOutcomingFile> OutcomingFiles { get; set; }

        public DbSet<EtlIncomingMail> IncomingMails { get; set; }

        public DbSet<EtlOutcomingMail> OutcomingMails { get; set; }

        public DbSet<EtlCounter> Counters { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EtlIncomingFile>().ToTable("EtlIncomingMailAttachments", _schemaName);
            modelBuilder.Entity<EtlOutcomingFile>().ToTable("EtlOutcomingMailAttachments", _schemaName);
            modelBuilder.Entity<EtlIncomingMail>().ToTable("EtlIncomingMails", _schemaName);
            modelBuilder.Entity<EtlOutcomingMail>().ToTable("EtlOutcomingMails", _schemaName);
            modelBuilder.Entity<EtlCounter>().ToTable("EtlCounters", _schemaName);
        }
    }
}
