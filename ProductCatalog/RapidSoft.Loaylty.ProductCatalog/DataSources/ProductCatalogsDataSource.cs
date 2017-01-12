namespace RapidSoft.Loaylty.ProductCatalog.DataSources
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    using Extensions;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    public class ProductCatalogsDataSource
    {
        #region Constants

        private const string PartnerProductCatalogInsert =
            @"

INSERT INTO [prod].[PartnerProductCatalogs]
           ([PartnerId]
           ,[Key]
           ,[IsActive]
           ,[InsertedDate])
     VALUES
           (@PartnerId
           ,@Key
           ,@IsActive
           ,getdate())

";

        private const string PartnerProductCatalogSetActive =
            @"

UPDATE [prod].[PartnerProductCatalogs]
   SET [IsActive] = 0      
      ,[UpdatedDate] = GETDATE()
 WHERE 
    [PartnerId]=@PartnerId
    and
    [IsActive] = 1
 
UPDATE [prod].[PartnerProductCatalogs]
   SET [IsActive] = 1      
      ,[UpdatedDate] = GETDATE()
 WHERE 
    [PartnerId]=@PartnerId
    and
    [Key] = @Key

";

        private const string PartnerProductCatalogSelectActive =
            @"

SELECT [PartnerId]
      ,[Key]
      ,[IsActive]
      ,[InsertedDate]
      ,[UpdatedDate]
  FROM [prod].[PartnerProductCatalogs]
WHERE 
IsActive = 1
and
PartnerId = @PartnerId

";

        private const string PartnerProductCatalogGetActiveKeys =
@"

SELECT [Key]
FROM [prod].[PartnerProductCatalogs]
where 
[IsActive] = 1

";

        #endregion

        #region Methods
        
        public static string InsertCatalog(int partnerId, string dateKey)
        {
            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();
                var catalogKey = LoyaltyDBSpecification.GetImportKey(partnerId, dateKey);
                var cmdText = string.Format(PartnerProductCatalogInsert);

                using (var sqlCmd = new SqlCommand(cmdText, conn))
                {
                    sqlCmd.Parameters.AddWithValue("PartnerId", partnerId);
                    sqlCmd.Parameters.AddWithValue("Key", catalogKey);
                    sqlCmd.Parameters.AddWithValue("IsActive", false);
                    sqlCmd.ExecuteNonQuery();
                }

                return catalogKey;
            }
        }

        public static void SetActiveCatalog(int partnerId, string newKey)
        {
            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    using (var sqlCmd = new SqlCommand(PartnerProductCatalogSetActive, conn, tran))
                    {
                        sqlCmd.Parameters.AddWithValue("PartnerId", partnerId);
                        sqlCmd.Parameters.AddWithValue("Key", newKey);
                        sqlCmd.ExecuteNonQuery();
                    }

                    var keys = new List<string>();

                    using (var sqlCmd1 = new SqlCommand(PartnerProductCatalogGetActiveKeys, conn, tran))
                    {
                        using (var reader = sqlCmd1.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                keys.Add(reader.GetString("Key"));
                            }
                        }
                    }

                    if (keys.Count > 0)
                    {
                        var query = string.Format(
@"
ALTER VIEW [prod].[Products]
AS                        
{0}",
    string.Join(" UNION ALL ", keys.Select(k => string.Format(@"SELECT * FROM prod.Products_{0} ", k))));

                        query.ExecuteNonQuery(conn, tran);
                    }

                    tran.Commit();
                }
            }
        }

        public PartnerProductCatalog GetActiveCatalog(int partnerId)
        {
            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();

                using (var sqlCmd = new SqlCommand(PartnerProductCatalogSelectActive, conn))
                {
                    sqlCmd.Parameters.AddWithValue("PartnerId", partnerId);
                    using (var reader = sqlCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var productCatalog = new PartnerProductCatalog();

                            productCatalog.Key = reader.GetString("Key");
                            productCatalog.PartnerId = reader.GetInt32("PartnerId");

                            return productCatalog;
                        }
                    }
                }
            }

            return null;
        }

        public void DeleteForeingKeyFromCatalog(PartnerProductCatalog catalogKey)
        {
            const string SQLFormat = @"IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[prod].[FK_Products_{0}_ProductCategories]') AND parent_object_id = OBJECT_ID(N'[prod].[Products_{0}]'))
ALTER TABLE [prod].[Products_{0}] DROP CONSTRAINT [FK_Products_{0}_ProductCategories]";

            var sql = string.Format(SQLFormat, catalogKey.Key);

            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();

                using (var sqlCmd = new SqlCommand(sql, conn))
                {
                    sqlCmd.ExecuteNonQuery();
                }
            }
        }
        
        #endregion
    }
}