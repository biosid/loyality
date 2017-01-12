using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace RapidSoft.Etl.Runtime.Agents
{
	[Serializable]
	public sealed class EtlAgentInfo
    {
        #region Properties

        public string EtlAgentType
        {
            get;
            set;
        }

		public string ConnectionString 
        { 
            get; 
            set; 
        }

        public string SchemaName
        {
            get;
            set;
        }

        #endregion
    }
}