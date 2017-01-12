using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;

using RapidSoft.Etl.Runtime.Agents;
using RapidSoft.Etl.Logging;

namespace RapidSoft.Etl.Console
{
    [Serializable]
    public sealed class EtlMailSubscription
    {
        public string From
        {
            get;
            set;
        }

        public string Subject
        {
            get;
            set;
        }

        public string MailTemplatePath
        {
            get;
            set;
        }

        public bool EnableSsl
        {
            get;
            set;
        }

        public bool AllowEmptyMail
        {
            get;
            set;
        }

        [XmlArrayItem("EtlAgent")]
        public List<EtlAgentInfo> EtlAgents
        {
            [DebuggerStepThrough]
            get
            {
                return _agents;
            }
        }
        private readonly List<EtlAgentInfo> _agents = new List<EtlAgentInfo>();

        public int OnLastSeconds
        {
            get;
            set;
        }

        [XmlArrayItem("EtlStatus")]
        public List<EtlStatus> EtlStatuses
        {
            [DebuggerStepThrough]
            get
            {
                return _etlStatuses;
            }
        }
        private readonly List<EtlStatus> _etlStatuses = new List<EtlStatus>();

        [XmlArrayItem("Subscriber")]
        public List<EtlMailSubscriber> Subscribers
        {
            [DebuggerStepThrough]
            get
            {
                return _subscribers;
            }
        }
        private readonly List<EtlMailSubscriber> _subscribers = new List<EtlMailSubscriber>();
    }
}