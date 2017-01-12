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
    public class EtlUploadFileStepFtpTests
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

        #region FTP Tests

        [TestMethod]
        [DeploymentItem(@"Files\Sample.xml")]
        public void CanUploadFileToFtp()
        {
            var sourceFileName = @"Sample.xml";
            var destinationUrl = string.Concat(_ftpPath, "/", "Sample.xml");

            var package = new EtlPackage
            {
                Id = Guid.NewGuid().ToString(),
                Steps =
                {
                    new EtlUploadFileStep
                    {
                        Source = new EtlFileInfo
                        {
                            FilePath = sourceFileName
                        },
                        Destination = new EtlResourceInfo
                        {
                            Uri = destinationUrl,
                            Method = "STOR",
                        }
                    }
                }
            };

            Assert.IsTrue(File.Exists(sourceFileName));

            var logger = new MemoryEtlLogger();
            var session = package.Invoke(logger);

            Assert.AreEqual(1, logger.EtlSessions.Count);
            Assert.AreEqual(package.Id, logger.EtlSessions[0].EtlPackageId);
            Assert.AreEqual(session.EtlSessionId, logger.EtlSessions[0].EtlSessionId);
            Assert.AreEqual(EtlStatus.Succeeded, logger.EtlSessions[0].Status);
            Assert.AreEqual(EtlStatus.Succeeded, session.Status);
            
            //var destinationData = File.ReadAllLines(destinationFileName);
            //Assert.IsNotNull(destinationData);
            //Assert.AreNotEqual("", destinationData);
        }

        #endregion
    }
}
