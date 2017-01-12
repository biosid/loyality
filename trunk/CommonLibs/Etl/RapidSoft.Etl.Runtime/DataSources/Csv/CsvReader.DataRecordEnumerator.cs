using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace RapidSoft.Etl.Runtime.DataSources.Csv
{
    public partial class CsvReader
    {
        public class DataRecordEnumerator : IEnumerator<IDataRecord>
        {
            #region Constructors

            public DataRecordEnumerator(CsvReader reader)
            {
                if (reader == null)
                {
                    throw new ArgumentNullException("reader");
                }

                _reader = reader;
                //_current = null;
                _currentRecordIndex = reader._currentRecordIndex;
            }

            #endregion

            #region Fields

            private CsvReader _reader;
            private long _currentRecordIndex;

            #endregion

            #region IEnumerator<IDataRecord> Members

            public IDataRecord Current
            {
                get
                {
                    return _reader;
                }
            }

            public bool MoveNext()
            {
                if (_reader._currentRecordIndex != _currentRecordIndex)
                {
                    throw new InvalidOperationException(Properties.Resources.CsvReaderEnumerationVersionCheckFailed);
                }

                if (_reader.ReadNextRecord(false, false))
                {
                    //_current = new string[_sourceReader._fieldCount];
                    //_sourceReader.CopyCurrentRecordTo(_current);
                    _currentRecordIndex = _reader._currentRecordIndex;

                    return true;
                }
                else
                {
                    //_current = null;
                    _currentRecordIndex = _reader._currentRecordIndex;

                    return false;
                }
            }

            #endregion

            #region IEnumerator Members

            public void Reset()
            {
                if (_reader._currentRecordIndex != _currentRecordIndex)
                {
                    throw new InvalidOperationException(Properties.Resources.CsvReaderEnumerationVersionCheckFailed);
                }

                _reader.MoveTo(-1);

                //_current = null;
                _currentRecordIndex = _reader._currentRecordIndex;
            }

            object IEnumerator.Current
            {
                get
                {
                    if (_reader._currentRecordIndex != _currentRecordIndex)
                    {
                        throw new InvalidOperationException(Properties.Resources.CsvReaderEnumerationVersionCheckFailed);
                    }

                    return this.Current;
                }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                _reader = null;
                //_current = null;
            }

            #endregion
        }
    }
}