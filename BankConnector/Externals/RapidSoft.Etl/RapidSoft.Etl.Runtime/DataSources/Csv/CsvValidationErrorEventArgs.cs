using System;

namespace RapidSoft.Etl.Runtime.DataSources.Csv
{
    public class CsvValidationErrorEventArgs : EventArgs
    {
        #region Constructors

        public CsvValidationErrorEventArgs
        (
            string errorMessage,
            long? errorPosition,
            long errorRecordIndex,
            int? errorFieldIndex,
            string errorFieldName
        )
        {
            _errorMessage = errorMessage;
            _errorPosition = errorPosition;
            _errorRecordIndex = errorRecordIndex;
            _errorFieldIndex = errorFieldIndex;
            _errorFieldName = errorFieldName;
        }

        #endregion

        #region Fields

        private readonly string _errorMessage;
        private readonly long? _errorPosition;
        private readonly long _errorRecordIndex;
        private readonly int? _errorFieldIndex;
        private readonly string _errorFieldName;

        #endregion

        #region Properties

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
        }

        public long? ErrorPosition
        {
            get
            {
                return _errorPosition;
            }
        }

        public long ErrorRecordIndex
        {
            get
            {
                return _errorRecordIndex;
            }
        }

        public int? ErrorFieldIndex
        {
            get
            {
                return _errorFieldIndex;
            }
        }

        public string ErrorFieldName
        {
            get
            {
                return _errorFieldName;
            }
        }

        #endregion
    }
}