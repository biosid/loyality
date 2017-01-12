using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace RapidSoft.Etl.Monitor.Configuration
{
    [ConfigurationCollection(typeof(EtlAgentsCollectionElement), AddItemName = "etlAgent")]
    public class EtlAgentsCollection : ConfigurationElementCollection
    {
        public EtlAgentsCollectionElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as EtlAgentsCollectionElement;
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new EtlAgentsCollectionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return String.Concat(
                ((EtlAgentsCollectionElement)element).EtlAgentType,
                ((EtlAgentsCollectionElement)element).ConnectionString,
                ((EtlAgentsCollectionElement)element).SchemaName);
        }
    }

}
