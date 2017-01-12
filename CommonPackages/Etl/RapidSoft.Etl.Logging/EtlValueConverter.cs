using System;
using System.Data;
using System.Xml.Serialization;

using System.ComponentModel;
using System.Globalization;

namespace RapidSoft.Etl.Logging
{
    public static class EtlValueConverter
    {
        #region Methods

        #region ToString

        public static string ToString(object value)
        {
            return ToString(value, Convert.GetTypeCode(value));
        }

        private static string ToString(object value, TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    return ((Boolean)value).ToString(CultureInfo.InvariantCulture);

                case TypeCode.Byte:
                    return ((Byte)value).ToString(CultureInfo.InvariantCulture);

                case TypeCode.Char:
                    return value.ToString();

                case TypeCode.DateTime:
                    return ((DateTime)value).ToString("o"); //Round-trip date/time pattern; conforms to ISO 8601

                case TypeCode.DBNull:
                    return null;

                case TypeCode.Decimal:
                    // return ((Decimal)value).ToString(CultureInfo.InvariantCulture);
                    return ((Decimal)value).ToString("#.00#", CultureInfo.InvariantCulture); // Decimal is mainly used for money.

                case TypeCode.Double:
                    return ((Double)value).ToString(CultureInfo.InvariantCulture);

                case TypeCode.Empty:
                    return null;

                case TypeCode.Int16:
                    return ((Int16)value).ToString(CultureInfo.InvariantCulture);

                case TypeCode.Int32:
                    return ((Int32)value).ToString(CultureInfo.InvariantCulture);

                case TypeCode.Int64:
                    return ((Int64)value).ToString(CultureInfo.InvariantCulture);

                case TypeCode.SByte:
                    return ((SByte)value).ToString(CultureInfo.InvariantCulture);

                case TypeCode.Single:
                    return ((Single)value).ToString(CultureInfo.InvariantCulture);

                case TypeCode.String:
                    return (string)value;

                case TypeCode.UInt16:
                    return ((UInt16)value).ToString(CultureInfo.InvariantCulture);

                case TypeCode.UInt32:
                    return ((UInt32)value).ToString(CultureInfo.InvariantCulture);

                case TypeCode.UInt64:
                    return ((UInt64)value).ToString(CultureInfo.InvariantCulture);

                case TypeCode.Object:
                default:
                    var type = value.GetType();
                    if (type == typeof(Guid))
                    {
                        return ((Guid)value).ToString("D"); //32 digits separated by hyphens: 00000000-0000-0000-0000-000000000000
                    }
                    else if (type == typeof(DateTimeOffset))
                    {
                        return ((DateTimeOffset)value).ToString("o"); //Round-trip date/time pattern; conforms to ISO 8601
                    }
                    else
                    {
                        return value.ToString();
                    }
            }
        }

        #endregion

        #region ParseType

        public static object ParseType(string str, TypeCode typeCode)
        {
            switch (typeCode)
            { 
                case TypeCode.Empty:
                    return null;

                case TypeCode.DBNull:
                    return null;

                case TypeCode.Boolean:
                    return ParseBoolean(str);

                case TypeCode.Char:
                    return ParseChar(str);

                case TypeCode.SByte:
                    return ParseSByte(str);

                case TypeCode.Byte:
                    return ParseByte(str);

                case TypeCode.Int16:
                    return ParseInt16(str);

                case TypeCode.UInt16:
                    return ParseUInt16(str);

                case TypeCode.Int32:
                    return ParseInt32(str);

                case TypeCode.UInt32:
                    return ParseUInt32(str);

                case TypeCode.Int64:
                    return ParseInt64(str);

                case TypeCode.UInt64:
                    return ParseUInt64(str);

                case TypeCode.Single:
                    return ParseSingle(str);

                case TypeCode.Double:
                    return ParseDouble(str);

                case TypeCode.Decimal:
                    return ParseDecimal(str);

                case TypeCode.DateTime:
                    return ParseDateTime(str);

                case TypeCode.String:
                    return str;

                case TypeCode.Object:
                default:
                    throw new InvalidOperationException(string.Format(Properties.Resources.CannotConvertValueToObject, str));
            }
        }

        #endregion

        #region Boolean

        public static bool TryParseBoolean(string str, out Boolean result)
        {
            if (string.Equals(str, Boolean.TrueString, StringComparison.InvariantCultureIgnoreCase))
            {
                result = true;
                return true;
            }
            else if (string.Equals(str, "1", StringComparison.InvariantCultureIgnoreCase))
            {
                result = true;
                return true;
            }
            else if (string.Equals(str, Boolean.FalseString, StringComparison.InvariantCultureIgnoreCase))
            {
                result = false;
                return true;
            }
            else if (string.Equals(str, "0", StringComparison.InvariantCultureIgnoreCase))
            {
                result = false;
                return true;
            }
            else
            {
                result = false;
                return false;
            }
        }

        public static Boolean ParseBoolean(string str)
        {
            Boolean result;
            if (TryParseBoolean(str, out result))
            {
                return result;
            }
            else
            {
                throw GetFormatException(str, typeof(Boolean));
            }
        }

        public static Boolean? ParseBooleanOrNull(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            return ParseBoolean(str);
        }

        public static bool TryParseBoolean(object obj, out Boolean result)
        {
            var typeCode = Convert.GetTypeCode(obj);
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    result = (Boolean)obj;
                    return true;
                default:
                    return TryParseBoolean(ToString(obj, typeCode), out result);
            }
        }

        public static Boolean ParseBoolean(object obj)
        {
            Boolean result;
            if (TryParseBoolean(obj, out result))
            {
                return result;
            }
            else
            {
                throw GetFormatException(obj, typeof(Boolean));
            }
        }

        public static Boolean? ParseBooleanOrNull(object obj)
        {
            var typeCode = Convert.GetTypeCode(obj);
            switch (typeCode)
            {
                case TypeCode.DBNull:
                case TypeCode.Empty:
                    return null;

                case TypeCode.Boolean:
                    return (Boolean)obj;

                case TypeCode.String:
                    var str = (string)obj;
                    if (str == "")
                    {
                        return null;
                    }
                    else
                    {
                        Boolean result;
                        if (TryParseBoolean(str, out result))
                        {
                            return result;
                        }
                        else
                        {
                            throw GetFormatException(obj, typeof(Boolean));
                        }
                    }

                default:
                    {
                        Boolean result;
                        if (TryParseBoolean(ToString(obj, typeCode), out result))
                        {
                            return result;
                        }
                        else
                        {
                            throw GetFormatException(obj, typeof(Boolean));
                        }
                    }
            }
        }

        #endregion

        #region Byte

        public static bool TryParseByte(string str, out Byte result)
        {
            if (Byte.TryParse(str, NumberStyles.None, CultureInfo.InvariantCulture, out result))
            {
                return true;
            }
            else
            {
                result = default(Byte);
                return false;
            }
        }

        public static Byte ParseByte(string str)
        {
            Byte result;
            if (TryParseByte(str, out result))
            {
                return result;
            }
            else
            {
                throw GetFormatException(str, typeof(Byte));
            }
        }

        public static Byte? ParseByteOrNull(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            return ParseByte(str);
        }

        public static bool TryParseByte(object obj, out Byte result)
        {
            var typeCode = Convert.GetTypeCode(obj);
            switch (typeCode)
            {
                case TypeCode.Byte:
                    result = (Byte)obj;
                    return true;
                default:
                    return TryParseByte(ToString(obj, typeCode), out result);
            }
        }

        public static Byte ParseByte(object obj)
        {
            Byte result;
            if (TryParseByte(obj, out result))
            {
                return result;
            }
            else
            {
                throw GetFormatException(obj, typeof(Byte));
            }
        }

        public static Byte? ParseByteOrNull(object obj)
        {
            var typeCode = Convert.GetTypeCode(obj);
            switch (typeCode)
            {
                case TypeCode.DBNull:
                case TypeCode.Empty:
                    return null;

                case TypeCode.Byte:
                    return (Byte)obj;

                case TypeCode.String:
                    var str = (string)obj;
                    if (str == "")
                    {
                        return null;
                    }
                    else
                    {
                        Byte result;
                        if (TryParseByte(str, out result))
                        {
                            return result;
                        }
                        else
                        {
                            throw GetFormatException(obj, typeof(Byte));
                        }
                    }

                default:
                    {
                        Byte result;
                        if (TryParseByte(ToString(obj, typeCode), out result))
                        {
                            return result;
                        }
                        else
                        {
                            throw GetFormatException(obj, typeof(Byte));
                        }
                    }
            }
        }

        #endregion

        #region Char

        public static bool TryParseChar(string str, out Char result)
        {
            if (str == null || str.Length != 1)
            {
                result = default(Char);
                return false;
            }
            else
            {
                result = str[0];
                return false;
            }
        }

        public static Char ParseChar(string str)
        {
            Char result;
            if (TryParseChar(str, out result))
            {
                return result;
            }
            else
            {
                throw GetFormatException(str, typeof(Char));
            }
        }

        public static Char? ParseCharOrNull(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            return ParseChar(str);
        }

        public static bool TryParseChar(object obj, out Char result)
        {
            var typeCode = Convert.GetTypeCode(obj);
            switch (typeCode)
            {
                case TypeCode.Char:
                    result = (Char)obj;
                    return true;
                default:
                    return TryParseChar(ToString(obj, typeCode), out result);
            }
        }

        public static Char ParseChar(object obj)
        {
            Char result;
            if (TryParseChar(obj, out result))
            {
                return result;
            }
            else
            {
                throw GetFormatException(obj, typeof(Char));
            }
        }

        public static Char? ParseCharOrNull(object obj)
        {
            var typeCode = Convert.GetTypeCode(obj);
            switch (typeCode)
            {
                case TypeCode.DBNull:
                case TypeCode.Empty:
                    return null;

                case TypeCode.Char:
                    return (Char)obj;

                case TypeCode.String:
                    var str = (string)obj;
                    if (str == "")
                    {
                        return null;
                    }
                    else
                    {
                        Char result;
                        if (TryParseChar(str, out result))
                        {
                            return result;
                        }
                        else
                        {
                            throw GetFormatException(obj, typeof(Char));
                        }
                    }

                default:
                    {
                        Char result;
                        if (TryParseChar(ToString(obj, typeCode), out result))
                        {
                            return result;
                        }
                        else
                        {
                            throw GetFormatException(obj, typeof(Char));
                        }
                    }
            }
        }

        #endregion

        #region DateTime

        private static bool TryParseNonEmptyDateTime(string str, out DateTime result)
        {
            if (DateTime.TryParseExact(str, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out result)) //conforms to ISO 8601
            {
                return true;
            }
            else if (DateTime.TryParseExact(str, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out result)) //conforms to ISO 8601
            {
                return true;
            }
            else if (DateTime.TryParseExact(str, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out result)) //conforms to ISO 8601
            {
                return true;
            }
            else if (DateTime.TryParseExact(str, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out result)) //Round-trip date/time pattern; conforms to ISO 8601
            {
                return true;
            }
            else if (DateTime.TryParseExact(str, "s", CultureInfo.InvariantCulture, DateTimeStyles.None, out result)) //Sortable date/time pattern; conforms to ISO 8601
            {
                return true;
            }
            else
            {
                result = default(DateTime);
                return false;
            }
        }

        public static bool TryParseDateTime(string str, out DateTime result)
        {
            if (DateTime.TryParseExact(str, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out result)) //conforms to ISO 8601
            {
                return true;
            }
            else if (DateTime.TryParseExact(str, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out result)) //conforms to ISO 8601
            {
                return true;
            }
            else if (DateTime.TryParseExact(str, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out result)) //conforms to ISO 8601
            {
                return true;
            }
            else if (DateTime.TryParseExact(str, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out result)) //Round-trip date/time pattern; conforms to ISO 8601
            {
                return true;
            }
            else if (DateTime.TryParseExact(str, "s", CultureInfo.InvariantCulture, DateTimeStyles.None, out result)) //Sortable date/time pattern; conforms to ISO 8601
            {
                return true;
            }
            else
            {
                result = default(DateTime);
                return false;
            }
        }

        public static DateTime ParseDateTime(string str)
        {
            DateTime result;
            if (TryParseDateTime(str, out result))
            {
                return result;
            }
            else
            {
                throw GetFormatException(str, typeof(DateTime));
            }
        }

        public static DateTime? ParseDateTimeOrNull(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            return ParseDateTime(str);
        }

        public static bool TryParseDateTime(object obj, out DateTime result)
        {
            var typeCode = Convert.GetTypeCode(obj);
            switch (typeCode)
            {
                case TypeCode.DateTime:
                    result = (DateTime)obj;
                    return true;
                default:
                    return TryParseDateTime(ToString(obj, typeCode), out result);
            }
        }

        public static DateTime ParseDateTime(object obj)
        {
            DateTime result;
            if (TryParseDateTime(obj, out result))
            {
                return result;
            }
            else
            {
                throw GetFormatException(obj, typeof(DateTime));
            }
        }

        public static DateTime? ParseDateTimeOrNull(object obj)
        {
            var typeCode = Convert.GetTypeCode(obj);
            switch (typeCode)
            {
                case TypeCode.DBNull:
                case TypeCode.Empty:
                    return null;

                case TypeCode.DateTime:
                    return (DateTime)obj;

                case TypeCode.String:
                    var str = (string)obj;
                    if (str == "")
                    {
                        return null;
                    }
                    else
                    {
                        DateTime result;
                        if (TryParseDateTime(str, out result))
                        {
                            return result;
                        }
                        else
                        {
                            throw GetFormatException(obj, typeof(DateTime));
                        }
                    }

                default:
                    {
                        DateTime result;
                        if (TryParseDateTime(ToString(obj, typeCode), out result))
                        {
                            return result;
                        }
                        else
                        {
                            throw GetFormatException(obj, typeof(DateTime));
                        }
                    }
            }
        }

        #endregion

        #region Decimal

        public static bool TryParseDecimal(string str, out Decimal result)
        {
            if (Decimal.TryParse(str, NumberStyles.None, CultureInfo.InvariantCulture, out result))
            {
                return true;
            }
            else
            {
                result = default(Decimal);
                return false;
            }
        }

        public static Decimal ParseDecimal(string str)
        {
            Decimal result;
            if (TryParseDecimal(str, out result))
            {
                return result;
            }
            else
            {
                throw GetFormatException(str, typeof(Decimal));
            }
        }

        public static Decimal? ParseDecimalOrNull(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            return ParseDecimal(str);
        }

        public static bool TryParseDecimal(object obj, out Decimal result)
        {
            var typeCode = Convert.GetTypeCode(obj);
            switch (typeCode)
            {
                case TypeCode.Decimal:
                    result = (Decimal)obj;
                    return true;
                default:
                    return TryParseDecimal(ToString(obj, typeCode), out result);
            }
        }

        public static Decimal ParseDecimal(object obj)
        {
            Decimal result;
            if (TryParseDecimal(obj, out result))
            {
                return result;
            }
            else
            {
                throw GetFormatException(obj, typeof(Decimal));
            }
        }

        public static Decimal? ParseDecimalOrNull(object obj)
        {
            var typeCode = Convert.GetTypeCode(obj);
            switch (typeCode)
            {
                case TypeCode.DBNull:
                case TypeCode.Empty:
                    return null;

                case TypeCode.Decimal:
                    return (Decimal)obj;

                case TypeCode.String:
                    var str = (string)obj;
                    if (str == "")
                    {
                        return null;
                    }
                    else
                    {
                        Decimal result;
                        if (TryParseDecimal(str, out result))
                        {
                            return result;
                        }
                        else
                        {
                            throw GetFormatException(obj, typeof(Decimal));
                        }
                    }

                default:
                    {
                        Decimal result;
                        if (TryParseDecimal(ToString(obj, typeCode), out result))
                        {
                            return result;
                        }
                        else
                        {
                            throw GetFormatException(obj, typeof(Decimal));
                        }
                    }
            }
        }

        #endregion

        #region Double

        public static bool TryParseDouble(string str, out Double result)
        {
            if (Double.TryParse(str, NumberStyles.None, CultureInfo.InvariantCulture, out result))
            {
                return true;
            }
            else
            {
                result = default(Double);
                return false;
            }
        }

        public static Double ParseDouble(string str)
        {
            Double result;
            if (TryParseDouble(str, out result))
            {
                return result;
            }
            else
            {
                throw GetFormatException(str, typeof(Double));
            }
        }

        public static Double? ParseDoubleOrNull(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            return ParseDouble(str);
        }

        public static bool TryParseDouble(object obj, out Double result)
        {
            var typeCode = Convert.GetTypeCode(obj);
            switch (typeCode)
            {
                case TypeCode.Double:
                    result = (Double)obj;
                    return true;
                default:
                    return TryParseDouble(ToString(obj, typeCode), out result);
            }
        }

        public static Double ParseDouble(object obj)
        {
            Double result;
            if (TryParseDouble(obj, out result))
            {
                return result;
            }
            else
            {
                throw GetFormatException(obj, typeof(Double));
            }
        }

        public static Double? ParseDoubleOrNull(object obj)
        {
            var typeCode = Convert.GetTypeCode(obj);
            switch (typeCode)
            {
                case TypeCode.DBNull:
                case TypeCode.Empty:
                    return null;

                case TypeCode.Double:
                    return (Double)obj;

                case TypeCode.String:
                    var str = (string)obj;
                    if (str == "")
                    {
                        return null;
                    }
                    else
                    {
                        Double result;
                        if (TryParseDouble(str, out result))
                        {
                            return result;
                        }
                        else
                        {
                            throw GetFormatException(obj, typeof(Double));
                        }
                    }

                default:
                    {
                        Double result;
                        if (TryParseDouble(ToString(obj, typeCode), out result))
                        {
                            return result;
                        }
                        else
                        {
                            throw GetFormatException(obj, typeof(Double));
                        }
                    }
            }
        }

        #endregion

        #region Guid

        public static bool TryParseGuid(string str, out Guid result)
        {
            if (string.IsNullOrEmpty(str))
            {
                result = Guid.Empty;
                return false;
            }

            try
            {
                result = new Guid(str);
                return true;
            }
            catch (FormatException)
            {
                result = Guid.Empty;
                return false;
            }
        }

        public static Guid ParseGuid(string str)
        {
            Guid result;
            if (TryParseGuid(str, out result))
            {
                return result;
            }
            else
            {
                throw GetFormatException(str, typeof(Guid));
            }
        }

        public static Guid? ParseGuidOrNull(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            return ParseGuid(str);
        }

        public static bool TryParseGuid(object obj, out Guid result)
        {
            if (obj == null)
            {
                result = Guid.Empty;
                return false;
            }
            else if (obj is Guid)
            {
                result = (Guid)obj;
                return true;
            }
            else
            {
                return TryParseGuid(ToString(obj), out result);
            }
        }

        public static Guid ParseGuid(object obj)
        {
            Guid result;
            if (TryParseGuid(obj, out result))
            {
                return result;
            }
            else
            {
                throw GetFormatException(obj, typeof(Guid));
            }
        }

        public static Guid? ParseGuidOrNull(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            else if (obj is Guid)
            {
                return (Guid)obj;
            }
            else if (obj is string)
            {
                var str = (string)obj;
                if (str == "")
                {
                    return null;
                }
                else
                {
                    Guid result;
                    if (TryParseGuid(str, out result))
                    {
                        return result;
                    }
                    else
                    {
                        throw GetFormatException(obj, typeof(Guid));
                    }
                }
            }
            else
            {
                Guid result;
                if (TryParseGuid(ToString(obj), out result))
                {
                    return result;
                }
                else
                {
                    throw GetFormatException(obj, typeof(Guid));
                }
            }
        }

        #endregion

        #region Int16

        public static bool TryParseInt16(string str, out Int16 result)
        {
            if (Int16.TryParse(str, NumberStyles.None, CultureInfo.InvariantCulture, out result))
            {
                return true;
            }
            else
            {
                result = default(Int16);
                return false;
            }
        }

        public static Int16 ParseInt16(string str)
        {
            Int16 result;
            if (TryParseInt16(str, out result))
            {
                return result;
            }
            else
            {
                throw GetFormatException(str, typeof(Int16));
            }
        }

        public static Int16? ParseInt16OrNull(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            return ParseInt16(str);
        }

        public static bool TryParseInt16(object obj, out Int16 result)
        {
            var typeCode = Convert.GetTypeCode(obj);
            switch (typeCode)
            {
                case TypeCode.Int16:
                    result = (Int16)obj;
                    return true;
                default:
                    return TryParseInt16(ToString(obj, typeCode), out result);
            }
        }

        public static Int16 ParseInt16(object obj)
        {
            Int16 result;
            if (TryParseInt16(obj, out result))
            {
                return result;
            }
            else
            {
                throw GetFormatException(obj, typeof(Int16));
            }
        }

        public static Int16? ParseInt16OrNull(object obj)
        {
            var typeCode = Convert.GetTypeCode(obj);
            switch (typeCode)
            {
                case TypeCode.DBNull:
                case TypeCode.Empty:
                    return null;

                case TypeCode.Int16:
                    return (Int16)obj;

                case TypeCode.String:
                    var str = (string)obj;
                    if (str == "")
                    {
                        return null;
                    }
                    else
                    {
                        Int16 result;
                        if (TryParseInt16(str, out result))
                        {
                            return result;
                        }
                        else
                        {
                            throw GetFormatException(obj, typeof(Int16));
                        }
                    }

                default:
                    {
                        Int16 result;
                        if (TryParseInt16(ToString(obj, typeCode), out result))
                        {
                            return result;
                        }
                        else
                        {
                            throw GetFormatException(obj, typeof(Int16));
                        }
                    }
            }
        }

        #endregion

        #region Int32

        public static bool TryParseInt32(string str, out Int32 result)
        {
            if (Int32.TryParse(str, NumberStyles.None, CultureInfo.InvariantCulture, out result))
            {
                return true;
            }
            else
            {
                result = default(Int32);
                return false;
            }
        }

        public static Int32 ParseInt32(string str)
        {
            Int32 result;
            if (TryParseInt32(str, out result))
            {
                return result;
            }
            else
            {
                throw GetFormatException(str, typeof(Int32));
            }
        }

        public static Int32? ParseInt32OrNull(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            return ParseInt32(str);
        }

        public static bool TryParseInt32(object obj, out Int32 result)
        {
            var typeCode = Convert.GetTypeCode(obj);
            switch (typeCode)
            {
                case TypeCode.Int32:
                    result = (Int32)obj;
                    return true;
                default:
                    return TryParseInt32(ToString(obj, typeCode), out result);
            }
        }

        public static Int32 ParseInt32(object obj)
        {
            Int32 result;
            if (TryParseInt32(obj, out result))
            {
                return result;
            }
            else
            {
                throw GetFormatException(obj, typeof(Int32));
            }
        }

        public static Int32? ParseInt32OrNull(object obj)
        {
            var typeCode = Convert.GetTypeCode(obj);
            switch (typeCode)
            {
                case TypeCode.DBNull:
                case TypeCode.Empty:
                    return null;

                case TypeCode.Int32:
                    return (Int32)obj;

                case TypeCode.String:
                    var str = (string)obj;
                    if (str == "")
                    {
                        return null;
                    }
                    else
                    {
                        Int32 result;
                        if (TryParseInt32(str, out result))
                        {
                            return result;
                        }
                        else
                        {
                            throw GetFormatException(obj, typeof(Int32));
                        }
                    }

                default:
                    {
                        Int32 result;
                        if (TryParseInt32(ToString(obj, typeCode), out result))
                        {
                            return result;
                        }
                        else
                        {
                            throw GetFormatException(obj, typeof(Int32));
                        }
                    }
            }
        }

        #endregion

        #region Int64

        public static bool TryParseInt64(string str, out Int64 result)
        {
            if (Int64.TryParse(str, NumberStyles.None, CultureInfo.InvariantCulture, out result))
            {
                return true;
            }
            else
            {
                result = default(Int64);
                return false;
            }
        }

        public static Int64 ParseInt64(string str)
        {
            Int64 result;
            if (TryParseInt64(str, out result))
            {
                return result;
            }
            else
            {
                throw GetFormatException(str, typeof(Int64));
            }
        }

        public static Int64? ParseInt64OrNull(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            return ParseInt64(str);
        }

        public static bool TryParseInt64(object obj, out Int64 result)
        {
            var typeCode = Convert.GetTypeCode(obj);
            switch (typeCode)
            {
                case TypeCode.Int64:
                    result = (Int64)obj;
                    return true;
                default:
                    return TryParseInt64(ToString(obj, typeCode), out result);
            }
        }

        public static Int64 ParseInt64(object obj)
        {
            Int64 result;
            if (TryParseInt64(obj, out result))
            {
                return result;
            }
            else
            {
                throw GetFormatException(obj, typeof(Int64));
            }
        }

        public static Int64? ParseInt64OrNull(object obj)
        {
            var typeCode = Convert.GetTypeCode(obj);
            switch (typeCode)
            {
                case TypeCode.DBNull:
                case TypeCode.Empty:
                    return null;

                case TypeCode.Int64:
                    return (Int64)obj;

                case TypeCode.String:
                    var str = (string)obj;
                    if (str == "")
                    {
                        return null;
                    }
                    else
                    {
                        Int64 result;
                        if (TryParseInt64(str, out result))
                        {
                            return result;
                        }
                        else
                        {
                            throw GetFormatException(obj, typeof(Int64));
                        }
                    }

                default:
                    {
                        Int64 result;
                        if (TryParseInt64(ToString(obj, typeCode), out result))
                        {
                            return result;
                        }
                        else
                        {
                            throw GetFormatException(obj, typeof(Int64));
                        }
                    }
            }
        }

        #endregion

        #region SByte

        public static bool TryParseSByte(string str, out SByte result)
        {
            if (SByte.TryParse(str, NumberStyles.None, CultureInfo.InvariantCulture, out result))
            {
                return true;
            }
            else
            {
                result = default(SByte);
                return false;
            }
        }

        public static SByte ParseSByte(string str)
        {
            SByte result;
            if (TryParseSByte(str, out result))
            {
                return result;
            }
            else
            {
                throw GetFormatException(str, typeof(SByte));
            }
        }

        public static SByte? ParseSByteOrNull(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            return ParseSByte(str);
        }

        public static bool TryParseSByte(object obj, out SByte result)
        {
            var typeCode = Convert.GetTypeCode(obj);
            switch (typeCode)
            {
                case TypeCode.SByte:
                    result = (SByte)obj;
                    return true;
                default:
                    return TryParseSByte(ToString(obj, typeCode), out result);
            }
        }

        public static SByte ParseSByte(object obj)
        {
            SByte result;
            if (TryParseSByte(obj, out result))
            {
                return result;
            }
            else
            {
                throw GetFormatException(obj, typeof(SByte));
            }
        }

        public static SByte? ParseSByteOrNull(object obj)
        {
            var typeCode = Convert.GetTypeCode(obj);
            switch (typeCode)
            {
                case TypeCode.DBNull:
                case TypeCode.Empty:
                    return null;

                case TypeCode.SByte:
                    return (SByte)obj;

                case TypeCode.String:
                    var str = (string)obj;
                    if (str == "")
                    {
                        return null;
                    }
                    else
                    {
                        SByte result;
                        if (TryParseSByte(str, out result))
                        {
                            return result;
                        }
                        else
                        {
                            throw GetFormatException(obj, typeof(SByte));
                        }
                    }

                default:
                    {
                        SByte result;
                        if (TryParseSByte(ToString(obj, typeCode), out result))
                        {
                            return result;
                        }
                        else
                        {
                            throw GetFormatException(obj, typeof(SByte));
                        }
                    }
            }
        }

        #endregion

        #region Single

        public static bool TryParseSingle(string str, out Single result)
        {
            if (Single.TryParse(str, NumberStyles.None, CultureInfo.InvariantCulture, out result))
            {
                return true;
            }
            else
            {
                result = default(Single);
                return false;
            }
        }

        public static Single ParseSingle(string str)
        {
            Single result;
            if (TryParseSingle(str, out result))
            {
                return result;
            }
            else
            {
                throw GetFormatException(str, typeof(Single));
            }
        }

        public static Single? ParseSingleOrNull(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            return ParseSingle(str);
        }

        public static bool TryParseSingle(object obj, out Single result)
        {
            var typeCode = Convert.GetTypeCode(obj);
            switch (typeCode)
            {
                case TypeCode.Single:
                    result = (Single)obj;
                    return true;
                default:
                    return TryParseSingle(ToString(obj, typeCode), out result);
            }
        }

        public static Single ParseSingle(object obj)
        {
            Single result;
            if (TryParseSingle(obj, out result))
            {
                return result;
            }
            else
            {
                throw GetFormatException(obj, typeof(Single));
            }
        }

        public static Single? ParseSingleOrNull(object obj)
        {
            var typeCode = Convert.GetTypeCode(obj);
            switch (typeCode)
            {
                case TypeCode.DBNull:
                case TypeCode.Empty:
                    return null;

                case TypeCode.Single:
                    return (Single)obj;

                case TypeCode.String:
                    var str = (string)obj;
                    if (str == "")
                    {
                        return null;
                    }
                    else
                    {
                        Single result;
                        if (TryParseSingle(str, out result))
                        {
                            return result;
                        }
                        else
                        {
                            throw GetFormatException(obj, typeof(Single));
                        }
                    }

                default:
                    {
                        Single result;
                        if (TryParseSingle(ToString(obj, typeCode), out result))
                        {
                            return result;
                        }
                        else
                        {
                            throw GetFormatException(obj, typeof(Single));
                        }
                    }
            }
        }

        #endregion

        #region UInt16

        public static bool TryParseUInt16(string str, out UInt16 result)
        {
            if (UInt16.TryParse(str, NumberStyles.None, CultureInfo.InvariantCulture, out result))
            {
                return true;
            }
            else
            {
                result = default(UInt16);
                return false;
            }
        }

        public static UInt16 ParseUInt16(string str)
        {
            UInt16 result;
            if (TryParseUInt16(str, out result))
            {
                return result;
            }
            else
            {
                throw GetFormatException(str, typeof(UInt16));
            }
        }

        public static UInt16? ParseUInt16OrNull(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            return ParseUInt16(str);
        }

        public static bool TryParseUInt16(object obj, out UInt16 result)
        {
            var typeCode = Convert.GetTypeCode(obj);
            switch (typeCode)
            {
                case TypeCode.UInt16:
                    result = (UInt16)obj;
                    return true;
                default:
                    return TryParseUInt16(ToString(obj, typeCode), out result);
            }
        }

        public static UInt16 ParseUInt16(object obj)
        {
            UInt16 result;
            if (TryParseUInt16(obj, out result))
            {
                return result;
            }
            else
            {
                throw GetFormatException(obj, typeof(UInt16));
            }
        }

        public static UInt16? ParseUInt16OrNull(object obj)
        {
            var typeCode = Convert.GetTypeCode(obj);
            switch (typeCode)
            {
                case TypeCode.DBNull:
                case TypeCode.Empty:
                    return null;

                case TypeCode.UInt16:
                    return (UInt16)obj;

                case TypeCode.String:
                    var str = (string)obj;
                    if (str == "")
                    {
                        return null;
                    }
                    else
                    {
                        UInt16 result;
                        if (TryParseUInt16(str, out result))
                        {
                            return result;
                        }
                        else
                        {
                            throw GetFormatException(obj, typeof(UInt16));
                        }
                    }

                default:
                    {
                        UInt16 result;
                        if (TryParseUInt16(ToString(obj, typeCode), out result))
                        {
                            return result;
                        }
                        else
                        {
                            throw GetFormatException(obj, typeof(UInt16));
                        }
                    }
            }
        }

        #endregion

        #region UInt32

        public static bool TryParseUInt32(string str, out UInt32 result)
        {
            if (UInt32.TryParse(str, NumberStyles.None, CultureInfo.InvariantCulture, out result))
            {
                return true;
            }
            else
            {
                result = default(UInt32);
                return false;
            }
        }

        public static UInt32 ParseUInt32(string str)
        {
            UInt32 result;
            if (TryParseUInt32(str, out result))
            {
                return result;
            }
            else
            {
                throw GetFormatException(str, typeof(UInt32));
            }
        }

        public static UInt32? ParseUInt32OrNull(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            return ParseUInt32(str);
        }

        public static bool TryParseUInt32(object obj, out UInt32 result)
        {
            var typeCode = Convert.GetTypeCode(obj);
            switch (typeCode)
            {
                case TypeCode.UInt32:
                    result = (UInt32)obj;
                    return true;
                default:
                    return TryParseUInt32(ToString(obj, typeCode), out result);
            }
        }

        public static UInt32 ParseUInt32(object obj)
        {
            UInt32 result;
            if (TryParseUInt32(obj, out result))
            {
                return result;
            }
            else
            {
                throw GetFormatException(obj, typeof(UInt32));
            }
        }

        public static UInt32? ParseUInt32OrNull(object obj)
        {
            var typeCode = Convert.GetTypeCode(obj);
            switch (typeCode)
            {
                case TypeCode.DBNull:
                case TypeCode.Empty:
                    return null;

                case TypeCode.UInt32:
                    return (UInt32)obj;

                case TypeCode.String:
                    var str = (string)obj;
                    if (str == "")
                    {
                        return null;
                    }
                    else
                    {
                        UInt32 result;
                        if (TryParseUInt32(str, out result))
                        {
                            return result;
                        }
                        else
                        {
                            throw GetFormatException(obj, typeof(UInt32));
                        }
                    }

                default:
                    {
                        UInt32 result;
                        if (TryParseUInt32(ToString(obj, typeCode), out result))
                        {
                            return result;
                        }
                        else
                        {
                            throw GetFormatException(obj, typeof(UInt32));
                        }
                    }
            }
        }

        #endregion

        #region UInt64

        public static bool TryParseUInt64(string str, out UInt64 result)
        {
            if (UInt64.TryParse(str, NumberStyles.None, CultureInfo.InvariantCulture, out result))
            {
                return true;
            }
            else
            {
                result = default(UInt64);
                return false;
            }
        }

        public static UInt64 ParseUInt64(string str)
        {
            UInt64 result;
            if (TryParseUInt64(str, out result))
            {
                return result;
            }
            else
            {
                throw GetFormatException(str, typeof(UInt64));
            }
        }

        public static UInt64? ParseUInt64OrNull(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            return ParseUInt64(str);
        }

        public static bool TryParseUInt64(object obj, out UInt64 result)
        {
            var typeCode = Convert.GetTypeCode(obj);
            switch (typeCode)
            {
                case TypeCode.UInt64:
                    result = (UInt64)obj;
                    return true;
                default:
                    return TryParseUInt64(ToString(obj, typeCode), out result);
            }
        }

        public static UInt64 ParseUInt64(object obj)
        {
            UInt64 result;
            if (TryParseUInt64(obj, out result))
            {
                return result;
            }
            else
            {
                throw GetFormatException(obj, typeof(UInt64));
            }
        }

        public static UInt64? ParseUInt64OrNull(object obj)
        {
            var typeCode = Convert.GetTypeCode(obj);
            switch (typeCode)
            {
                case TypeCode.DBNull:
                case TypeCode.Empty:
                    return null;

                case TypeCode.UInt64:
                    return (UInt64)obj;

                case TypeCode.String:
                    var str = (string)obj;
                    if (str == "")
                    {
                        return null;
                    }
                    else
                    {
                        UInt64 result;
                        if (TryParseUInt64(str, out result))
                        {
                            return result;
                        }
                        else
                        {
                            throw GetFormatException(obj, typeof(UInt64));
                        }
                    }

                default:
                    {
                        UInt64 result;
                        if (TryParseUInt64(ToString(obj, typeCode), out result))
                        {
                            return result;
                        }
                        else
                        {
                            throw GetFormatException(obj, typeof(UInt64));
                        }
                    }
            }
        }

        #endregion

        #region Exceptions

        private static Exception GetFormatException(object obj, Type type)
        {
            throw new FormatException(string.Format(
                "String \"{0}\" was not recognized as a valid {1}",
                ToString(obj),
                type.Name
            ));
        }

        private static Exception GetStringifyException(object obj, Type type)
        {
            return new InvalidOperationException(string.Format(
                "Cannot convert value of type {0} to string. Only primitive types supported",
                obj.GetType().FullName
            ));
        }

        #endregion

        #endregion
    }
}
