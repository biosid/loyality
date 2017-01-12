using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RapidSoft.Etl.Runtime.DataSources.DB
{
    [Serializable]
    [Flags]
    internal enum DBTableWriterErrorFlags
    {
        ReadError = 1,
        WriteError = 2
    }
}
