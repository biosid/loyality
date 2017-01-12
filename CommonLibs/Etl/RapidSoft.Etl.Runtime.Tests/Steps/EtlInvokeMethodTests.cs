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
    public class EtlInvokeMethodTests
    {
        #region Fields

        private static readonly string _connectionStringName = "EtlTestDB";
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
        [DeploymentItem(@"Files\Temp.txt")]
        public void CanInvokeMethod()
        {
            var fileName = "Temp.txt";

            var a = "Hello, world";
            var b = "123";
            var c = new DateTime(2013, 12, 15);

            var package = new EtlPackage
            {
                Id = Guid.NewGuid().ToString(),
                Variables = 
                {
                    new EtlVariableInfo("a", EtlVariableModifier.Input, a),
                    new EtlVariableInfo("b", EtlVariableModifier.Input, b),
                    new EtlVariableInfo("c", EtlVariableModifier.Input, c.ToString("o")),
                },
                Steps =
                {
                    new EtlInvokeMethodStep
                    {
                        Source = new EtlMethodSourceInfo
                        {
                            AssemblyName = "RapidSoft.Etl.Runtime.Tests",
                            TypeName = "RapidSoft.Etl.Runtime.Tests.Steps.TestPlugin",
                            MethodName = "DoSomething",
                            Parameters = 
                            {
                                new EtlMethodParameter("c", "$(c)"),
                                new EtlMethodParameter("b", "$(b)"),
                                new EtlMethodParameter("a", "$(a)"),
                                new EtlMethodParameter("fileName", fileName),
                            }
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
            Assert.AreEqual(fileData, string.Format("{0}, {1}, {2:o}", a, b, c));
        }

        #endregion
    }

    public class TestPlugin
    {
        public static void DoSomething(EtlContext context, IEtlLogger logger, string a, int b, DateTime c, string fileName)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            File.WriteAllText(fileName, string.Format("{0}, {1}, {2:o}", a, b, c));
        }
    }
}
