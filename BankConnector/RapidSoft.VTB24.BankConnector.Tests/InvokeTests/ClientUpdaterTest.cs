namespace RapidSoft.VTB24.BankConnector.Tests.InvokeTests
{
    using System;
    using System.IO;
    using System.Linq;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource;
    using RapidSoft.VTB24.BankConnector.DataSource.Interface;
    using RapidSoft.VTB24.BankConnector.EtlExecutionWrapper;
    using RapidSoft.VTB24.BankConnector.Processors;
    using RapidSoft.VTB24.BankConnector.Tests.Helpers;
    using RapidSoft.VTB24.VtbEncryption;

    [TestClass]
    public class ClientUpdaterTest : TestBase
    {
        private IClientUpdatesRepository clientUpdatesRepository;

        private const string UpdateClientsRequestFileName = "VTB_20120531_3.anketaPL";
        private const string SrcTestFilesDirName = "TestFiles";
        private const string DestTestFilesDirName = "anketaPL";
        private const string ResponseEmailSubject = "AnketaPL_Response";

        [TestMethod]
        [DeploymentItem(SrcTestFilesDirName, DestTestFilesDirName)]
        public void ShouldUpdateClientsTest()
        {
            // Очистка старых данных
            EmailHelper.CleanupLoyaltyMailBox(UpdateClientsRequestFileName);
            EmailHelper.CleanupTeradataMailBox(ResponseEmailSubject);
            DropUpdates();

            // Подготовка тест данных
            this.UploadTestFile(UpdateClientsRequestFileName);

            // Выполнение задачи
            var job = TestHelper.GetWrapper(EtlPackageIds.ClientUpdatesJob);
            var sessionId = Guid.Parse(job.Execute());

            // Проверяем что нет критичных ошибок выполнения
            TestHelper.AssertJobResult(job, 2);

            // Проверяем обновления клиентов в БД
            var updates = this.GetClientUpdates(sessionId);
            Assert.IsNotNull(updates);
            Assert.AreEqual(6, updates.Length);

            /* NOTE: Отправка файла временно отключена
            // Проверяем выгруженный файл
            var resFileDir = Path.Combine(Environment.CurrentDirectory, "AnketaPLFromTeradata");
            EmailHelper.DownloadFilesFromTeradata(ResponseEmailSubject, resFileDir);
            VtbEncryptionHelper.Decrypt(resFileDir);

            var teradataFileLines = File.ReadAllLines(Path.Combine(resFileDir, UpdateClientsRequestFileName) + ".response");

            Assert.IsNotNull(teradataFileLines);
            Assert.AreEqual(7, teradataFileLines.Length);
            
            var line1 = teradataFileLines[0].Split(';');
            Assert.AreEqual("ClientId", line1[0]);
            Assert.AreEqual("Status", line1[1]);

            // Проверка данных по клиентам
            AssertClient(updates, teradataFileLines, 1, "Success", "0");
            AssertClient(updates, teradataFileLines, 2, "profileFail", "1");
            AssertClient(updates, teradataFileLines, 3, "profileNotExists", "2");
            AssertClient(updates, teradataFileLines, 4, "wrongGender", "3");
            AssertClient(updates, teradataFileLines, 5, "wrongSegment", "4");
            AssertClient(updates, teradataFileLines, 6, "createdClient", "5");
             * */
        }

        private static void AssertClient(ClientUpdate[] updates, string[] teradataFileLines, int lineIndex, string clientId, string updStatus)
        {
            var clientUpdate = updates.First(c => c.ClientId == clientId);
            Assert.AreEqual(Int32.Parse(updStatus), clientUpdate.UpdStatus);

            Assert.AreEqual(ProcessConstants.NotSpecified, clientUpdate.FirstName);
            Assert.AreEqual(ProcessConstants.NotSpecified, clientUpdate.LastName);
            Assert.AreEqual(ProcessConstants.NotSpecified, clientUpdate.MiddleName);
            Assert.AreEqual(ProcessConstants.NotSpecified, clientUpdate.Email);
            Assert.AreEqual(DateTime.MinValue, clientUpdate.BirthDate);


            var split = teradataFileLines[lineIndex].Split(';');
            Assert.AreEqual(clientId, split[0]);
            Assert.AreEqual(updStatus, split[1]);
        }

        private static void DropUpdates()
        {
            using (var uow = CreateUow())
            {
                var updatesRepository = uow.ClientUpdatesRepository;
                foreach (var update in updatesRepository.GetAll())
                {
                    updatesRepository.Delete(update);
                }

                uow.Save();
            }
        }

        private ClientUpdate[] GetClientUpdates(Guid sessionId)
        {
            using (var uow = CreateUow())
            {
                this.clientUpdatesRepository = uow.ClientUpdatesRepository;
                return this.clientUpdatesRepository.GetBySessionId(sessionId).ToArray();
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