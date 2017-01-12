using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RapidSoft.Etl.Logging.Dumps
{
    [Serializable]
    public class EtlDump
    {
        public DateTime DumpDateTime
        {
            get;
            set;
        }

        public DateTime DumpUtcDateTime
        {
            get;
            set;
        }

        [XmlArrayItem("Session")]
        public List<EtlSessionSummary> Sessions
        {
            [DebuggerStepThrough]
            get
            {
                return _sessions;
            }
        }
        private readonly List<EtlSessionSummary> _sessions = new List<EtlSessionSummary>();
    }
}