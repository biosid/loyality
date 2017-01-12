using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace RapidSoft.Etl.Runtime.Agents
{
	public static class EtlAgents
    {
        #region Methods

        public static IEtlAgent CreateAgent(EtlAgentInfo agentInfo)
        {
            if (agentInfo == null)
            {
                throw new ArgumentNullException("agentInfo");
            }

            var providerType = Type.GetType(agentInfo.EtlAgentType);
            if (providerType == null)
            {
                throw new InvalidOperationException(string.Format("Type {0} was not found", agentInfo.EtlAgentType));
            }

            if (!typeof(IEtlAgent).IsAssignableFrom(providerType))
            {
                throw new InvalidOperationException(string.Format("Type {0} must implements {1} interface", providerType.FullName, typeof(IEtlAgent).FullName));
            }

            var ctorWithSource = providerType.GetConstructor(new[] { typeof(EtlAgentInfo) });
            var ctorWithoutParams = providerType.GetConstructor(Type.EmptyTypes);

            if (ctorWithSource != null)
            {
                return (IEtlAgent)ctorWithSource.Invoke(new object[] { agentInfo });
            }
            else if (ctorWithoutParams != null)
            {
                return (IEtlAgent)ctorWithoutParams.Invoke(null);
            }
            else
            {
                throw new InvalidOperationException(string.Format("{0} must have parameterless constructor or constructor with parameter of type {1}", providerType.FullName, typeof(IEtlAgent).FullName));
            }
        }

        #endregion
    }
}