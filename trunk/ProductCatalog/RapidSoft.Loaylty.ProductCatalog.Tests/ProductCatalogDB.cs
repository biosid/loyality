namespace RapidSoft.Loaylty.ProductCatalog.Tests
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;

    using Extensions;

    using ProductCatalog.DataSources;
    using ProductCatalog.Services;

    public class ProductCatalogDB
    {
        private static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["LoyaltyProductCatalogDB"].ConnectionString;

        public static void DropAllProductCatalogs()
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                var tables = new List<string>();

                var selectTablesSql = @"
SELECT * FROM INFORMATION_SCHEMA.TABLES
WHERE  
TABLE_NAME like 'PartnerProductCaterories[_]%'
OR
TABLE_NAME like 'Products[_]%'
";

                using (var sqlCmd = new SqlCommand(selectTablesSql, conn))
                {
                    using (var reader = sqlCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var tabName = reader.GetString("TABLE_NAME");
                            tables.Add(tabName);
                        }
                    }
                }

                foreach (var table in tables)
                {
                    new SqlCommand("drop table prod." + table, conn).ExecuteNonQuery();
                }

                new SqlCommand(
@"
delete from [prod].[PartnerProductCatalogs]
", 
 conn).ExecuteNonQuery();

            }
        }

        public static void DeleteFakeTestData()
        {
            DropOldProductCatalogs();
            DropProductsInTestCategories();
            DropTestCategories();
            DropTestPartners();
        }

        public static void DropProductsInTestCategories()
        {
            @"
delete from [prod].[Products] where productId like '%test%'

delete p 
from prod.Products p 
inner join 
[prod].[ProductCategories] c
on p.CategoryId = c.Id
WHERE c.NAME LIKE '%Test%'
".ExecuteNonQuery(DataSourceConfig.ConnectionString);
        }

        public static void DropTestPartners()
        {
            @"

-- Удаляем каталоги тестовых партнёров
delete c from 
[prod].[Partners] p inner join [prod].[PartnerProductCatalogs] c
on p.Id=c.PartnerId
WHERE p.NAME LIKE '%Test%'
            
-- Удаляем тестовых партнёров
delete from [prod].[Partners] WHERE NAME LIKE '%Test%'".ExecuteNonQuery(DataSourceConfig.ConnectionString);
        }

        public static void DropTestCategories()
        {
            @"
DELETE p 
from [prod].[ProductCategoriesPermissions] p inner join [prod].[ProductCategories] c
on p.CategoryId = c.Id
WHERE c.NAME LIKE '%Test%'
            
DELETE from [prod].[ProductCategories]
WHERE NAME like '%Test%'".ExecuteNonQuery(DataSourceConfig.ConnectionString);
        }

        public static void DropOldProductCatalogs()
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                var tables = new List<string>();

                var selectTablesSql = @"
SELECT *
FROM INFORMATION_SCHEMA.TABLES s
left join 
[prod].[PartnerProductCatalogs] c
on SUBSTRING( TABLE_NAME,CHARINDEX('_',TABLE_NAME)+1,20) = c.[Key]
WHERE  
(TABLE_NAME like 'PartnerProductCaterories[_]%'
OR
TABLE_NAME like 'Products[_]%')
and 
(IsActive=0 or IsActive is null)

";

                using (var sqlCmd = new SqlCommand(selectTablesSql, conn))
                {
                    using (var reader = sqlCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var tabName = reader.GetString("TABLE_NAME");
                            tables.Add(tabName);
                        }
                    }
                }

                foreach (var table in tables)
                {
                    new SqlCommand("drop table prod." + table, conn).ExecuteNonQuery();
                }
            }
        }

        public static void DeleteCategory(int id)
        {
            MockFactory.GetCatalogAdminService().DeleteCategory(TestDataStore.TestUserId, id);
        }
    }
}