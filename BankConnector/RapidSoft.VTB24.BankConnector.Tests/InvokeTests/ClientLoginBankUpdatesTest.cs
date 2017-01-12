namespace RapidSoft.VTB24.BankConnector.Tests.InvokeTests
{
    using System;
    using System.IO;
    using System.Linq;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Quartz.Core;
    using Quartz.Impl;
    using Quartz.Plugin.Xml;

    using RapidSoft.Etl.Runtime;
    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource;
    using RapidSoft.VTB24.BankConnector.DataSource.Interface;
    using RapidSoft.VTB24.BankConnector.EtlExecutionWrapper;
    using RapidSoft.VTB24.BankConnector.Processors;
    using RapidSoft.VTB24.BankConnector.Tests.Helpers;
    using RapidSoft.VTB24.VtbEncryption;

    [TestClass]
    public class ClientLoginBankUpdatesTest : TestBase
    {
        private const string UpdateClientsRequestFileName = "VTB_20120531_3.mobilePhonePL";
        private const string SrcTestFilesDirName = "TestFiles";
        private const string DestTestFilesDirName = "mobilePhonePL";
        private const string SubjectTerm = "mobilePhonePL";
        private const string ResponseEmailSubject = "MobilePhonePL_Response";

        [TestMethod]
        [DeploymentItem(SrcTestFilesDirName, DestTestFilesDirName)]
        public void ShouldUpdateClientLoginsTest()
        {
            // Очистка старых данных
            EmailHelper.CleanupLoyaltyMailBox(UpdateClientsRequestFileName);
            EmailHelper.CleanupTeradataMailBox(ResponseEmailSubject);
            DropUpdates();

            // Подготовка тест данных
            this.UploadTestFile(UpdateClientsRequestFileName);

            // Выполнение задачи
            var job = TestHelper.GetWrapper(EtlPackageIds.ClientLoginBankUpdatesJob, GetAssign());

            var sessionId = Guid.Parse(job.Execute());

            // Проверяем что нет критичных ошибок выполнения
            TestHelper.AssertJobResult(job, 0, 0);

            // Проверяем обновления клиентов в БД
            var updates = this.GetClientUpdates(sessionId);
            Assert.IsNotNull(updates);
            Assert.AreEqual(1, updates.Length);

            // Проверяем выгруженный файл
            var resFileDir = Path.Combine(Environment.CurrentDirectory, "MobilePhonePL");
            EmailHelper.DownloadFilesFromTeradata(ResponseEmailSubject, resFileDir);

            var fileName = Path.Combine(resFileDir, UpdateClientsRequestFileName) + ".response";
            FileReaderHelper.UncompressFile(fileName, resFileDir);
            var teradataFileLines = File.ReadAllLines(fileName);

            Assert.IsNotNull(teradataFileLines);
            Assert.AreEqual(2, teradataFileLines.Length);
            
            var line1 = teradataFileLines[0].Split(';');
            Assert.AreEqual("ClientId", line1[0]);
            Assert.AreEqual("Status", line1[1]);
            Assert.AreEqual("Message", line1[2]);

            // Проверка данных по клиентам
            AssertUpdate(updates, teradataFileLines, 1, "Success", "0");
        }

        private EtlVariableAssignment[] GetAssign()
        {
            return new[]
                   {
                       new EtlVariableAssignment(EtlVariableKeys.SubjectTerm, SubjectTerm), 
                   };
        }

        private static void AssertUpdate(ClientLoginBankUpdatesResponse[] updates, string[] teradataFileLines, int lineIndex, string clientId, string updStatus)
        {
            var clientUpdate = updates.First(c => c.ClientId == clientId);
            Assert.AreEqual(int.Parse(updStatus), clientUpdate.Status);

            var split = teradataFileLines[lineIndex].Split(';');
            Assert.AreEqual(clientId, split[0]);
            Assert.AreEqual(updStatus, split[1]);
        }

        private static void DropUpdates()
        {
            using (var uow = CreateUow())
            {
                foreach (var update in uow.ClientLoginBankUpdatesRepository.GetAll())
                {
                    uow.ClientLoginBankUpdatesRepository.Delete(update);
                }

                foreach (var update in uow.ClientLoginBankUpdatesResponseRepository.GetAll())
                {
                    uow.ClientLoginBankUpdatesResponseRepository.Delete(update);
                }

                uow.Save();
            }
        }

        private ClientLoginBankUpdatesResponse[] GetClientUpdates(Guid sessionId)
        {
            using (var uow = CreateUow())
            {
                return uow.ClientLoginBankUpdatesResponseRepository.GetBySessionId(sessionId).ToArray();
            }
        }

        private void UploadTestFile(string fileName)
        {
            var testDir = Path.Combine(Environment.CurrentDirectory, DestTestFilesDirName);

            var filePath = Path.Combine(testDir, fileName);
            
            VtbEncryptionHelper.Encrypt(testDir);
            EmailHelper.UploadFileToLoyaltySmtpServer(filePath, fileName);
        }
    }
}