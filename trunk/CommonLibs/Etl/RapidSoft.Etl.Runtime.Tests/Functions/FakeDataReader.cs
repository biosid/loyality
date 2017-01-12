using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Xml;

namespace RapidSoft.Etl.Runtime.Tests.Functions
{
    [DebuggerDisplay("FieldCount = {FieldCount}")]
    internal sealed class FakeDataRecord : IDataRecord
    {
        #region Constructors

        public FakeDataRecord()
        {
        }

        public FakeDataRecord(string fieldName, object value)
        {
            _fields.Add(new Field(fieldName, value));
        }

        public FakeDataRecord(IDictionary<string, object> dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }

            foreach (var pair in dictionary)
            {
                _fields.Add(new Field(pair.Key, pair.Value));
            }
        }

        #endregion

        #region Fields

        private readonly List<Field> _fields = new List<Field>();

        #endregion

        #region Properties

        public int FieldCount
        {
            [DebuggerStepThrough]
            get
            {
                return _fields.Count;
            }
        }

        public object this[string fieldName]
        {
            get
            {
                return GetValue(fieldName);
            }
        }

        #endregion

        #region Methods

        private int FindFieldIndex(string fieldName)
        {
            for (var i = 0; i < _fields.Count; i++)
            {
                var field = _fields[i];

                if (field.HasTheSameName(fieldName))
                {
                    return i;
                }
            }

            return -1;
        }

        private int FindFieldIndex(string fieldName, object value)
        {
            for (var i = 0; i < _fields.Count; i++)
            {
                var field = _fields[i];

                if (field.IsTheSame(fieldName, value))
                {
                    return i;
                }
            }

            return -1;
        }

        private Field FindField(string fieldName)
        {
            for (var i = 0; i < _fields.Count; i++)
            {
                var field = _fields[i];

                if (field.HasTheSameName(fieldName))
                {
                    return field;
                }
            }

            return null;
        }

        public object GetValue(string fieldName)
        {
            if (fieldName == null)
            {
                throw new ArgumentNullException("fieldName");
            }

            var field = FindField(fieldName);

            if (field == null)
            {
                return null;
            }
            else
            {
                return field.Value;
            }
        }

        private static Exception GetFieldNullException(string fieldName, Type expectedType)
        {
            return new InvalidCastException
            (
                string.Format
                (
                    "Field \"{0}\" is null but expected to has value of type \"{1}\"",
                    fieldName,
                    expectedType.FullName
                )
            );
        }

        private static Exception GetFieldNullException(int i, Type expectedType)
        {
            return new InvalidCastException
            (
                string.Format
                (
                    "Field {0} is null but expected to has value of type \"{1}\"",
                    i,
                    expectedType.FullName
                )
            );
        }
        #endregion

        #region IDataRecord Members

        public bool GetBoolean(int i)
        {
            var value = GetValue(i);

            if (value != null && value != DBNull.Value)
            {
                var b = Convert.ToBoolean(value);
                return b;
            }
            else
            {
                throw GetFieldNullException(i, typeof(Boolean));
            }
        }

        public byte GetByte(int i)
        {
            var value = GetValue(i);

            if (value != null && value != DBNull.Value)
            {
                var b = Convert.ToByte(value);
                return b;
            }
            else
            {
                throw GetFieldNullException(i, typeof(Byte));
            }
        }

        public long GetBytes(int i, long fieldoffset, byte[] buffer, int bufferoffset, int length)
        {
            var value = GetValue(i);

            if (value != null && value != DBNull.Value)
            {
                var bytes = (byte[])value;

                if (buffer == null)
                {
                    return bytes.Length;
                }
                else
                {
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
            }
            else
            {
                return 0;
            }
        }

        public char GetChar(int i)
        {
            var value = GetValue(i);

            if (value != null && value != DBNull.Value)
            {
                return Char.Parse(value.ToString());
            }
            else
            {
                throw GetFieldNullException(i, typeof(Char));
            }
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            var value = GetValue(i);

            if (value != null && value != DBNull.Value)
            {
                var bytes = (char[])value;

                if (buffer == null)
                {
                    return bytes.Length;
                }
                else
                {
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
            }
            else
            {
                return 0;
            }
        }

        public IDataReader GetData(int i)
        {
            var field = _fields[i];

            if (field.Value != null)
            {
                throw new InvalidCastException(string.Format("Cannot convert value of type {0} to value of type {1}", field.Value.GetType(), typeof(IDataReader)));
            }
            else
            {
                throw new InvalidCastException(string.Format("Cannot convert null to value of type {0}", typeof(IDataReader)));
            }
        }

        public string GetDataTypeName(int i)
        {
            var field = _fields[i];

            if (field.Value == null)
            {
                return null;
            }
            else
            {
                return field.Value.GetType().Name;
            }
        }

        public DateTime GetDateTime(int i)
        {
            var value = GetValue(i);

            if (value != null && value != DBNull.Value)
            {
                return DateTime.Parse(value.ToString());
            }
            else
            {
                throw GetFieldNullException(i, typeof(DateTime));
            }
        }

        public decimal GetDecimal(int i)
        {
            var value = GetValue(i);

            if (value != null && value != DBNull.Value)
            {
                return Decimal.Parse(value.ToString());
            }
            else
            {
                throw GetFieldNullException(i, typeof(Decimal));
            }
        }

        public double GetDouble(int i)
        {
            var value = GetValue(i);

            if (value != null && value != DBNull.Value)
            {
                return Double.Parse(value.ToString());
            }
            else
            {
                throw GetFieldNullException(i, typeof(Double));
            }
        }

        public Type GetFieldType(int i)
        {
            var field = _fields[i];

            if (field.Value == null)
            {
                return null;
            }
            else
            {
                return field.Value.GetType();
            }
        }

        public float GetFloat(int i)
        {
            var value = GetValue(i);

            if (value != null && value != DBNull.Value)
            {
                return Single.Parse(value.ToString());
            }
            else
            {
                throw GetFieldNullException(i, typeof(Single));
            }
        }

        public Guid GetGuid(int i)
        {
            var value = GetValue(i);

            if (value != null && value != DBNull.Value)
            {
                return new Guid(value.ToString());
            }
            else
            {
                throw GetFieldNullException(i, typeof(Guid));
            }
        }

        public short GetInt16(int i)
        {
            var value = GetValue(i);

            if (value != null && value != DBNull.Value)
            {
                return Int16.Parse(value.ToString());
            }
            else
            {
                throw GetFieldNullException(i, typeof(Int16));
            }
        }

        public int GetInt32(int i)
        {
            var value = GetValue(i);

            if (value != null && value != DBNull.Value)
            {
                return Int32.Parse(value.ToString());
            }
            else
            {
                throw GetFieldNullException(i, typeof(Int32));
            }
        }

        public long GetInt64(int i)
        {
            var value = GetValue(i);

            if (value != null && value != DBNull.Value)
            {
                return Int64.Parse(value.ToString());
            }
            else
            {
                throw GetFieldNullException(i, typeof(Int64));
            }
        }

        public string GetName(int i)
        {
            var field = _fields[i];
            return field.Name;
        }

        public int GetOrdinal(string name)
        {
            var index = FindFieldIndex(name);
            return index;
        }

        public string GetString(int i)
        {
            var value = GetValue(i);

            if (value != null && value != DBNull.Value)
            {
                return value.ToString();
            }
            else
            {
                return null;
            }
        }

        public object GetValue(int i)
        {
            var field = _fields[i];
            return field.Value;
        }

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
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

        #endregion

        //#region IDataReader Members

        //public void Close()
        //{
        //    _isClosed = true;
        //}

        //public int Depth
        //{
        //    get
        //    {
        //        return 0;
        //    }
        //}

        //public DataTable GetSchemaTable()
        //{
        //    return null;
        //}

        //public bool IsClosed
        //{
        //    get
        //    {
        //        return _isClosed;
        //    }
        //}

        //public bool NextResult()
        //{
        //    return false;
        //}

        //public bool Read()
        //{
        //    if (!_wasRead)
        //    {
        //        _wasRead = true;
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public int RecordsAffected
        //{
        //    get
        //    {
        //        return 0;
        //    }
        //}

        //#endregion

        //#region IDisposable Members

        //public void Dispose()
        //{
        //}

        //#endregion

        #region Nested classes

        [Serializable]
        [DebuggerDisplay("{Name}, {Value}")]
        private sealed class Field
        {
            #region Constructors

            public Field()
            {
            }

            public Field(string name, object value)
            {
                this.Name = name;
                this.Value = value;
            }

            #endregion

            #region Fields

            public string Name;
            public object Value;

            #endregion

            #region Methods

            public bool IsTheSame(Field field)
            {
                if (field == null)
                {
                    return false;
                }

                return IsTheSame(field.Name, field.Value);
            }

            public bool IsTheSame(string name, object value)
            {
                if (!HasTheSameName(name))
                {
                    return false;
                }

                if (!object.Equals(this.Value, value))
                {
                    return false;
                }

                return true;
            }

            public bool HasTheSameName(string name)
            {
                return string.Equals(this.Name, name, StringComparison.CurrentCulture);
            }

            #endregion
        }

        #endregion
    }
}
