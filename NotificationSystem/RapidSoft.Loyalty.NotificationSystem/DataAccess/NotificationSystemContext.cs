using System.Configuration;
using System.Data.Entity;
using Rapidsoft.Loyalty.NotificationSystem.API.Entities;

namespace Rapidsoft.Loyalty.NotificationSystem.DataAccess
{
    internal class NotificationSystemContext : DbContext
    {
        static NotificationSystemContext()
        {
            Database.SetInitializer<NotificationSystemContext>(null);
        }

        public NotificationSystemContext()
            : base(ConfigurationManager.ConnectionStrings["LoyaltyDB"].ConnectionString)
        {
        }

        public DbSet<Thread> Threads { get; set; }
        public DbSet<ThreadMessage> ThreadMessages { get; set; }
        public DbSet<MessageAttachment> Attachments { get; set; }
        public DbSet<MessageToNotify> MessagesToNotify { get; set; }
        public DbSet<EmailBlackList> EmailBlackList { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            OnThreadModelCreating(modelBuilder);
            OnThreadMessageModelCreating(modelBuilder);
            OnAttachmentModelCreating(modelBuilder);
            OnMessageToNotifyModelCreating(modelBuilder);
            OnEmailBlackListModelCreating(modelBuilder);
        }

        private static void OnAttachmentModelCreating(DbModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<MessageAttachment>();
            entity.ToTable("Attachments", "mess");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ThreadId).IsRequired();
            entity.Property(e => e.MessageId).IsRequired();
            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.FileSize).IsRequired();
        }

        private static void OnThreadMessageModelCreating(DbModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<ThreadMessage>();
            entity.ToTable("ThreadMessages", "mess");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ThreadId).IsRequired();
            entity.Property(e => e.MessageBody).IsRequired();
            entity.Property(e => e.Index).IsRequired();
            entity.Property(e => e.IsUnread).IsRequired();
            entity.Property(e => e.MessageType).IsRequired();
            entity.Property(e => e.AuthorFullName).HasMaxLength(255);
            entity.Property(e => e.AuthorId);
            entity.Property(e => e.AuthorEmail).HasMaxLength(255);
            entity.Property(e => e.InsertedDate).IsRequired();
        }

        private static void OnThreadModelCreating(DbModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Thread>();
            entity.ToTable("Threads", "mess");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Type).IsRequired();
            entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
            entity.Property(e => e.ClientType).IsRequired();
            entity.Property(e => e.IsClosed).IsRequired();
            entity.Property(e => e.ClientId).HasMaxLength(64);
            entity.Property(e => e.ClientFullName).HasMaxLength(255);
            entity.Property(e => e.ClientEmail).HasMaxLength(255);
            entity.Ignore(e => e.TopicMessage);
            entity.Property(e => e.MessagesCount).IsRequired();
            entity.Property(e => e.UnreadMessagesCount).IsRequired();
            entity.Property(e => e.FirstMessageTime).IsRequired();
            entity.Property(e => e.LastMessageTime).IsRequired();
            entity.Property(e => e.FirstMessageBy).HasMaxLength(500);
            entity.Property(e => e.LastMessageBy).HasMaxLength(500);
            entity.Property(e => e.ShowSince);
            entity.Property(e => e.ShowUntil);
            entity.Property(e => e.FirstMessageType).IsRequired();
            entity.Property(e => e.LastMessageType).IsRequired();
            entity.Property(e => e.IsDeleted).IsRequired();
        }

        private static void OnMessageToNotifyModelCreating(DbModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<MessageToNotify>();
            entity.ToTable("MessagesToNotify", "mess");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ThreadId).IsRequired();
            entity.Property(e => e.MessageIndex).IsRequired();
            entity.Property(e => e.MessageTime).IsRequired();
        }

        private static void OnEmailBlackListModelCreating(DbModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<EmailBlackList>();
            entity.ToTable("EmailBlackList", "mess");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ClientEmail).HasMaxLength(255);
            entity.Property(e => e.InsertedDate).IsRequired();
        }
    }
}
