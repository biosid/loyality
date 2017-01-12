using System;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Net.Security;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RapidSoft.Etl.Logging;
using RapidSoft.Etl.Runtime.Steps;

namespace RapidSoft.Etl.Runtime.Tests.Steps
{
    [TestClass]
    public class EtlDownloadFileStepHttpTests
    {
        #region Fields

        private static readonly string _baseHttpUrl = ConfigurationManager.AppSettings["EtlTestHttp"];
        private static readonly string _baseHttpsUrl = ConfigurationManager.AppSettings["EtlTestHttps"];

        #endregion

        #region Initialization

        [TestInitialize]
        public void Initialize()
        {
        }

        #endregion

        #region Tests

        [TestMethod]
        [DeploymentItem(@"Files\Sample.xml")]
        public void CanDownloadFileFromHttp()
        {
            var sourceUrl = string.Concat(_baseHttpUrl, "/", "Sample.xml");
            var destinationFileName = @"Sample.xml";

            var package = new EtlPackage
            {
                Id = Guid.NewGuid().ToString(),
                Steps =
                {
                    new EtlDownloadFileStep
                    {
                        Source = new EtlResourceInfo
                        {
                            Uri = sourceUrl,
                            Method = "GET",
                        },
                        Destination = new EtlFileInfo
                        {
                            FilePath = destinationFileName
                        }
                    }
                }
            };

            var logger = new MemoryEtlLogger();
            var session = package.Invoke(logger);

            Assert.AreEqual(1, logger.EtlSessions.Count);
            Assert.AreEqual(package.Id, logger.EtlSessions[0].EtlPackageId);
            Assert.AreEqual(session.EtlSessionId, logger.EtlSessions[0].EtlSessionId);
            Assert.AreEqual(EtlStatus.Succeeded, logger.EtlSessions[0].Status);

            Assert.AreEqual(EtlStatus.Succeeded, session.Status);
            Assert.IsTrue(File.Exists(destinationFileName));

            var destinationData = File.ReadAllLines(destinationFileName);
            Assert.IsNotNull(destinationData);
            Assert.AreNotEqual("", destinationData);
        }

        [TestMethod]
        [DeploymentItem(@"Files\Sample.xml")]
        public void CanDownloadFileFromHttps()
        {
            var sourceUrl = string.Concat(_baseHttpsUrl, "/", "Sample.xml");
            var destinationFileName = @"Sample.xml";

            var package = new EtlPackage
            {
                Id = Guid.NewGuid().ToString(),
                Steps =
                {
                    new EtlDownloadFileStep
                    {
                        Source = new EtlResourceInfo
                        {
                            AllowInvalidCertificates = true, //"E=test@test.com, CN=etltestbadcert, OU=Test, O=Test, L=Moscow, S=Moscow, C=RU",
                            Uri = sourceUrl,
                            Method = "GET",
                            //UserName = "",
                            //Password = "",
                        },
                        Destination = new EtlFileInfo
                        {
                            FilePath = destinationFileName
                        }
                    }
                }
            };

            var logger = new MemoryEtlLogger();
            var session = package.Invoke(logger);

            Assert.AreEqual(1, logger.EtlSessions.Count);
            Assert.AreEqual(package.Id, logger.EtlSessions[0].EtlPackageId);
            Assert.AreEqual(session.EtlSessionId, logger.EtlSessions[0].EtlSessionId);
            Assert.AreEqual(EtlStatus.Succeeded, logger.EtlSessions[0].Status);

            Assert.AreEqual(EtlStatus.Succeeded, session.Status);
            Assert.IsTrue(File.Exists(destinationFileName));

            var destinationData = File.ReadAllLines(destinationFileName);
            Assert.IsNotNull(destinationData);
            Assert.AreNotEqual("", destinationData);
        }

        #endregion
    }
}
