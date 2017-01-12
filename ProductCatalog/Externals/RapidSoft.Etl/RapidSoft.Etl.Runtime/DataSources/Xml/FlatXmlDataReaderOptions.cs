using System;
using System.Collections.Generic;

namespace RapidSoft.Etl.Runtime.DataSources.Xml
{
    [Serializable]
    public struct FlatXmlDataReaderOptions
    {
        public bool TreatEmptyStringAsNull
        {
            get;
            set;
        }
    }
}
