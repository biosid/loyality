namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    using API.InputParameters;
    using API.OutputResults;

    using Extensions;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces;
    using RapidSoft.Loaylty.ProductCatalog.Entities;

    internal class ProductsRepository : BaseRepository, IProductsRepository
    {
        // NOTE: Поиск данных перенесен на таблицу ProductsFromAllPartners, для компинсации проблем в Админке добавлено дублирование обновления.
        // TODO: Убрать дублированное обновление данных.
        // TODO: Перенести в Stored Procedure.
        private const string GROUPUPDATEQUERY = @"
                DECLARE @d datetime = GetDate();
                update prod.Products_{0} set {1} = @param, UpdatedDate = @d, UpdatedUserId = @updatedUserId where ProductId in (select * from [dbo].[ParamParserString] (@productIds, ','));
                update prod.ProductsFromAllPartners set {1} = @param, UpdatedDate = @d, UpdatedUserId = @updatedUserId where ProductId in (select * from [dbo].[ParamParserString] (@productIds, ','));
";
        private const string UPDATEQUERYTEMPLATE = @"
                DECLARE @d datetime = GetDate();
                update prod.Products_{0} set [Status] = @statusParam, [UpdatedDate] = @d, [UpdatedUserId] = @userIdParam where PartnerProductId in (select * from [dbo].[ParamParserString] (@productIdsParam, ','));
                update prod.ProductsFromAllPartners set [Status] = @statusParam, [UpdatedDate] = @d, [UpdatedUserId] = @userIdParam where PartnerProductId in (select * from [dbo].[ParamParserString] (@productIdsParam, ','))
";
        private const string GROUPDELETEQUERY = @"
                delete from prod.Products_{0} where ProductId in (select * from [dbo].[ParamParserString] (@productIds, ','));
                delete from prod.ProductsFromAllPartners where ProductId in (select * from [dbo].[ParamParserString] (@productIds, ','));
";

        private const string TRANSACTION = @"
BEGIN TRY
    BEGIN TRANSACTION
    {0}
    COMMIT TRANSACTION
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK
END CATCH
";
        
        private ProductCatalogsDataSource productCatalogsDataSource;

        public ProductsRepository()
            : base(DataSourceConfig.ConnectionString)
        {
        }

        public ProductsRepository(string connectionString)
            : base(connectionString)
        {
        }

        private ProductCatalogsDataSource ProductCatalogsDataSource
        {
            get
            {
                return this.productCatalogsDataSource ?? (productCatalogsDataSource = new ProductCatalogsDataSource());
            }
        }

        public ResultBase MoveProducts(MoveProductsParameters parameters)
        {
            var result = ResultBase.BuildSuccess();

            using (var ctx = this.DbNewContext(parameters.UserId))
            {
                var partnerLookup = GetProductsLookup(ctx, parameters.ProductIds);

                foreach (var partnerLook in partnerLookup)
                {
                    var activeCatalog = GetActiveCatalog(partnerLook.Key);

                    if (activeCatalog == null || string.IsNullOrEmpty(activeCatalog.Key))
                    {
                        continue;
                    }

                    var updateScript = string.Format(TRANSACTION, string.Format(GROUPUPDATEQUERY, activeCatalog.Key, "categoryId"));

                    var ids = string.Join(",", partnerLook.Select(p => p));

                    ctx.ExecuteSqlCommand(
                        updateScript,
                        new SqlParameter("param", parameters.TargetCategoryId),
                        new SqlParameter("productIds", ids),
                        new SqlParameter("updatedUserId", parameters.UserId));
                }
            }

            return result;
        }

        public ResultBase ChangeStatuses<T>(T parameters)
        {
            string paramField;
            int paramValue;
            string[] productIds;
            string userId;
           
            if (typeof(T) == typeof(ChangeStatusParameters))
            {
                var param = parameters as ChangeStatusParameters;
                paramField = "status";
                paramValue = (int)param.ProductStatus;
                productIds = param.ProductIds;
                userId = param.UserId;
            }
            else if (typeof(T) == typeof(ChangeModerationStatusParameters))
            {
                var param = parameters as ChangeModerationStatusParameters;
                paramField = "ModerationStatus";
                paramValue = (int)param.ProductModerationStatus;
                productIds = param.ProductIds;
                userId = param.UserId;
            }
            else if (typeof(T) == typeof(RecommendParameters))
            {
                var param = parameters as RecommendParameters;
                paramField = "IsRecommended";
                paramValue = param.IsRecommended ? 1 : 0;
                productIds = param.ProductIds;
                userId = param.UserId;
            }
            else
            {
                throw new ArgumentException("parameters unexpected type");
            }

            var result = ResultBase.BuildSuccess();

            using (var ctx = DbNewContext(userId))
            {
                var partnerLookup = GetProductsLookup(ctx, productIds);

                foreach (var partnerLook in partnerLookup)
                {
                    var activeCatalog = GetActiveCatalog(partnerLook.Key);

                    if (activeCatalog == null || string.IsNullOrEmpty(activeCatalog.Key))
                    {
                        continue;
                    }

                    var updateScript = string.Format(TRANSACTION, string.Format(GROUPUPDATEQUERY, activeCatalog.Key, paramField));

                    var ids = string.Join(",", partnerLook.Select(p => p));

                    var num = ctx.ExecuteSqlCommand(
                        updateScript,
                        new SqlParameter("param", paramValue),
                        new SqlParameter("productIds", ids),
                        new SqlParameter("updatedUserId", userId));
                }
            }

            return result;
        }

        public ResultBase ChangeStatusesByPartner(ChangeStatusByPartnerParameters parameters)
        {
            var activeCatalog = GetActiveCatalog(parameters.PartnerId);

            if (activeCatalog == null || string.IsNullOrEmpty(activeCatalog.Key))
            {
                return ResultBase.BuildSuccess();
            }

            var status = (int)parameters.ProductStatus;

            var productIds = string.Join(",", parameters.PartnerProductIds);

            var updateQuery = string.Format(TRANSACTION, string.Format(UPDATEQUERYTEMPLATE, activeCatalog.Key));

            using (var ctx = DbNewContext(parameters.UserId))
            {
                ctx.ExecuteSqlCommand(
                    updateQuery,
                    new SqlParameter("statusParam", status),
                    new SqlParameter("userIdParam", parameters.UserId),
                    new SqlParameter("productIdsParam", productIds));
            }

            return ResultBase.BuildSuccess();
        }

        public List<ProductSortProjection> GetAll()
        {
            using (var ctx = DbNewContext())
            {
                return ctx.ProductSortProjections.ToList();
            }
        }

        public ProductSortProjection GetById(LoyaltyDBEntities context, string productId)
        {
            return context.ProductSortProjections.FirstOrDefault(p => p.ProductId == productId);
        }

        public ProductSortProjection GetById(string productId)
        {
            using (var ctx = DbNewContext())
            {
                return GetById(ctx, productId);
            }
        }

        public void DeleteProducts(DeleteProductParameters parameters)
        {
            using (var ctx = DbNewContext(parameters.UserId))
            {
                var partnerLookup = GetProductsLookup(ctx, parameters.ProductIds);

                foreach (var partnerLook in partnerLookup)
                {
                    var activeCatalog = GetActiveCatalog(partnerLook.Key);

                    if (activeCatalog == null || string.IsNullOrEmpty(activeCatalog.Key))
                    {
                        continue;
                    }

                    var updateScript = string.Format(TRANSACTION, string.Format(GROUPDELETEQUERY, activeCatalog.Key));

                    var ids = string.Join(",", partnerLook.Select(p => p));

                    ctx.ExecuteSqlCommand(updateScript, new SqlParameter("productIds", ids));                    
                }
            }
        }

        public int GetProductPartnerId(string productId)
        {
            using (var ctx = this.DbNewContext())
            {
                var product = ctx.ProductSortProjections.FirstOrDefault(p => p.ProductId == productId);

                return product == null ? 0 : product.PartnerId;
            }
        }

        public List<ProductSortProjection> GetByPartnerId(int partnerId)
        {
            using (var ctx = this.DbNewContext())
            {
                var products = ctx.ProductSortProjections.Where(x => x.PartnerId == partnerId).ToList();
                return products;
            }
        }

        public string[] FilterByTargetAudiences(string[] productIds, string[] targetAudienceIds)
        {
            using (var ctx = DbNewContext())
            {
                var products = ctx.ProductSortProjections
                                  .Where(p => productIds.Contains(p.ProductId));

                if (targetAudienceIds == null || targetAudienceIds.Length == 0)
                {
                    return products.Where(p => !ctx.ProductTargetAudiences
                        .Any(pta => pta.ProductId == p.ProductId))
                                   .Select(p => p.ProductId)
                                   .ToArray();
                }

                return products.Where(p => !ctx.ProductTargetAudiences
                                               .Any(pta => pta.ProductId == p.ProductId) ||
                                           ctx.ProductTargetAudiences
                                              .Any(pta => pta.ProductId == p.ProductId &&
                                                          targetAudienceIds.Contains(pta.TargetAudienceId)))
                               .Select(p => p.ProductId)
                               .ToArray();
            }
        }

        public void RemoveProductTargetAudiences(string userId, string[] productIds)
        {
            using (var ctx = this.DbNewContext(userId))
            {
                var objs = ctx.ProductTargetAudiences.Where(a => productIds.Contains(a.ProductId)).ToList();
                objs.ForEach(o => ctx.ProductTargetAudiences.Remove(o));

                ctx.SaveChanges();
            }
        }

        public void AddProductTargetAudiences(string userId, string[] productIds, string[] targetAudienceIds)
        {
            var date = DateTime.Now;

            using (var ctx = this.DbNewContext(userId))
            {
                var objs = (from productId in productIds
                            from targetAudienceId in targetAudienceIds.Select(x => x.ToUpperInvariant()).Distinct()
                            select new ProductTargetAudience
                                   {
                                       InsertedUserId = userId,
                                       InsertedDate = date,
                                       ProductId = productId,
                                       TargetAudienceId = targetAudienceId
                                   }).ToList();

                objs.ForEach(o => ctx.ProductTargetAudiences.Add(o));
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Get products ids group by partner ids
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="productIds">
        /// products ids
        /// </param>
        /// <returns>
        /// partner id as key, values products ids
        /// </returns>
        private static IEnumerable<IGrouping<int, string>> GetProductsLookup(LoyaltyDBEntities context, string[] productIds)
        {
            return context.ProductSortProjections.Where(p => productIds.Contains(p.ProductId))
                       .ToLookup(k => k.PartnerId, k => k.ProductId);
        }

        private PartnerProductCatalog GetActiveCatalog(int partnerId)
        {
            return ProductCatalogsDataSource.GetActiveCatalog(partnerId);
        }
    }
}