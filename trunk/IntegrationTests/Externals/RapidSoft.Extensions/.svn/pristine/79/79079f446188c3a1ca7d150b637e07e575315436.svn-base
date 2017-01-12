namespace RapidSoft.Extensions
{
    using System;

    public static class DBNullableExtensions
    {
        public static object ToDBValue<T>(this T? value) where T : struct
        {
            return value.HasValue ? (object)value.Value : DBNull.Value;
        }

        public static object ToDBValue<T>(this T value)
        {
            return value == null ? DBNull.Value : (object)value;
        }
    }
}