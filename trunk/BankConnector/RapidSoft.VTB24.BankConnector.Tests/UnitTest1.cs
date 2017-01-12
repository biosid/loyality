using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RapidSoft.VTB24.BankConnector.Tests.Helpers;

namespace RapidSoft.VTB24.BankConnector.Tests
{
    using RapidSoft.Loaylty.ProductCatalog.WsClients.WishListService;

    [TestClass]
    public class MailSendTest
    {
        [TestMethod]
        public void ShouldReadAfterSend()
        {
            Assert.Inconclusive("Отладочный тест для Алексея Маречева");

            const string TestMailSubject = "TestMail";
            const int messCount = 20;
            var tempGuid = Guid.NewGuid().ToString();
            var tempFile = Path.GetRandomFileName();

            var testText = string.Format("Test mail file text ({0})", tempGuid);

            for (var i = 0; i < messCount; i++)
            {
                File.AppendAllText(tempFile + i, testText);
            }

            EmailHelper.CleanupLoyaltyMailBox(TestMailSubject);

            for (var i = 0; i < messCount; i++)
            {
                EmailHelper.UploadFileToLoyaltySmtpServer(tempFile + i, TestMailSubject);
            }

            var tempFolder = Path.GetTempPath() + "Test_File_From_Mail_" + tempGuid;
            EmailHelper.DownloadFilesFromLoyalty(TestMailSubject, tempFolder);

            var fileCount = 0;
            var totalFilesCount = 0;
            int attempts = 0;
            var fileContent = new List<string>();
            while (totalFilesCount < messCount && attempts < 5)
            {
                fileContent.AddRange(FileReaderHelper.ReadAllLinesFromFolderFiles(tempFolder, out fileCount));
                totalFilesCount += fileCount;
            }

            Assert.AreEqual(messCount, fileCount, "В почтовом ящике находится неверное количество писем");
            Assert.IsTrue(fileContent.TrueForAll(x=>x.Equals(testText)), "Содержимое файла в ходе отправки и получения изменилось");
        }

        [Ignore]
        [TestMethod]
        public void ShouldDelTest()
        {
            using (var client = new WishListServiceClient())
            {
                var res = client.GetWishList(
                    new GetWishListParameters()
                    {

                    });
            }
        }
    }
}
