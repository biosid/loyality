using System;
using System.Globalization;

namespace RapidSoft.Extensions
{
    public static class StringParsingExtensions
    {
        public static bool? ParseNullableBool(this string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return null;
            }
            bool result;
            return bool.TryParse(str, out result) ? (bool?) result : null;
        }

        public static int? ParseNullableInt(this string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return null;
            }
            int result;
            return int.TryParse(str, out result) ? (int?)result : null;
        }

        public static long? ParseNullableLong(this string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return null;
            }
            long result;
            return long.TryParse(str, out result) ? (long?)result : null;
        }

        public static decimal? ParseNullableDecimal(this string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return null;
            }
            decimal result;
            return decimal.TryParse(str, out result) ? (decimal?)result : null;
        }

        public static decimal? ParseNullableDecimal(this string str, IFormatProvider formatProvider)
        {
            if (String.IsNullOrEmpty(str))
            {
                return null;
            }
            decimal result;
            return decimal.TryParse(str, NumberStyles.Any, formatProvider, out result) ? (decimal?)result : null;
        }

        public static DateTime? ParseNullableDateTime(this string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return null;
            }
            DateTime result;
            return DateTime.TryParse(str, out result) ? (DateTime?)result : null;
        }

        public static DateTime? ParseNullableDateTime(this string str, IFormatProvider formatProvider)
        {
            if (String.IsNullOrEmpty(str))
            {
                return null;
            }
            DateTime result;
            return DateTime.TryParse(str, formatProvider, DateTimeStyles.None ,out result) ? (DateTime?)result : null;
        }

        public static Guid? ParseNullableGuid(this string str)
        {
            try
            {
                return new Guid(str);
            } catch
            {
                return null;
            }
        }
    }
}