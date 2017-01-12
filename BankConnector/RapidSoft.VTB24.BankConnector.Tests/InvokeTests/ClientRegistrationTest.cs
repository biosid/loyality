using System;
using System.Collections.Generic;
using System.Linq;

namespace RapidSoft.VTB24.BankConnector.Tests.InvokeTests
{
    using System.IO;

    using Faker;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource;
    using RapidSoft.VTB24.BankConnector.EtlExecutionWrapper;
    using RapidSoft.VTB24.BankConnector.Tests.Helpers;
    using RapidSoft.VTB24.BankConnector.Tests.StubServices;
    using RapidSoft.VTB24.Site.SecurityWebApi;
    using RapidSoft.VTB24.VtbEncryption;

    using S22.Imap;

    [TestClass]
    public class ClientRegistrationTest : TestBase
    {
        private const string TestFilesFolder = "ClientRegistrationReceive";
        private const string TestFilesDirName = "TestFiles";        
        private const string ResponseAttachment = "VTB_20130531_8.regPL.RESPONSE";
        private const string ResponseAttachmentFail = "VTB_20130000_1.regPL.RESPONSE";

		private Random random = new Random();

        private string alreadyRegistredClientId = "TestClientId3";

        [TestInitialize]
        public void Init()
        {
            using (var uow = CreateUow())
            {
                var repository = uow.ClientForRegistrationRepository;
                foreach (var t in repository.GetAll()) repository.Delete(t);

                var regResponseRepo = uow.ClientForRegistrationResponseRepository;
                foreach (var t in regResponseRepo.GetAll())
                    regResponseRepo.Delete(t);
                uow.Save();
            }
        }

        [TestMethod]
        public void Send()
        {
            EmailHelper.CleanupTeradataMailBox("Registration_Request_");

            // Add clients to DB
            var numOfRecords = 5;
            List<string> requestFilesContent = new List<string>();
            requestFilesContent = this.CreateTestRequest(numOfRecords);

            // Выполнение задачи
            var job = TestHelper.GetWrapper(EtlPackageIds.SendRegistrationClientsJob);
            job.Execute();
			
            Assert.IsTrue(job.IsSuccess(), "EtlSession должна завершиться с успешным статусом");

			var tempFolder = Path.Combine(Path.GetTempPath(), string.Format("RegisterClients_{0}", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")));

            var subject = string.Format("Registration_Request_{0}", DateTime.Now.ToString("yyyy_MM_dd"));
            
            EmailHelper.DownloadFilesFromTeradata(subject, tempFolder);

			VtbEncryptionHelper.Decrypt(tempFolder);

            int filesCount;
			var targetFileContent = FileReaderHelper.ReadAllLinesFromFolderFiles(tempFolder, out filesCount, null, true);
            Assert.IsTrue(filesCount > 0, "С почтового ящика не было загружено ни одного файла");

	        var sessionId = Guid.Parse(job.SessionId);

	        using (var uow = CreateUow())
	        {
		        var repository = uow.ClientForRegistrationRepository;
		        var sendedClients = repository.GetAll().Where(x => x.RequestSessionId == sessionId).ToArray();
		        Assert.IsTrue(sendedClients.Length >= numOfRecords, string.Format("Не все тестовые записи клиентов были обновлены при отправке"));
		        var monthLimit = DateTime.Now.AddMonths(-1);
				Assert.IsTrue(sendedClients.All(x => x.FirstName == "N/A" && x.MiddleName == "N/A" && x.LastName == "N/A" && x.BirthDate > monthLimit && x.Gender == (int)Gender.Other), "У отправленных записей должны отсутствовать данные: ФИО, дата рождения и пол");
	        }

	        foreach (var shouldBeContent in requestFilesContent)
	        {
	            Assert.IsTrue(
	                targetFileContent.Any(y => y.Equals(shouldBeContent, StringComparison.InvariantCultureIgnoreCase)),
	                "Выгруженные файлы должны содержать строку \r\n {0} \r\n содержимое файлов: \r\n {1}",
	                shouldBeContent,
	                targetFileContent.Aggregate((x, y) => x + "\r\n" + y));
	        }
        }

        private List<string> CreateTestRequest(decimal numOfRecords)
        {
            var requestFilesContent = new List<string>
                                  {
                                      "ClientId;LastName;FirstName;MiddleName;BirthDate;MobilePhone;Email"
                                  };

            using (var uow = CreateUow())
            {
                var repository = uow.ClientForRegistrationRepository;
                for (int j = 0; j < numOfRecords; j++)
                {
                    var client = this.GetTestClientDBRecord(Name.First());

                    requestFilesContent.Add(
                        string.Format(
                            "{0};{1};{2};{3};{4};{5};{6}",
                            client.ClientId,
                            client.LastName,
                            client.FirstName,
                            client.MiddleName,
                            client.BirthDate.ToString("yyyy-MM-dd"),
                            client.MobilePhone.Substring(1),
                            client.Email));
                    repository.Add(client);
                }
                uow.Save();
            }

            return requestFilesContent;
        }

        [TestMethod]
        [DeploymentItem(TestFilesDirName + "\\" + ResponseAttachment, TestFilesFolder)]
        [DeploymentItem(TestFilesDirName + "\\" + ResponseAttachmentFail, TestFilesFolder)]
	    public void Receive()
	    {
	        var clientIdAlreadyRegistered = string.Empty;
            EmailHelper.CleanupLoyaltyMailBox("regPL");
            EmailHelper.CleanupTeradataMailBox("Registration_Response2_");

            // Подготовка тестовых заявок на регистрацию
		    this.CreateRegistrationRequests(ref clientIdAlreadyRegistered);

            this.UploadTestFile(ResponseAttachmentFail);
            this.UploadTestFile(ResponseAttachment);

            var clientsWrapper = TestHelper.GetWrapper(EtlPackageIds.ReceiveRegistrationClientsJob);
		    var sessionId = Guid.Parse(clientsWrapper.Execute());
            TestHelper.AssertSuccess(clientsWrapper);

            // NOTE: Надо проверить что отправлен ответ на банковский ответ
            var tempFolder = Path.Combine(Path.GetTempPath(), string.Format("RegisterClients_{0}", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")));
            var subject = string.Format("Registration_Response2_{0}", DateTime.Now.ToString("yyyy_MM_dd"));
            EmailHelper.DownloadFilesFromTeradata(subject, tempFolder);
            VtbEncryptionHelper.Decrypt(tempFolder);
            int filesCount;
            FileReaderHelper.ReadAllLinesFromFolderFiles(tempFolder, out filesCount);
            Assert.IsTrue(filesCount > 0, "С почтового ящика не было загружено ни одного файла");

            List<ClientForRegistrationResponse> clientsFromDB;

            using (var uow = CreateUow())
            {
                var repository = uow.ClientForRegistrationResponseRepository;
                clientsFromDB = repository.GetAll().ToList();
            }

		    Assert.IsTrue(clientsFromDB.Count >= 4);            

	        Assert.IsTrue(
                clientsFromDB.Any(x => x.ClientId == "TestClientId0" && x.RegStatus == RegClientStatuses.InvalidSegment && x.Segment == -1),
                "Клиент с ид TestClientId0 должен быть InvalidSegment");
            Assert.IsTrue(
                clientsFromDB.Any(x => x.ClientId == "TestClientId1" && x.RegStatus == RegClientStatuses.Cancelled),
                "Клиент с ид TestClientId1 должен быть Cancelled");
	        Assert.IsTrue(
                clientsFromDB.Any(x => x.ClientId == "TestClientId2" && x.RegStatus == RegClientStatuses.Registred),
                "Клиент с ид TestClientId2 должен быть Cancelled");
            Assert.IsTrue(
               clientsFromDB.Any(x => x.ClientId == this.alreadyRegistredClientId && x.RegStatus == RegClientStatuses.AlreadyRegistred),
               "Клиент с ид TestClientId3 должен быть AlreadyRegistred");
            Assert.IsTrue(
               clientsFromDB.Any(x => x.ClientId == "RequestNotFound" && x.RegStatus == RegClientStatuses.RequestNotFound),
               "Клиент с ид RequestNotFound должен быть RequestNotFound");
            Assert.IsTrue(
               clientsFromDB.Any(x => x.ClientId == "RejectedClient" && x.RegStatus == RegClientStatuses.Cancelled),
               "Клиент с ид RejectedClient должен быть RequestNotFound");
            Assert.IsTrue(
               clientsFromDB.Any(x => x.ClientId == "NonFoundedClient" && x.RegStatus == RegClientStatuses.Cancelled),
               "Клиент с ид NonFoundedClient должен быть RequestNotFound");
        }

        private void UploadTestFile(string fileName)
        {
            var testDir = Path.Combine(this.TestContext.TestDeploymentDir, TestFilesFolder);
            
            var filePath = Path.Combine(testDir, fileName);

            VtbEncryptionHelper.Encrypt(testDir);
            EmailHelper.UploadFileToLoyaltySmtpServer(filePath, fileName);
        }

        private void CreateRegistrationRequests(ref string clientIdAlreadyRegistered)
        {            
            using (var uow = CreateUow())
            {
                var repository = uow.ClientForRegistrationRepository;

                repository.Add(this.GetTestClientDBRecord("TestClientId0"));
                repository.Add(this.GetTestClientDBRecord("TestClientId1"));
                repository.Add(this.GetTestClientDBRecord("TestClientId2"));

                var testClientDBRecord = this.GetTestClientDBRecord(this.alreadyRegistredClientId);
                repository.Add(testClientDBRecord);
                var stub = new SecurityWebApiStub();

                var parameters = new CreateUserOptions
                {
                    ClientId = testClientDBRecord.ClientId,
                    PhoneNumber = testClientDBRecord.MobilePhone,
                    RegistrationType = RegistrationType.SiteRegistration
                };
                stub.CreateUser(parameters);
                clientIdAlreadyRegistered = this.alreadyRegistredClientId;
                
                repository.Add(this.GetTestClientDBRecord("RejectedClient"));
                repository.Add(this.GetTestClientDBRecord("NonFoundedClient"));                
                
                uow.Save();
            }
        }

        [TestMethod]
        public void ShouldRemoveProgressBoxTest()
        {
            using (var client = new ImapClient("localhost", 143))
            {
                client.Login("loyalty@vtb24.loyalty", "mail", AuthMethod.Auto);               

                var messagesUid = new List<Tuple<uint, string>>();

                client.ListMailboxes()
                      .ToList()
                      .ForEach(mb => messagesUid.AddRange(client.Search(SearchCondition.Undeleted(), mb).Select(uid => new Tuple<uint, string>(uid,mb)).ToArray()));

                foreach (var uid in messagesUid)
                {
                    client.DeleteMessage(uid.Item1, uid.Item2);
                }                
            }
        }

		private ClientForRegistration GetTestClientDBRecord(string clientId)
		{
			var phone = "7" + random.Next(100000, 999999) + random.Next(1000, 9999);
			return new ClientForRegistration
				       {
					       ClientId = clientId,
					       Email = Internet.Email(),
					       FirstName = Name.First(),
					       LastName = Name.Last(),
					       MiddleName = "X",
					       MobilePhone = phone,
					       BirthDate = new DateTime(2013, 6, 15).AddYears(-random.Next(20, 50)),
				       };
		}
    }
}