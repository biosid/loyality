using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RapidSoft.Etl.Runtime.DataSources.Csv
{
    [Serializable]
    public class CsvSyntaxInfo
    {
        #region Properties

        public bool HasHeaders
        {
            get;
            set;
        }

        public char LineDelimiter1
        {
            get;
            set;
        }

        public char LineDelimiter2
        {
            get;
            set;
        }

        public char FieldDelimiter
        {
            get;
            set;
        }

        public char Quote
        {
            get;
            set;
        }

        public char Escape
        {
            get;
            set;
        }

        #endregion
    }
}
