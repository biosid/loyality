using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Resources;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RapidSoft.Etl.Logging;
using RapidSoft.Etl.Logging.DBScripts;
using RapidSoft.Etl.Runtime.Steps;

namespace RapidSoft.Etl.Runtime.Tests.Steps
{
    [TestClass]
    public class EtlExportCsvFileStepTests
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
                }
            );
        }

        #endregion

        #region Tests

        [TestMethod]
        [DeploymentItem(@"Files\AllDataTypes.csv")]
        public void CanExportAllDataTypesCsv()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString;
            var importedFileName = "AllDataTypes.csv";
            var exportedFileName = "AllDataTypes_Out.csv";

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
                    new EtlImportCsvFileStep
                    {
                        Source = new EtlCsvFileInfo
                        {
                            FilePath = importedFileName,
                            CodePage = 1251,
                            FieldDelimiter = ";",
                            HasHeaders = true,
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
                    },
                    new EtlExportCsvFileStep
                    {
                        Source = new EtlQuerySourceInfo
                        {
                            ConnectionString = "$(connstr)",
                            ProviderName = "$(db_prov)",
                            Text = "select * from dbo.AllDataTypesTable",
                        },
                        Destination = new EtlCsvFileInfo
                        {
                            FilePath = exportedFileName,
                            CodePage = 1251,
                            FieldDelimiter = ";",
                            HasHeaders = true,
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

            //todo: decide to test that imported and exported files are identical
            var importedFileData = File.ReadAllText(importedFileName);
            var exportedFileData = File.ReadAllText(exportedFileName);

            //Assert.AreEqual(importedFileData, exportedFileData, "Exported and imported data are not identical");
        }

        #endregion
    }
}
