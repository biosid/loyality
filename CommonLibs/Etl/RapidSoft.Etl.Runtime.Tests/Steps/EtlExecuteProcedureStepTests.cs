using System;
using System.Configuration;
using System.Resources;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RapidSoft.Etl.Logging;
using RapidSoft.Etl.Logging.DBScripts;
using RapidSoft.Etl.Runtime.Functions;
using RapidSoft.Etl.Runtime.Steps;

namespace RapidSoft.Etl.Runtime.Tests.Steps
{
    [TestClass]
    public class EtlExecuteProcedureStepTests
    {
        #region Fields

        private static readonly string _connectionStringName = "EtlTestDB";
        private static readonly string _providerName = "System.Data.SqlClient";
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString;

        #endregion

        #region Initialization

        [TestInitialize]
        public void Initialize()
        {
            ScriptHelper.ExecuteScripts
            (
                _connectionStringName,
                new[]
                {
                    Properties.Resources.AllDataTypesTable,
                    Properties.Resources.AllDataTypesTableData,
                    Properties.Resources.CopyAllDataTypesTable,
                }
            );
        }

        #endregion

        #region Tests

        [TestMethod]
        public void CanExecuteProcedure()
        {
            var package = new EtlPackage
            {
                Id = Guid.NewGuid().ToString(),
                Variables = 
                {
                    new EtlVariableInfo("connstr", EtlVariableModifier.Input, _connectionString),
                    new EtlVariableInfo("ipn", EtlVariableModifier.Input, _providerName),
                    new EtlVariableInfo("pid", EtlVariableModifier.Bound, EtlVariableBinding.EtlPackageId),
                    new EtlVariableInfo("sid", EtlVariableModifier.Bound, EtlVariableBinding.EtlSessionId),
                    new EtlVariableInfo("dt", EtlVariableModifier.Bound, EtlVariableBinding.EtlSessionDateTime),
                    new EtlVariableInfo("udt", EtlVariableModifier.Bound, EtlVariableBinding.EtlSessionUtcDateTime),
                    new EtlVariableInfo("a", EtlVariableModifier.Output),
                },
                Steps =
                {
                    new EtlExecuteProcedureStep
                    {
                        Source = new EtlProcedureSourceInfo
                        {
                            ProviderName = "$(ipn)",
                            ConnectionString = "$(connstr)",
                            ProcedureName = "dbo.CopyAllDataTypesTable",
                            Parameters = 
                            {
                                new EtlProcedureParameter("etlPackageId", "$(pid)"),
                                new EtlProcedureParameter("etlSessionId", "$(sid)"),
                            },
                        },
                    }
                }
            };

            var logger = new MemoryEtlLogger();
            var session = package.Invoke(logger, null, null);
            Assert.IsNotNull(session);

            Assert.AreEqual(1, logger.EtlSessions.Count);
            Assert.AreEqual(package.Id, logger.EtlSessions[0].EtlPackageId);
            Assert.AreEqual(session.EtlSessionId, logger.EtlSessions[0].EtlSessionId);
            Assert.AreEqual(EtlStatus.Succeeded, logger.EtlSessions[0].Status);

            //todo: assert that stored procedure made some work
        }

        #endregion
    }
}
