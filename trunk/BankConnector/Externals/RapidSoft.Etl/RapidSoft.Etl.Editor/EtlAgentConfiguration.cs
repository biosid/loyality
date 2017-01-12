using System;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using RapidSoft.Etl.Logging;
using System.Diagnostics;

using RapidSoft.Etl.Runtime.Agents;

namespace RapidSoft.Etl.Editor
{
    [Serializable]
    public class EtlAgentConfiguration : IConfigurationSectionHandler
    {
        #region Constructors

        public EtlAgentConfiguration()
            : this(null)
        {
        }

        public EtlAgentConfiguration(object section)
        {
            if (section != null)
            {
                var outerConfig = (EtlAgentConfiguration)section;
                _etlAgents.AddRange(outerConfig.EtlAgents);
            }
        }

        #endregion

        #region Fields

        private static EtlAgentConfiguration _rootElement;

        #endregion

        #region Properties

        public static EtlAgentConfiguration Current
        {
            get
            {
                if (_rootElement == null)
                {
                    _rootElement = (EtlAgentConfiguration)ConfigurationManager.GetSection(typeof(EtlAgentConfiguration).Name);
                }
                return _rootElement;
            }
        }


        [XmlArrayItem("EtlAgent")]
        public List<EtlAgentInfo> EtlAgents
        {
            [DebuggerStepThrough]
            get
            {
                return _etlAgents;
            }
        }
        private readonly List<EtlAgentInfo> _etlAgents = new List<EtlAgentInfo>();

        #endregion

        #region IConfigurationSectionHandler implementation

        object IConfigurationSectionHandler.Create(object parent, object configContext, XmlNode section)
        {
            var node = section;
            var nodeName = section.Name;

            using (var rd = new XmlNodeReader(node))
            {
                var ser = new XmlSerializer(typeof(EtlAgentConfiguration), new XmlRootAttribute(nodeName));
                var element = ser.Deserialize(rd);

                var custom = Activator.CreateInstance(GetType(), element);
                return custom;
            }
        }

        #endregion
    }
}