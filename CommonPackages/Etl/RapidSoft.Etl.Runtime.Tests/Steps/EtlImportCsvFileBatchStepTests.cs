using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidSoft.Etl.Runtime.Tests.Steps
{
    using System.Configuration;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Etl.Logging;
    using RapidSoft.Etl.Logging.DBScripts;
    using RapidSoft.Etl.Runtime.Steps;

    [TestClass]
    public class EtlImportCsvFileBatchStepTests
    {
        #region Fields

        private static readonly string _connectionStringName = "EtlTestDB";

        #endregion

        #region Initialization

        [TestInitialize]
        public void Initialize()
        {
            ScriptHelper.ExecuteScripts(
                _connectionStringName,
                new[] { Properties.Resources.AllDataTypesTable, Properties.Resources.AllDataTypesTableData });
        }

        #endregion

        [TestMethod]
        [DeploymentItem(@"Files\BatchGood1.test0.csv")]
        [DeploymentItem(@"Files\BatchGood2.test0.csv")]
        public void CanImportAllDataTypesCsvFromTwoFiles()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString;

            var package = new EtlPackage
            {
                Id = Guid.NewGuid().ToString(),
                Variables = 
                {
                    new EtlVariableInfo("connstr", EtlVariableModifier.Input, connectionString),
                    new EtlVariableInfo("db_prov", EtlVariableModifier.Input, "System.Data.SqlClient"),
                    new EtlVariableInfo("pid", EtlVariableModifier.Bound, EtlVariableBinding.EtlPackageId),
                    new EtlVariableInfo("sid", EtlVariableModifier.Bound, EtlVariableBinding.EtlSessionId),
                    new EtlVariableInfo("dt", EtlVariableModifier.Bound, EtlVariableBinding.EtlSessionDateTime),
                    new EtlVariableInfo("udt", EtlVariableModifier.Bound, EtlVariableBinding.EtlSessionUtcDateTime),
                },
                Steps =
                {
                    new EtlImportCsvFileBatchStep
                    {
                        DataLossBehavior = EtlImportDataLossBehavior.Skip,
                        BatchSize = 5000,
                        Source = new EtlCsvFileBatchInfo
                                 {
                                     CodePage = 1251,
                                     FieldDelimiter = ";",
                                     HasHeaders = true,
                                     FilePath = "Files",
                                     FileMask = "*.test0.csv"
                                 },
                        Destination = new EtlTableInfo
                        {
                            ConnectionString = "$(connstr)",
                            ProviderName = "$(db_prov)",
                            TableName = "dbo.AllDataTypesTable",
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
                    }
                }
            };

            var logger = new MemoryEtlLogger();
            var session = package.Invoke(logger);
            Assert.IsNotNull(session);

            Assert.AreEqual(1, logger.EtlSessions.Count);
            Assert.AreEqual(EtlStatus.Succeeded, logger.EtlSessions[0].Status);
            Assert.AreEqual(package.Id, logger.EtlSessions[0].EtlPackageId);
            Assert.AreEqual(session.EtlSessionId, logger.EtlSessions[0].EtlSessionId);
        }

        [TestMethod]
        [DeploymentItem(@"Files\BatchBad.test1.csv")]
        [DeploymentItem(@"Files\BatchGood.test1.csv")]
        public void CanImportAllDataTypesCsvFromOneFileSkipOther()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString;

            var package = new EtlPackage
            {
                Id = Guid.NewGuid().ToString(),
                Variables = 
                {
                    new EtlVariableInfo("connstr", EtlVariableModifier.Input, connectionString),
                    new EtlVariableInfo("db_prov", EtlVariableModifier.Input, "System.Data.SqlClient"),
                    new EtlVariableInfo("pid", EtlVariableModifier.Bound, EtlVariableBinding.EtlPackageId),
                    new EtlVariableInfo("sid", EtlVariableModifier.Bound, EtlVariableBinding.EtlSessionId),
                    new EtlVariableInfo("dt", EtlVariableModifier.Bound, EtlVariableBinding.EtlSessionDateTime),
                    new EtlVariableInfo("udt", EtlVariableModifier.Bound, EtlVariableBinding.EtlSessionUtcDateTime),
                },
                Steps =
                {
                    new EtlImportCsvFileBatchStep
                    {
                        DataLossBehavior = EtlImportDataLossBehavior.Skip,
                        BatchSize = 5000,
                        Source = new EtlCsvFileBatchInfo
                                 {
                                     CodePage = 1251,
                                     FieldDelimiter = ";",
                                     HasHeaders = true,
                                     FilePath = "Files",
                                     FileMask = "*.test1.csv"
                                 },
                        Destination = new EtlTableInfo
                        {
                            ConnectionString = "$(connstr)",
                            ProviderName = "$(db_prov)",
                            TableName = "dbo.AllDataTypesTable",
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
                    }
                }
            };

            var logger = new MemoryEtlLogger();
            var session = package.Invoke(logger);
            Assert.IsNotNull(session);

            Assert.AreEqual(1, logger.EtlSessions.Count);
            Assert.AreEqual(EtlStatus.FinishedWithLosses, logger.EtlSessions[0].Status);
            Assert.AreEqual(package.Id, logger.EtlSessions[0].EtlPackageId);
            Assert.AreEqual(session.EtlSessionId, logger.EtlSessions[0].EtlSessionId);
            Assert.IsTrue(logger.EtlMessages.Any(x => x.Text.Contains("File Files\\BatchBad.test1.csv not has column(s)")));
        }

        [TestMethod]
        [DeploymentItem(@"Files\BatchBad1.test2.csv")]
        [DeploymentItem(@"Files\BatchBad2.test2.csv")]
        public void CanNotImportAllDataTypesCsv()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString;

            var package = new EtlPackage
            {
                Id = Guid.NewGuid().ToString(),
                Variables = 
                {
                    new EtlVariableInfo("connstr", EtlVariableModifier.Input, connectionString),
                    new EtlVariableInfo("db_prov", EtlVariableModifier.Input, "System.Data.SqlClient"),
                    new EtlVariableInfo("pid", EtlVariableModifier.Bound, EtlVariableBinding.EtlPackageId),
                    new EtlVariableInfo("sid", EtlVariableModifier.Bound, EtlVariableBinding.EtlSessionId),
                    new EtlVariableInfo("dt", EtlVariableModifier.Bound, EtlVariableBinding.EtlSessionDateTime),
                    new EtlVariableInfo("udt", EtlVariableModifier.Bound, EtlVariableBinding.EtlSessionUtcDateTime),
                },
                Steps =
                {
                    new EtlImportCsvFileBatchStep
                    {
                        DataLossBehavior = EtlImportDataLossBehavior.Skip,
                        Source = new EtlCsvFileBatchInfo
                                 {
                                     CodePage = 1251,
                                     FieldDelimiter = ";",
                                     HasHeaders = true,
                                     FilePath = "Files",
                                     FileMask = "*.test2.csv"
                                 },
                        Destination = new EtlTableInfo
                        {
                            ConnectionString = "$(connstr)",
                            ProviderName = "$(db_prov)",
                            TableName = "dbo.AllDataTypesTable",
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
                    }
                }
            };

            var logger = new MemoryEtlLogger();
            var session = package.Invoke(logger);
            Assert.IsNotNull(session);

            Assert.AreEqual(1, logger.EtlSessions.Count);
            Assert.AreEqual(EtlStatus.Failed, logger.EtlSessions[0].Status);
            Assert.AreEqual(package.Id, logger.EtlSessions[0].EtlPackageId);
            Assert.AreEqual(session.EtlSessionId, logger.EtlSessions[0].EtlSessionId);
            Assert.IsTrue(logger.EtlMessages.Any(x => x.Text.Contains("File Files\\BatchBad1.test2.csv not has column(s)")));
            Assert.IsTrue(logger.EtlMessages.Any(x => x.Text.Contains("File Files\\BatchBad2.test2.csv not has column(s)")));
        }
    }
}
