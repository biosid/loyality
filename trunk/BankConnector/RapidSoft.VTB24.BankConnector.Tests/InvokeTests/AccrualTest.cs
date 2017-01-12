
namespace RapidSoft.VTB24.BankConnector.Tests.InvokeTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource;
    using RapidSoft.VTB24.BankConnector.Tests.Helpers;
    using RapidSoft.VTB24.VtbEncryption;

    [TestClass]
    public class AccrualTest:TestBase
    {
		private const string TestFilesFolder = "AccrualReceiveTransactions";
		private const string AttachFileName = "VTB_20130610_5.Nachislpl";
        
		[TestMethod]
        [DeploymentItem("TestFiles\\" + AttachFileName, TestFilesFolder)]
        public void ReceiveAccruals()
		{
			EmailHelper.CleanupLoyaltyMailBox(AttachFileName);

			var testDir = Path.Combine(TestContext.TestDeploymentDir, TestFilesFolder);
			var filePath = Path.Combine(testDir, AttachFileName);
			VtbEncryptionHelper.Encrypt(testDir);

            EmailHelper.UploadFileToLoyaltySmtpServer(filePath, AttachFileName);

            var job = TestHelper.GetWrapper(EtlPackageIds.ReceiveAccrualsJob);

			var sessionId = Guid.Parse(job.Execute());

            TestHelper.AssertJobResult(job, 0, 1);

			List<Accrual> importedAccruals;
		    using (var uow = CreateUow())
		    {
		        importedAccruals = uow.AccrualRepository.GetAll().Where(x => x.ReceiveEtlSessionId == sessionId).ToList();
		        CleanupTestRecords(uow, sessionId);
		    }

			Assert.AreEqual(4, importedAccruals.Count);
			Assert.IsTrue(importedAccruals.All(x => x.ReceiveEtlSessionId == sessionId));
            Assert.IsTrue(importedAccruals.Where(x => !String.IsNullOrEmpty(x.ExternalId)).All(x => x.ExternalId.IndexOf('.') == 36));

		}

		private void CleanupTestRecords(IUnitOfWork uow, Guid sessionId)
		{
			var accrualRepository = uow.AccrualRepository;
            var testAccruals = accrualRepository.GetAll().Where(x => x.ReceiveEtlSessionId == sessionId).ToList();
			testAccruals.ForEach(accrualRepository.Delete);
			uow.Save();
		}
    }
}
