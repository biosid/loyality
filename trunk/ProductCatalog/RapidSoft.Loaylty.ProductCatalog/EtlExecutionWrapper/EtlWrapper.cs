namespace RapidSoft.Loaylty.ProductCatalog.EtlExecutionWrapper
{
    using System.Collections.Generic;
    using System.Linq;
    using RapidSoft.Etl.Runtime;
    using RapidSoft.Etl.Runtime.Agents;
    using RapidSoft.Etl.Runtime.Agents.Sql;
    using RapidSoft.Loaylty.ProductCatalog.EtlExecutionWrapper.Configuration;

    internal class EtlWrapper
    {
        public EtlWrapper(string packageId)
        {
            PackageId = packageId;
        }

        public string PackageId { get; private set; }

        public string SessionId { get; private set; }

        public string Execute()
        {
            var etlVariables = GetVariableAssignment().ToArray();

            var agentInfo = new EtlAgentInfo
            {
                ConnectionString = EtlConfigHelper.ConnectionString,
                SchemaName = EtlConfigHelper.Schema,
            };

            var agent = new SqlEtlAgent(agentInfo);

            var etlSession = agent.InvokeEtlPackage(PackageId, etlVariables, null);

            SessionId = etlSession.EtlSessionId;

            return SessionId;
        }

        protected static IEnumerable<EtlVariableAssignment> GetVariableAssignment()
        {
            var etlConfigSection = EtlConfigSection.Current;
            if (etlConfigSection == null)
            {
                return Enumerable.Empty<EtlVariableAssignment>();
            }

            return etlConfigSection.Variables
                                   .Cast<EtlVariableElement>()
                                   .Select(v => new EtlVariableAssignment
                                   {
                                       Name = v.Name,
                                       Value = v.Value
                                   }).ToArray();
        }
    }
}
