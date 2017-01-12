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
    public class EtlDownloadFileStepFtpTests
    {
        #region Fields

        private static readonly string _ftpPath = ConfigurationManager.AppSettings["EtlTestFtp"];

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
        public void CanDownloadFileFromFtp()
        {
            var sourceUrl = string.Concat(_ftpPath, "/", "Sample.xml");
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
                            Method = "RETR",
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
        [DeploymentItem(@"Files\DirList.txt")]
        public void CanListFtpDirectory()
        {
            var sourceUrl = _ftpPath;
            var destinationFileName = @"DirList.txt";

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
                            Method = "NLST",
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
