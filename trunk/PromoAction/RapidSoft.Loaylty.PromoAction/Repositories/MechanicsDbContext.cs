namespace RapidSoft.Loaylty.PromoAction.Repositories
{
    using System.Data.Entity;
    using System.Threading;
    using System.Web;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Api.Entities.History;

    /// <summary>
    /// Контекст доступа к данным на основне EF.
    /// </summary>
    public partial class MechanicsDbContext : DbContext
    {
        /// <summary>
        /// Ключ для хранения <see cref="MechanicsDbContext"/> в runtime контексте.
        /// </summary>
        private const string Key = "01A2F292-16DE-43E4-B39D-CE997A9FA4AA";

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

        static MechanicsDbContext()
        {
            Database.SetInitializer<MechanicsDbContext>(null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MechanicsDbContext"/> class.
        /// </summary>
        /// <param name="nameOrConnectionString">
        /// Название строки подключения в config-файле или строка подключения.
        /// </param>
        private MechanicsDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        /// <summary>
        /// Коллекция доменов правил.
        /// </summary>
        public DbSet<RuleDomain> RuleDomains { get; set; }

        /// <summary>
        /// Коллекция правил.
        /// </summary>
        public DbSet<Rule> Rules { get; set; }

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
        /// Статический метод получения экземпляра <see cref="MechanicsDbContext"/>.
        /// </summary>
        /// <returns>
        /// Экземпляр контекста доступа к данных.
        /// </returns>
        public static MechanicsDbContext Get()
        {
            var context = GetDbContext();

            if (context == null)
            {
                lock (SyncObj)
                {
                    context = GetDbContext();
                    if (context == null)
                    {
                        context = new MechanicsDbContext(ConnectionConfigName);
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

        protected override void Dispose(bool disposing)
        {
            var context = GetDbContext();
            if (context != null)
            {
                lock (SyncObj)
                {
                    SetDbContext(null);
                }
            }

            base.Dispose(disposing);
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

            this.RuleDomainModelCreating(modelBuilder);

            this.RuleModelCreating(modelBuilder);

            this.TargetAudienceModelCreating(modelBuilder);

            this.TargetAudienceClientLinkModelCreating(modelBuilder);
        }

        /// <summary>
        /// Получает <see cref="MechanicsDbContext"/> из runtime контекста
        /// </summary>
        /// <returns>
        /// Контекст доступа к данным.
        /// </returns>
        private static MechanicsDbContext GetDbContext()
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Items[Key] as MechanicsDbContext;
            }

            var contextSlot = Thread.GetNamedDataSlot(Key);
            return Thread.GetData(contextSlot) as MechanicsDbContext;
        }

        /// <summary>
        /// Сохраняет <paramref name="context"/> в runtime контексте или отчищает runtime контекст, если передан <c>null</c>.
        /// </summary>
        /// <param name="context">
        /// Контекст доступа к данным.
        /// </param>
        private static void SetDbContext(MechanicsDbContext context)
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
        /// Выполняет маппинг сущностей <see cref="Rule"/> и <see cref="RuleHistory"/>.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        private void RuleModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rule>().ToTable("Rules", SchemaName);
            modelBuilder.Entity<Rule>().HasKey(e => e.Id);
            modelBuilder.Entity<Rule>().Property(e => e.Name).HasMaxLength(256).IsRequired();
            modelBuilder.Entity<Rule>().Property(e => e.RuleDomainId).IsRequired();
            modelBuilder.Entity<Rule>().HasRequired(x => x.RuleDomain);
            modelBuilder.Entity<Rule>().Property(e => e.Type).IsRequired();
            modelBuilder.Entity<Rule>().Property(e => e.DateTimeFrom).IsOptional();
            modelBuilder.Entity<Rule>().Property(e => e.DateTimeTo).IsOptional();
            modelBuilder.Entity<Rule>().Property(e => e.Status).IsRequired();
            modelBuilder.Entity<Rule>().Property(e => e.IsExclusive).IsRequired();
            modelBuilder.Entity<Rule>().Property(e => e.IsNotExcludedBy).IsRequired();
            modelBuilder.Entity<Rule>().Property(e => e.Priority).IsRequired();
            modelBuilder.Entity<Rule>().Property(e => e.Predicate).HasColumnType("xml").IsOptional().IsMaxLength();
            modelBuilder.Entity<Rule>().Property(e => e.Factor).HasPrecision(18, 5).IsRequired();
            modelBuilder.Entity<Rule>().Property(e => e.ConditionalFactors).HasColumnType("xml").IsOptional();
            modelBuilder.Entity<Rule>().Property(e => e.Approved);
            modelBuilder.Entity<Rule>().Property(e => e.ApproveDescription).IsOptional();

            this.CommonModelProperty<Rule>(modelBuilder);

            modelBuilder.Entity<RuleHistory>().ToTable("RuleHistories", SchemaName);
            modelBuilder.Entity<RuleHistory>().Property(e => e.RuleId);
            modelBuilder.Entity<RuleHistory>().Property(e => e.Name).HasMaxLength(256).IsRequired();
            modelBuilder.Entity<RuleHistory>().Property(e => e.RuleDomainId);
            modelBuilder.Entity<RuleHistory>().Property(e => e.Type);
            modelBuilder.Entity<RuleHistory>().Property(e => e.DateTimeFrom);
            modelBuilder.Entity<RuleHistory>().Property(e => e.DateTimeTo);
            modelBuilder.Entity<RuleHistory>().Property(e => e.Status).IsRequired();
            modelBuilder.Entity<RuleHistory>().Property(e => e.IsExclusive);
            modelBuilder.Entity<RuleHistory>().Property(e => e.IsNotExcludedBy);
            modelBuilder.Entity<RuleHistory>().Property(e => e.Priority);
            modelBuilder.Entity<RuleHistory>().Property(e => e.Predicate).HasColumnType("xml");
            modelBuilder.Entity<RuleHistory>().Property(e => e.Factor).HasPrecision(18, 5);
            modelBuilder.Entity<RuleHistory>().Property(e => e.ConditionalFactors).HasColumnType("xml");
            modelBuilder.Entity<RuleHistory>().Property(e => e.Approved);
            modelBuilder.Entity<RuleHistory>().Property(e => e.ApproveDescription).IsOptional();
            this.CommonHistoryModelProperty<RuleHistory>(modelBuilder);
        }

        /// <summary>
        /// Выполняет маппинг сущностей <see cref="RuleDomain"/> и <see cref="RuleDomainHistory"/>.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        private void RuleDomainModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RuleDomain>().ToTable("RuleDomains", SchemaName);
            modelBuilder.Entity<RuleDomain>().HasKey(e => e.Id);
            modelBuilder.Entity<RuleDomain>().Property(e => e.Name).IsRequired().HasMaxLength(150);
            modelBuilder.Entity<RuleDomain>().Property(e => e.Description).IsRequired().IsMaxLength();
            modelBuilder.Entity<RuleDomain>().Property(e => e.Metadata).HasColumnType("xml").IsOptional().IsMaxLength();
            modelBuilder.Entity<RuleDomain>().Property(e => e.LimitType).IsRequired();
            modelBuilder.Entity<RuleDomain>().Property(e => e.LimitFactor).IsRequired();
            modelBuilder.Entity<RuleDomain>().Property(e => e.StopLimitType).IsRequired();
            modelBuilder.Entity<RuleDomain>().Property(e => e.StopLimitFactor).IsRequired();
            modelBuilder.Entity<RuleDomain>().Property(e => e.DefaultBaseAdditionFactor).IsRequired();
            modelBuilder.Entity<RuleDomain>().Property(e => e.DefaultBaseMultiplicationFactor).IsRequired();
            modelBuilder.Entity<RuleDomain>().HasMany(e => e.Rules).WithRequired(x => x.RuleDomain);
            this.CommonModelProperty<RuleDomain>(modelBuilder);

            modelBuilder.Entity<RuleDomainHistory>().ToTable("RuleDomainHistories", SchemaName);
            modelBuilder.Entity<RuleDomainHistory>().Property(e => e.RuleDomainId);
            modelBuilder.Entity<RuleDomainHistory>().Property(e => e.Name).IsRequired().HasMaxLength(150);
            modelBuilder.Entity<RuleDomainHistory>().Property(e => e.Description).IsRequired().IsMaxLength();
            modelBuilder.Entity<RuleDomainHistory>().Property(e => e.Metadata).HasColumnType("xml").IsOptional().IsMaxLength();
            modelBuilder.Entity<RuleDomainHistory>().Property(e => e.LimitType).IsRequired();
            modelBuilder.Entity<RuleDomainHistory>().Property(e => e.LimitFactor).IsRequired();
            modelBuilder.Entity<RuleDomainHistory>().Property(e => e.StopLimitType).IsRequired();
            modelBuilder.Entity<RuleDomainHistory>().Property(e => e.StopLimitFactor).IsRequired();
            modelBuilder.Entity<RuleDomainHistory>().Property(e => e.DefaultBaseAdditionFactor).IsRequired();
            modelBuilder.Entity<RuleDomainHistory>().Property(e => e.DefaultBaseMultiplicationFactor).IsRequired();
            this.CommonHistoryModelProperty<RuleDomainHistory>(modelBuilder);
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
            modelBuilder.Entity<TargetAudience>().Property(e => e.CreateUserId).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<TargetAudience>().Property(e => e.UpdateDateTime).IsOptional();
            modelBuilder.Entity<TargetAudience>().Property(e => e.UpdateDateTimeUtc).IsOptional();
            modelBuilder.Entity<TargetAudience>().Property(e => e.UpdateUserId).IsOptional().HasMaxLength(50);

            modelBuilder.Entity<TargetAudienceHistory>().ToTable("TargetAudienceHistories", SchemaName);
            modelBuilder.Entity<TargetAudienceHistory>().HasKey(e => e.HistoryId);
            modelBuilder.Entity<TargetAudienceHistory>().Property(e => (int)e.Event).IsRequired();
            modelBuilder.Entity<TargetAudienceHistory>().Property(e => e.TargetAudienceId).IsRequired();
            modelBuilder.Entity<TargetAudienceHistory>().Property(e => e.Name).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<TargetAudienceHistory>().Property(e => e.CreateDateTime).IsRequired();
            modelBuilder.Entity<TargetAudienceHistory>().Property(e => e.CreateDateTimeUtc).IsRequired();
            modelBuilder.Entity<TargetAudienceHistory>().Property(e => e.CreateUserId).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<TargetAudienceHistory>().Property(e => e.UpdateDateTime).IsOptional();
            modelBuilder.Entity<TargetAudienceHistory>().Property(e => e.UpdateDateTimeUtc).IsOptional();
            modelBuilder.Entity<TargetAudienceHistory>().Property(e => e.UpdateUserId).IsOptional().HasMaxLength(50);
            modelBuilder.Entity<TargetAudienceHistory>().Property(e => e.DeleteDateTime).IsOptional();
            modelBuilder.Entity<TargetAudienceHistory>().Property(e => e.DeleteDateTimeUtc).IsOptional();
            modelBuilder.Entity<TargetAudienceHistory>().Property(e => e.DeleteUserId).IsOptional().HasMaxLength(50);
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
            modelBuilder.Entity<TargetAudienceClientLink>().Property(e => e.CreateUserId).IsRequired().HasMaxLength(50);

            modelBuilder.Entity<TargetAudienceClientLinkHistory>().ToTable("TargetAudienceClientLinkHistories", SchemaName);
            modelBuilder.Entity<TargetAudienceClientLinkHistory>().HasKey(e => e.HistoryId);
            modelBuilder.Entity<TargetAudienceClientLinkHistory>().Property(e => (int)e.Event).IsRequired();
            modelBuilder.Entity<TargetAudienceClientLinkHistory>().Property(e => e.TargetAudienceId).IsRequired();
            modelBuilder.Entity<TargetAudienceClientLinkHistory>().Property(e => e.ClientId).IsRequired();
            modelBuilder.Entity<TargetAudienceClientLinkHistory>().Property(e => e.CreateDateTime).IsRequired();
            modelBuilder.Entity<TargetAudienceClientLinkHistory>().Property(e => e.CreateDateTimeUtc).IsRequired();
            modelBuilder.Entity<TargetAudienceClientLinkHistory>().Property(e => e.CreateUserId).IsRequired();
            modelBuilder.Entity<TargetAudienceClientLinkHistory>().Property(e => e.DeleteDateTime).IsOptional();
            modelBuilder.Entity<TargetAudienceClientLinkHistory>().Property(e => e.DeleteDateTimeUtc).IsOptional();
            modelBuilder.Entity<TargetAudienceClientLinkHistory>().Property(e => e.DeleteUserId).IsOptional();
        }

        /// <summary>
        /// Выполняет маппинг общих свойств бизнес сущностей.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        /// <typeparam name="TEntity">
        /// Тип бизнес сущности
        /// </typeparam>
        private void CommonModelProperty<TEntity>(DbModelBuilder modelBuilder) where TEntity : BaseEntity
        {
            modelBuilder.Entity<TEntity>().Property(e => e.UpdatedUserId).HasColumnName("UserId").IsRequired().HasMaxLength(64);
            modelBuilder.Entity<TEntity>().Property(e => e.InsertedDate).IsRequired();
            modelBuilder.Entity<TEntity>().Property(e => e.UpdatedDate).IsOptional();
        }

        /// <summary>
        /// Выполняет маппинг общих свойств исторических сущностей.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        /// <typeparam name="TEntity">
        /// Тип исторической сущности
        /// </typeparam>
        private void CommonHistoryModelProperty<TEntity>(DbModelBuilder modelBuilder) where TEntity : BaseHistoryEntity
        {
            modelBuilder.Entity<TEntity>().HasKey(e => e.HistoryId);
            modelBuilder.Entity<TEntity>().Property(e => e.Event).IsRequired();
            this.CommonModelProperty<TEntity>(modelBuilder);
        }
    }
}