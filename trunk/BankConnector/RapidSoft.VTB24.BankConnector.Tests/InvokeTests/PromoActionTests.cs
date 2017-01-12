using System;
using System.IO;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RapidSoft.VTB24.BankConnector.EtlExecutionWrapper;
using RapidSoft.VTB24.BankConnector.Tests.Helpers;
using RapidSoft.VTB24.BankConnector.Tests.StubServices;

namespace RapidSoft.VTB24.BankConnector.Tests.InvokeTests
{
    using System.Globalization;

    using RapidSoft.Etl.Runtime;
    using RapidSoft.VTB24.BankConnector.Quartz_Jobs;
    using RapidSoft.VTB24.VtbEncryption;

    [TestClass]
    public class PromoActionTests
    {
        [TestMethod]
        public void ShouldSendPromoAction()
        {
            var dateTime = DateTime.Now.AddDays(-50);

            var assigments = new[]
            {
                new EtlVariableAssignment("FileDate", dateTime.ToString("yyyyMMdd", CultureInfo.InvariantCulture)), 
                new EtlVariableAssignment("FileNum", 0.ToString(CultureInfo.InvariantCulture))
            };

            var wrapper = TestHelper.GetWrapper("7772648e-708f-43d6-8153-c4caa3e2fb05", assigments);

			AdminMechanicsServiceStub.InitRules();
			var etlSession = wrapper.Execute();

            var isSuccess = TestHelper.IsSuccess(wrapper.PackageId, etlSession);
            Assert.IsTrue(isSuccess, "Пакет должен завершиться успешно");

			var countSended = TestHelper.CountSendedPromoAction(etlSession);
			Assert.AreEqual(2, countSended, "Заглушка в AdminMechanicsServiceProvider всегда возвращает 2 промоакции");
            EmailHelper.CleanupTeradataMailBox("PromoAction_");
        }

        [TestMethod]
        public void ShouldReceivePromoAction()
        {
            this.SendFile();

            var wrapper = TestHelper.GetWrapper("77500805-8169-4ae6-87a5-7fcae0fbf398");
            var etlSession = wrapper.Execute();

            var isSuccess = TestHelper.IsSuccess(wrapper.PackageId, etlSession);
            Assert.IsTrue(isSuccess, "Пакет должен завершиться успешно");

            var countReceived = TestHelper.CountReceivedPromoAction(etlSession);
            Assert.IsTrue(countReceived >= 3, "Отправили  3 промоакции в SendFile, но могли зацепить файлы предыдущих упавших тестов");
        }

        private void SendFile()
        {
            var rnd = new Random();
            var fileName = string.Format("VTB_20{0}1929_{1}.promoPL.response", rnd.Next(10, 100), rnd.Next());

            var uniqDic = Path.Combine(Directory.GetCurrentDirectory(), Guid.NewGuid().ToString());

            Directory.CreateDirectory(uniqDic);

            var file = Path.Combine(uniqDic, fileName);

            using (var st = File.Create(file))
            {
                const string FileContent = @"PromoId;Status
3;1
4;2
-5;3";
                var buf = Encoding.Default.GetBytes(FileContent);
                st.Write(buf, 0, buf.Length);
            }

            VtbEncryptionHelper.Encrypt(uniqDic);

            EmailHelper.CleanupLoyaltyMailBox("promoPL.response");
            EmailHelper.UploadFileToLoyaltySmtpServer(file, fileName);
        }
	}
}