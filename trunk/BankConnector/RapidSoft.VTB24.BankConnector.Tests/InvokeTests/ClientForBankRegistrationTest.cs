using System.Linq;
using System.Text.RegularExpressions;

using RapidSoft.VTB24.BankConnector.Tests.StubServices;

namespace RapidSoft.VTB24.BankConnector.Tests.InvokeTests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.VTB24.BankConnector.DataSource;
    using RapidSoft.VTB24.BankConnector.EtlExecutionWrapper;
    using RapidSoft.VTB24.BankConnector.Tests.Helpers;
    using RapidSoft.VTB24.VtbEncryption;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestClass]
    public class ClientForBankRegistrationTest : TestBase
    {
        private const string TestFileSource = "TestFiles\\VTB_20120531_2.regB";
        private const string EmailSubject = @"BankRegistration_";
        private const string EmailSubjectRequest = @"VTB_20120531_2.regB";
        private static readonly List<int> Data = new List<int> { 1, 2, 3 };

        [TestInitialize]
        public override void Init()
        {
            using (var uow = CreateUow())
            {
                var repo = uow.ClientForBankRegistrationRepository;
                foreach (var t in repo.GetAll())
                    repo.Delete(t);
                var repo2 = uow.ClientForBankRegistrationResponseRepository;
                foreach (var t in repo2.GetAll())
                    repo2.Delete(t);
                uow.Save();
            }
            base.Init();
			SecurityWebApiStub.CleanUpService();
        }

        [TestMethod]
        [DeploymentItem(TestFileSource)]
        public void ReceiveThenSendResponse()
        {
            const string guidRegex = "[0-9a-fA-F]{8}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{12}";

            #region Prepare mail to receive

            EmailHelper.CleanupLoyaltyMailBox("regB");
            EmailHelper.CleanupTeradataMailBox(EmailSubject);

            var fileName = Path.GetFileName(TestFileSource);
            File.Copy(fileName, Path.Combine(EncryptionTempFolder, fileName));
            VtbEncryptionHelper.Encrypt(EncryptionTempFolder);
            EmailHelper.UploadFileToLoyaltySmtpServer(Path.Combine(EncryptionTempFolder, fileName), EmailSubjectRequest);

            #endregion

            var job = TestHelper.GetWrapper(EtlPackageIds.RegisterBankClientsJob);
            var session = Guid.Parse(job.Execute());

            TestHelper.AssertJobResult(job, 0, 0);

            // NOTE: VTBPLK-1807: Проверим что заявки помечены IsDelete и что персональные данные удалены
            using (var uow = CreateUow())
            {
                var repo = uow.ClientForBankRegistrationRepository;

                // NOTE: Проверяем только если в сессии было хоть что-то
                var count = repo.GetAll().Count(x => x.SessionId == session);

                if (count > 0)
                {
                    var countEmail = repo.GetAll().Count(x => x.SessionId == session && x.Email != "N/A");
                    Assert.AreEqual(0, countEmail, "E-mail'ы должны быть удалены");

                    var countIsDelete = repo.GetAll().Count(x => x.SessionId == session && x.IsDeleted);
                    var countNotIsDelete = repo.GetAll().Count(x => x.SessionId == session && !x.IsDeleted);
                    Assert.AreEqual(4, countIsDelete);
                    Assert.AreEqual(0, countNotIsDelete);
                }
            }

            var resultfileName = EmailHelper.DownloadFilesFromTeradata(EmailSubject, DecryptionTempFolder);

            Assert.IsNotNull(resultfileName);
            Assert.IsTrue(resultfileName.Contains(EmailSubjectRequest), "Имя ответного файла должно совпадать с запрашиваемым");

            VtbEncryptionHelper.Decrypt(DecryptionTempFolder);

            int fileCount;
            var resultFilesContent = FileReaderHelper.ReadAllLinesFromFolderFiles(DecryptionTempFolder, out fileCount, null, true);

            Assert.IsTrue(resultFilesContent.Any(x => Regex.IsMatch(x, "^" + guidRegex + ";9168981541;1$")), "Отсутствует запись в результирующих файлах для номера 9168981541");
            Assert.IsTrue(resultFilesContent.Any(x => Regex.IsMatch(x, "^" + guidRegex + ";9168981542;1$")), "Отсутствует запись в результирующих файлах для номера 9168981542");
            Assert.IsTrue(resultFilesContent.Any(x => Regex.IsMatch(x, "^" + guidRegex + ";9168981543;1$")), "Отсутствует запись в результирующих файлах для номера 9168981543");
            Assert.IsTrue(resultFilesContent.Any(x => Regex.IsMatch(x, "^" + guidRegex + ";9168981543;2$")), "Отсутствует запись в результирующих файлах для номера 9168981543");

            EmailHelper.CleanupTeradataMailBox(EmailSubject);
        }
    }
}
