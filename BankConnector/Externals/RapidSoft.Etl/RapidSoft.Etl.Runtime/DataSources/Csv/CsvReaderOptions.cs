using System;

namespace RapidSoft.Etl.Runtime.DataSources.Csv
{
    [Serializable]
    public struct CsvReaderOptions
    {
        public int BufferSize { get; set; }

        public ValueTrimmingOptions ValueTrimmingOptions { get; set; }

        public bool PreserveEmptyStrings { get; set; }

        public bool SkipEmptyRows { get; set; }

        public bool NullForSkipColumn { get; set; }
    }
}
