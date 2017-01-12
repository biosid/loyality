namespace RapidSoft.Loaylty.ProductCatalog.DataSources
{
    using System.Data.SqlClient;

    public static class SqlConnectionExtension
    {
        public static void ExecuteNonQuery(this SqlConnection conn, string cmdText, int timeout = 0)
        {
            using (var sqlCmd = new SqlCommand(cmdText, conn))
            {
                sqlCmd.CommandTimeout = timeout;
               sqlCmd.ExecuteNonQuery();
            }
        }

        public static object ExecuteScalar(this SqlConnection conn, string cmdText, int timeout = 0)
        {
            using (var sqlCmd = new SqlCommand(cmdText, conn))
            {
                sqlCmd.CommandTimeout = timeout;
                var retVal = sqlCmd.ExecuteScalar();
                return retVal;
            }
        }

        public static void SetUserContext(this SqlConnection conn, string userId)
        {
            using (var cmd = conn.CreateCommand())
            {
                var parm = cmd.CreateParameter();
                parm.ParameterName = "@userName";
                parm.Value = userId;

                cmd.CommandText = "prod.SetUserContext";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(parm);

                cmd.ExecuteNonQuery();
            }
        }
    }
}