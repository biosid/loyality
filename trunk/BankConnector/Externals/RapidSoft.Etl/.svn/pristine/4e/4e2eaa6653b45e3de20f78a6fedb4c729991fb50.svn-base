using System;

namespace RapidSoft.Etl.Runtime.DataSources.Csv
{
    public class CsvParseErrorEventArgs : EventArgs
    {
        #region Constructors

        public CsvParseErrorEventArgs(MalformedCsvException error, CsvParseErrorAction defaultAction)
            : base()
        {
            _error = error;
            _action = defaultAction;
        }

        #endregion

        #region Fields

        private readonly MalformedCsvException _error;
        private CsvParseErrorAction _action;

        #endregion

        #region Properties

        public MalformedCsvException Error
        {
            get
            {
                return _error;
            }
        }

        public CsvParseErrorAction Action
        {
            get
            {
                return _action;
            }
            set
            {
                _action = value;
            }
        }

        #endregion
    }
}