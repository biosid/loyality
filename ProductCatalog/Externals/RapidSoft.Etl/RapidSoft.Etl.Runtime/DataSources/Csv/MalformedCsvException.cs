using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;

using RapidSoft.Etl.Runtime.Properties;

namespace RapidSoft.Etl.Runtime.DataSources.Csv
{
    [Serializable()]
    public class MalformedCsvException : Exception
    {
        #region Constructors

        public MalformedCsvException()
            : this(null, null)
        {
        }

        /// <summary>
        public MalformedCsvException(string message)
            : this(message, null)
        {
        }

        public MalformedCsvException(string message, Exception innerException)
            : base(String.Empty, innerException)
        {
            _message = (message == null ? string.Empty : message);

            _rawData = string.Empty;
            _currentPosition = -1;
            _currentRecordIndex = -1;
            _currentFieldIndex = -1;
        }

        public MalformedCsvException(string rawData, int currentPosition, long currentRecordIndex, int currentFieldIndex)
            : this(rawData, currentPosition, currentRecordIndex, currentFieldIndex, null)
        {
        }

        public MalformedCsvException(string rawData, int currentPosition, long currentRecordIndex, int currentFieldIndex, Exception innerException)
            : base(String.Empty, innerException)
        {
            _rawData = (rawData == null ? string.Empty : rawData);
            _currentPosition = currentPosition;
            _currentRecordIndex = currentRecordIndex;
            _currentFieldIndex = currentFieldIndex;

            _message = String.Format(CultureInfo.InvariantCulture, Properties.Resources.CsvReaderMalformedCsvException, _currentRecordIndex, _currentFieldIndex, _currentPosition, _rawData);
        }

        public MalformedCsvException(string message, string rawData, int currentPosition, long currentRecordIndex, int currentFieldIndex, Exception innerException)
            : base(message, innerException)
        {
            _rawData = (rawData == null ? string.Empty : rawData);
            _currentPosition = currentPosition;
            _currentRecordIndex = currentRecordIndex;
            _currentFieldIndex = currentFieldIndex;

            _message = message;
        }

        protected MalformedCsvException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _message = info.GetString("MyMessage");

            _rawData = info.GetString("RawData");
            _currentPosition = info.GetInt32("CurrentPosition");
            _currentRecordIndex = info.GetInt64("CurrentRecordIndex");
            _currentFieldIndex = info.GetInt32("CurrentFieldIndex");
        }

        #endregion

        #region Fields

        private readonly string _message;
        private readonly string _rawData;
        private readonly int _currentFieldIndex;
        private readonly long _currentRecordIndex;
        private readonly int _currentPosition;

        #endregion

        #region Properties

        public string RawData
        {
            get
            {
                return _rawData;
            }
        }
        public int CurrentPosition
        {
            get
            {
                return _currentPosition;
            }
        }

        public long CurrentRecordIndex
        {
            get
            {
                return _currentRecordIndex;
            }
        }

        public int CurrentFieldIndex
        {
            get
            {
                return _currentFieldIndex;
            }
        }

        #endregion

        #region Overrides

        public override string Message
        {
            get
            {
                return _message;
            }
        }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("MyMessage", _message);

            info.AddValue("RawData", _rawData);
            info.AddValue("CurrentPosition", _currentPosition);
            info.AddValue("CurrentRecordIndex", _currentRecordIndex);
            info.AddValue("CurrentFieldIndex", _currentFieldIndex);
        }

        #endregion
    }
}