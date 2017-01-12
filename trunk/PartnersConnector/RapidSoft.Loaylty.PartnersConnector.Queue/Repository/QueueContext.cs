using System.Configuration;
using System.Data.Entity;

using RapidSoft.Loaylty.PartnersConnector.Queue.Entities;

namespace RapidSoft.Loaylty.PartnersConnector.Queue.Repository
{
    internal class QueueContext : DbContext
    {
        public const string ConnectionString = "Queue";

        static QueueContext()
        {
            Database.SetInitializer<QueueContext>(null);
        }

        public QueueContext(string connectionString)
            : base(connectionString)
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        public QueueContext()
            : base(ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString)
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<CommitOrderQueueItem> CommitOrderQueueItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            this.OnCommitOrderQueueItemModelCreating(modelBuilder);
        }

        private void OnCommitOrderQueueItemModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommitOrderQueueItem>().ToTable("CommitOrderQueueItems", "connect");
            modelBuilder.Entity<CommitOrderQueueItem>().HasKey(e => e.Id);
            modelBuilder.Entity<CommitOrderQueueItem>().Property(e => e.PartnerId).IsRequired();
            modelBuilder.Entity<CommitOrderQueueItem>().Property(e => e.Order).HasColumnType("xml").IsRequired().IsMaxLength();
            modelBuilder.Entity<CommitOrderQueueItem>().Property(e => e.ClientId).IsRequired();
            modelBuilder.Entity<CommitOrderQueueItem>().Property(e => e.InsertedDate).IsRequired();
            modelBuilder.Entity<CommitOrderQueueItem>().Property(e => e.ProcessStatus).IsRequired();
            modelBuilder.Entity<CommitOrderQueueItem>().Property(e => e.ProcessStatusDescription).IsOptional();
        }
    }
}
