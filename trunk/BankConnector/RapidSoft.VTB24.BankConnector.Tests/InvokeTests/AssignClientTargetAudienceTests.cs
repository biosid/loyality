namespace RapidSoft.VTB24.BankConnector.Tests.InvokeTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.VTB24.BankConnector.EtlExecutionWrapper;
    using RapidSoft.VTB24.BankConnector.Tests.Helpers;
    using RapidSoft.VTB24.VtbEncryption;

    [TestClass]
    public class AssignClientTargetAudienceTests : TestBase
    {
        private const string EmailSubject = @"VTB_20130610_5.campaignPL";

        private const string ReceivePathFormat = EmailSubject + "{0}\\Receive";
        private const string ResponsePathFormat = EmailSubject + "{0}\\Response";

        private static readonly string ReceiveFilesFolder = Path.Combine(Path.GetTempPath(),
            string.Format(ReceivePathFormat, Guid.NewGuid() + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")));
        private static readonly string ResponseFilesFolder = Path.Combine(Path.GetTempPath(),
            string.Format(ResponsePathFormat, Guid.NewGuid() + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")));

        private static readonly string clientId = Guid.NewGuid().ToString();

        private static Dictionary<string, string> Data;

        [TestMethod]
        public void AssignClientTargetAudience()
        {
	        Data = new Dictionary<string, string>()
		               {
			               { Guid.NewGuid().ToString(), clientId },
			               { Guid.NewGuid().ToString(), clientId }
		               };
            Init();
            var wrapper = ExecuteEtl();
            AssertEtlExecutionResult(wrapper);
        }
        
        private static void AssertEtlExecutionResult(WrapperBase wrapper)
        {
	        Assert.IsTrue(wrapper.IsSuccess());

            EmailHelper.DownloadFilesFromTeradata("AssignClientTargetAudience", ResponseFilesFolder);

            var files = Directory.GetFiles(ResponseFilesFolder).Where(f => f.Contains("campaignPL.response")).ToList();

            Assert.IsTrue(files.Any(), "С почтового ящика не было загружено ни одного файла с подходящим именем");

            VtbEncryptionHelper.Decrypt(ResponseFilesFolder);

            var result = new List<string>();

            foreach(var filePath in files.Select(file => Path.Combine(ResponseFilesFolder, file)))
            {
                FileReaderHelper.UncompressFile(filePath, ResponseFilesFolder);

                using (var tr = new StreamReader(filePath))
                {
                    string str;
                    do
                    {
                        str = tr.ReadLine();

                        if (string.IsNullOrEmpty(str))
                        {
                            continue;
                        }

                        result.AddRange(str.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries));
                    }
                    while (!string.IsNullOrEmpty(str));
                }
            }

            Assert.IsTrue(result.Any());

            foreach (var i in Data)
            {
                Assert.IsTrue(result.Contains(i.Key), string.Format("Отправленные данные должны включать ключ ({0}), данные в отправленных файлах: \r\n {1}", i.Key, result));
                Assert.IsTrue(result.Contains(i.Value), string.Format("Отправленные данные должны включать значение ({0}), данные в отправленных файлах: \r\n {1}", i.Value, result));
            }
        }
        
        private static WrapperBase ExecuteEtl()
        {
            var wrapper = TestHelper.GetWrapper(EtlPackageIds.AssignClientTargetAudienceJob);
	        wrapper.Execute();
            return wrapper;
        }

        private static void Init()
        {
            if (!Directory.Exists(ReceiveFilesFolder))
            {
                Directory.CreateDirectory(ReceiveFilesFolder);
            }
            
            if (!Directory.Exists(ResponseFilesFolder))
            {
                Directory.CreateDirectory(ResponseFilesFolder);
            }

            var requestFileContent = new StringBuilder("PromoId;ClientId");

            foreach (var i in Data)
            {
                requestFileContent.AppendFormat("\r\n{0};{1}", i.Key, i.Value);
            }

            var testFileName = Path.Combine(ReceiveFilesFolder, string.Format("VTB_{0}_5.campaignPL", DateTime.Now.ToString("yyyyMMdd")));

            File.AppendAllText(testFileName, requestFileContent.ToString());

            VtbEncryptionHelper.Encrypt(ReceiveFilesFolder);

            EmailHelper.CleanupLoyaltyMailBox("campaignPL");
            EmailHelper.CleanupTeradataMailBox("AssignClientTargetAudience");
            EmailHelper.UploadFileToLoyaltySmtpServer(testFileName, EmailSubject);
        }
    }
}
