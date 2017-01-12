namespace RapidSoft.Loaylty.ProductCatalog.DataSources
{
    using System;
    using System.Data;
    using System.Data.Entity;
    using System.Text;

    using API.Entities;
    using API.OutputResults;
    using Entities;
    using RapidSoft.Loaylty.Logging;

    using PartnerProductCategoryLink = Entities.PartnerProductCategoryLink;
    using WishListItem = API.Entities.WishListItem;

    public class LoyaltyDBEntities : DbContext
    {
        private readonly ILog log = LogManager.GetLogger(typeof(LoyaltyDBEntities));

        static LoyaltyDBEntities()
        {
            Database.SetInitializer<LoyaltyDBEntities>(null);
        }

        public LoyaltyDBEntities()
            : this(DataSourceConfig.ConnectionString)
        {
        }

        public LoyaltyDBEntities(string connectionString)
            : base(connectionString)
        {
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<WishListItem> WishListItems { get; set; }

        public DbSet<BasketItem> BasketItems { get; set; }

        [Obsolete]
        public DbSet<ProductViewStatistic> ProductViewStatistics { get; set; }

        public DbSet<ProductViewsByDay> ProductViewsByDays { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<Partner> Partners { get; set; }

        public DbSet<PartnerSettings> PartnerSettings { get; set; }

        public DbSet<PartnerProductCatalog> PartnerProductCatalogs { get; set; }

        public DbSet<PartnerProductCategoryLink> PartnerProductCategoryLink { get; set; }

        public DbSet<CategoryPermission> CategoryPermissions { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<ProductImportTask> ProductImportTasks { get; set; }

        public DbSet<ProductSortProjection> ProductSortProjections { get; set; }

        public DbSet<PopularProduct> PopularProducts { get; set; }

        public DbSet<ProductTargetAudience> ProductTargetAudiences { get; set; }

        public DbSet<OrderStatusWorkFlowItem> OrderStatusWorkFlowItems { get; set; }

        public DbSet<WishListItemNotification> WishListItemNotifications { get; set; }

        public DbSet<DeliveryLocation> DeliveryBindings { get; set; }

        public DbSet<DeliveryLocationHistory> DeliveryBindingHistory { get; set; }

        public DbSet<DeliveryRate> DeliveryRates { get; set; }

        public DbSet<OrdersNotificationsEmail> OrdersNotificationsEmails { get; set; }

        public DbSet<DeactivatedCategory> DeactivatedCategories { get; set; }

        public DbSet<ProductFixBasePriceDate> ProductsFixBasePriceDates { get; set; }

        public string UserId { get; set; }

        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            SetUserContext();
            
            using (var cmd = this.Database.Connection.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteNonQuery();
            }
        }

        public override int SaveChanges()
        {
            try
            {
                SetUserContext();
                return base.SaveChanges();
            }
            catch (Exception e)
            {
                if (e is System.Data.Entity.Validation.DbEntityValidationException)
                {
                    var ex = e as System.Data.Entity.Validation.DbEntityValidationException;
                    foreach (var entityValidationResult in ex.EntityValidationErrors)
                    {
                        var errors = new StringBuilder();
                        foreach (var validationError in entityValidationResult.ValidationErrors)
                        {
                            errors.Append(validationError.PropertyName).Append(": ").Append(validationError.ErrorMessage).Append("; ");
                        }

                        var typeName = entityValidationResult.Entry.Entity.GetType().Name;
                        log.Error(typeName + ": " + errors);
                    }
                }
                else
                {
                    log.Error("Ошибка работы с БД: " + e.Message, e);
                }

                throw;
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            OnWishListItemModelCreating(modelBuilder);
            OnBasketItemModelCreating(modelBuilder);
            OnProductViewStatisticModelCreating(modelBuilder);
            OnProductViewsByDayModelCreating(modelBuilder);
            OnProductCategoriesModelCreating(modelBuilder);
            OnPartnerCreating(modelBuilder);
            OnPartnerProductCatalogsCreating(modelBuilder);
            OnPartnerProductCategoryLinkModelCreating(modelBuilder);
            OnOrdersModelCreating(modelBuilder);
            OnCategoryPermissionModelCreating(modelBuilder);
            OnProductSortProjectionModelCreating(modelBuilder);
            OnProductCatalogLoadTaskModelCreating(modelBuilder);
            OnPopularProductModelCreating(modelBuilder);
            OnProductTargetAudiencesModelCreating(modelBuilder);
            OnOrderStatusWorkFlowItemModelCreating(modelBuilder);
            OnWishListItemNotificationModelCreating(modelBuilder);
            OnPartnerSettingsModelCreating(modelBuilder);
            OnDeliveryBindingsModelCreating(modelBuilder);
            OnDeliveryBindingHistoryModelCreating(modelBuilder);
            OnDeliveryRateModelCreating(modelBuilder);
            OnOrdersNotificationsEmailModelCreating(modelBuilder);
            OnDeactivatedCategoryModelCreating(modelBuilder);
            OnProductFixBasePriceDateModelCreating(modelBuilder);
        }

        private void SetUserContext()
        {
            if (string.IsNullOrWhiteSpace(UserId))
            {
                return;
            }

            if (this.Database.Connection.State != ConnectionState.Open)
            {
                this.Database.Connection.Open();
            }

            using (var cmd = this.Database.Connection.CreateCommand())
            {
                var parm = cmd.CreateParameter();
                parm.ParameterName = "@userName";
                parm.Value = UserId;

                cmd.CommandText = "prod.SetUserContext";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(parm);

                cmd.ExecuteNonQuery();
            }
        }

        private void OnDeliveryRateModelCreating(DbModelBuilder modelBuilder)
        {
            var model = modelBuilder.Entity<DeliveryRate>();

            model.ToTable("DeliveryRates", "prod");
            model.HasKey(e => e.Id);
            model.Property(e => e.LocationId).IsRequired();
            model.Property(e => e.MaxWeightGram).IsRequired();
            model.Property(e => e.MinWeightGram).IsRequired();
            model.Property(e => e.PriceRur).IsRequired();
            model.Property(e => e.PartnerId).IsRequired();
        }

        private void OnDeliveryBindingHistoryModelCreating(DbModelBuilder modelBuilder)
        {
            var model = modelBuilder.Entity<DeliveryLocationHistory>();

            model.ToTable("DeliveryLocationsHistoryLog", "prod");
            model.HasKey(e => new { e.TriggerDate, e.Id });
            model.Property(e => e.Action).IsRequired();
            model.Property(e => e.TriggerDate).IsRequired();
            model.Property(e => e.Id).IsRequired();
            model.Property(e => e.PartnerId).IsRequired();
            model.Property(e => e.LocationName).IsRequired().IsMaxLength();

            model.Property(e => e.UpdateUserId).IsOptional();
            model.Property(e => e.EtlSessionId).IsOptional();

            model.Property(e => e.InsertDateTime).IsRequired();
            model.Property(e => e.UpdateDateTime).IsOptional();

            model.Property(e => e.OldExternaLocationlId).IsOptional();
            model.Property(e => e.NewExternaLocationlId).IsOptional();

            model.Property(e => e.NewKladr).IsOptional();
            model.Property(e => e.OldKladr).IsOptional();

            model.Property(e => e.NewStatus).IsOptional();
            model.Property(e => e.OldStatus).IsOptional();
        }

        private void OnDeliveryBindingsModelCreating(DbModelBuilder modelBuilder)
        {
            var model = modelBuilder.Entity<DeliveryLocation>();

            model.ToTable("DeliveryLocations", "prod");
            model.HasKey(e => e.Id);
            model.Property(e => e.PartnerId).IsRequired();
            model.Property(e => e.LocationName).IsRequired().IsMaxLength();
            model.Property(e => e.ExternalLocationId).IsOptional();
            model.Property(e => e.Kladr).IsOptional().HasMaxLength(13);
            model.Property(e => e.Status).IsRequired();
            model.Property(e => e.InsertDateTime).IsRequired();
            model.Property(e => e.UpdateDateTime).IsOptional();
            model.Property(e => e.UpdateUserId).IsOptional();
            model.Property(e => e.UpdateSource).IsRequired();
        }

        private void OnPartnerSettingsModelCreating(DbModelBuilder modelBuilder)
        {
            var model = modelBuilder.Entity<PartnerSettings>();

            model.ToTable("PartnerSettings", "prod");
            model.HasKey(e => e.Id);
            model.Property(e => e.PartnerId).IsRequired();
            model.Property(e => e.Key).IsRequired();
            model.Property(e => e.Value);
        }

        private void OnWishListItemNotificationModelCreating(DbModelBuilder modelBuilder)
        {
            var model = modelBuilder.Entity<WishListItemNotification>();
            model.ToTable("WishListItemNotifications", "prod");
            model.HasKey(
                m => new
                     {
                         m.ClientId,
                         m.ProductId
                     });
            model.Ignore(m => m.ClientBalance);
            model.Ignore(m => m.LocationKladr);
            model.Ignore(m => m.Product);
        }

        private void OnOrderStatusWorkFlowItemModelCreating(DbModelBuilder modelBuilder)
        {
            var model = modelBuilder.Entity<OrderStatusWorkFlowItem>();

            model.ToTable("OrderStatusWorkFlow", "prod");
            model.HasKey(e => new { e.FromStatus, e.ToStatus });
            model.Property(e => e.FromStatus).IsRequired();
            model.Property(e => e.ToStatus).IsRequired();
        }

        private void OnProductTargetAudiencesModelCreating(DbModelBuilder modelBuilder)
        {
            var model = modelBuilder.Entity<ProductTargetAudience>();
            model.ToTable("ProductTargetAudiences", "prod");
            model.HasKey(e => new { e.ProductId, e.TargetAudienceId });
            model.Property(e => e.ProductId).HasMaxLength(256).IsRequired();
            model.Property(e => e.TargetAudienceId).HasMaxLength(256).IsRequired();
        }

        private void OnPopularProductModelCreating(DbModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<PopularProduct>();
            entity.ToTable("PopularProducts", "prod");
            entity.HasKey(e => new { e.ProductId, e.PopularType });
            entity.Ignore(e => e.Product);
            entity.Property(e => e.ProductRate).HasColumnName("PopularRate");
        }

        private void OnPartnerProductCatalogsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PartnerProductCatalog>().ToTable("PartnerProductCatalogs", "prod");
            modelBuilder.Entity<PartnerProductCatalog>()
                        .HasKey(e => e.PartnerId)
                        .Property(e => e.PartnerId)
                        .IsRequired();
        }

        private void OnProductSortProjectionModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductSortProjection>().ToTable("ProductsFromAllPartners", "prod");
            modelBuilder.Entity<ProductSortProjection>().HasKey(e => e.ProductId);
            
            modelBuilder.Entity<ProductSortProjection>().Property(e => e.ProductId).IsRequired();
            modelBuilder.Entity<ProductSortProjection>().Property(e => e.ModerationStatusCode).HasColumnName("ModerationStatus");
            modelBuilder.Entity<ProductSortProjection>().Property(e => e.StatusCode).HasColumnName("Status");
            
            modelBuilder.Entity<ProductSortProjection>().Ignore(e => e.Status);
            modelBuilder.Entity<ProductSortProjection>().Ignore(e => e.ModerationStatus);
            modelBuilder.Entity<ProductSortProjection>().Ignore(e => e.Param);
            modelBuilder.Entity<ProductSortProjection>()
                        .Property(e => e.ParamsXml)
                        .HasColumnName("Param")
                        .HasColumnType("xml");
        }

        private void OnOrdersModelCreating(DbModelBuilder modelBuilder)
        {
            var model = modelBuilder.Entity<Order>();
            
            model.ToTable("Orders", "prod");
            model.Property(o => o.DeliveryInfoXml).HasColumnName("DeliveryInfo").HasColumnType("xml");
            model.Ignore(e => e.DeliveryInfo);
            model.Property(o => o.ItemsXml).HasColumnName("Items").HasColumnType("xml");
            model.Ignore(e => e.Items);
            model.Ignore(e => e.PublicStatus);
        }

        private void OnProductCategoriesModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>().ToTable("ProductCategories", "prod");
            modelBuilder.Entity<ProductCategory>().Ignore(p => p.ProductsCount);
            modelBuilder.Entity<ProductCategory>().Ignore(p => p.SubCategories);
            modelBuilder.Entity<ProductCategory>().Ignore(p => p.SubCategoriesCount);
            modelBuilder.Entity<ProductCategory>().Ignore(p => p.NestingLevel);
        }

        private void OnProductViewStatisticModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductViewStatistic>().ToTable("ProductViewStatistics", "prod");
            modelBuilder.Entity<ProductViewStatistic>().HasKey(e => new { e.ProductId, e.ClientId });
            modelBuilder.Entity<ProductViewStatistic>().Property(e => e.ClientId).IsRequired();
            modelBuilder.Entity<ProductViewStatistic>().Property(e => e.ProductId).IsRequired();
            modelBuilder.Entity<ProductViewStatistic>().Property(e => e.ViewCount).IsRequired();
        }

        private void OnProductViewsByDayModelCreating(DbModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<ProductViewsByDay>();
            entity.ToTable("ProductViewsByDay", "prod");
            entity.HasKey(e => new { e.ViewsDate, e.ProductId });
            entity.Property(e => e.ViewsDate).IsRequired();
            entity.Property(e => e.ProductId).IsRequired().HasMaxLength(256);
            entity.Property(e => e.ViewsCount).IsRequired();
        }

        private void OnWishListItemModelCreating(DbModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<WishListItem>();
            entity.ToTable("WishListItems", "prod");
            entity.HasKey(e => new
            {
                e.ClientId,
                e.ProductId
            });
            entity.Property(e => e.ProductsQuantity).IsRequired();
        }

        private void OnBasketItemModelCreating(DbModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<BasketItem>();
            entity.ToTable("BasketItems", "prod");
            entity.HasKey(e => new
            {
                e.ClientId,
                e.ProductId
            });
            entity.Property(e => e.ClientId).IsRequired().HasMaxLength(64);
            entity.Property(e => e.ProductId).IsRequired().HasMaxLength(64);
            entity.Property(e => e.CreatedDate).IsRequired();
            entity.Property(e => e.ProductsQuantity).IsRequired();
            entity.Property(e => e.BasketItemGroupId).IsOptional();
            entity.Ignore(p => p.TotalPrice);
            entity.Ignore(p => p.TotalPriceRur);
            entity.Ignore(p => p.ItemPrice);
            entity.Ignore(p => p.Product);
            entity.Ignore(p => p.AvailabilityStatus);
        }

        private void OnPartnerCreating(DbModelBuilder modelBuilder)
        {
            var entityPartner = modelBuilder.Entity<Partner>();
            entityPartner.ToTable("Partners", "prod");
            entityPartner.HasKey(t => t.Id);
            entityPartner.Property(t => t.Description);
            entityPartner.Property(t => t.InsertedUserId).IsRequired();
            entityPartner.Property(t => t.UpdatedUserId).IsOptional();
            entityPartner.Property(t => t.Name).IsRequired();
            entityPartner.Property(t => t.Type).IsRequired();
            entityPartner.Property(t => t.Status).IsRequired();
            entityPartner.HasOptional(t => t.Carrier).WithMany().HasForeignKey(t => t.CarrierId);
            entityPartner.Ignore(t => t.Settings);
        }

        private void OnPartnerProductCategoryLinkModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PartnerProductCategoryLink>().ToTable("PartnerProductCategoryLinks", "prod");
        }

        private void OnCategoryPermissionModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryPermission>().ToTable("ProductCategoriesPermissions", "prod");
            modelBuilder.Entity<CategoryPermission>().HasKey(e => new { e.PartnerId, e.CategoryId });
            modelBuilder.Entity<CategoryPermission>().Property(e => e.InsertedUserId).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<CategoryPermission>().Property(e => e.InsertedDate).IsRequired();
        }

        private void OnProductCatalogLoadTaskModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductImportTask>().ToTable("ProductImportTasks", "prod");
            modelBuilder.Entity<ProductImportTask>().HasKey(e => e.Id);
            modelBuilder.Entity<ProductImportTask>().Property(e => e.PartnerId).IsRequired();
            modelBuilder.Entity<ProductImportTask>().Property(e => e.FileUrl).IsRequired();

            modelBuilder.Entity<ProductImportTask>().Property(e => e.Status).IsRequired();

            modelBuilder.Entity<ProductImportTask>().Property(e => e.StartDateTime).IsOptional();
            modelBuilder.Entity<ProductImportTask>().Property(e => e.EndDateTime).IsOptional();
            modelBuilder.Entity<ProductImportTask>().Property(e => e.InsertedUserId).IsRequired();
            modelBuilder.Entity<ProductImportTask>().Property(e => e.InsertedDate).IsRequired();
            modelBuilder.Entity<ProductImportTask>().Property(e => e.CountSuccess).IsRequired();
            modelBuilder.Entity<ProductImportTask>().Property(e => e.CountFail).IsRequired();
            modelBuilder.Entity<ProductImportTask>().Property(e => e.WeightProcessType).IsRequired();
        }

        private void OnOrdersNotificationsEmailModelCreating(DbModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<OrdersNotificationsEmail>();

            entity.ToTable("OrdersNotificationsEmails", "prod");
            entity.HasKey(m => m.Id);
            entity.Property(m => m.EtlSessionId).IsRequired();
            entity.Property(m => m.Recipients).IsRequired();
            entity.Property(m => m.Subject).IsRequired();
            entity.Property(m => m.Body).IsRequired();
            entity.Property(m => m.Status).IsRequired();
        }

        private void OnDeactivatedCategoryModelCreating(DbModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<DeactivatedCategory>();

            entity.ToTable("DeactivatedCategories", "prod");
        }

        private void OnProductFixBasePriceDateModelCreating(DbModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<ProductFixBasePriceDate>();

            entity.ToTable("ProductsFixedPrices", "prod");
            entity.HasKey(m => m.ProductId);
        }
    }
}