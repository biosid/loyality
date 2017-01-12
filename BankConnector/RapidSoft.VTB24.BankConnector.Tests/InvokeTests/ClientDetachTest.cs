using System.Collections.Generic;
using System.ComponentModel.Design;
using RapidSoft.VTB24.BankConnector.API;
using RapidSoft.VTB24.BankConnector.Tests.StubServices;

namespace RapidSoft.VTB24.BankConnector.Tests.InvokeTests
{
    using System;
    using System.IO;
    using System.Linq;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource;
    using RapidSoft.VTB24.BankConnector.EtlExecutionWrapper;
    using RapidSoft.VTB24.BankConnector.Tests.Helpers;
    using RapidSoft.VTB24.VtbEncryption;

    [TestClass]
    public class ClientDetachTest:TestBase
    {
	    private const string TestFilesFolder = "ClientDetachReceive";
        private const string TestClientId = "detached_client_id";
		private const string TestClientId2 = "not_activated_client_id2";
        private const string ResponseSubject = "VTB_20130610_1.cancelPL.response";
		private const string ResponseAttachment = "VTB_20130610_1.cancelPL.response";

		[TestInitialize]
		public void Initialize()
		{
			using (var uow = CreateUow())
			{
				CleanupTestRecords(uow, TestClientId);
				CleanupTestRecords(uow, TestClientId2);
				uow.Save();
			}
			
			ClientProfileServiceStub.ResetStub();
		}

		[TestMethod]
        public void ShouldAddClientToDeleteList()
        {
            using (var container = DataSourceHelper.CreateContainer())
            {
                var service = container.Resolve<IBankConnectorService>();
                service.BlockClientToDelete(TestClientId);
            }

            ClientForDeletion newClientForDeletion;

            using (var uow = CreateUow())
            {
                var detachRepository = uow.ClientForDeletionRepository;
                newClientForDeletion = detachRepository.GetById(x => x.ExternalClientId == TestClientId);
				CleanupTestRecords(uow, TestClientId2);
			}

            Assert.IsNotNull(newClientForDeletion);
            Assert.AreEqual(TestClientId, newClientForDeletion.ExternalClientId);
            Assert.IsNull(newClientForDeletion.Status);
        }

		[TestMethod]
		public void ShouldSendClientsToDetachListToMail()
		{
			EmailHelper.CleanupTeradataMailBox("CancelPL_Request_");

            using (var container = DataSourceHelper.CreateContainer())
            {
                var service = container.Resolve<IBankConnectorService>();
                service.BlockClientToDelete(TestClientId);
                service.BlockClientToDelete(TestClientId2);
            }

			Assert.AreEqual(1, ClientProfileServiceStub.lockedIds.Count, "Блокировка должна вызываться только для активированных клиентов");
			Assert.AreEqual(TestClientId, ClientProfileServiceStub.lockedIds[0], "Блокировка должна выполняться только для активированных клиентов");

            var sendDetachList = TestHelper.GetWrapper(EtlPackageIds.SendDetachListJob);
			var sessionId = sendDetachList.Execute();

			ClientForDeletion sendedDeletion;

			using (var uow = CreateUow())
			{
				var detachRepository = uow.ClientForDeletionRepository;
				sendedDeletion = detachRepository.GetById(x => x.ExternalClientId == TestClientId2);
				CleanupTestRecords(uow, TestClientId2);
			}

			var tempFolder = Path.Combine(Path.GetTempPath(), string.Format("DetachClients_{0}", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")));

			if (!Directory.Exists(tempFolder))
			{
				Directory.CreateDirectory(tempFolder);
			}

			EmailHelper.DownloadFilesFromTeradata("CancelPL_Request_", tempFolder);

			VtbEncryptionHelper.Decrypt(tempFolder);

		    int filesCount;
			var targetFileContent = FileReaderHelper.ReadAllLinesFromFolderFiles(tempFolder, out filesCount, null, true);
            Assert.IsTrue(filesCount > 0, "С почтового ящика не было загружено ни одного файла");

            Assert.IsTrue(sendDetachList.IsSuccess());

            Assert.AreEqual(Guid.Parse(sessionId), sendedDeletion.SendEtlSessionId);

			Assert.IsTrue(targetFileContent.Count > 1);
			Assert.AreEqual("ClientId", targetFileContent[0]);
			Assert.IsTrue(targetFileContent.Any(x => x.Equals(TestClientId)));
			Assert.IsTrue(targetFileContent.Any(x => x.Equals(TestClientId2)));
		}

		[TestMethod]
		[DeploymentItem("TestFiles\\VTB_20130610_1.cancelPL.response", TestFilesFolder)]
        public void ShouldReceiveResponseMail()
		{
            // Очистка тест данных
            EmailHelper.CleanupLoyaltyMailBox(ResponseSubject);
            using (var uow = CreateUow())
            {
                CleanupTestRecords(uow, "-100");
                uow.Save();
            }

            // Подготовка тест данных
			var testDir = Path.Combine(TestContext.TestDeploymentDir, TestFilesFolder);
			var filePath = Path.Combine(testDir, ResponseAttachment);
			VtbEncryptionHelper.Encrypt(testDir);

			EmailHelper.UploadFileToLoyaltySmtpServer(filePath, ResponseSubject);

            using (var container = DataSourceHelper.CreateContainer())
            {
                var service = container.Resolve<IBankConnectorService>();
                service.BlockClientToDelete(TestClientId);
                service.BlockClientToDelete(TestClientId2);
            }

            // Выполнение джобы
            var job = TestHelper.GetWrapper(EtlPackageIds.ReceiveDetachCountJob);
			var sessionId = Guid.Parse(job.Execute());
			List<ClientForDeletionResponse> response;

            using (var uow = CreateUow())
            {
                var repository = uow.ClientForDeletionResponseRepository;
                response = repository.GetAll().Where(x => x.EtlSessionId == sessionId).ToList();
                CleanupTestRecords(uow, "-100");
                uow.Save();
            }

            TestHelper.AssertJobResult(job, 2);

            Assert.IsNotNull(response);
            Assert.IsTrue(response.Any(x => x.ClientId == TestClientId));

            var deletedClient =
                response.FirstOrDefault(
                    x => x.ClientId == TestClientId && x.DetachStatus == (int?)ClientForDeletionResponseDetachStatus.Success);

            Assert.IsNotNull(deletedClient, "Отсутствует клиент, который должен был быть удалён");
            Assert.IsTrue(response.Any(x => x.ClientId == "-100"));
            Assert.IsTrue(response.Any(x => x.ClientId == "-1005001"));
        }

		[TestMethod]
		public void ShouldReturnTrueIfClientInDeleteList()
		{
            using (var container = DataSourceHelper.CreateContainer())
            {
                var service = container.Resolve<IBankConnectorService>();

                Assert.IsFalse(service.IsClientAddedToDetachList(TestClientId).Result);
                service.BlockClientToDelete(TestClientId);
                Assert.IsTrue(service.IsClientAddedToDetachList(TestClientId).Result);
            }
		}

	    private static void CleanupTestRecords(IUnitOfWork uow, string ClientId)
        {
            var detachRepository = uow.ClientForDeletionRepository;

			var testDelClient = detachRepository.GetAll().Where(x => x.ExternalClientId.Contains(ClientId));
	        foreach (var clientForDeletion in testDelClient)
	        {
                detachRepository.Delete(clientForDeletion);
	        }

            var detachResponseRepository = uow.ClientForDeletionResponseRepository;

            var testDelClientResponse = detachResponseRepository.GetAll().Where(x => x.ClientId.Contains(ClientId));
            foreach (var clientForDeletionResponse in testDelClientResponse)
            {
                detachResponseRepository.Delete(clientForDeletionResponse);
            }
            uow.Save();
        }
    }
}
