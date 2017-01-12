namespace RapidSoft.Loaylty.ProductCatalog.DataSources
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;

    using API.Entities;

    using API.InputParameters;

    using API.OutputResults;

    using Entities;

    using Extensions;

    using Interfaces;

    using PromoAction.WsClients.MechanicsService;

    using Repositories;

    using Services;

    public class ProductCategoriesDataSource : IProductCategoriesDataSource
    {
        private static AdoMapper<ProductCategory> mapper;

        private readonly AdoMapperColumn[] columns = new[]
            {
                NewColumn("Id", typeof(Guid)), 
                NewColumn("Type", typeof(int)), 
                NewColumn("ParentId", typeof(Guid?)), 
                NewColumn("Name", typeof(string)), 
                NewColumn("NamePath", typeof(string)), 
                NewColumn("Status", typeof(int)), 
                NewColumn("InsertedUserId", typeof(string)), 
                NewColumn("UpdatedUserId", typeof(string)), 
                NewColumn("OnlineCategoryUrl", typeof(string)), 
                NewColumn("NotifyOrderStatusUrl", typeof(string)), 
                NewColumn("OnlineCategoryPartnerId", typeof(int)), 
                NewColumn("InsertedDate", typeof(DateTime)), 
                NewColumn("UpdatedDate", typeof(DateTime))
            };

        private readonly IDeliveryRatesRepository deliveryRatesRepository;

        public ProductCategoriesDataSource()
        {
            mapper = new AdoMapper<ProductCategory>(columns);
            deliveryRatesRepository = new DeliveryRatesRepository();
        }

        public static string CreatePath(string parentNamePath, string categoryName)
        {
            return parentNamePath == null ? "/" + categoryName + "/" : parentNamePath + categoryName + "/";
        }

        public static ProductCategory GetProductCategoryByNamePath(string namePath)
        {
            var res = GetProductCategoriesByNamePaths(new[]
            {
                namePath
            });

            if (res == null || res.Length == 0)
            {
                return null;
            }

            return res[0];
        }

        public static ProductCategory[] GetProductCategoriesByNamePaths(IEnumerable<string> namePats)
        {
            var res = new List<ProductCategory>();
            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();

                string queryText =
                    string.Format(
@"SELECT {0}
  FROM [prod].[ProductCategories]
where 
    NamePath in (select value from dbo.ParamParserString(@Names,','))",
                        mapper.GetCommaSeparatedColumnNames());
                using (var sqlCmd = new SqlCommand(queryText, conn))
                {
                    sqlCmd.AddParameter("Names", string.Join(",", namePats));
                    using (var reader = sqlCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var entity = MapProductCategoryFromReader(reader);
                            res.Add(entity);
                        }
                    }
                }
            }

            return res.ToArray();
        }

        public ProductCategorySearchResult GetPublicCategories(GenerateResult mechanicsSql, string locationCode, ProductCategoryStatuses? categoriesStatus = null, int? parentId = null, int? nestingLevel = null, int? countToTake = null, int? countToSkip = null, bool? calcTotalCount = null, string audienceIds = null, bool includeParent = false, ProductCategoryTypes? type = null, int[] categoriesIds = null)
        {
            var categories = new List<ProductCategory>();
            int? totalCount = null;
            int childrenCount = 0;
            var activePartnerIds = string.Join(",", deliveryRatesRepository.GetDeliveringPartnerIds(locationCode));

            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();

                using (var sqlCmd = conn.CreateCommand())
                {
                    const string QueryText = "prod.GetCategories";
                    sqlCmd.CommandText = QueryText;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    
                    sqlCmd.AddParameter("status", categoriesStatus);
                    sqlCmd.AddParameter("parentId", parentId);
                    sqlCmd.AddParameter("nestingLevel", nestingLevel);
                    sqlCmd.AddParameter("countToTake", countToTake);
                    sqlCmd.AddParameter("countToSkip", countToSkip);
                    sqlCmd.AddParameter("calcTotalCount", calcTotalCount);
                    sqlCmd.AddParameter("includeParent", includeParent);
                    sqlCmd.AddParameter("activePartnerIds", activePartnerIds);
                    sqlCmd.AddParameter("targetAudiencesIds", audienceIds != null ? audienceIds.Replace(';', ',') : null);
                    sqlCmd.AddParameter("type", type);
                    sqlCmd.AddParameter("contextKey", ContextKeyProvider.GetContextKey(locationCode, audienceIds));

                    sqlCmd.AddParameter("baseSql", mechanicsSql.BaseSql);
                    sqlCmd.AddParameter("actionSql", mechanicsSql.ActionSql);

                    if (categoriesIds != null)
                    {
                        sqlCmd.AddParameter("catIds", PrepareParamValue(categoriesIds.Select(x => x.ToString(CultureInfo.InvariantCulture))));
                    }

                    var totalCountParam = sqlCmd.AddOutParameter("totalCount", SqlDbType.Int, true);
                    var childrenCountParam = sqlCmd.AddOutParameter("childrenCount", SqlDbType.Int);

                    using (var reader = sqlCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var category = MapProductCategoryFromReader(reader);
                            category.ProductsCount = reader.GetInt32("ProductsCount");
                            category.SubCategoriesCount = reader.GetInt32("SubCategoriesCount");
                            category.NestingLevel = reader.GetInt32("NestingLevel");
                            category.OnlineCategoryPartnerId = reader.GetInt32OrNull("OnlineCategoryPartnerId");
                            categories.Add(category);
                        }
                    }

                    totalCount = totalCountParam.Value as int?;
                    childrenCount = childrenCountParam.Value as int? ?? default(int);
                }
            }

            return
                new ProductCategorySearchResult
                {
                    Categories = categories.ToArray(), 
                    TotalCount = totalCount, 
                    ChildrenCount = childrenCount
                };
        }

        public ProductCategorySearchResult AdminGetCategories(
            ProductCategoryStatuses? categoriesStatus = null,
            int? parentId = null,
            int? nestingLevel = null,
            int? countToTake = null,
            int? countToSkip = null,
            bool? calcTotalCount = null,
            bool includeParent = false,
            ProductCategoryTypes? type = null)
        {
            var categories = new List<ProductCategory>();
            int? totalCount;
            int childrenCount;

            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();

                using (var sqlCmd = conn.CreateCommand())
                {
                    const string QueryText = "prod.AdminGetCategories";
                    sqlCmd.CommandText = QueryText;
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    sqlCmd.AddParameter("status", categoriesStatus);
                    sqlCmd.AddParameter("parentId", parentId);
                    sqlCmd.AddParameter("nestingLevel", nestingLevel);
                    sqlCmd.AddParameter("countToTake", countToTake);
                    sqlCmd.AddParameter("countToSkip", countToSkip);
                    sqlCmd.AddParameter("calcTotalCount", calcTotalCount);
                    sqlCmd.AddParameter("includeParent", includeParent);
                    sqlCmd.AddParameter("type", type);

                    var totalCountParam = sqlCmd.AddOutParameter("totalCount", SqlDbType.Int, true);
                    var childrenCountParam = sqlCmd.AddOutParameter("childrenCount", SqlDbType.Int);

                    using (var reader = sqlCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var category = MapProductCategoryFromReader(reader);
                            category.ProductsCount = reader.GetInt32("ProductsCount");
                            category.SubCategoriesCount = reader.GetInt32("SubCategoriesCount");
                            category.NestingLevel = reader.GetInt32("NestingLevel");
                            category.OnlineCategoryPartnerId = reader.GetInt32OrNull("OnlineCategoryPartnerId");
                            categories.Add(category);
                        }
                    }

                    totalCount = totalCountParam.Value as int?;
                    childrenCount = childrenCountParam.Value as int? ?? default(int);
                }
            }

            return ProductCategorySearchResult.Build(categories, totalCount, childrenCount);
        }

        public ProductCategory[] GetParentCategoriesPath(int categoryId)
        {
            var categories = new List<ProductCategory>();

            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();

                using (var sqlCmd = new SqlCommand("prod.GetParentCategoriesPath", conn))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    sqlCmd.AddParameter("categoryId", categoryId);
                    using (var reader = sqlCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var category = MapProductCategoryFromReader(reader);
                            categories.Add(category);
                        }
                    }
                }
            }

            return categories.ToArray();
        }

        public ProductCategory CreateProductCategory(CreateCategoryParameters parameters)
        {
            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();

                string path;

                if (parameters.ParentCategoryId.HasValue)
                {
                    var parent = GetProductCategoryById(parameters.ParentCategoryId.Value);

                    if (parent == null)
                    {
                        const string Mess = "Родительская категория не найдена";
                        throw new OperationException(ResultCodes.PARENT_CATEGORY_NOT_FOUND, Mess);
                    }

                    if (parent.Type == ProductCategoryTypes.Online)
                    {
                        string mess = string.Format(
                            "Не возможно создать подкатегорию в динамической категории {0}", 
                            parent.Id);
                        throw new OperationException(ResultCodes.PARENT_CATEGORY_IS_DYNAMIC, mess);
                    }

                    path = CreatePath(parent.NamePath, parameters.Name);
                }
                else
                {
                    path = CreatePath(null, parameters.Name);
                    path = "/" + parameters.Name + "/";
                }

                var exists = GetProductCategoryByNamePath(path);

                if (exists != null)
                {
                    var mess = string.Format("Категория {0} уже существует", path);
                    throw new OperationException(ResultCodes.CATEGORY_WITH_NAME_EXISTS, mess);
                }

                var maxOrderSql = "SELECT MAX(CatOrder) FROM [prod].[ProductCategories]";
                int maxOrder;

                using (var sqlCmd = new SqlCommand(maxOrderSql, conn))
                {
                    var res = sqlCmd.ExecuteScalar();
                    maxOrder = Convert.IsDBNull(res) ? 0 : Convert.ToInt32(res);
                }

                var productCategory = new ProductCategory
                {
                    ParentId = parameters.ParentCategoryId, 
                    Name = parameters.Name, 
                    NamePath = path, 
                    Status = parameters.Status, 
                    InsertedUserId = parameters.UserId, 
                    Type = parameters.Type, 
                    OnlineCategoryUrl = parameters.OnlineCategoryUrl, 
                    NotifyOrderStatusUrl = parameters.NotifyOrderStatusUrl,
                    OnlineCategoryPartnerId = parameters.OnlineCategoryPartnerId,
                    InsertedDate = DateTime.Now,
                    CatOrder = ++maxOrder
                };

                var queryText = string.Format(@"

INSERT INTO [prod].[ProductCategories]
    ([ParentId],[Name],[NamePath],[Status],[InsertedUserId], Type, OnlineCategoryUrl, NotifyOrderStatusUrl, OnlineCategoryPartnerId, InsertedDate,[CatOrder])
  VALUES
    (@parentId,@Name,@NamePath,@Status,@InsertedUserId,@Type,@OnlineCategoryUrl,@NotifyOrderStatusUrl, @OnlineCategoryPartnerId, @InsertedDate,@CatOrder)
select @@IDENTITY as [identity], [prod].[GetCategoryNestingLevel] (@@IDENTITY, null) as NestingLevel

");
                using (var sqlCmd = new SqlCommand(queryText, conn))
                {
                    sqlCmd.AddParameter("parentId", productCategory.ParentId.ToDBValue());
                    sqlCmd.AddParameter("Name", productCategory.Name);
                    sqlCmd.AddParameter("NamePath", productCategory.NamePath);
                    sqlCmd.AddParameter("Status", (int)productCategory.Status);
                    sqlCmd.AddParameter("InsertedUserId", productCategory.InsertedUserId);
                    sqlCmd.AddParameter("Type", (int)productCategory.Type);
                    sqlCmd.AddParameter("OnlineCategoryUrl", productCategory.OnlineCategoryUrl.ToDBValue());
                    sqlCmd.AddParameter("NotifyOrderStatusUrl", productCategory.NotifyOrderStatusUrl.ToDBValue());
                    sqlCmd.AddParameter("OnlineCategoryPartnerId", productCategory.OnlineCategoryPartnerId.ToDBValue());
                    sqlCmd.AddParameter("InsertedDate", productCategory.InsertedDate);
                    sqlCmd.AddParameter("CatOrder", productCategory.CatOrder);

                    conn.SetUserContext(parameters.UserId);

                    using (var reader = sqlCmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            return null;
                        }

                        productCategory.Id = Convert.ToInt32(reader.GetValue(0));
                        productCategory.NestingLevel = Convert.ToInt32(reader.GetValue(1));
                         
                        return productCategory;
                    }
                }
            }
        }

        public ProductCategory UpdateCategory(UpdateCategoryParameters parameters)
        {
            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();
                conn.SetUserContext(parameters.UserId);

                var entity = GetProductCategoryById(parameters.CategoryId);

                if (entity == null)
                {
                    var mess = string.Format("Категория с идентификатором {0} не найдена", parameters.CategoryId);
                    throw new OperationException(ResultCodes.NOT_FOUND, mess);
                }

                if (entity.Type == ProductCategoryTypes.Online && string.IsNullOrEmpty(parameters.NewOnlineCategoryUrl))
                {
                    throw new ArgumentException("OnlineCategoryUrl должен быть заполнен для динамической категории");
                }

                string newNamePath = null;

                if (entity.Name != parameters.NewName)
                {
                    var newParent = entity.ParentId.HasValue ? this.GetProductCategoryById(entity.ParentId.Value) : null;
                    newNamePath = CreatePath(newParent == null ? null : newParent.NamePath, parameters.NewName);
                    var existedPathCat = GetProductCategoryByNamePath(newNamePath);

                    if (existedPathCat != null)
                    {
                        var mess = string.Format("Категория {0} уже существует", newNamePath);
                        throw new OperationException(ResultCodes.CATEGORY_WITH_NAME_EXISTS, mess);
                    }

                    new ProductCategoryRepository().UpdateChildrenNamePaths(parameters.UserId, entity.Id, entity.Name, parameters.NewName);
                }

                entity.Name = parameters.NewName;
                entity.NamePath = string.IsNullOrEmpty(newNamePath) ? entity.NamePath : newNamePath;
                entity.Status = parameters.NewStatus;
                entity.UpdatedUserId = parameters.UserId;
                entity.OnlineCategoryUrl = parameters.NewOnlineCategoryUrl;
                entity.NotifyOrderStatusUrl = parameters.NewNotifyOrderStatusUrl;
                entity.OnlineCategoryPartnerId = parameters.OnlineCategoryPartnerId;
                entity.UpdatedDate = DateTime.Now;

                var queryText = string.Format(@"
UPDATE [prod].[ProductCategories]
SET 
    [Name] = @Name, 
    [NamePath] = @NamePath, 
    Status = @Status,
    UpdatedUserId = @UpdatedUserId,
    OnlineCategoryUrl = @OnlineCategoryUrl,
    NotifyOrderStatusUrl = @NotifyOrderStatusUrl,
    OnlineCategoryPartnerId = @OnlineCategoryPartnerId,
    UpdatedDate = getdate()
WHERE [Id] = @Id
");
                using (var sqlCmd = new SqlCommand(queryText, conn))
                {
                    sqlCmd.AddParameter("Id", entity.Id);
                    sqlCmd.AddParameter("Name", entity.Name);
                    sqlCmd.AddParameter("NamePath", entity.NamePath);
                    sqlCmd.AddParameter("Status", entity.Status);
                    sqlCmd.AddParameter("UpdatedUserId", entity.UpdatedUserId);
                    sqlCmd.AddParameter("OnlineCategoryUrl", entity.OnlineCategoryUrl.ToDBValue());
                    sqlCmd.AddParameter("NotifyOrderStatusUrl", entity.NotifyOrderStatusUrl.ToDBValue());
                    sqlCmd.AddParameter("OnlineCategoryPartnerId", entity.OnlineCategoryPartnerId.ToDBValue());
                    sqlCmd.AddParameter("UpdatedDate", entity.UpdatedDate);

                    sqlCmd.ExecuteNonQuery();
                }

                return entity;
            }
        }

        public ProductCategory GetProductCategoryById(int id)
        {
            var productCategoriesByIds = GetProductCategoriesByIds(new[]
            {
                id
            });

            if (productCategoriesByIds == null || productCategoriesByIds.Length == 0)
            {
                return null;
            }

            return productCategoriesByIds[0];
        }

        public ProductCategory[] GetProductCategoriesByIds(IEnumerable<int> existedCategoryIds)
        {
            var res = new List<ProductCategory>();
            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();

                string queryText =
                    string.Format(
@"SELECT {0}
    FROM [prod].[ProductCategories]
where 
    Id in (select value from dbo.ParamParserString(@Ids,','))", 
                        mapper.GetCommaSeparatedColumnNames());
                using (var sqlCmd = new SqlCommand(queryText, conn))
                {
                    sqlCmd.AddParameter("Ids", string.Join(",", existedCategoryIds));
                    using (var reader = sqlCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var entity = MapProductCategoryFromReader(reader);
                            res.Add(entity);
                        }
                    }
                }
            }

            return res.ToArray();
        }
        
        public void InsertPartnerProductCategories(PartnerProductCategory[] newProductCategories, string tableKey)
        {
            foreach (var productCategory in newProductCategories)
            {
                var cmdText = string.Format(
@"
INSERT INTO [prod].[PartnerProductCaterories_{0}]
           ([Id]
           ,[PartnerId]
           ,[ParentId]
           ,[Name]
           ,[PartnerCategoryId]
            ,Status)
     VALUES
           (@Id
           ,@PartnerId
           ,@ParentId
           ,@Name
           ,@PartnerCategoryId
           ,@Status)
", 
 tableKey);
                using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
                {
                    conn.Open();

                    using (var sqlCmd = new SqlCommand(cmdText, conn))
                    {
                        sqlCmd.AddParameter("Id", productCategory.Id);
                        sqlCmd.AddParameter("ParentId", productCategory.ParentId.ToDBValue());
                        sqlCmd.AddParameter("PartnerId", productCategory.PartnerId);
                        sqlCmd.AddParameter("Name", productCategory.Name.ToDBValue());
                        sqlCmd.AddParameter("PartnerCategoryId", DBNull.Value);
                        sqlCmd.AddParameter("Status", (int)productCategory.Status);

                        sqlCmd.ExecuteNonQuery();
                    }
                }
            }
        }
        
        public void CreatePartnerProductCateroriesTable(string tableKey)
        {
            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();
                var cmdText = string.Format(
@"
CREATE TABLE [prod].[PartnerProductCaterories_{0}](
    [Id] [nvarchar](50) NOT NULL,
    [PartnerId] [int] NOT NULL,
    [ParentId] [nvarchar](50) NULL,
    [Name] [nvarchar](256) NOT NULL,
    [PartnerCategoryId] [int] NULL,
    [NamePath] [nvarchar](MAX) NULL,
    [Status] int NOT NULL,
CONSTRAINT [PK_PartnerProductCaterories_{0}] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

", 
 tableKey);

                conn.ExecuteNonQuery(cmdText);
            }
        }

        public void RecalculatePartnerProductCategoriesPath(string importKey)
        {
            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();
                var cmdText = string.Format(
                    @"

;WITH PartnerProductCaterories 
([Id], [PartnerId], [Name], [ParentId], [NamePathCalc])
AS 
(
    SELECT [Id], [PartnerId], [Name], [ParentId], CAST('/' + [Name] + '/' AS nvarchar(max))
    FROM [prod].[PartnerProductCaterories_{0}] L1
    WHERE L1.[ParentId] IS NULL
        UNION ALL
    SELECT L2.[Id], L2.[PartnerId], L2.[Name], L2.[ParentId], CAST(parent.[NamePathCalc] + L2.[Name] + '/' AS nvarchar(max))
    FROM [prod].[PartnerProductCaterories_{0}] L2
    INNER JOIN PartnerProductCaterories parent 
        ON L2.[ParentId] = parent.[Id] AND L2.[PartnerId] = parent.[PartnerId]
)
UPDATE cats 
SET cats.[NamePath] = vw.[NamePathCalc] 
FROM [prod].[PartnerProductCaterories_{0}] cats
JOIN PartnerProductCaterories vw
ON vw.[Id] = cats.[Id]

", 
                    importKey);

                conn.ExecuteNonQuery(cmdText);
            }
        }
        
        private static AdoMapperColumn NewColumn(string columnName, Type dotNetType)
        {
            return new AdoMapperColumn { ColumnName = columnName, DotNetType = dotNetType, };
        }

        private static string PrepareParamValue(IEnumerable<string> values)
        {
            if (values == null)
            {
                return null;
            }

            var escaped = values.Select(x => x.Replace(",", "\\,"));
            return string.Join(",", escaped);
        }

        private static ProductCategory MapProductCategoryFromReader(SqlDataReader reader)
        {
            return mapper.GetEntity(reader);
        }
    }
}