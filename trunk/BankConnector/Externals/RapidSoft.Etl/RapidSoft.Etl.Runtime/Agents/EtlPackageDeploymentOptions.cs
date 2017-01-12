using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace RapidSoft.Etl.Runtime.Agents
{
	[Serializable]
	public sealed class EtlPackageDeploymentOptions
    {
        #region Properties

        public bool Overwrite
        {
            get;
            set;
        }

        #endregion
    }
}