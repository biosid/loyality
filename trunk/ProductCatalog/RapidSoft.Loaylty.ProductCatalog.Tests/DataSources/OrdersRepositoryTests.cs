using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidSoft.Loaylty.ProductCatalog.API.Entities;
using RapidSoft.Loaylty.ProductCatalog.DataSources;
using RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories;

namespace RapidSoft.Loaylty.ProductCatalog.Tests.DataSources
{
    using System.Linq;

    using Common;

    [TestClass]
    public class OrdersRepositoryTests
    {
        private string clientId = TestDataStore.TestClientId;
        private string userId = TestDataStore.TestUserId;

        [TestMethod]
        public void ShouldExists()
        {
            var repository = new OrdersRepository();

            Assert.IsFalse(repository.Exists(91919191));
            Assert.IsFalse(repository.Exists(null, "ofjfjfjf"));

            var source = new OrdersDataSource();
            var externalId = Guid.NewGuid();

            var order = TestDataStore.GetOrder();
            order.ExternalOrderId = externalId.ToString();

            var orderId = source.Insert(order);

            Assert.IsTrue(repository.Exists(orderId));
            Assert.IsTrue(repository.Exists(null, externalId.ToString()));

            repository.Delete(source.GetOrder(orderId));

            Assert.IsFalse(repository.Exists(orderId));
        }
        
        [TestMethod]
        public void ShouldReturnOrdersForPayment()
        {
            var orderDs = new OrdersDataSource();
            var repo = new OrdersRepository();

            var page = repo.GetForPayment(0, 10000);
            var total = page.TotalCount.Value;

            Order order1 = RandomHelper.RandomOrder();
            order1.PaymentStatus = OrderPaymentStatuses.No;
            order1.Status = OrderStatuses.DeliveryWaiting;
            var order1Id = orderDs.Insert(order1);

            Order order2 = RandomHelper.RandomOrder();
            order2.PaymentStatus = OrderPaymentStatuses.No;
            order2.Status = OrderStatuses.Delivery;
            var order2Id = orderDs.Insert(order2);

            Order order3 = RandomHelper.RandomOrder();
            order3.PaymentStatus = OrderPaymentStatuses.No;
            order3.Status = OrderStatuses.Delivered;
            var order3Id = orderDs.Insert(order3);

            Order order4 = RandomHelper.RandomOrder();
            order4.PaymentStatus = OrderPaymentStatuses.No;
            order4.Status = OrderStatuses.DeliveredWithDelay;
            var order4Id = orderDs.Insert(order4);

            Order order5 = RandomHelper.RandomOrder();
            order5.PaymentStatus = OrderPaymentStatuses.No;
            order5.Status = OrderStatuses.DeliveredWithDelay;
            var order5Id = orderDs.Insert(order5);

            page = repo.GetForPayment(0, total + 50);

            Assert.IsTrue(page.Any(x => x.Id == order1Id));
            Assert.IsTrue(page.Any(x => x.Id == order2Id));
            Assert.IsTrue(page.Any(x => x.Id == order3Id));
            Assert.IsTrue(page.Any(x => x.Id == order4Id));
            Assert.IsTrue(page.Any(x => x.Id == order5Id));

            repo.Delete(orderDs.GetOrder(order1Id));
            repo.Delete(orderDs.GetOrder(order2Id));
            repo.Delete(orderDs.GetOrder(order3Id));
            repo.Delete(orderDs.GetOrder(order4Id));
            repo.Delete(orderDs.GetOrder(order5Id));
        }

         [TestMethod]
        public void ShouldReturnOrderId()
         {
             Partner p = new Partner
                         {
                             Name = RandomHelper.RandomString(256),
                             Type = PartnerType.Direct,
                             InsertedUserId = TestDataStore.TestUserId,
                             InsertedDate = DateTime.Now
                         };
             var partnerRepository = new PartnerRepository();
             p = partnerRepository.CreateOrUpdate(userId, p, settings: null);

             var orderDs = new OrdersDataSource();

             Order order = RandomHelper.RandomOrder();
             order.PartnerId = p.Id;
             order.PaymentStatus = OrderPaymentStatuses.No;
             order.Status = OrderStatuses.DeliveryWaiting;
             var orderId = orderDs.Insert(order);

             var repo = new OrdersRepository();
             var ids = repo.GetIds(OrderStatuses.DeliveryWaiting, OrderPaymentStatuses.No, PartnerType.Direct, null);

             Assert.IsTrue(ids.Any(x => x == orderId));

             repo.Delete(orderDs.GetOrder(orderId));
             partnerRepository.Delete(p.Id);
         }

        [TestMethod]
        public void NoOrdersForPayment()
        {
            var repo = new OrdersRepository();
            var orderDs = new OrdersDataSource();

            var page = repo.GetForPayment(0, 1);
            var before = page.TotalCount.Value;

            Order order = RandomHelper.RandomOrder();
            order.PaymentStatus = OrderPaymentStatuses.No;
            order.Status = OrderStatuses.Draft;
            var orderId = orderDs.Insert(order);

            page = repo.GetForPayment(0, before + 5);

            Assert.IsTrue(page.All(x => x.Id != orderId));
            
            repo.Delete(orderDs.GetOrder(orderId));
        }
    }
}
