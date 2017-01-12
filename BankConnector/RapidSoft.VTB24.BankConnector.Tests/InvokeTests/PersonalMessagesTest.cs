namespace RapidSoft.VTB24.BankConnector.Tests.InvokeTests
{
    using System.IO;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    using RapidSoft.VTB24.VtbEncryption;

    using RapidSoft.VTB24.BankConnector.DataSource;
    using RapidSoft.VTB24.BankConnector.EtlExecutionWrapper;
    using RapidSoft.VTB24.BankConnector.Tests.Helpers;

    using Rapidsoft.Loyalty.NotificationSystem.WsClients.ClientMessageService;

    [TestClass]
    public class PersonalMessagesTest : TestBase
    {
        private const string TestFileSource = "TestFiles\\VTB_20130610_5.messagePL";
        private const string EmailSubject = @"PersonalMessages_";
		private const string EmailSubjectRequest = "VTB_20130610_5.messagePL";

        [TestInitialize]
        public override void Init()
        {
            using (var uow = CreateUow())
            {
                var repository = uow.ClientPersonalMessageRepository;
                foreach (var t in repository.GetAll())
                    repository.Delete(t);
                var repoResponse = uow.ClientPersonalMessageResponseRepository;
                foreach (var t in repoResponse.GetAll())
                    repoResponse.Delete(t);
                uow.Save();
            }
            base.Init();
        }

        [TestMethod]
        [DeploymentItem(TestFileSource)]
        public void ReceiveThenSendResponse()
        {
            // Prepare mail to receive
            EmailHelper.CleanupLoyaltyMailBox(EmailSubject);
            var fileName = Path.GetFileName(TestFileSource);
            File.Copy(fileName, Path.Combine(EncryptionTempFolder, fileName));
            VtbEncryptionHelper.Encrypt(EncryptionTempFolder);
            EmailHelper.UploadFileToLoyaltySmtpServer(Path.Combine(EncryptionTempFolder, fileName), EmailSubjectRequest);

            var job = TestHelper.GetWrapper(EtlPackageIds.PersonalMessagesJob);
            job.Execute();
            
            Assert.IsTrue(job.IsSuccess());            
            EmailHelper.CleanupTeradataMailBox(EmailSubject);
        }
    }
}