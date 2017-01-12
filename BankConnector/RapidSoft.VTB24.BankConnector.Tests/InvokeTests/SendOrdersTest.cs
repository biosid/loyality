using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RapidSoft.Loaylty.ProductCatalog.WsClients.OrderManagementService;
using RapidSoft.VTB24.BankConnector.DataModels;
using RapidSoft.VTB24.BankConnector.DataSource;
using RapidSoft.VTB24.BankConnector.EtlExecutionWrapper;
using RapidSoft.VTB24.BankConnector.Tests.Helpers;

using Microsoft.Practices.Unity;

using RapidSoft.VTB24.BankConnector.Tests.StubServices;

namespace RapidSoft.VTB24.BankConnector.Tests.InvokeTests
{
    using RapidSoft.VTB24.BankConnector.Processors;
    using RapidSoft.VTB24.VtbEncryption;

    [TestClass]
	public class SendOrdersTest : TestBase
	{
		[TestInitialize]
		public void RepositoryCleanup()
		{
            DataSourceHelper.CleanUpTestOrder(315);
            DataSourceHelper.CleanUpTestOrder(316);
		}

		#region Тест выполнения отправки заказов на почту

		[TestMethod]
		public void ShouldSendOrder()
		{
            var subjectPrefix = string.Format("Orders_Request_{0}", DateTime.Now.ToString("yyyy_MM_dd"));

            EmailHelper.CleanupTeradataMailBox(subjectPrefix);

			OrderManagementServiceStub.InitTestOrders();

            var job = TestHelper.GetWrapper(EtlPackageIds.SendOrdersJob);
			job.Execute();

            // Проверяем что нет критичных ошибок выполнения
            TestHelper.AssertSuccess(job);

			var tempFolder = Path.Combine(Path.GetTempPath(), string.Format("SendOrders_{0}", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")));

			EmailHelper.DownloadFilesFromTeradata(subjectPrefix, tempFolder);

            int fileCount;
		    FileReaderHelper.ReadAllLinesFromFolderFiles(tempFolder, out fileCount, null, true);
		    Assert.IsTrue(fileCount > 0, "С почтового ящика не было загружено ни одного файла");

			Console.WriteLine("Расшифровка файлов в папке ({0})", tempFolder);
			VtbEncryptionHelper.Decrypt(tempFolder);

			var targetFileContent = FileReaderHelper.ReadAllLinesFromFolderFiles(tempFolder, out fileCount);
		    Assert.IsTrue(fileCount > 0, "Отсутствуют файлы после расшифровки");

			Assert.IsTrue(targetFileContent.Count > 1, "В выгруженных на почтовый ящик файлах нет данных");
			Assert.AreEqual(
                "OrderId;OrderItemId;PartnerId;PartnerOrderNum;ClientId;ArticleId;ArticleName;Amount;OrderBonusCost;OrderTotalCost;OrderDateTime;POSId;DeliveryRegion;DeliveryCity;DeliveryAddress;ContactName;ContactPhone;ContactEmail;Advance;AdvancePOSId;AdvanceRRN",
				targetFileContent[0], "Неверная строка заголовков CSV файла");

            const string expectedMask =
                "^315;315_1;1;externalOrderId;vtb_2;ProductId;ItemName;17;18\\.00;19\\.00;20[0-9]{2}-[0-9]{2}-[0-9]{2}T[0-9]{2}:[0-9]{2}:[0-9]{2};vtb_partner_1;_RegionTitle;_CityTitle;Курьерская доставка. _AddressText;LastName FirstName MiddleName;9271234567;email;;;$";

		    const string expectedDeliveryMask =
                "^315;315_0;1;externalOrderId;vtb_2;;Доставка;1;10\\.00;13\\.00;20[0-9]{2}-[0-9]{2}-[0-9]{2}T[0-9]{2}:[0-9]{2}:[0-9]{2};vtb_partner_1;_RegionTitle;_CityTitle;Курьерская доставка. _AddressText;LastName FirstName MiddleName;9271234567;email;;;$";

			Assert.IsTrue(targetFileContent.Any(x => Regex.IsMatch(x, expectedMask)),
				string.Format("Files does not contains test order record. Expected record regex: \r\n {0} \r\n Files content:\r\n {1}",
                    expectedMask, targetFileContent.Aggregate((x, y) => string.Format("{0}\r\n{1}", x, y))));

            Assert.IsTrue(targetFileContent.Any(x => Regex.IsMatch(x, expectedDeliveryMask)),
                string.Format("Files does not contains test order record. Expected record regex: \r\n {0} \r\n Files content:\r\n {1}",
                    expectedDeliveryMask, targetFileContent.Aggregate((x, y) => string.Format("{0}\r\n{1}", x, y))));

		}

		#endregion

		#region Тест выполнения чтения ответного файла с сервера

		[TestMethod]
		public void ShouldReceiveOrder()
		{
			this.SendFile();

            OrderManagementServiceStub.InitTestOrders();

            var job = TestHelper.GetWrapper(EtlPackageIds.ReceiveOrdersJob);
		    var testSession = TestHelper.CreateWaitingEtlSession(job.PackageId);		    

		    var testOrders = OrderManagementServiceStub.TestOrders;
		    using (var uow = CreateUow())
            {
                foreach (var testOrder in testOrders)
                {
                    var order = SendOrdersProcessor.Map(testOrder);
                    var orderItems = SendOrdersProcessor.MapOrderItems(testOrder, null);

                    order.UnitellerItemsShopId = "vtb_partner_1";
                    order.UnitellerDeliveryShopId = "vtb_partner_1";
                    order.POSId = "vtb_partner_1";
                    order.EtlSessionId = new Guid(testSession.EtlSessionId);

                    uow.OrderForPaymentRepository.Add(order);

                    foreach (var orderItem in orderItems)
                    {
                        orderItem.UnitellerItemsShopId = "vtb_partner_1";
                        orderItem.UnitellerDeliveryShopId = "vtb_partner_1";
                        orderItem.POSId = "vtb_partner_1";

                        orderItem.EtlSessionId = new Guid(testSession.EtlSessionId);

                        uow.OrderItemsForPaymentRepository.Add(orderItem);
                    }
                }
                
                uow.Save();
            }

		    var sessionId = Guid.Parse(job.Execute());

            TestHelper.AssertSuccess(job);

		    OrderPaymentResponse[] receivedOrders;
		    OrderPaymentResponse2[] response2Orders;
            using (var uow = CreateUow())
            {
                receivedOrders = uow.OrderPaymentResponseRepository.GetAll().Where(x => x.EtlSessionId == sessionId).ToArray();

                response2Orders =
                    uow.OrderPaymentResponse2Repository.GetAll().Where(x => x.EtlSessionId == sessionId).ToArray();
            }

			Assert.IsTrue(receivedOrders.Any(x => x.OrderId == 315 && x.Status == (int)ReceivedOrderStatus.Approved));

            Assert.IsTrue(response2Orders.Any(x => x.OrderId == 315 && 
                          x.ItemPaymentStatus == (int)OrderPaymentStatuses.Yes && 
                          x.DeliveryPaymentStatus == (int)OrderDeliveryPaymentStatus.Yes));

            DataSourceHelper.CleanUpTestOrder(315);
            EmailHelper.CleanupTeradataMailBox("Orders_Response2_{0}");
		}

		private void SendFile()
		{
			var rnd = new Random();
			var fileName = string.Format("VTB_20{0}1929_{1}.orderPL.response", rnd.Next(10, 100), rnd.Next());

			var uniqDic = Path.Combine(Directory.GetCurrentDirectory(), Guid.NewGuid().ToString());

			Directory.CreateDirectory(uniqDic);

			var file = Path.Combine(uniqDic, fileName);

			using (var st = File.Create(file))
			{
				const string FileContent = @"OrderId;Status;Reason
315;1;;";
				var buf = Encoding.Default.GetBytes(FileContent);
				st.Write(buf, 0, buf.Length);
			}

			VtbEncryptionHelper.Encrypt(uniqDic);

			var subjectPrefix = "Orders_Request_{0}";
			EmailHelper.CleanupLoyaltyMailBox("orderPL");
            EmailHelper.CleanupTeradataMailBox(subjectPrefix);

			DataSourceHelper.CleanUpTestOrder(-3);
			DataSourceHelper.CleanUpTestOrder(-4);
			DataSourceHelper.CleanUpTestOrder(-5);
            EmailHelper.UploadFileToLoyaltySmtpServer(file, fileName);
		}
		
        #endregion
	}
}
