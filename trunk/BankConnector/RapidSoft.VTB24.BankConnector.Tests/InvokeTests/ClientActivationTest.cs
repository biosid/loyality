namespace RapidSoft.VTB24.BankConnector.Tests.InvokeTests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource;
    using RapidSoft.VTB24.BankConnector.EtlExecutionWrapper;
    using RapidSoft.VTB24.BankConnector.Tests.Helpers;
    using RapidSoft.VTB24.VtbEncryption;

    [TestClass]
    public class ClientActivationTest : TestBase
    {
        private const string ClientIdOkActivated = "okActivated";
        private const string ClientIdNotRegisred = "notRegisred";
        private const string ClientIdProfileNotExists = "profileNotExists";
        private const string ClientIdCanNotBeActivated = "canNotBeActivated";        
        
        private const string TestMailSubjectPrefix = "VTB_20130730_5.activPL";

        private const string ReceivePathFormat = "ActivationTestFiles_{0}\\Receive";

        private const string ResponsePathFormat = "ActivationTestFiles_{0}\\Response";

        private const string TestFileSource = "TestFiles\\VTB_20130606_5.activPL";

        private readonly string ResponseSubjectPrefix = "Activate_Response";

        [TestMethod]
        [DeploymentItem(TestFileSource)]
        public void ShouldUploadCorrectResponse()
        {
            EmailHelper.CleanupLoyaltyMailBox(TestMailSubjectPrefix);
            EmailHelper.CleanupTeradataMailBox(this.ResponseSubjectPrefix);

            // Формирование тест файла
            var testFilesFolderReceive = Path.Combine(
                Path.GetTempPath(), string.Format(ReceivePathFormat, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")));

            var testFilesFolderResponse = Path.Combine(
                Path.GetTempPath(), string.Format(ResponsePathFormat, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")));

            if (!Directory.Exists(testFilesFolderReceive))
            {
                Directory.CreateDirectory(testFilesFolderReceive);
            }

            if (!Directory.Exists(testFilesFolderResponse))
            {
                Directory.CreateDirectory(testFilesFolderResponse);
            }

            var testFileName = Path.Combine(
                testFilesFolderReceive, string.Format("VTB_{0}_5.activPL", DateTime.Now.ToString("yyyyMMdd")));
            
            string testData;

            using (StreamReader sr = new StreamReader(Path.GetFileName(TestFileSource), Encoding.GetEncoding(1251)))
            {
                testData = sr.ReadToEnd();    
            }                        

            this.CleanUpTestRecords();

            File.WriteAllText(testFileName, testData, Encoding.GetEncoding(1251));

            // Загрузка тест файла
            EmailHelper.UploadFileToLoyaltySmtpServer(testFileName, string.Format(TestMailSubjectPrefix));

            // Выполнение пакета
            var activateWrapper = TestHelper.GetWrapper(EtlPackageIds.ActivateClientsJob);
            activateWrapper.Execute();
            TestHelper.AssertJobResult(activateWrapper, 3, 1);

            // очистка тестовых данных
            this.CleanUpTestRecords();

            // Загрузка и проверка ответного файла
            EmailHelper.DownloadFilesFromTeradata(this.ResponseSubjectPrefix, testFilesFolderResponse);

            VtbEncryptionHelper.Decrypt(testFilesFolderResponse);

            int filesCount;
            var responseContent = FileReaderHelper.ReadAllLinesFromFolderFiles(testFilesFolderResponse, out filesCount, null, true);
            Assert.IsTrue(filesCount > 0, "С почтового ящика не было загружено ни одного файла");

            Assert.AreEqual(5, responseContent.Count);

            Assert.AreEqual("ClientId;Status", responseContent[0]);

            this.AssertActivated(1, responseContent, ActivateClientStatus.WrongClientId, ClientIdCanNotBeActivated);
            this.AssertActivated(2, responseContent, ActivateClientStatus.WrongClientId, ClientIdNotRegisred);
            this.AssertActivated(3, responseContent, ActivateClientStatus.Success, ClientIdOkActivated);
            this.AssertActivated(4, responseContent, ActivateClientStatus.WrongClientId, ClientIdProfileNotExists);
        }

        private void AssertActivated(int lineIndex, List<string> responseContent, ActivateClientStatus expectedStatus, string clientId)
        {
            var arr = responseContent[lineIndex].Split(';');

            Assert.AreEqual(clientId, arr[0]);
            Assert.AreEqual(((int)expectedStatus).ToString(CultureInfo.InvariantCulture), arr[1]);
        }

        private void CleanUpTestRecords()
        {
            using (var uow = CreateUow())
            {
                var repository = uow.ClientForActivationRepository;
                foreach (var clientId in new[]
                                         {
                                             ClientIdNotRegisred, 
                                             ClientIdOkActivated,
                                             ClientIdCanNotBeActivated,
                                             ClientIdProfileNotExists
                                         })
                {
                    repository.DeleteByClientId(clientId);
                }

                uow.Save();
            }
        }
    }
}