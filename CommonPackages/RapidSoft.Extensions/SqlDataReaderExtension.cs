using System;
using System.Data.SqlClient;

namespace RapidSoft.Extensions
{
    using System.Data;
    using System.Data.SqlTypes;

    public static class SqlDataReaderExtension
    {
        public static Guid GetGuid(this SqlDataReader reader, string fieldName)
        {
            var ordinal = reader.GetOrdinal(fieldName);
            return reader.GetGuid(ordinal);
        }

        public static Guid? GetGuidOrNull(this SqlDataReader reader, string fieldName)
        {
            var ordinal = reader.GetOrdinal(fieldName);
            var guidOrNull = reader.GetSqlGuid(ordinal);
            if (guidOrNull.IsNull)
            {
                return null;
            }
            return guidOrNull.Value;
        }

        public static string GetStringOrNull(this SqlDataReader reader, string fieldName)
        {
            var ordinal = reader.GetOrdinal(fieldName);
            var sqlVal = reader[ordinal];

            return Convert.IsDBNull(sqlVal) ? null : Convert.ToString(sqlVal);
        }

        public static DateTime? GetDateTimeOrNull(this SqlDataReader reader, string fieldName)
        {
            var ordinal = reader.GetOrdinal(fieldName);
            var sqlString = reader.GetSqlDateTime(ordinal);
            if (sqlString.IsNull)
            {
                return null;
            }
            return sqlString.Value;
        }

        public static DateTime GetDateTime(this SqlDataReader reader, string fieldName)
        {
            var ordinal = reader.GetOrdinal(fieldName);
            var sqlString = reader.GetSqlDateTime(ordinal);
            if (sqlString.IsNull)
            {
                throw new InvalidOperationException(string.Format("DateTime {0} dbVal.IsNull", fieldName));                
            }
            return sqlString.Value;
        }

        public static decimal? GetDecimalOrNull(this SqlDataReader reader, string fieldName)
        {
            var ordinal = reader.GetOrdinal(fieldName);
            var sqlVal = reader[ordinal];
            return Convert.IsDBNull(sqlVal) ? (decimal?)null : Convert.ToDecimal(sqlVal);
        }

        public static decimal Round(this decimal num)
        {
            return Math.Round(num, MidpointRounding.AwayFromZero);
        }

        public static decimal? Round(this decimal? num)
        {
            return num.HasValue ? Math.Round(num.Value, MidpointRounding.AwayFromZero) : (decimal?)null;
        }

        public static string GetString(this SqlDataReader reader, string fieldName)
        {
            var ordinal = reader.GetOrdinal(fieldName);
            return reader.GetString(ordinal);
        }

        public static int GetInt32(this SqlDataReader reader, string fieldName)
        {
            var ordinal = reader.GetOrdinal(fieldName);
            return reader.GetInt32(ordinal);
        }

        public static long GetInt64(this SqlDataReader reader, string fieldName)
        {
            var ordinal = reader.GetOrdinal(fieldName);
            return reader.GetInt64(ordinal);
        }

        public static int? GetInt32OrNull(this SqlDataReader reader, string fieldName)
        {
            var ordinal = reader.GetOrdinal(fieldName);
            var dbVal = reader.GetSqlInt32(ordinal);
            if (dbVal.IsNull)
            {
                return null;
            }
            return dbVal.Value;
        }

        public static T GetValueOrDefault<T>(this SqlDataReader reader, string fieldName)
        {
            var index = reader.GetOrdinal(fieldName);
            return reader.IsDBNull(index) ? default(T) : (T)reader.GetValue(index);
        }

        public static decimal GetDecimal(this SqlDataReader reader, string fieldName)
        {
            var ordinal = reader.GetOrdinal(fieldName);
            return reader.GetDecimal(ordinal);
        }

        public static bool GetBoolean(this SqlDataReader reader, string fieldName)
        {
            var ordinal = reader.GetOrdinal(fieldName);
            return reader.GetBoolean(ordinal);
        }

        public static T GetEnum<T>(this SqlDataReader reader, string fieldName)
        {
            var ordinal = reader.GetOrdinal(fieldName);
            var dbVal = reader.GetSqlInt32(ordinal);
            if (dbVal.IsNull)
            {
                throw new InvalidOperationException(string.Format("Enum {0} dbVal.IsNull", fieldName));
            }
            return (T)Enum.Parse(typeof(T), dbVal.Value.ToString());
        }
    }
}