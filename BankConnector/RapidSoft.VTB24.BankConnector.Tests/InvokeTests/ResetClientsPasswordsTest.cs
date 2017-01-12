namespace RapidSoft.VTB24.BankConnector.Tests.InvokeTests
{
    using System;
    using System.IO;
    using System.Linq;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Etl.Runtime;
    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource;
    using RapidSoft.VTB24.BankConnector.EtlExecutionWrapper;
    using RapidSoft.VTB24.BankConnector.Tests.Helpers;
    using RapidSoft.VTB24.VtbEncryption;

    [TestClass]
    public class ResetClientsPasswordsTest : TestBase
    {
        private const string RequestFileName = "VTB_20130331_1.resetPassPL";
        private const string SrcTestFilesDirName = "TestFiles";
        private const string DestTestFilesDirName = "resetPassPL";
        private const string SubjectTerm = "resetPassPL";
        private const string ResponseEmailSubject = "ResetPassPL_Response";

        [TestMethod]
        [DeploymentItem(SrcTestFilesDirName, DestTestFilesDirName)]
        public void ShouldResetClientsPasswordsTest()
        {
            // Очистка старых данных
            EmailHelper.CleanupLoyaltyMailBox(RequestFileName);
            EmailHelper.CleanupTeradataMailBox(ResponseEmailSubject);
            DropUpdates();

            // Подготовка тест данных
            this.UploadTestFile(RequestFileName);

            // Выполнение задачи
            var job = TestHelper.GetWrapper(EtlPackageIds.ResetClientsPasswordsJob, this.GetAssign());

            var sessionId = Guid.Parse(job.Execute());

            // Проверяем что нет критичных ошибок выполнения
            TestHelper.AssertJobResult(job, 0, 0);

            // Проверяем обновления клиентов в БД
            var responses = this.GetResponses(sessionId);
            Assert.IsNotNull(responses);
            Assert.IsTrue(responses.Length == 1);

            // Проверяем выгруженный файл
            var resFileDir = Path.Combine(Environment.CurrentDirectory, "ResetPassPL");
            EmailHelper.DownloadFilesFromTeradata(ResponseEmailSubject, resFileDir);

            var fileName = Path.Combine(resFileDir, RequestFileName) + ".response";
            FileReaderHelper.UncompressFile(fileName, resFileDir);
            var teradataFileLines = File.ReadAllLines(fileName);

            Assert.IsNotNull(teradataFileLines);
            Assert.AreEqual(2, teradataFileLines.Length);

            var line1 = teradataFileLines[0].Split(';');
            Assert.AreEqual("ClientId", line1[0]);
            Assert.AreEqual("Status", line1[1]);
            Assert.AreEqual("Message", line1[2]);

            // Проверка данных по клиентам
            AssertUpdate(responses, teradataFileLines, 1, "Success", "0");
        }

        private EtlVariableAssignment[] GetAssign()
        {
            return new[]
                   {
                       new EtlVariableAssignment(EtlVariableKeys.SubjectTerm, SubjectTerm), 
                   };
        }

        private static void AssertUpdate(ClientForBankPwdResetResponse[] responses, string[] teradataFileLines, int lineIndex, string clientId, string updStatus)
        {
            var response = responses.First(c => c.ClientId == clientId);
            Assert.AreEqual(int.Parse(updStatus), response.Status);

            var split = teradataFileLines[lineIndex].Split(';');
            Assert.AreEqual(clientId, split[0]);
            Assert.AreEqual(updStatus, split[1]);
        }

        private static void DropUpdates()
        {
            using (var uow = CreateUow())
            {
                foreach (var item in uow.ClientForBankPwdResetRepository.GetAll())
                {
                    uow.ClientForBankPwdResetRepository.Delete(item);
                }

                foreach (var item in uow.ClientForBankPwdResetResponseRepository.GetAll())
                {
                    uow.ClientForBankPwdResetResponseRepository.Delete(item);
                }

                uow.Save();
            }
        }

        private ClientForBankPwdResetResponse[] GetResponses(Guid sessionId)
        {
            using (var uow = CreateUow())
            {
                return uow.ClientForBankPwdResetResponseRepository.GetBySessionId(sessionId).ToArray();
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
