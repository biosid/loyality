using System;

namespace RapidSoft.VTB24.BankConnector.Tests.RepositoryTests
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource;

    [TestClass]
    public class OrderForPaymentTests : TestBase
    {
        private const string TestSessionId = "F6C6D2A4-450C-4FF7-8C43-73991148C3ED";

        [TestInitialize]
        public void RefreshTestData()
        {
            using (var uow = CreateUow())
            {
                uow.OrderForPaymentRepository.DeleteBySessionId(Guid.Parse(TestSessionId));
                uow.OrderItemsForPaymentRepository.DeleteBySessionId(Guid.Parse(TestSessionId));
                uow.Save();
            }
        }

        [TestMethod]
        public void AddOrdersForPaymentTest()
        {
            var order = new OrderForPayment();
            order.OrderId = 1;
            order.PartnerId = 1;
            order.PartnerOrderNum = "stub";
            order.ClientId = "vtb_2";
            order.ArticleId = "stub";
            order.ArticleName = "stub";
            order.Amount = 1;
            order.OrderBonusCost = 1;
            order.OrderTotalCost = 1;
            order.OrderDateTime = DateTime.Now;
            order.POSId = "0";
            order.DeliveryRegion = "stub";
            order.DeliveryCity = "stub";
            order.DeliveryAddress = "stub";
            order.ContactName = "stub";
            order.ContactPhone = "stub";
            order.ContactEmail = "stub";
            order.EtlSessionId = Guid.Parse(TestSessionId);
            order.UnitellerDeliveryShopId = "vtb_partner_1";
            order.UnitellerItemsShopId = "vtb_partner_1";
            using (var uow = CreateUow())
            {
                uow.OrderForPaymentRepository.Add(order);                
                uow.Save();
            }
        }

        [TestMethod]
        public void AddOrderItemsForPaymentTest()
        {
            var order = new OrderItemsForPayment();
            order.OrderId = 1;
            order.OrderItemId = "1_0";
            order.PartnerId = 1;
            order.PartnerOrderNum = "stub";
            order.ClientId = "vtb_2";
            order.ArticleId = "stub";
            order.ArticleName = "stub";
            order.Amount = 1;
            order.OrderBonusCost = 1;
            order.OrderTotalCost = 1;
            order.OrderDateTime = DateTime.Now;
            order.POSId = "0";
            order.DeliveryRegion = "stub";
            order.DeliveryCity = "stub";
            order.DeliveryAddress = "stub";
            order.ContactName = "stub";
            order.ContactPhone = "stub";
            order.ContactEmail = "stub";
            order.EtlSessionId = Guid.Parse(TestSessionId);
            order.UnitellerDeliveryShopId = "vtb_partner_1";
            order.UnitellerItemsShopId = "vtb_partner_1";
            using (var uow = CreateUow())
            {
                uow.OrderItemsForPaymentRepository.Add(order);
                uow.Save();
            }
        }
    }
}
