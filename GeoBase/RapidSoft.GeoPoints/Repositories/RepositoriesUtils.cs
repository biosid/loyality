using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace RapidSoft.GeoPoints.Repositories
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

            if (p_obj is int)
                return (int)p_obj;

            return Convert.ToInt32(p_obj);
        }

        public static Guid ConvertToGuid(object p_obj)
        {
            if (p_obj == null || p_obj == DBNull.Value)
                return Guid.Empty;

            if (p_obj is Guid)
                return (Guid)p_obj;

            return new Guid(p_obj.ToString());
        }

        public static Guid? GetNullableGuidValue(Object obj)
        {
            return obj == null || obj == DBNull.Value ? (Guid?)null : new Guid(obj.ToString());
        }
    }
}