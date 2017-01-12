using System;
using System.Configuration;
using System.IO;
using System.Resources;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RapidSoft.Etl.Logging;
using RapidSoft.Etl.Logging.DBScripts;
using RapidSoft.Etl.Runtime.Functions;
using RapidSoft.Etl.Runtime.Steps;

namespace RapidSoft.Etl.Runtime.Tests.Steps
{
    [TestClass]
    public class EtlOutputVariablesTests
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
        [DeploymentItem(@"Files\Temp.csv")]
        public void CanChangeVariableAfterExecuteQuery()
        {
            var fileName = "Temp.csv";

            var varDefaultValue = "123";
            var varNewValue = "321";

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
                    new EtlVariableInfo("mutable_var", EtlVariableModifier.Output, varDefaultValue),
                },
                Steps =
                {
                    new EtlExecuteQueryStep
                    {
                        Source = new EtlQuerySourceInfo
                        {
                            ProviderName = "$(ipn)",
                            ConnectionString = "$(connstr)",
                            Text = "select @number as colNumber",
                            Parameters = 
                            {
                                new EtlQueryParameter("number", varNewValue),
                            },
                        },
                        OutputVariables = new EtlExecuteQueryOutputVariableSet
                        {
                            FirstRow = 
                            {
                                new EtlFieldToVariableAssignment
                                {
                                    SourceFieldName = "colNumber",
                                    VariableName = "mutable_var",
                                }
                            }
                        },
                    },
                    new EtlExportCsvFileStep
                    {
                        Source = new EtlQuerySourceInfo
                        {
                            ProviderName = "$(ipn)",
                            ConnectionString = "$(connstr)",
                            Text = "select @number as colNumber",
                            Parameters = 
                            {
                                new EtlQueryParameter("number", "$(mutable_var)"),
                            },
                        },
                        Destination = new EtlCsvFileInfo
                        {
                            FilePath = fileName,
                            CodePage = 1251,
                            FieldDelimiter = ";",
                            HasHeaders = false,
                        },
                        Mappings = 
                        {
                            new EtlFieldMapping{DestinationFieldName="colNumber", SourceFieldName="colNumber"},
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

            var fileData = File.ReadAllText(fileName);
            Assert.AreEqual(fileData, varNewValue);
        }

        #endregion
    }
}
