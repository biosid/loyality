using System.Data.SqlClient;

namespace RapidSoft.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    public static class SqlCommandExtension
    {
        public static void ExecuteNonQuery(this string queryText, string connectionString)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (var sqlCmd = new SqlCommand(queryText, conn))
                {
                    sqlCmd.ExecuteNonQuery();
                }
            }
        }

        public static void ExecuteNonQuery(this string queryText, SqlConnection conn, SqlTransaction tran)
        {
            using (var sqlCmd = new SqlCommand(queryText, conn, tran))
            {
                sqlCmd.ExecuteNonQuery();
            }
        }

        public static T ExecuteReader<T>(this string queryText, string connectionString, IDictionary<string, string> parameters,  Func<SqlDataReader, T> mapper) where T : class 
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (var sqlCmd = new SqlCommand(queryText, conn))
                {
                    foreach (var parameter in parameters)
                    {
                        sqlCmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }

                    using (var reader = sqlCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return mapper(reader);
                        }
                    }
                }
            }

            return default(T);
        }

        public static object ExecuteScalar(this string queryText, string connectionString)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var sqlCmd = new SqlCommand(queryText, conn))
                {
                    var retVal = sqlCmd.ExecuteScalar();
                    return retVal;
                }
            }
        }
        
        public static void AddParameter(this SqlCommand sqlCommand, string parameterName, object value)
        {
            sqlCommand.Parameters.AddWithValue(parameterName, value);
        }

        public static SqlParameter AddOutParameter(this SqlCommand sqlCommand, string parameterName, SqlDbType sqlDbType, bool isNullable = false)
        {
            var parameter = new SqlParameter(parameterName, sqlDbType)
                                      {
                                          Direction = ParameterDirection.Output,
                                          IsNullable = isNullable
                                      };
            sqlCommand.Parameters.Add(parameter);
            return parameter;
        }
    }
}