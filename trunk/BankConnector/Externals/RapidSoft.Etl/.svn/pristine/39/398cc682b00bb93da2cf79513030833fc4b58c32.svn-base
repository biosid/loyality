using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace RapidSoft.Etl.Runtime.DataSources.Xml
{
    public class FlatXmlDataReader : IDataReader
    {
        #region Constructors

        public FlatXmlDataReader(XmlReader reader, string dataElementPath, string[] fields, FlatXmlDataReaderOptions options)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            if (dataElementPath == null)
            {
                throw new ArgumentNullException("dataElementPath");
            }

            if (dataElementPath.Length == 0)
            {
                throw new ArgumentException("Parameter \"dataElementPath\" cannot be empty", "dataElementPath");
            }

            if (fields == null)
            {
                throw new ArgumentNullException("fields");
            }

            if (fields.Length == 0)
            {
                throw new ArgumentException("Parameter \"fields\" cannot be empty", "fields");
            }

            _xmlReader = reader;
            _fields = fields;
            _options = options;

            _dataElementPath = dataElementPath;
            _dataElementPathItems = SimpleXPathParser.Parse(dataElementPath);
            _dataElementPathItemIndex = _dataElementPathItems.Length - 1;
            _dataElementName = _dataElementPathItems[_dataElementPathItemIndex];

            MoveToDataElement();
        }

        #endregion

        #region Fields

        private static readonly Type _fieldType = typeof(string);

        private readonly XmlReader _xmlReader;
        private readonly string _dataElementPath;

        private readonly string[] _dataElementPathItems;
        private readonly string[] _fields;

        private readonly int _dataElementPathItemIndex;
        private readonly string _dataElementName;

        private readonly FlatXmlDataReaderOptions _options;

        private string[] _currentValues = null;
        private int _currentLevel = -1;
        private int _itemCount = 0;
        private bool _eof = false;
        private bool _isClosed = false;        

        #endregion

        #region Properties

        public int FieldCount
        {
            [DebuggerStepThrough]
            get
            {
                return _fields.Length;
            }
        }

        public string this[string name]
        {
            get
            {
                var index = GetIndex(name);
                return this[index];
            }
        }

        public string this[int i]
        {
            get
            {
                CheckIndex(i);
                return _currentValues[i];
            }
        }

        public int Depth
        {
            [DebuggerStepThrough]
            get
            {
                return 1;
            }
        }

        public bool IsClosed
        {
            [DebuggerStepThrough]
            get
            {
                return _isClosed;
            }
        }

        public int RecordsAffected
        {
            [DebuggerStepThrough]
            get
            {
                return -1;
            }
        }

        #endregion

        #region Methods

        private void MoveToDataElement()
        {
            var parentElementPathItemIndex = _dataElementPathItems.Length - 2;

            if (parentElementPathItemIndex < 0)
            {
                return;
            }

            var elements = new Stack<string>();
            var found = false;

            while (_xmlReader.Read())
            {
                if (_xmlReader.NodeType == XmlNodeType.Element)
                {
                    if (!_xmlReader.IsEmptyElement)
                    {
                        _currentLevel++;
                        elements.Push(_xmlReader.LocalName);

                        if (_currentLevel < _dataElementPathItems.Length)
                        {
                            if (XmlNameComparer.IsEqualElementNames(_xmlReader.LocalName, _dataElementPathItems[_currentLevel]))
                            {
                                if (_currentLevel == parentElementPathItemIndex)
                                {
                                    found = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (_xmlReader.NodeType == XmlNodeType.EndElement)
                {
                    var name = elements.Pop();
                    _currentLevel--;

                    if (!XmlNameComparer.IsEqualElementNames(name, _xmlReader.LocalName))
                    {
                        throw new FormatException(string.Format(Properties.Resources.XmlReaderExpectedClosingTag, name, _xmlReader.LocalName));
                    }
                }
            }

            if (!found)
            {
                throw new FormatException(string.Format(Properties.Resources.XmlReaderElementNotFound, _dataElementPath));
            }
        }

        public bool Read()
        {            
            _currentValues = null;

            if (_eof)
            {
                return false;
            }

            if (!_xmlReader.Read())
            {
                _eof = true;
                return false;
            }

            do
            {
                if (_xmlReader.NodeType == XmlNodeType.Element) 
                {
                    var level = _currentLevel + 1;

                    if (!_xmlReader.IsEmptyElement)
                    {
                        _currentLevel++;
                    }

                    if (level != _dataElementPathItemIndex)
                    {
                        throw new FormatException(string.Format(Properties.Resources.XmlReaderExpectedElement, _dataElementName, _xmlReader.LocalName));
                    }
                       
                    if (XmlNameComparer.IsEqualElementNames(_xmlReader.LocalName, _dataElementName))
                    {
                        _currentValues = new string[this.FieldCount];
                        ReadItemAttributes();
                        _itemCount++;

                        return true;
                    }
                }
                else if (_xmlReader.NodeType == XmlNodeType.EndElement)
                {
                    if (_currentLevel < _dataElementPathItemIndex)
                    {
                        _eof = true;
                        return false;
                    }

                    _currentLevel--;
                }
            } while (_xmlReader.Read());

            return false;
        }

        private void ReadItemAttributes()
        {
            if (_xmlReader.AttributeCount == 0)
            {
                return;
            }

            for (var i = 0; i < _xmlReader.AttributeCount; i++)
            {
                _xmlReader.MoveToAttribute(i);

                var fieldIndex = GetIndex(_xmlReader.LocalName);
                if (fieldIndex >= 0)
                {
                    _currentValues[fieldIndex] = ReadValue();
                }
            }

            _xmlReader.MoveToElement();
        }

        private string ReadValue()
        {
            //if (_xmlReader.IsEmptyElement)
            //{
            //    return null;
            //}

            var value = _xmlReader.Value;
            if (_options.TreatEmptyStringAsNull && string.IsNullOrEmpty(value))
            {
                return null;
            }
            else
            {
                return value;
            }
        }

        private int GetIndex(string name)
        {
            for (var i = 0; i < _fields.Length; i++)
            {
                if (XmlNameComparer.IsEqualElementNames(_fields[i], name))
                {
                    return i;
                }
            }

            return -1;
        }

        private Exception GetCastException(Type type)
        {
            return new InvalidCastException(string.Format(Properties.Resources.XmlReaderCannotConvertValue, type.FullName));
        }

        private void CheckIndex(int i)
        {
            if (i < 0 || i >= _fields.Length)
            {
                throw new IndexOutOfRangeException(string.Format(Properties.Resources.XmlReaderFieldIndexNotFound, i));
            }
        }

        #endregion

        #region IDataReader Members

        object IDataRecord.this[string name]
        {
            get
            {
                var index = GetIndex(name);
                return GetValue(index);
            }
        }

        object IDataRecord.this[int i]
        {
            get
            {
                return GetValue(i);
            }
        }

        public DataTable GetSchemaTable()
        {
            return null;
        }

        public bool NextResult()
        {
            return false;
        }

        public void Close()
        {
            _isClosed = true;
            _xmlReader.Close();
        }

        public bool GetBoolean(int i)
        {
            CheckIndex(i);
            throw GetCastException(typeof(Boolean));
        }

        public byte GetByte(int i)
        {
            CheckIndex(i);
            throw GetCastException(typeof(Byte));
        }

        public long GetBytes(int i, long fieldoffset, byte[] buffer, int bufferoffset, int length)
        {
            var value = GetValue(i);
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

        public char GetChar(int i)
        {
            CheckIndex(i);
            throw GetCastException(typeof(Char));
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            var value = GetValue(i);
            if (value == null)
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
            CheckIndex(i);
            throw GetCastException(typeof(IDataReader));
        }

        public string GetDataTypeName(int i)
        {
            CheckIndex(i);
            return _fieldType.FullName;
        }

        public DateTime GetDateTime(int i)
        {
            CheckIndex(i);
            throw GetCastException(typeof(DateTime));
        }

        public decimal GetDecimal(int i)
        {
            CheckIndex(i);
            throw GetCastException(typeof(Decimal));
        }

        public double GetDouble(int i)
        {
            CheckIndex(i);
            throw GetCastException(typeof(Double));
        }

        public Type GetFieldType(int i)
        {
            CheckIndex(i);
            return _fieldType;
        }

        public float GetFloat(int i)
        {
            CheckIndex(i);
            throw GetCastException(typeof(Single));
        }

        public Guid GetGuid(int i)
        {
            CheckIndex(i);
            throw GetCastException(typeof(Guid));
        }

        public short GetInt16(int i)
        {
            CheckIndex(i);
            throw GetCastException(typeof(Int16));
        }

        public int GetInt32(int i)
        {
            CheckIndex(i);
            throw GetCastException(typeof(Int32));
        }

        public long GetInt64(int i)
        {
            CheckIndex(i);
            throw GetCastException(typeof(Int64));
        }

        public string GetName(int i)
        {
            CheckIndex(i);
            return _fields[i];
        }

        public int GetOrdinal(string name)
        {
            var fieldIndex = GetIndex(name);
            if (fieldIndex < 0)
            {
                throw new IndexOutOfRangeException(string.Format(Properties.Resources.XmlReaderFieldNameNotFound, name));
            }
            return fieldIndex;
        }

        public string GetString(int i)
        {
            return this[i];
        }

        public object GetValue(int i)
        {
            return this[i];
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
                values[i] = this[i];
            }
            return countToCopy;
        }

        public bool IsDBNull(int i)
        {
            return false;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            ((IDisposable)_xmlReader).Dispose();
        }

        #endregion
    }
}
