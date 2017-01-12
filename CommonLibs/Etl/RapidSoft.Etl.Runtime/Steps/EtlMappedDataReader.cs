using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

using RapidSoft.Etl.Logging;

namespace RapidSoft.Etl.Runtime.Steps
{
    //todo: add try-catch for name reading and translating
    internal sealed class EtlMappedDataReader : IDataReader
    {
        #region Constructors

        public EtlMappedDataReader(IDataReader sourceReader, ICollection<EtlFieldMapping> mappings)
        {
            if (sourceReader == null)
            {
                throw new ArgumentNullException("sourceReader");
            }

            if (mappings == null)
            {
                throw new ArgumentNullException("mappings");
            }

            if (mappings.Count == 0)
            {
                throw new ArgumentException("Parameter \"mappings\" cannot be empty", "mappings");
            }

            _sourceReader = sourceReader;
            _mappings = new List<EtlFieldMapping>(mappings);
        }

        #endregion

        #region Constants

        private const string TOO_LONG_EXCEPTION_VALUE_SUFFIX = "...";
        private const int SHORT_STRING_MAX_LENGTH = 255;

        #endregion

        #region Fields

        private static readonly Type _defaultFieldType = typeof(String);

        private readonly IDataReader _sourceReader;
        private readonly List<EtlFieldMapping> _mappings;

        #endregion

        #region IDataRecord Members

        public int FieldCount
        {
            [DebuggerStepThrough]
            get
            {
                return _mappings.Count;
            }
        }

        public Boolean GetBoolean(int i)
        {
            var mapping = GetMapping(i);
            var value = MapValue(mapping);

            bool result;
            if (EtlValueConverter.TryParseBoolean(value, out result))
            {
                return result;
            }
            else
            {
                throw GetConvertException(value, mapping, typeof(Boolean));
            }
        }

        public Byte GetByte(int i)
        {
            var mapping = GetMapping(i);
            var value = MapValue(mapping);

            Byte result;
            if (EtlValueConverter.TryParseByte(value, out result))
            {
                return result;
            }
            else
            {
                throw GetConvertException(value, mapping, typeof(Byte));
            }
        }

        public long GetBytes(int i, long fieldoffset, byte[] buffer, int bufferoffset, int length)
        {
            var mapping = GetMapping(i);
            var value = MapValue(mapping);

            if (value == null || value == DBNull.Value)
            {
                return 0;
            }

            var bytes = (byte[])value;
            if (buffer == null)
            {
                return bytes.Length;
            }

            var copiedCount = 0;
            var bufferPos = bufferoffset;

            for (var bytePos = fieldoffset; bytePos < bytes.Length; bytePos++)
            {
                if (copiedCount >= length)
                {
                    break;
                }

                buffer[bufferPos] = bytes[bytePos];
                bufferPos = 0;
            }

            return copiedCount;
        }

        public Char GetChar(int i)
        {
            var mapping = GetMapping(i);
            var value = MapValue(mapping);

            Char result;
            if (EtlValueConverter.TryParseChar(value, out result))
            {
                return result;
            }
            else
            {
                throw GetConvertException(value, mapping, typeof(Char));
            }
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            var mapping = GetMapping(i);
            var value = MapValue(mapping);
            if (value == null || value == DBNull.Value)
            {
                return 0;
            }

            var bytes = (char[])value;
            if (buffer == null)
            {
                return bytes.Length;
            }

            var copiedCount = 0;
            var bufferPos = bufferoffset;

            for (var bytePos = fieldoffset; bytePos < bytes.Length; bytePos++)
            {
                if (copiedCount >= length)
                {
                    break;
                }

                buffer[bufferPos] = bytes[bytePos];
                bufferPos = 0;
            }

            return copiedCount;
        }

        public IDataReader GetData(int i)
        {
            var mapping = GetMapping(i);
            var value = MapValue(mapping);
            var result = value as IDataReader;

            if (result == null)
            {
                throw GetConvertException(value, mapping, typeof(IDataReader));
            }
            else
            {
                return result;
            }
        }

        public string GetDataTypeName(int i)
        {
            GetMapping(i);
            return _defaultFieldType.FullName;
        }

        public DateTime GetDateTime(int i)
        {
            var mapping = GetMapping(i);
            var value = MapValue(mapping);

            DateTime result;
            if (EtlValueConverter.TryParseDateTime(value, out result))
            {
                return result;
            }
            else
            {
                throw GetConvertException(value, mapping, typeof(DateTime));
            }
        }

        public Decimal GetDecimal(int i)
        {
            var mapping = GetMapping(i);
            var value = MapValue(mapping);

            Decimal result;
            if (EtlValueConverter.TryParseDecimal(value, out result))
            {
                return result;
            }
            else
            {
                throw GetConvertException(value, mapping, typeof(DateTime));
            }
        }

        public Double GetDouble(int i)
        {
            var mapping = GetMapping(i);
            var value = MapValue(mapping);

            Double result;
            if (EtlValueConverter.TryParseDouble(value, out result))
            {
                return result;
            }
            else
            {
                throw GetConvertException(value, mapping, typeof(Double));
            }
        }

        public Type GetFieldType(int i)
        {
            GetMapping(i);
            return _defaultFieldType;
        }

        public Single GetFloat(int i)
        {
            var mapping = GetMapping(i);
            var value = MapValue(mapping);

            Single result;
            if (EtlValueConverter.TryParseSingle(value, out result))
            {
                return result;
            }
            else
            {
                throw GetConvertException(value, mapping, typeof(Single));
            }
        }

        public Guid GetGuid(int i)
        {
            var mapping = GetMapping(i);
            var value = MapValue(mapping);

            Guid result;
            if (EtlValueConverter.TryParseGuid(value, out result))
            {
                return result;
            }
            else
            {
                throw GetConvertException(value, mapping, typeof(Guid));
            }
        }

        public Int16 GetInt16(int i)
        {
            var mapping = GetMapping(i);
            var value = MapValue(mapping);

            Int16 result;
            if (EtlValueConverter.TryParseInt16(value, out result))
            {
                return result;
            }
            else
            {
                throw GetConvertException(value, mapping, typeof(Int16));
            }
        }

        public Int32 GetInt32(int i)
        {
            var mapping = GetMapping(i);
            var value = MapValue(mapping);

            Int32 result;
            if (EtlValueConverter.TryParseInt32(value, out result))
            {
                return result;
            }
            else
            {
                throw GetConvertException(value, mapping, typeof(Int32));
            }
        }

        public long GetInt64(int i)
        {
            var mapping = GetMapping(i);
            var value = MapValue(mapping);

            Int64 result;
            if (EtlValueConverter.TryParseInt64(value, out result))
            {
                return result;
            }
            else
            {
                throw GetConvertException(value, mapping, typeof(Int64));
            }
        }

        public string GetName(int i)
        {
            var mapping = GetMapping(i);
            return mapping.DestinationFieldName;
        }

        public int GetOrdinal(string name)
        {
            for (var i = 0; i < _mappings.Count; i++)
            {
                if (string.Equals(_mappings[i].DestinationFieldName, name, StringComparison.InvariantCulture))
                {
                    return i;
                }
            }

            throw new IndexOutOfRangeException(string.Format(Properties.Resources.FieldNotFound, name));
        }

        public string GetString(int i)
        {
            var mapping = GetMapping(i);
            var value = MapValue(mapping);
            return EtlValueConverter.ToString(value);
        }

        public object GetValue(int i)
        {
            var mapping = GetMapping(i);
            var value = MapValue(mapping);
            return value;
        }

        public int GetValues(object[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            var countToCopy = Math.Min(values.Length, this.FieldCount);
            for (var i = 0; i < countToCopy; i++)
            {
                values[i] = GetValue(i);
            }
            return countToCopy;
        }

        public bool IsDBNull(int i)
        {
            var value = GetValue(i);
            return value == DBNull.Value;
        }

        public object this[int i]
        {
            get
            {
                return GetValue(i);
            }
        }

        public object this[string name]
        {
            get
            {
                var index = GetOrdinal(name);
                var value = GetValue(index);
                return value;
            }
        }

        #endregion

        #region IDataReader Members

        public void Close()
        {
            _sourceReader.Close();
        }

        public int Depth
        {
            get
            {
                return _sourceReader.Depth;
            }
        }

        public DataTable GetSchemaTable()
        {
            return null;
        }

        public bool IsClosed
        {
            get
            {
                return _sourceReader.IsClosed;
            }
        }

        public bool NextResult()
        {
            return _sourceReader.NextResult();
        }

        public bool Read()
        {
            return _sourceReader.Read();
        }

        public int RecordsAffected
        {
            get
            {
                return _sourceReader.RecordsAffected;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _sourceReader.Dispose();
        }

        #endregion

        #region Methods

        private EtlFieldMapping GetMapping(int i)
        {
            if (i < 0 || i >= _mappings.Count)
            {
                throw new IndexOutOfRangeException(string.Format(Properties.Resources.FieldNotMapped, i));
            }

            return _mappings[i];
        }

        private object MapValue(EtlFieldMapping mapping)
        {
            var mappedValue = EtlValueTranslation.Evaluate(mapping.SourceFieldName, mapping.SourceFieldTranslation, _sourceReader, mapping.DefaultValue);
            return mappedValue;
        }

        private Exception GetConvertException(object value, EtlFieldMapping mapping, Type targetType)
        {
            return new FormatException
            (
                string.Format
                (
                    Properties.Resources.FieldCannotBeConverted,
                    mapping.SourceFieldName,
                    value,
                    targetType.FullName
                )
            );
        }

        #endregion
    }
}
