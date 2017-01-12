using System;
using System.Configuration;
using System.IO;
using System.Resources;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RapidSoft.Etl.Logging.DBScripts;
using RapidSoft.Etl.Logging;
using RapidSoft.Etl.Runtime.Steps;

namespace RapidSoft.Etl.Runtime.Tests.Steps
{
    [TestClass]
    public class EtlImportFlatXmlFileStepTests
    {
        #region Fields

        private static readonly string _connectionStringName = "EtlTestDB";

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
        [DeploymentItem(@"Files\AllDataTypes.xml")]
        public void CanImportAllDataTypesXml()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString;

            var package = new EtlPackage
            {
                Id = Guid.NewGuid().ToString(),
                Variables = 
                {
                    new EtlVariableInfo("connstr", EtlVariableModifier.Input, connectionString),
                    new EtlVariableInfo("ipn", EtlVariableModifier.Input, "System.Data.SqlClient"),
                    new EtlVariableInfo("pid", EtlVariableModifier.Bound, EtlVariableBinding.EtlPackageId),
                    new EtlVariableInfo("sid", EtlVariableModifier.Bound, EtlVariableBinding.EtlSessionId),
                    new EtlVariableInfo("dt", EtlVariableModifier.Bound, EtlVariableBinding.EtlSessionDateTime),
                    new EtlVariableInfo("udt", EtlVariableModifier.Bound, EtlVariableBinding.EtlSessionUtcDateTime),
                },
                Steps =
                {
                    new EtlImportFlatXmlFileStep
                    {
                        Source = new EtlXmlFileInfo
                        {
                            FilePath = "AllDataTypes.xml",
                            DataElementPath = @"items/item",
                            TreatEmptyStringAsNull = true,
                        },
                        Destination = new EtlTableInfo
                        {
                            ProviderName = "$(ipn)",
                            ConnectionString = "$(connstr)",
                            TableName = "dbo.AllDataTypesTable",
                        },
                        Mappings = 
                        {
                            new EtlFieldMapping{DestinationFieldName="Id", SourceFieldName="id"},
                            new EtlFieldMapping{DestinationFieldName="Null", SourceFieldName="null"},
                            new EtlFieldMapping{DestinationFieldName="Boolean", SourceFieldName="boolean"},
                            new EtlFieldMapping{DestinationFieldName="Byte", SourceFieldName="byte"},
                            new EtlFieldMapping{DestinationFieldName="DateTime", SourceFieldName="datetime"},
                            new EtlFieldMapping{DestinationFieldName="Decimal", SourceFieldName="decimal"},
                            new EtlFieldMapping{DestinationFieldName="Double", SourceFieldName="double"},
                            new EtlFieldMapping{DestinationFieldName="Guid", SourceFieldName="guid"},
                            new EtlFieldMapping{DestinationFieldName="Int16", SourceFieldName="int16"},
                            new EtlFieldMapping{DestinationFieldName="Int32", SourceFieldName="int32"},
                            new EtlFieldMapping{DestinationFieldName="Int64", SourceFieldName="int64"},
                            new EtlFieldMapping{DestinationFieldName="Single", SourceFieldName="single"},
                            new EtlFieldMapping{DestinationFieldName="String", SourceFieldName="string"},
                            new EtlFieldMapping{DestinationFieldName="EtlPackageId", DefaultValue="$(pid)"},
                            new EtlFieldMapping{DestinationFieldName="EtlSessionId", DefaultValue="$(sid)"},
                            new EtlFieldMapping{DestinationFieldName="EtlInsertedDateTime", DefaultValue="$(dt)"},
                            new EtlFieldMapping{DestinationFieldName="EtlInsertedUtcDateTime", DefaultValue="$(udt)"},
                        }
                    }
                }
            };

            var logger = new MemoryEtlLogger();
            var session = package.Invoke(logger);
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

        [TestMethod]
        [DeploymentItem(@"Files\BadAllDataTypes.xml")]
        public void CanImportAllDataTypesXmlWithErrors()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString;

            var package = new EtlPackage
            {
                Id = Guid.NewGuid().ToString(),
                Variables = 
                {
                    new EtlVariableInfo("connstr", EtlVariableModifier.Input, connectionString),
                    new EtlVariableInfo("ipn", EtlVariableModifier.Input, "System.Data.SqlClient"),
                    new EtlVariableInfo("pid", EtlVariableModifier.Bound, EtlVariableBinding.EtlPackageId),
                    new EtlVariableInfo("sid", EtlVariableModifier.Bound, EtlVariableBinding.EtlSessionId),
                    new EtlVariableInfo("dt", EtlVariableModifier.Bound, EtlVariableBinding.EtlSessionDateTime),
                    new EtlVariableInfo("udt", EtlVariableModifier.Bound, EtlVariableBinding.EtlSessionUtcDateTime),
                },
                Steps =
                {
                    new EtlImportFlatXmlFileStep
                    {
                        DataLossBehavior = EtlImportDataLossBehavior.Skip, //should be ignored by this step
                        Source = new EtlXmlFileInfo
                        {
                            FilePath = "BadAllDataTypes.xml",
                            DataElementPath = @"items/item",
                            TreatEmptyStringAsNull = true,
                        },
                        Destination = new EtlTableInfo
                        {
                            ProviderName = "$(ipn)",
                            ConnectionString = "$(connstr)",
                            TableName = "dbo.AllDataTypesTable",
                        },
                        Mappings = 
                        {
                            new EtlFieldMapping{DestinationFieldName="Id", SourceFieldName="id"},
                            new EtlFieldMapping{DestinationFieldName="Null", SourceFieldName="null"},
                            new EtlFieldMapping{DestinationFieldName="Boolean", SourceFieldName="boolean"},
                            new EtlFieldMapping{DestinationFieldName="Byte", SourceFieldName="byte"},
                            new EtlFieldMapping{DestinationFieldName="DateTime", SourceFieldName="datetime"},
                            new EtlFieldMapping{DestinationFieldName="Decimal", SourceFieldName="decimal"},
                            new EtlFieldMapping{DestinationFieldName="Double", SourceFieldName="double"},
                            new EtlFieldMapping{DestinationFieldName="Guid", SourceFieldName="guid"},
                            new EtlFieldMapping{DestinationFieldName="Int16", SourceFieldName="int16"},
                            new EtlFieldMapping{DestinationFieldName="Int32", SourceFieldName="int32"},
                            new EtlFieldMapping{DestinationFieldName="Int64", SourceFieldName="int64"},
                            new EtlFieldMapping{DestinationFieldName="Single", SourceFieldName="single"},
                            new EtlFieldMapping{DestinationFieldName="String", SourceFieldName="string"},
                            new EtlFieldMapping{DestinationFieldName="EtlPackageId", DefaultValue="$(pid)"},
                            new EtlFieldMapping{DestinationFieldName="EtlSessionId", DefaultValue="$(sid)"},
                            new EtlFieldMapping{DestinationFieldName="EtlInsertedDateTime", DefaultValue="$(dt)"},
                            new EtlFieldMapping{DestinationFieldName="EtlInsertedUtcDateTime", DefaultValue="$(udt)"},
                        },
                    }
                }
            };

            var logger = new MemoryEtlLogger();
            var session = package.Invoke(logger);
            Assert.IsNotNull(session);

            Assert.AreEqual(1, logger.EtlSessions.Count);
            Assert.AreEqual(package.Id, logger.EtlSessions[0].EtlPackageId);
            Assert.AreEqual(session.EtlSessionId, logger.EtlSessions[0].EtlSessionId);
            Assert.AreEqual(EtlStatus.Failed, logger.EtlSessions[0].Status);

            //Assert.AreEqual(3, logger.EtlEntityCounters.Count);

            //Assert.AreEqual(package.Id, logger.EtlEntityCounters[0].EtlPackageId);
            //Assert.AreEqual(session.EtlSessionId, logger.EtlEntityCounters[0].EtlSessionId);
            //Assert.AreEqual("Source", logger.EtlEntityCounters[0].CounterName);
            //Assert.AreEqual(7, logger.EtlEntityCounters[0].CounterValue);

            //Assert.AreEqual(package.Id, logger.EtlEntityCounters[1].EtlPackageId);
            //Assert.AreEqual(session.EtlSessionId, logger.EtlEntityCounters[1].EtlSessionId);
            //Assert.AreEqual("Errors", logger.EtlEntityCounters[1].CounterName);
            //Assert.AreEqual(1, logger.EtlEntityCounters[1].CounterValue);

            //Assert.AreEqual(package.Id, logger.EtlEntityCounters[2].EtlPackageId);
            //Assert.AreEqual(session.EtlSessionId, logger.EtlEntityCounters[2].EtlSessionId);
            //Assert.AreEqual("Inserted", logger.EtlEntityCounters[2].CounterName);
            //Assert.AreEqual(6, logger.EtlEntityCounters[2].CounterValue);
        }

        #endregion
    }
}
