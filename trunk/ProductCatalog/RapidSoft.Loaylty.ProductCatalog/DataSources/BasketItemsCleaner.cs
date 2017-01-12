namespace RapidSoft.Loaylty.ProductCatalog.DataSources
{
    using System;

    using Etl.Logging;
    using Etl.Runtime;
    using Etl.Runtime.Agents;
    using Etl.Runtime.Agents.Sql;

    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.ProductCatalog.Settings;

    public class BasketItemsCleaner
    {
        private readonly ILog log = LogManager.GetLogger(typeof(BasketItemsCleaner));
        private Guid etlPackageId = new Guid("49df85e6-e3d6-47d1-aac8-7dd9d03ff845");
       
        public void Execute()
        {
            log.Info("Creating ETL agent...");
            
            var agentInfo = new EtlAgentInfo
            {
                ConnectionString = DataSourceConfig.ConnectionString,
                SchemaName = ApiSettings.EtlSchemaName,
            };

            var parameters = new[]
            {
                new EtlVariableAssignment("DB", DataSourceConfig.ConnectionString)
            };

            log.Info("Invoking package...");
            
            EtlSession etlSession = new SqlEtlAgent(agentInfo).InvokeEtlPackage(etlPackageId.ToString(), parameters, null);
            
            log.Info(string.Format("Package has been executed with result {0}", etlSession.Status));    
        }
    }
}