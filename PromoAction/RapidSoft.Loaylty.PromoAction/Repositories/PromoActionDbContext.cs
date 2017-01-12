namespace RapidSoft.Loaylty.PromoAction.Core.Repositories
{
    using System.Data.Entity;
    using System.Threading;
    using System.Web;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Api.Entities.History;

    /// <summary>
    /// Контекст доступа к данным на основне EF.
    /// </summary>
    public class PromoActionDbContext : DbContext
    {
        /// <summary>
        /// Ключ для хранения <see cref="PromoActionDbContext"/> в runtime контексте.
        /// </summary>
        private const string Key = "DAC0711A-0DF4-4A4D-BB9D-D2D8F48C6587";

        /// <summary>
        /// Название строки подключения в config-файле.
        /// </summary>
        private const string ConnectionConfigName = "LoyaltyDB";

        /// <summary>
        /// Название схемы.
        /// </summary>
        private const string SchemaName = "promo";

        /// <summary>
        /// Объект синхронизации.
        /// </summary>
        private static readonly object SyncObj = new object();

        /// <summary>
        /// Initializes static members of the <see cref="PromoActionDbContext"/> class.
        /// </summary>
        static PromoActionDbContext()
        {
            Database.SetInitializer<PromoActionDbContext>(null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PromoActionDbContext"/> class.
        /// </summary>
        /// <param name="nameOrConnectionString">
        /// Название строки подключения в config-файле или строка подключения.
        /// </param>
        private PromoActionDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        /// <summary>
        /// Целевые аудитории.
        /// </summary>
        public DbSet<TargetAudience> TargetAudiences { get; set; }

        /// <summary>
        /// Исторические сущности целевых аудиторий.
        /// </summary>
        public DbSet<TargetAudienceHistory> TargetAudienceHistories { get; set; }

        /// <summary>
        /// Связки "Целевая аудитория"-"Клиент (Профиль клиента)".
        /// </summary>
        public DbSet<TargetAudienceClientLink> TargetAudienceClientLinks { get; set; }

        /// <summary>
        /// Исторические сущности связок "Целевая аудитория"-"Клиент (Профиль клиента)".
        /// </summary>
        public DbSet<TargetAudienceClientLinkHistory> TargetAudienceClientLinkHistories { get; set; }

        /// <summary>
        /// Статический метод получения экземпляра <see cref="PromoActionDbContext"/>.
        /// </summary>
        /// <returns>
        /// Экземпляр контекста доступа к данных.
        /// </returns>
        public static PromoActionDbContext Get()
        {
            var context = GetDbContext();

            if (context == null)
            {
                lock (SyncObj)
                {
                    context = GetDbContext();
                    if (context == null)
                    {
                        context = new PromoActionDbContext(ConnectionConfigName);
                        SetDbContext(context);
                    }
                }
            }

            return context;
        }

        /// <summary>
        /// Статический метод для освобождения ресурсов.
        /// </summary>
        public static void DisposeContext()
        {
            var context = GetDbContext();
            if (context == null)
            {
                return;
            }

            lock (SyncObj)
            {
                SetDbContext(null);
            }

            context.Dispose();
        }

        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but
        /// before the model has been locked down and used to initialize the context.  The default
        /// implementation of this method does nothing, but it can be overridden in a derived class
        /// such that the model can be further configured before it is locked down.
        /// </summary>
        /// <remarks>
        /// Typically, this method is called only once when the first instance of a derived context
        /// is created.  The model for that context is then cached and is for all further instances of
        /// the context in the app domain.  This caching can be disabled by setting the ModelCaching
        /// property on the given ModelBuidler, but note that this can seriously degrade performance.
        /// More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        /// classes directly.
        /// </remarks>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            this.TargetAudienceModelCreating(modelBuilder);

            this.TargetAudienceClientLinkModelCreating(modelBuilder);
        }

        /// <summary>
        /// Получает <see cref="PromoActionDbContext"/> из runtime контекста
        /// </summary>
        /// <returns>
        /// Контекст доступа к данным.
        /// </returns>
        private static PromoActionDbContext GetDbContext()
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Items[Key] as PromoActionDbContext;
            }

            var contextSlot = Thread.GetNamedDataSlot(Key);
            return Thread.GetData(contextSlot) as PromoActionDbContext;
        }

        /// <summary>
        /// Сохраняет <paramref name="context"/> в runtime контексте или отчищает runtime контекст, если передан <c>null</c>.
        /// </summary>
        /// <param name="context">
        /// Контекст доступа к данным.
        /// </param>
        private static void SetDbContext(PromoActionDbContext context)
        {
            if (HttpContext.Current != null)
            {
                if (context != null)
                {
                    HttpContext.Current.Items[Key] = context;
                    return;
                }

                HttpContext.Current.Items.Remove(Key);
                return;
            }

            if (context != null)
            {
                var contextSlot = Thread.GetNamedDataSlot(Key);
                Thread.SetData(contextSlot, context);
                return;
            }

            Thread.FreeNamedDataSlot(Key);
        }

        /// <summary>
        /// Выполняет маппинг сущностей <see cref="TargetAudience"/>.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        private void TargetAudienceModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TargetAudience>().ToTable("TargetAudiences", SchemaName);
            modelBuilder.Entity<TargetAudience>().HasKey(e => e.Id);
            modelBuilder.Entity<TargetAudience>().Property(e => e.Name).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<TargetAudience>().Property(e => e.CreateDateTime).IsRequired();
            modelBuilder.Entity<TargetAudience>().Property(e => e.CreateDateTimeUtc).IsRequired();
            modelBuilder.Entity<TargetAudience>().Property(e => e.CreateUserName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<TargetAudience>().Property(e => e.UpdateDateTime).IsOptional();
            modelBuilder.Entity<TargetAudience>().Property(e => e.UpdateDateTimeUtc).IsOptional();
            modelBuilder.Entity<TargetAudience>().Property(e => e.UpdateUserName).IsOptional().HasMaxLength(50);

            modelBuilder.Entity<TargetAudienceHistory>().ToTable("TargetAudienceHistories", SchemaName);
            modelBuilder.Entity<TargetAudienceHistory>().HasKey(e => e.HistoryId);
            modelBuilder.Entity<TargetAudienceHistory>().Property(e => (int)e.Event).IsRequired();
            modelBuilder.Entity<TargetAudienceHistory>().Property(e => e.TargetAudienceId).IsRequired();
            modelBuilder.Entity<TargetAudienceHistory>().Property(e => e.Name).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<TargetAudienceHistory>().Property(e => e.CreateDateTime).IsRequired();
            modelBuilder.Entity<TargetAudienceHistory>().Property(e => e.CreateDateTimeUtc).IsRequired();
            modelBuilder.Entity<TargetAudienceHistory>().Property(e => e.CreateUserName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<TargetAudienceHistory>().Property(e => e.UpdateDateTime).IsOptional();
            modelBuilder.Entity<TargetAudienceHistory>().Property(e => e.UpdateDateTimeUtc).IsOptional();
            modelBuilder.Entity<TargetAudienceHistory>().Property(e => e.UpdateUserName).IsOptional().HasMaxLength(50);
            modelBuilder.Entity<TargetAudienceHistory>().Property(e => e.DeleteDateTime).IsOptional();
            modelBuilder.Entity<TargetAudienceHistory>().Property(e => e.DeleteDateTimeUtc).IsOptional();
            modelBuilder.Entity<TargetAudienceHistory>().Property(e => e.DeleteUserName).IsOptional().HasMaxLength(50);
        }

        /// <summary>
        /// Выполняет маппинг сущностей <see cref="TargetAudienceClientLink"/>.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        private void TargetAudienceClientLinkModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TargetAudienceClientLink>().ToTable("TargetAudienceClientLinks", SchemaName);
            modelBuilder.Entity<TargetAudienceClientLink>().HasKey(e => new { e.TargetAudienceId, e.ClientId });
            modelBuilder.Entity<TargetAudienceClientLink>().HasRequired(x => x.TargetAudience);
            modelBuilder.Entity<TargetAudienceClientLink>().Property(e => e.CreateDateTime).IsRequired();
            modelBuilder.Entity<TargetAudienceClientLink>().Property(e => e.CreateDateTimeUtc).IsRequired();
            modelBuilder.Entity<TargetAudienceClientLink>().Property(e => e.CreateUserName).IsRequired().HasMaxLength(50);

            modelBuilder.Entity<TargetAudienceClientLinkHistory>().ToTable("TargetAudienceClientLinkHistories", SchemaName);
            modelBuilder.Entity<TargetAudienceClientLinkHistory>().HasKey(e => e.HistoryId);
            modelBuilder.Entity<TargetAudienceClientLinkHistory>().Property(e => (int)e.Event).IsRequired();
            modelBuilder.Entity<TargetAudienceClientLinkHistory>().Property(e => e.TargetAudienceId).IsRequired();
            modelBuilder.Entity<TargetAudienceClientLinkHistory>().Property(e => e.ClientId).IsRequired();
            modelBuilder.Entity<TargetAudienceClientLinkHistory>().Property(e => e.CreateDateTime).IsRequired();
            modelBuilder.Entity<TargetAudienceClientLinkHistory>().Property(e => e.CreateDateTimeUtc).IsRequired();
            modelBuilder.Entity<TargetAudienceClientLinkHistory>().Property(e => e.CreateUserName).IsRequired();
            modelBuilder.Entity<TargetAudienceClientLinkHistory>().Property(e => e.DeleteDateTime).IsOptional();
            modelBuilder.Entity<TargetAudienceClientLinkHistory>().Property(e => e.DeleteDateTimeUtc).IsOptional();
            modelBuilder.Entity<TargetAudienceClientLinkHistory>().Property(e => e.DeleteUserName).IsOptional();
        }
    }
}