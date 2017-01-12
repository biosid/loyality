using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace RapidSoft.Loaders.IPDB
{
    internal class RepositoriesUtils
    {
        public static void AddParameter(SqlCommand command, string parameterName, object value)
        {
            if (value == null)
            {
                command.Parameters.AddWithValue(parameterName, DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue(parameterName, value);
            }
        }

        public static int? ConvertToInt(object p_obj)
        {
            if (p_obj == null || p_obj == DBNull.Value)
                return null;
            return Convert.ToInt32(p_obj);
        }

        
    }
}