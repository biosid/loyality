using System;
using System.Configuration;
using System.Resources;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RapidSoft.Etl.Logging.DBScripts;
using RapidSoft.Etl.Logging;
using RapidSoft.Etl.Runtime.Functions;
using RapidSoft.Etl.Runtime.Steps;

namespace RapidSoft.Etl.Runtime.Tests.Steps
{
    [TestClass]
    public class EtlImportQueryStepTests
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
                }
            );
        }

        #endregion

        #region Tests

        [TestMethod]
        public void CanImportSqlQuery()
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
                },
                Steps =
                {
                    new EtlImportQueryStep
                    {
                        Source = new EtlQuerySourceInfo
                        {
                            ProviderName = "$(ipn)",
                            ConnectionString = "$(connstr)",
                            Text = "select * from dbo.AllDataTypesTable",
                        },
                        Destination = new EtlTableInfo
                        {
                            ProviderName = "$(ipn)",
                            ConnectionString = "$(connstr)",
                            TableName = "dbo.AllDataTypesTableCopy",
                        },
                        Mappings = 
                        {
                            new EtlFieldMapping{DestinationFieldName="Id", SourceFieldName="Id"},
                            new EtlFieldMapping{DestinationFieldName="Null", SourceFieldName="Null"},
                            new EtlFieldMapping{DestinationFieldName="Boolean", SourceFieldName="Boolean"},
                            new EtlFieldMapping{DestinationFieldName="Byte", SourceFieldName="Byte"},
                            new EtlFieldMapping{DestinationFieldName="DateTime", SourceFieldName="DateTime"},
                            new EtlFieldMapping{DestinationFieldName="Decimal", SourceFieldName="Decimal"},
                            new EtlFieldMapping{DestinationFieldName="Double", SourceFieldName="Double"},
                            new EtlFieldMapping{DestinationFieldName="Guid", SourceFieldName="Guid"},
                            new EtlFieldMapping{DestinationFieldName="Int16", SourceFieldName="Int16"},
                            new EtlFieldMapping{DestinationFieldName="Int32", SourceFieldName="Int32"},
                            new EtlFieldMapping{DestinationFieldName="Int64", SourceFieldName="Int64"},
                            new EtlFieldMapping{DestinationFieldName="Single", SourceFieldName="Single"},
                            new EtlFieldMapping{DestinationFieldName="String", SourceFieldName="String"},
                            new EtlFieldMapping{DestinationFieldName="EtlPackageId", DefaultValue="$(pid)"},
                            new EtlFieldMapping{DestinationFieldName="EtlSessionId", DefaultValue="$(sid)"},
                            new EtlFieldMapping{DestinationFieldName="EtlInsertedDateTime", DefaultValue="$(dt)"},
                            new EtlFieldMapping{DestinationFieldName="EtlInsertedUtcDateTime", DefaultValue="$(udt)"},
                        },
                    },
                }
            };

            var logger = new MemoryEtlLogger();
            var session = package.Invoke(logger, null, null);
            Assert.IsNotNull(session);

            Assert.AreEqual(1, logger.EtlSessions.Count);
            Assert.AreEqual(package.Id, logger.EtlSessions[0].EtlPackageId);
            Assert.AreEqual(session.EtlSessionId, logger.EtlSessions[0].EtlSessionId);
            Assert.AreEqual(EtlStatus.Succeeded, logger.EtlSessions[0].Status);

            //Assert.AreEqual(3, logger.EtlEntityCounters.Count);

            //Assert.AreEqual(package.Id, logger.EtlEntityCounters[0].EtlPackageId);
            //Assert.AreEqual(session.EtlSessionId, logger.EtlEntityCounters[0].EtlSessionId);
            //Assert.AreEqual("Source", logger.EtlEntityCounters[0].CounterName);
            //Assert.AreEqual(10, logger.EtlEntityCounters[0].CounterValue);

            //Assert.AreEqual(package.Id, logger.EtlEntityCounters[1].EtlPackageId);
            //Assert.AreEqual(session.EtlSessionId, logger.EtlEntityCounters[1].EtlSessionId);
            //Assert.AreEqual("Errors", logger.EtlEntityCounters[1].CounterName);
            //Assert.AreEqual(0, logger.EtlEntityCounters[1].CounterValue);

            //Assert.AreEqual(package.Id, logger.EtlEntityCounters[2].EtlPackageId);
            //Assert.AreEqual(session.EtlSessionId, logger.EtlEntityCounters[2].EtlSessionId);
            //Assert.AreEqual("Inserted", logger.EtlEntityCounters[2].CounterName);
            //Assert.AreEqual(10, logger.EtlEntityCounters[2].CounterValue);
        }

        #endregion
    }
}
