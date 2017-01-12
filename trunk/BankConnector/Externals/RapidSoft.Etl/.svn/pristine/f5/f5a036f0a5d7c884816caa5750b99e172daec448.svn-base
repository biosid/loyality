using System;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace RapidSoft.Etl.Console
{
    [Serializable]
    public class EtlMailReportConfiguration : IConfigurationSectionHandler
    {
        #region Constructors

        public EtlMailReportConfiguration()
            : this(null)
        {
        }

        public EtlMailReportConfiguration(object section)
        {
            if (section != null)
            {
                var outerConfig = (EtlMailReportConfiguration)section;
                _subscriptions.AddRange(outerConfig.Subscriptions);
            }
        }

        #endregion

        #region Fields

        private static EtlMailReportConfiguration _rootElement;

        #endregion

        #region Properties

        public static EtlMailReportConfiguration Current
        {
            get
            {
                if (_rootElement == null)
                {
                    _rootElement = (EtlMailReportConfiguration)ConfigurationManager.GetSection(typeof(EtlMailReportConfiguration).Name);
                }
                return _rootElement;
            }
        }

        [XmlArrayItem("Subscription")]
        public List<EtlMailSubscription> Subscriptions
        {
            get
            {
                return _subscriptions;
            }
        }
        private readonly List<EtlMailSubscription> _subscriptions = new List<EtlMailSubscription>();

        #endregion

        #region IConfigurationSectionHandler implementation

        object IConfigurationSectionHandler.Create(object parent, object configContext, XmlNode section)
        {
            var node = section;
            var nodeName = section.Name;

            using (var rd = new XmlNodeReader(node))
            {
                var ser = new XmlSerializer(typeof(EtlMailReportConfiguration), new XmlRootAttribute(nodeName));
                var element = ser.Deserialize(rd);

                var custom = Activator.CreateInstance(GetType(), element);
                return custom;
            }
        }

        #endregion
    }
}