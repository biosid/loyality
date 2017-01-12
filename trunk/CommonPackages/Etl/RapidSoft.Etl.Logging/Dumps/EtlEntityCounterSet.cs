using System;
using System.Xml.Serialization;

namespace RapidSoft.Etl.Logging.Dumps
{
    [Serializable]
    public class EtlEntityCounterSet
    {
        public string EntityName
        {
            get;
            set;
        }

        [XmlArrayItem("Counter")]
        public EtlCounter[] Counters
        {
            get;
            set;
        }
    }
}