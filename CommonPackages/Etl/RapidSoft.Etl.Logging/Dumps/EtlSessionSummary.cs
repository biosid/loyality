using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace RapidSoft.Etl.Logging.Dumps
{
    [Serializable]
    public class EtlSessionSummary
    {
        public EtlSessionSummary()
        {
            this.Variables = new List<EtlVariable>();
            this.Counters = new List<EtlCounter>();
            this.Messages = new List<EtlMessage>();
        }

        public string EtlPackageId
        {
            get;
            set;
        }

        public string EtlPackageName
        {
            get;
            set;
        }

        public string EtlSessionId
        {
            get;
            set;
        }

        public DateTime? StartUtcDateTime
        {
            get;
            set;
        }

        public DateTime? StartDateTime
        {
            get;
            set;
        }

        public DateTime? EndDateTime
        {
            get;
            set;
        }

        public DateTime? EndUtcDateTime
        {
            get;
            set;
        }

        public EtlStatus Status
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        [XmlArrayItem("Variable")]
        public List<EtlVariable> Variables { get; private set; }

        [XmlArrayItem("Counter")]
        public List<EtlCounter> Counters { get; private set; }

        [XmlArrayItem("Message")]
        public List<EtlMessage> Messages { get; set; }

        public int MessageTotalCount { get; set; }

        public int MessageSkipCount { get; set; }

        public int MessageTakeCount { get; set; }

        public EtlMessage LastErrorMessage
        {
            get;
            set;
        }
    }
}