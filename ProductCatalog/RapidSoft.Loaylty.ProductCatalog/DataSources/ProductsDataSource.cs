namespace RapidSoft.Loaylty.ProductCatalog.DataSources
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading;

    using Extensions;

    using Interfaces;

    using PromoAction.WsClients.MechanicsService;

    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.API.InputParameters;
    using RapidSoft.Loaylty.ProductCatalog.API.OutputResults;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories;
    using RapidSoft.Loaylty.ProductCatalog.Entities;
    using RapidSoft.Loaylty.ProductCatalog.Import;
    using RapidSoft.Loaylty.ProductCatalog.Services;
    using RapidSoft.Loaylty.ProductCatalog.Settings;

    using Vtb24.Common.Configuration;
    
    using ResultBase = RapidSoft.Loaylty.ProductCatalog.API.OutputResults.ResultBase;

    internal class ProductsDataSource : IProductsDataSource
    {
        public const string ProductsCreateTable =
            @"

CREATE TABLE [prod].[Products_{0}](
    {1}
 CONSTRAINT [PK_Products_{0}] PRIMARY KEY CLUSTERED 
(
    ProductId asc,
	PartnerId ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ALTER TABLE [prod].[Products_{0}]  WITH CHECK ADD  CONSTRAINT [FK_Products_{0}_ProductCategories] FOREIGN KEY([CategoryId])
REFERENCES [prod].[ProductCategories] ([Id])

ALTER TABLE [prod].[Products_{0}] CHECK CONSTRAINT [FK_Products_{0}_ProductCategories]

ALTER TABLE [prod].[Products_{0}] ADD  CONSTRAINT [DF_Products_InsertedDate_{0}]  DEFAULT (getdate()) FOR [InsertedDate]

ALTER TABLE [prod].[Products_{0}] ADD  CONSTRAINT [DF_Products_UpdatedDate_{0}]  DEFAULT (getdate()) FOR [UpdatedDate]

ALTER TABLE [prod].[Products_{0}] ADD  CONSTRAINT [DF_Products_IsRecommended_{0}]  DEFAULT 0 FOR [IsRecommended]

ALTER TABLE [prod].[Products_{0}] ADD  CONSTRAINT [DF_Products_IsDeliveredByEmail_{0}]  DEFAULT 0 FOR [IsDeliveredByEmail]

CREATE INDEX idx_prod_products_{0}_categoryId on [prod].[Products_{0}] (CategoryId); 
CREATE INDEX idx_prod_products_{0}_vendor on [prod].[Products_{0}] (Vendor); 
CREATE INDEX idx_prod_products_{0}_available on [prod].[Products_{0}] (Available) include (PartnerId, CategoryId, Weight);
CREATE INDEX idx_prod_products_{0}_status ON [prod].[Products_{0}] ([Status], [ModerationStatus]) INCLUDE ([ProductId], [CategoryId]);
CREATE INDEX idx_prod_products_{0}_partnerProductId ON [prod].[Products_{0}] ([PartnerProductId]);
CREATE INDEX idx_prod_products_{0}_price ON [prod].[Products_{0}] ([ProductId] ASC, [PartnerId] ASC, [PriceRUR] ASC, [Weight] ASC);
CREATE INDEX idx_prod_products_{0}_searchCount ON [prod].[Products_{0}] ([ModerationStatus] ASC) INCLUDE ([ProductId], [PartnerId], [InsertedDate], [CategoryId], [Status]);

";

        public const string ProductsCreateTableConstraints =
    @"
ALTER TABLE [prod].[Products_{0}]  WITH CHECK ADD  CONSTRAINT [CK_Products_{0}] CHECK  (([PartnerId]=({1})))
ALTER TABLE [prod].[Products_{0}] CHECK CONSTRAINT [CK_Products_{0}]
";

        public static readonly AdoMapperColumn[] ProductColumns = new[]
        {
            NewColumn("ProductId", typeof(string), "[ProductId] [nvarchar](256) NOT NULL,", 256, true, isSystem: true), 
            NewColumn("PartnerId", typeof(int), "[PartnerId] [int] NOT NULL,", isSystem: true), 
            NewColumn("InsertedDate", typeof(DateTime), "[InsertedDate] [datetime] NOT NULL,", isSystem: true), 
            NewColumn("UpdatedDate", typeof(DateTime?), "[UpdatedDate] [datetime] NULL,", isSystem: true), 
            NewColumn("PartnerProductId", typeof(string), "[PartnerProductId] [nvarchar](256) NOT NULL,", 256), 
            NewColumn("Type", typeof(string), "[Type] [nvarchar](20) NULL,", 20), 
            NewColumn("Bid", typeof(int), "[Bid] [int] NULL,"), 
            NewColumn("CBid", typeof(int), "[CBid] [int] NULL,"), 
            NewColumn("Available", typeof(bool), "[Available] [bit] NULL,"), 
            NewColumn("Name", typeof(string), "[Name] [nvarchar](256) NULL,", 256), 
            NewColumn("Url", typeof(string), "[Url] [nvarchar](256) NULL,", 256), 
            NewColumn("PriceRUR", typeof(decimal), "[PriceRUR] [money] NOT NULL,"), 
            NewColumn("CurrencyId", typeof(string), "[CurrencyId] [nchar](3) NOT NULL,", 3), 
            NewColumn("CategoryId", typeof(int), "[CategoryId] [int] NOT NULL,"), 
            NewColumn("Pictures", typeof(string), "[Pictures] [xml] NULL,", null, true, true, XmlSerializer.Serialize, a => XmlSerializer.Deserialize<string[]>((string)a)), 
            NewColumn("TypePrefix", typeof(string), "[TypePrefix] [nvarchar](50) NULL,", 50), 
            NewColumn("Vendor", typeof(string), "[Vendor] [nvarchar](256) NULL,", 256), 
            NewColumn("Model", typeof(string), "[Model] [nvarchar](256) NULL,", 256), 
            NewColumn("Store", typeof(bool), "[Store] [bit] NULL,"), 
            NewColumn("Pickup", typeof(bool), "[Pickup] [bit] NULL,"), 
            NewColumn("Delivery", typeof(bool), "[Delivery] [bit] NULL,"), 
            NewColumn("Description", typeof(string), "[Description] [nvarchar](2000) NULL,", 2000), 
            NewColumn("VendorCode", typeof(string), "[VendorCode] [nvarchar](256) NULL,", 256), 
            NewColumn("LocalDeliveryCost", typeof(decimal), "[LocalDeliveryCost] [money] NULL,"), 
            NewColumn("SalesNotes", typeof(string), "[SalesNotes] [nvarchar](50) NULL,", 50), 
            NewColumn("ManufacturerWarranty", typeof(bool), "[ManufacturerWarranty] [bit] NULL,"), 
            NewColumn("CountryOfOrigin", typeof(string), "[CountryOfOrigin] [nvarchar](256) NULL,", 256), 
            NewColumn("Downloadable", typeof(bool), "[Downloadable] [bit] NULL,"), 
            NewColumn("Adult", typeof(string), "[Adult] [nchar](10) NULL,", 10), 
            NewColumn("Barcode", typeof(string), "[Barcode] [xml] NULL,", null, true, true, XmlSerializer.Serialize, a => XmlSerializer.Deserialize<string[]>((string)a)), 
            NewColumn("Param", typeof(string), "[Param] [xml] NULL,", null, true, true, XmlSerializer.Serialize, a => XmlSerializer.Deserialize<ProductParam[]>((string)a)), 
            NewColumn("Author", typeof(string), "[Author] [nvarchar](256) NULL,", 256), 
            NewColumn("Publisher", typeof(string), "[Publisher] [nvarchar](256) NULL,", 256), 
            NewColumn("Series", typeof(string), "[Series] [nvarchar](256) NULL,", 256), 
            NewColumn("Year", typeof(int), "[Year] [int] NULL,"), 
            NewColumn("ISBN", typeof(string), "[ISBN] [nvarchar](256) NULL,", 256), 
            NewColumn("Volume", typeof(int), "[Volume] [int] NULL,"), 
            NewColumn("Part", typeof(int), "[Part] [int] NULL,"), 
            NewColumn("Language", typeof(string), "[Language] [nvarchar](50) NULL,", 50), 
            NewColumn("Binding", typeof(string), "[Binding] [nvarchar](50) NULL,", 50), 
            NewColumn("PageExtent", typeof(int), "[PageExtent] [int] NULL,"), 
            NewColumn("TableOfContents", typeof(string), "[TableOfContents] [nvarchar](512) NULL,", 512), 
            NewColumn("PerformedBy", typeof(string), "[PerformedBy] [nvarchar](50) NULL,", 50), 
            NewColumn("PerformanceType", typeof(string), "[PerformanceType] [nvarchar](50) NULL,", 50), 
            NewColumn("Format", typeof(string), "[Format] [nvarchar](50) NULL,", 50), 
            NewColumn("Storage", typeof(string), "[Storage] [nvarchar](50) NULL,", 50), 
            NewColumn("RecordingLength", typeof(string), "[RecordingLength] [nvarchar](50) NULL,", 50), 
            NewColumn("Artist", typeof(string), "[Artist] [nvarchar](256) NULL,", 256), 
            NewColumn("Media", typeof(string), "[Media] [nvarchar](50) NULL,", 50), 
            NewColumn("Starring", typeof(string), "[Starring] [nvarchar](256) NULL,", 256), 
            NewColumn("Director", typeof(string), "[Director] [nvarchar](50) NULL,", 50), 
            NewColumn("OriginalName", typeof(string), "[OriginalName] [nvarchar](50) NULL,", 50), 
            NewColumn("Country", typeof(string), "[Country] [nvarchar](50) NULL,", 50), 
            NewColumn("WorldRegion", typeof(string), "[WorldRegion] [nvarchar](50) NULL,", 50), 
            NewColumn("Region", typeof(string), "[Region] [nvarchar](50) NULL,", 50), 
            NewColumn("Days", typeof(int), "[Days] [int] NULL,"), 
            NewColumn("DataTour", typeof(string), "[DataTour] [nvarchar](50) NULL,", 50), 
            NewColumn("HotelStars", typeof(string), "[HotelStars] [nvarchar](50) NULL,", 50), 
            NewColumn("Room", typeof(string), "[Room] [nchar](10) NULL,", 10), 
            NewColumn("Meal", typeof(string), "[Meal] [nchar](10) NULL,", 10), 
            NewColumn("Included", typeof(string), "[Included] [nvarchar](256) NULL,", 256), 
            NewColumn("Transport", typeof(string), "[Transport] [nvarchar](256) NULL,", 256), 
            NewColumn("Place", typeof(string), "[Place] [nvarchar](256) NULL,", 256), 
            NewColumn("HallPlan", typeof(string), "[HallPlan] [nvarchar](256) NULL,", 256), 
            NewColumn("Date", typeof(DateTime), "[Date] [datetime] NULL,"), 
            NewColumn("IsPremiere", typeof(bool), "[IsPremiere] [bit] NULL,"), 
            NewColumn("IsKids", typeof(bool), "[IsKids] [bit] NULL,"), 
            NewColumn("Status", typeof(int), "[Status] [int] NOT NULL,", isSystem: true), 
            NewColumn("ModerationStatus", typeof(int), "[ModerationStatus] [int] NOT NULL,", isSystem: true), 
            NewColumn("Weight", typeof(int), "[Weight] [int] NULL,"),
            NewColumn("UpdatedUserId", typeof(int), "[UpdatedUserId] [nvarchar](50) NULL,", isSystem: true),
            NewColumn("IsRecommended", typeof(bool), "[IsRecommended] [bit] NOT NULL,"),
            NewColumn("BasePriceRUR", typeof(decimal), "[BasePriceRUR] [money] NULL,"),
            NewColumn("IsDeliveredByEmail", typeof(bool), "[IsDeliveredByEmail] [bit] NOT NULL,")
        };
        
        private static AdoMapper<Product> mapper;

        private readonly ILog log = LogManager.GetLogger(typeof(ProductsDataSource));

        private readonly IDeliveryRatesRepository deliveryRatesRepository;
        private ProductsRepository productsRepository;

        public ProductsDataSource()
        {
            mapper = new AdoMapper<Product>(ProductColumns);
            deliveryRatesRepository = new DeliveryRatesRepository();
        }

        private ProductsRepository ProductsRepository
        {
            get
            {
                return productsRepository ?? (this.productsRepository = new ProductsRepository());
            }
        }

        public static string CreateProductsTable(string tableKey, int partnerId)
        {
            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();
                var cmdText = string.Format(ProductsCreateTable, tableKey, mapper.GetCreateTableColumns(), partnerId);
                conn.ExecuteNonQuery(cmdText);
                return tableKey;
            }
        }

        public static string CreateProductsTableConstraints(string tableKey, int partnerId)
        {
            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();
                var cmdText = string.Format(ProductsCreateTableConstraints, tableKey, partnerId);
                conn.ExecuteNonQuery(cmdText);
                return tableKey;
            }
        }

        public void ExecuteNonQuery(string userId, string queryText, Product product)
        {
            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();
                conn.SetUserContext(userId);

                using (var sqlCmd = new SqlCommand(queryText, conn))
                {
                    MapProductToDB(product, sqlCmd);
                    sqlCmd.ExecuteNonQuery();
                }
            }
        }

        public DataRow[] MapToErrorDataRows(ICollection<ProductImportError> errors)
        {
            var table = new DataTable();

            var columnPartnerProductId = new DataColumn();
            columnPartnerProductId.ColumnName = "PartnerProductId";
            columnPartnerProductId.AllowDBNull = false;
            columnPartnerProductId.DataType = typeof(string);

            table.Columns.Add(columnPartnerProductId);

            var columnCode = new DataColumn();
            columnCode.ColumnName = "Code";
            columnCode.AllowDBNull = false;
            columnCode.DataType = typeof(int);

            table.Columns.Add(columnCode);

            var columnPartnerCategoryId = new DataColumn();
            columnPartnerCategoryId.ColumnName = "PartnerCategoryId";
            columnPartnerCategoryId.AllowDBNull = true;
            columnPartnerCategoryId.DataType = typeof(string);

            table.Columns.Add(columnPartnerCategoryId);

            foreach (var err in errors)
            {
                var row = table.NewRow();

                row[columnPartnerProductId.ColumnName] = err.PartnerProductId;
                row[columnCode.ColumnName] = Convert.ToInt32(err.Code);
                row[columnPartnerCategoryId.ColumnName] = err.PartnerCategoryId;

                table.Rows.Add(row);
            }

            return table.Select();
        }

        public void RebuildIndexes(string tableKey)
        {
            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();

                var cmd =
                    "ALTER INDEX ALL ON [prod].[Products_{0}] REBUILD; ALTER INDEX ALL ON [prod].[ProductCategories] REBUILD;";

                var cmdText = string.Format(cmd, tableKey);

                conn.ExecuteNonQuery(cmdText);
            }
        }

        public void AddHistoryTriggers(string tableKey)
        {
            var cmdSql = @"
CREATE TRIGGER [prod].[LogProducts_{0}_{1}] ON [prod].[Products_{0}]
FOR {1}
AS
    INSERT INTO [prod].[ProductsHistory]
    select	'{2}',
            dbo.GetCurrentUserId(),
            getdate(),
            getutcdate(),
            *
    from {3}
";
            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();

                var cmdText = string.Format(cmdSql, tableKey, "INSERT", "I", "INSERTED");
                conn.ExecuteNonQuery(cmdText);

                cmdText = string.Format(cmdSql, tableKey, "UPDATE", "U", "INSERTED");
                conn.ExecuteNonQuery(cmdText);

                cmdText = string.Format(cmdSql, tableKey, "DELETE", "D", "DELETED");
                conn.ExecuteNonQuery(cmdText);
            }
        }

        public Product GetById(string productId, int partnerId)
        {
            var activeCatalog = new ProductCatalogsDataSource().GetActiveCatalog(partnerId);

            if (activeCatalog == null)
            {
                throw new OperationException(
                    ResultCodes.PARTNER_CATALOG_NOT_FOUND, "Активный каталог продуктов партнера не найден.");
            }

            return GetById(productId, activeCatalog.Key);
        }

        public Product GetById(string productId, string productCatalogKey)
        {
            const string Query = @"SELECT {1} FROM [prod].[Products_{0}] where ProductId = @Id";
            
            var queryText = string.Format(Query, productCatalogKey, mapper.GetCommaSeparatedColumnNames());
            
            var param = new Dictionary<string, string>();
            param.Add("Id", productId);

            return queryText.ExecuteReader(DataSourceConfig.ConnectionString, param, mapper.GetEntity);
        }

        public Product[] GetProductsByPartnerProductIds(IEnumerable<string> productIds, string productCatalogKey)
        {
            var res = new List<Product>();
            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();

                var queryText = string.Format(
                    @"
SELECT {1}
  FROM [prod].[Products_{0}]
where 
PartnerProductId in (select value from dbo.ParamParserString(@Ids,','))
", 
                    productCatalogKey, 
                    mapper.GetCommaSeparatedColumnNames());
                using (var sqlCmd = new SqlCommand(queryText, conn))
                {
                    sqlCmd.Parameters.AddWithValue("Ids", string.Join(",", productIds));
                    using (var reader = sqlCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var product = mapper.GetEntity(reader);
                            res.Add(product);
                        }
                    }
                }
            }

            return res.ToArray();
        }
        
        public void BulkInsertPartnerProducts(IEnumerable<Product> productsToInsert, string tableKey)
        {
            var internalProductsdToInsert = productsToInsert as Product[] ?? productsToInsert.ToArray();
            foreach (
                var product in internalProductsdToInsert.Where(product => product.InsertedDate == DateTime.MinValue))
            {
                product.InsertedDate = DateTime.Now;
            }

            using (var bulkCopy = new SqlBulkCopy(DataSourceConfig.ConnectionString, SqlBulkCopyOptions.CheckConstraints))
            {
                bulkCopy.BulkCopyTimeout = 300;

                var rows = mapper.MapToDataRows(internalProductsdToInsert);

                bulkCopy.DestinationTableName = string.Format("[prod].[Products_{0}]", tableKey);

                bulkCopy.WriteToServer(rows);
            }
        }

        public SearchProductsResult AdminSearchProducts(AdminSearchProductsParameters parameters)
        {
            var param = parameters ?? new AdminSearchProductsParameters();

            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();

                using (var sqlCmd = new SqlCommand("[prod].[AdminSearchProducts]", conn))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    sqlCmd.AddParameter("sort", (int?)param.SortType);
                    sqlCmd.AddParameter("countToSkip", param.CountToSkip);
                    sqlCmd.AddParameter("countToTake", param.CountToTake);
                    sqlCmd.AddParameter("status", (int?)param.Status);
                    sqlCmd.AddParameter("moderationStatus", (int?)param.ModerationStatus);
                    sqlCmd.AddParameter("isRecommended", param.IsRecommended);
                    sqlCmd.AddParameter("prodCategoryIds", PrepareParamValue(param.ParentCategories));
                    sqlCmd.AddParameter("includeSubCategory", param.IncludeSubCategory);
                    sqlCmd.AddParameter("searchTerm", param.SearchTerm);
                    sqlCmd.AddParameter("partnerIds", PrepareParamValue(param.PartnerIds));
                    sqlCmd.AddParameter("productIds", PrepareParamValue(param.ProductIds));
                    sqlCmd.AddParameter("calcTotalCount", true);

                    var totalCountParam = sqlCmd.AddOutParameter("totalCount", SqlDbType.BigInt, true);

                    var res = new List<Product>();
                    
                    using (var reader = sqlCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            res.Add(MapAdminProductRow(reader));
                        }
                    }

                    var totalCount = totalCountParam.Value as long?;

                    if (!totalCount.HasValue)
                    {
                        totalCount = 0;
                    }

                    return SearchProductsResult.BuildSuccess(res, totalCount, ApiSettings.MaxResultsCountProducts);
                }
            }
        }

        public SearchProductsResult SearchPublicProducts(SearchProductsParameters parameters, GenerateResult mechanicsSql)
        {
            int retryCount = 0;
            while (true)
            {
                try
                {
                    var result = SearchProductsInternal(parameters, MapPublicProductsResult, mechanicsSql);

                    return new SearchProductsResult
                    {
                        Products = result.Item1.ToArray(),
                        TotalCount = result.Item2,
                        MaxPrice = result.Item3,
                        ResultCode = ResultCodes.SUCCESS,
                        MaxCountToTake = ApiSettings.MaxResultsCountProducts
                    };
                }
                catch (SqlException sqlException)
                {
                    if (sqlException.Number != 1205
                        || retryCount > 3)
                    {
                        throw;
                    }

                    if (retryCount == 0)
                    {
                        log.Error("Deadlock при поиске товаров", sqlException);
                    }

                    retryCount++;
                    Thread.Sleep(500);
                }
            }
        }

        public CalculatedProductPrices[] CalculateProductsPrices(string[] productsIds, GenerateResult mechanicsSql)
        {
            if (productsIds == null || productsIds.Length == 0)
            {
                return new CalculatedProductPrices[0];
            }

            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();

                using (var sqlCmd = new SqlCommand("[prod].[CalculateProductsPrices]", conn))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    sqlCmd.AddParameter("productsIds", string.Join(",", productsIds.Select(id => "'" + id + "'")));
                    sqlCmd.AddParameter("baseSql", mechanicsSql.BaseSql);
                    sqlCmd.AddParameter("actionSql", mechanicsSql.ActionSql);

                    using (var reader = sqlCmd.ExecuteReader())
                    {
                        var prices = ReadCalculatedProductPrices(reader).ToArray();

                        return prices;
                    }
                }
            }
        }
        
        public void InsertProduct(string userId, Product product, string tableKey)
        {
            var cmdText = GetInsertProductQuery(tableKey);

            ExecuteNonQuery(userId, cmdText, product);
        }

        public void UpdateProduct(string userId, Product product, string tableKey)
        {
            var cmdText = GetUpdateProductQuery(tableKey, product.ProductId);
            ExecuteNonQuery(userId, cmdText, product);
        }

        public ResultBase ChangeProductsStatus(ChangeStatusParameters parameters)
        {
            return ProductsRepository.ChangeStatuses(parameters);
        }

        public ResultBase ChangeProductsModerationStatus(ChangeModerationStatusParameters parameters)
        {
            return ProductsRepository.ChangeStatuses(parameters);
        }

        public CreateProductResult CreateProduct(CreateProductParameters productParameters)
        {
            var activeCatalog = new ProductCatalogsDataSource().GetActiveCatalog(productParameters.PartnerId);

            if (activeCatalog == null)
            {
                throw new OperationException(
                    ResultCodes.PARTNER_CATALOG_NOT_FOUND, "Активный каталог продуктов партнера не найден.");
            }

            var product = GetNewProductInstance(productParameters);
            product.ProductId = LoyaltyDBSpecification.GetProductId(
                productParameters.PartnerId, productParameters.PartnerProductId);
            product.PartnerProductId = productParameters.PartnerProductId;

            product.ModerationStatus = this.GetProductModerationStatusWithCalculator(new List<Product>(), product);

            var exists = ProductsRepository.GetById(product.ProductId);

            if (exists != null)
            {
                throw new OperationException(
                    ResultCodes.ALLREADY_EXISTS, "Продукт с таким ид уже существует.");
            }

            product.InsertedDate = DateTime.Now;

            if (FeaturesConfiguration.Instance.Site505EnableActionPrice)
            {
                product.BasePriceRUR = productParameters.BasePriceRUR;
            }

            InsertProduct(productParameters.UserId, product, activeCatalog.Key);

            var insertedProduct = this.GetById(product.ProductId, activeCatalog.Key);

            return new CreateProductResult
                   {
                       ResultCode = ResultCodes.SUCCESS,
                       Product = insertedProduct
                   };
        }

        public ResultBase UpdateProduct(UpdateProductParameters parameters)
        {
            var activeCatalog = new ProductCatalogsDataSource().GetActiveCatalog(parameters.PartnerId);

            if (activeCatalog == null)
            {
                throw new OperationException(
                    ResultCodes.PARTNER_CATALOG_NOT_FOUND, "Активный каталог продуктов партнера не найден.");
            }

            var productDataSource = new ProductsDataSource();

            var updateProduct = productDataSource.GetById(parameters.ProductId, parameters.PartnerId); 

            if (updateProduct == null)
            {
                throw new OperationException(ResultCodes.NOT_FOUND, "Продукт не найден.");
            }

            if (parameters.PartnerId != updateProduct.PartnerId)
            {
                throw new OperationException(ResultCodes.PARTNER_ID_CAN_NOT_BE_CHANGED, "Нельзя изменять партнера.");
            }

            var existDeepCopy = XmlSerializer.Serialize(updateProduct).Deserialize<Product>();

            var existingProducts = new List<Product>
                           {
                               existDeepCopy
                           };

            if (FeaturesConfiguration.Instance.Site505EnableActionPrice)
            {
                var productService = new ProductService();

                // REVIEW: почему один и тот же товар передаётся как старый и новый? А нужны ли там вообще продукты, тем более два?
                productService.CalculateProductBasePrice(updateProduct, updateProduct, parameters.PriceRUR, parameters.BasePriceRUR, false);
            }

            SetProductParameters(updateProduct, parameters);
            updateProduct.ModerationStatus = this.GetProductModerationStatusWithCalculator(existingProducts, updateProduct);

            UpdateProduct(parameters.UserId, updateProduct, activeCatalog.Key);

            return ResultBase.BuildSuccess();
        }

        public void DeleteCache(int insertedTimeout = 0, int disableTimeout = 10, int caheToDelete = 1000)
        {
            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();

                using (var sqlCmd = new SqlCommand("[prod].[ClearCache]", conn))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.AddParameter("insertedTimeout", insertedTimeout);
                    sqlCmd.AddParameter("disableTimeout", disableTimeout);
                    sqlCmd.AddParameter("caheToDelete", disableTimeout);
                    sqlCmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateProductsFromAllPartners()
        {
            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();

                using (var sqlCmd = new SqlCommand("[prod].[UpdateProductsFromAllPartners]", conn))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.ExecuteNonQuery();
                }
            }
        }

        private static AdoMapperColumn NewColumn(
            string columnName,
            Type dotNetType,
            string columnSqlDeclare,
            int? columnLen = null,
            bool? isInsert = null,
            bool? isSelect = null,
            Func<object, object> objToDBMapFunc = null,
            Func<object, object> databaseToObjMapFunc = null,
            bool isSystem = false)
        {
            return new AdoMapperColumn
            {
                ColumnName = columnName,
                DotNetType = dotNetType,
                ColumnSqlDeclare = columnSqlDeclare,
                ColumnLen = columnLen,
                IsInsert = isInsert,
                IsSelect = isSelect,
                ObjToDBMapFunc = objToDBMapFunc,
                DBToObjMapFunc = databaseToObjMapFunc,
                IsSystem = isSystem
            };
        }

        private Tuple<TReturn, long?, decimal?> SearchProductsInternal<TReturn>(
            SearchProductsParameters searchParam,
            Func<SqlDataReader, TReturn> mapRowToEntity,
            GenerateResult mechanicsSql)           
            where TReturn : class
        {
            searchParam.ThrowIfNull("searchParam");
            searchParam.ClientContext.ThrowIfNull("searchParam.ClientContext");

            TReturn res;
            long? totalCount;
            decimal? maxPrice;

            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();

                using (var sqlCmd = new SqlCommand("[prod].[SearchProducts]", conn))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    sqlCmd.AddParameter("sort", (int?)searchParam.SortType);
                    sqlCmd.AddParameter("countToSkip", searchParam.CountToSkip);
                    sqlCmd.AddParameter("countToTake", searchParam.CountToTake);
                    sqlCmd.AddParameter("partnerProductId", searchParam.PartnerProductId);
                    sqlCmd.AddParameter("prodCategoryIds", PrepareParamValue(searchParam.ParentCategories));
                    sqlCmd.AddParameter("includeSubCategory", searchParam.IncludeSubCategory);
                    sqlCmd.AddParameter("searchTerm", searchParam.SearchTerm);
                    sqlCmd.AddParameter("partnerIds", PrepareParamValue(searchParam.PartnerIds));
                    sqlCmd.AddParameter("productIds", PrepareParamValue(searchParam.ProductIds));
                    sqlCmd.AddParameter("vendors", PrepareParamValue(searchParam.Vendors));
                    sqlCmd.AddParameter("returnEmptyVendorProducts", searchParam.ReturnEmptyVendorProducts);
                    sqlCmd.AddParameter("calcTotalCount", searchParam.CalcTotalCount);
                    sqlCmd.AddParameter("minInsertedDate", searchParam.MinInsertedDate);
                    sqlCmd.AddParameter("minRecommendedPrice", searchParam.MinRecommendedPrice);
                    sqlCmd.AddParameter("maxRecommendedPrice", searchParam.MaxRecommendedPrice);
                    sqlCmd.AddParameter("categoryIdToRecommendFor", searchParam.CategoryIdToRecommendFor);

                    var clientContextParser = new ClientContextParser();
                    var locationKladr = clientContextParser.GetLocationKladrCode(searchParam.ClientContext);

                    sqlCmd.AddParameter("contextKey", GetContextKey(searchParam, clientContextParser));

                    if (searchParam.ProductParams != null && searchParam.ProductParams.Any())
                    {
                        var escapedParams = Utils.EscapeQuotes(searchParam.ProductParams);
                        var productParam = XmlSerializer.SerializeWithNoNamespace(escapedParams);
                        sqlCmd.AddParameter("productParamsXml", productParam);
                    }

                    var activePartnerIds = string.Join(",", deliveryRatesRepository.GetDeliveringPartnerIds(locationKladr));
                    sqlCmd.AddParameter("activePartnerIds", activePartnerIds);

                    var targetIds = searchParam.ClientContext.FirstOrDefault(c => c.Key == ClientContextParser.AudiencesKey);

                    if (targetIds.Value != null)
                    {
                        sqlCmd.AddParameter("targetAudiencesIds", targetIds.Value.Replace(';', ','));
                    }

                    sqlCmd.AddParameter("calcMaxPrice", true);
                    sqlCmd.AddParameter("minPrice", searchParam.MinPrice);
                    sqlCmd.AddParameter("maxPrice", searchParam.MaxPrice);
                    sqlCmd.AddParameter("actionPrice", searchParam.IsActionPrice);

                    if (searchParam.PopularProductType.HasValue)
                    {
                        sqlCmd.AddParameter("popularType", (int)searchParam.PopularProductType.Value);
                    }

                    sqlCmd.AddParameter("baseSql", mechanicsSql.BaseSql);
                    sqlCmd.AddParameter("actionSql", mechanicsSql.ActionSql);

                    var calculatedMaxPriceParam = sqlCmd.AddOutParameter("calculatedMaxPrice", SqlDbType.Decimal, true);
                    var totalCountParam = sqlCmd.AddOutParameter("totalCount", SqlDbType.BigInt, true);

                    using (var reader = sqlCmd.ExecuteReader())
                    {
                        res = mapRowToEntity(reader);
                    }

                    totalCount = totalCountParam.Value as long?;
                    maxPrice = calculatedMaxPriceParam.Value as decimal?;

                    if (searchParam.CalcTotalCount.HasValue
                        && searchParam.CalcTotalCount.Value
                        && !totalCount.HasValue)
                    {
                        totalCount = 0;
                    }
                }
            }

            return new Tuple<TReturn, long?, decimal?>(res, totalCount, maxPrice);
        }

        private List<Product> MapPublicProductsResult(SqlDataReader reader)
        {
            var res = new List<Product>();

            while (reader.Read())
            {
                res.Add(MapPublicProductRow(reader));
            }

            return res;
        }

        private Product MapAdminProductRow(SqlDataReader reader)
        {
            var product = mapper.GetEntity(reader);

            product.PriceBase = reader.GetDecimal("PriceBase").Round();
            product.Price = reader.GetDecimal("PriceAction").Round();
            product.IsActionPrice = reader.GetBoolean("IsActionPrice");
            product.BasePriceRurDate = reader.GetDateTimeOrNull("BasePriceRurDate");

            product.CategoryName = reader.GetString("CategoryName");
            product.CategoryNamePath = reader.GetStringOrNull("CategoryNamePath");

            var targetAudiences = reader.GetStringOrNull("TargetAudienceIds");
            product.TargetAudiencesIds = string.IsNullOrEmpty(targetAudiences) ? null : targetAudiences.Split(',');
            
            return product;
        }

        private Product MapPublicProductRow(SqlDataReader reader)
        {
            var product = mapper.GetEntity(reader);

            product.PriceBase = reader.GetDecimal("PriceBase").Round();
            product.Price = reader.GetDecimal("PriceAction").Round();
            product.IsActionPrice = reader.GetBoolean("IsActionPrice");
            
            product.CategoryName = reader.GetString("CategoryName");

            product.PopularType = (PopularProductTypes)reader.GetValueOrDefault<int>("PopularType");
            product.ProductRate = reader.GetValueOrDefault<int>("PopularRate");

            return product;
        }

        private IEnumerable<CalculatedProductPrices> ReadCalculatedProductPrices(SqlDataReader reader)
        {
            while (reader.Read())
            {
                var prices = new CalculatedProductPrices
                {
                    ProductId = reader.GetString("ProductId"),
                    PriceRur = reader.GetDecimal("PriceRUR"),
                    PriceBase = reader.GetDecimal("PriceBase").Round(),
                    PricesAction = reader.GetDecimal("PriceAction").Round()
                };

                yield return prices;
            }
        }

        private string PrepareParamValue(IEnumerable<string> values)
        {
            if (values == null)
            {
                return null;
            }

            var escaped = values.Select(x => x.Replace(",", "\\,"));
            return string.Join(",", escaped);
        }

        private string PrepareParamValue(IEnumerable<int> values)
        {
            return values == null ? null : string.Join(",", values);
        }
        
        private string GetContextKey(SearchProductsParameters parameters, ClientContextParser clientContextParser)
        {
            return ContextKeyProvider.GetContextKey(
                clientContextParser.GetLocationKladrCode(parameters.ClientContext), 
                clientContextParser.GetAudienceIds(parameters.ClientContext));
        }

        private Product GetNewProductInstance(ProductParameters productParameters)
        {
            return new Product
                       {
                           PartnerId = productParameters.PartnerId,
                           CategoryId = productParameters.CategoryId,
                           Name = productParameters.Name,
                           PriceRUR = productParameters.PriceRUR,
                           Description = productParameters.Description,
                           Vendor = productParameters.Vendor,
                           Weight = productParameters.Weight,
						   IsDeliveredByEmail = productParameters.IsDeliveredByEmail,
                           Pictures = productParameters.Pictures,
                           Param = productParameters.Param,
                           UpdatedUserId = productParameters.UserId,
                           Available = true,
                           CurrencyId = productParameters.CurrencyId
                       };
        }

        private void SetProductParameters(Product product, ProductParameters productParameters)
        {
            product.Name = productParameters.Name;
            product.PriceRUR = productParameters.PriceRUR;
            product.Description = productParameters.Description;
            product.Vendor = productParameters.Vendor;
            product.Weight = productParameters.Weight;
	        product.IsDeliveredByEmail = productParameters.IsDeliveredByEmail;
            product.Pictures = productParameters.Pictures;
            product.Param = productParameters.Param;
            product.UpdatedDate = DateTime.Now;
            product.UpdatedUserId = productParameters.UserId;
        }

        private string GetInsertProductQuery(string tableKey)
        {
            return string.Format(
                @"
INSERT INTO [prod].[Products_{0}]({1}) VALUES ({2})
INSERT INTO [prod].[ProductsFromAllPartners]({1}) VALUES ({2})
",
                tableKey,
                mapper.GetCommaSeparatedColumnNames(),
                mapper.GetCommaSeparatedColumnParameters());
        }

        private string GetUpdateProductQuery(string tableKey, string productId)
        {
            return string.Format(
                @"
UPDATE [prod].[Products_{0}] SET {1} WHERE ProductId = '{2}'
UPDATE [prod].[ProductsFromAllPartners] set {1} WHERE ProductId = '{2}'",
                tableKey,
                mapper.GetCommaSeparatedColumnUpdateParameters(),
                productId);
        }

        private void MapProductToDB(Product product, SqlCommand sqlCmd)
        {
            var parameters = mapper.GetInsertSqlParameters(product);
            sqlCmd.Parameters.AddRange(parameters);
        }

        private ProductModerationStatuses GetProductModerationStatusWithCalculator(IList<Product> products, Product newProduct)
        {
            log.Debug("Определение статуса модерции");
            var partner = new PartnerRepository().GetById(newProduct.PartnerId);
            ModerationStatusCalculator moderationStatusCalculator;
            log.DebugFormat("Определение статуса модерции для партнера {0} ({1})", newProduct.PartnerId, partner.ThrustLevel);
            switch (partner.ThrustLevel)
            {
                case PartnerThrustLevel.Low:
                    moderationStatusCalculator = new LowPartnerTrustCalculator(products);
                    break;
                case PartnerThrustLevel.Middle:
                    moderationStatusCalculator = new MiddlePartnerTrustCalculator(products);
                    break;
                case PartnerThrustLevel.High:
                    moderationStatusCalculator = new HighPartnerTrustCalculator(products);
                    break;
                default:
                    throw new NotSupportedException("Trusted level not supported");
            }

            var retVal = moderationStatusCalculator.CalcModerationStatus(newProduct);
            return retVal;
        }
    }
}