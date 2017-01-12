using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RapidSoft.Loaders.KLADR.Utils
{
    public class SqlUtils : ISqlUtils
    {
        public SqlUtils(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; private set; }

        public void Execute(string sqlCommandText)
        {
            using (var connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();

                var sqlCommand = new SqlCommand(sqlCommandText, connection) { CommandTimeout = Configuration.DbTimeout };
                sqlCommand.ExecuteNonQuery();
            }
        }
        
        public object ExecuteScalar(string sqlCommandText)
        {
            using (var connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();

                var sqlCommand = new SqlCommand(sqlCommandText, connection) { CommandTimeout = Configuration.DbTimeout };
                return sqlCommand.ExecuteScalar();
            }
        }
    }
}
