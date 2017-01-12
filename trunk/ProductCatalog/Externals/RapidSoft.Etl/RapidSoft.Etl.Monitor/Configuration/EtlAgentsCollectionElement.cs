using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace RapidSoft.Etl.Monitor.Configuration
{
    public class EtlAgentsCollectionElement : ConfigurationElement
    {
        [ConfigurationProperty("etlAgentType", IsRequired = true)]
        public string EtlAgentType
        {
            get
            {
                return this["etlAgentType"].ToString();
            }
            set
            {
                this["etlAgentType"] = value;
            }
        }

        [ConfigurationProperty("connectionString", IsRequired = true)]
        public string ConnectionString
        {
            get
            {
                return this["connectionString"].ToString();
            }
            set
            {
                this["connectionString"] = value;
            }
        }

        [ConfigurationProperty("schemaName", IsRequired = true)]
        public string SchemaName
        {
            get
            {
                return this["schemaName"].ToString();
            }
            set
            {
                this["schemaName"] = value;
            }
        }
    }
}
