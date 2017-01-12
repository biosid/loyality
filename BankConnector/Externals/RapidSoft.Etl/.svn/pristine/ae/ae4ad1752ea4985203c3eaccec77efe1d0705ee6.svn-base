using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace RapidSoft.Etl.Runtime.DataSources.Csv
{
    [Serializable()]
    public class MissingFieldCsvException : MalformedCsvException
    {
        #region Constructors

        public MissingFieldCsvException()
            : base()
        {
        }

        public MissingFieldCsvException(string message)
            : base(message)
        {
        }

        public MissingFieldCsvException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public MissingFieldCsvException(string rawData, int currentPosition, long currentRecordIndex, int currentFieldIndex)
            : base(rawData, currentPosition, currentRecordIndex, currentFieldIndex)
        {
        }

        public MissingFieldCsvException(string rawData, int currentPosition, long currentRecordIndex, int currentFieldIndex, Exception innerException)
            : base(rawData, currentPosition, currentRecordIndex, currentFieldIndex, innerException)
        {
        }

        protected MissingFieldCsvException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}