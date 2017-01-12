using System;

namespace Vtb24.Site.Services.Infrastructure
{
    public class PluralizerFormatter: IFormatProvider, ICustomFormatter
    {
        public object GetFormat(Type formatType)
        {
            return (formatType == typeof(ICustomFormatter)) ? this : null;
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            format = format ?? "";
            var parts = format.Split('|');

            if (parts[0].ToLower() != "pluralize" || parts.Length < 3)
            {
                return FormatDefault(format, arg, formatProvider);
            }

            var singular = parts[1];
            var plural = parts[2];
            var plural2 = parts.Length == 4 ? parts[3] : null;


            if (arg is int || arg is long || arg is byte || arg is short)
            {
                return Convert.ToInt64(arg).Decline(singular, plural, plural2);
            }
            if (arg is double || arg is float)
            {
                return Convert.ToDouble(arg).Decline(singular, plural, plural2);
            }
            if (arg is decimal)
            {
                return Convert.ToDecimal(arg).Decline(singular, plural, plural2);
            }

            return FormatDefault(format, arg, formatProvider);
        }

        private static string FormatDefault(string format, object arg, IFormatProvider formatProvider)
        {
            var formattable = arg as IFormattable;
            return formattable == null ? arg.ToString() : formattable.ToString(format, formatProvider);
        }
    }
}