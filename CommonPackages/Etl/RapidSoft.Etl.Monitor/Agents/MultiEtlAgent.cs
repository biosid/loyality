using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using RapidSoft.Etl.Logging;
using RapidSoft.Etl.Runtime;
using RapidSoft.Etl.Runtime.Agents;

namespace RapidSoft.Etl.Monitor.Agents
{
    partial class MultiEtlAgent : IEtlAgent
    {
        #region Constructors

        public MultiEtlAgent(IEtlAgent etlAgent)
            : this(new[] { etlAgent })
        {
        }

        public MultiEtlAgent(IEtlAgent etlAgent0, IEtlAgent etlAgent1)
            : this(new[] { etlAgent0, etlAgent1 })
        {
        }

        public MultiEtlAgent(IEtlAgent etlAgent0, IEtlAgent etlAgent1, IEtlAgent etlAgent2)
            : this(new[] { etlAgent0, etlAgent1, etlAgent2 })
        {
        }

        public MultiEtlAgent(IEtlAgent etlAgent0, IEtlAgent etlAgent1, IEtlAgent etlAgent2, IEtlAgent etlAgent3)
            : this(new[] { etlAgent0, etlAgent1, etlAgent2, etlAgent3 })
        {
        }

        public MultiEtlAgent(IEtlAgent[] etlAgents)
        {
            _etlAgents = etlAgents ?? new IEtlAgent[0];
            _logParser = new _LogParser(_etlAgents);
        }

        #endregion

        #region Fields

        private readonly IEtlAgent[] _etlAgents;
        private readonly IEtlLogParser _logParser;

        #endregion

        #region Public methods

        public EtlAgentInfo GetEtlAgentInfo()
        {
            return new EtlAgentInfo
            {
                EtlAgentType = this.GetType().AssemblyQualifiedName,
                ConnectionString = null,
                SchemaName = null,
            };
        }

        public void DeployEtlPackage(EtlPackage package, EtlPackageDeploymentOptions options)
        {
            throw new NotImplementedException();
        }

        public EtlPackage[] GetEtlPackages()
        {
            var result = new List<EtlPackage>();

            foreach (var agent in _etlAgents)
            {
                result.AddRange(agent.GetEtlPackages());
            }

            return result.OrderBy(a => a.Name).ToArray();
        }

        public EtlPackage GetEtlPackage(string etlPackageId)
        {
            EtlPackage result = null;

            foreach (var agent in _etlAgents)
            {
                result = agent.GetEtlPackage(etlPackageId);
                if (result != null)
                    break;
            }

            return result;
        }

        public EtlSession InvokeEtlPackage(
            string etlPackageId,
            EtlVariableAssignment[] parameters,
            string parentSessionId,
            string user = null,
            string waitingEtlSessionId = null)
        {
            if (string.IsNullOrWhiteSpace(waitingEtlSessionId))
            {
                throw new NotSupportedException("Отложенные сессии не подерживаются MultiEtlAgent");
            }
            
            foreach (var agent in _etlAgents)
            {
                var packages = agent.GetEtlPackages();
                foreach (var package in packages)
                {
                    if (String.Equals(package.Id, etlPackageId, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return agent.InvokeEtlPackage(etlPackageId, parameters, parentSessionId);
                    }
                }
            }

            return null;
        }

        public IEtlLogParser GetEtlLogParser()
        {
            return _logParser;
        }

        public EtlSession CreateWaitingEtlSession(string etlPackageId, string user = null)
        {
            throw new NotSupportedException("Отложенные сессии не подерживаются MultiEtlAgent");
        }

        #endregion
    }
}
