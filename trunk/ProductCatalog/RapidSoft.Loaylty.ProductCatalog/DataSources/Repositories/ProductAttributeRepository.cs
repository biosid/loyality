namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    using API.InputParameters;

    using Interfaces;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    public class ProductAttributeRepository : IProductAttributeRepository
    {
        public string[] GetAll(ProductAttributes attribute, int? categoryId, ProductModerationStatuses? moderationStatus)
        {
            var columnName = attribute.ToString();
            var res = new List<string>();

            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();
                var queryText = "prod.GetAttributeValues";

                using (var sqlCmd = new SqlCommand(queryText, conn))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    sqlCmd.Parameters.AddWithValue("columnName", columnName);
                    sqlCmd.Parameters.AddWithValue("categoryId", categoryId);
                    sqlCmd.Parameters.AddWithValue(
                        "moderationStatus", moderationStatus.HasValue ? (int?)moderationStatus.Value : null);

                    using (var reader = sqlCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var entry = reader.GetString(0);
                            res.Add(entry);
                        }
                    }
                }
            }

            return res.ToArray();
        }
    }
}