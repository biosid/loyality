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
    public class EtlExecuteQueryStepTests
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
        }

        #endregion

        #region Tests

        [TestMethod]
        public void CanExecuteQuery()
        {
            var packageStepId = Guid.NewGuid().ToString();

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
                },
                Steps =
                {
                    new EtlExecuteQueryStep
                    {
                        Source = new EtlQuerySourceInfo
                        {
                            ProviderName = "$(ipn)",
                            ConnectionString = "$(connstr)",
                            Text = "select @etlPackageId as colPackageId, @etlSessionId as colSessionId, @number as colNumber",
                            Parameters = 
                            {
                                new EtlQueryParameter("etlPackageId", "$(pid)"),
                                new EtlQueryParameter("etlSessionId", "$(sid)"),
                                new EtlQueryParameter("number", "123"),
                            },
                        },
                        Counters = new EtlExecuteQueryCounterSet
                        {
                            RowCount = new EtlCounterBinding
                            {
                                EntityName = "TestEntity",
                                CounterName = "RowCount",
                            }
                        }
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

            Assert.AreEqual(1, logger.EtlCounters.Count);
            Assert.AreEqual("TestEntity", logger.EtlCounters[0].EntityName);
            Assert.AreEqual("RowCount", logger.EtlCounters[0].CounterName);
            Assert.AreEqual(package.Id, logger.EtlCounters[0].EtlPackageId);
            Assert.AreEqual(session.EtlSessionId, logger.EtlCounters[0].EtlSessionId);
            Assert.AreEqual(1, logger.EtlCounters[0].CounterValue);
        }

        #endregion
    }
}
