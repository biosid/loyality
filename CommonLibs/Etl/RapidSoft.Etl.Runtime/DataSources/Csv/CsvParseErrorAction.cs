using System;

namespace RapidSoft.Etl.Runtime.DataSources.Csv
{
    public enum CsvParseErrorAction
    {
        RaiseEvent = 0,
        AdvanceToNextLine = 1,
        ThrowException = 2,
    }
}
