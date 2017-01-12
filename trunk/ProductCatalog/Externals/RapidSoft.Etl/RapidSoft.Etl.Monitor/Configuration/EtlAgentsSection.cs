using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace RapidSoft.Etl.Monitor.Configuration
{
    public class EtlAgentsSection : ConfigurationSection
    {
        [ConfigurationProperty("etlAgents", IsRequired = true)]
        public EtlAgentsCollection EtlAgents
        {
            get
            {
                return (EtlAgentsCollection)this["etlAgents"];
            }
            set
            {
                this["etlAgents"] = value;
            }
        }
    }
}
