using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidSoft.Loaylty.ProductCatalog.API.Entities;
using RapidSoft.Loaylty.ProductCatalog.DataSources;
using RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories;
using RapidSoft.Loaylty.ProductCatalog.Entities;
using RapidSoft.Loaylty.ProductCatalog.Tests.Common;

namespace RapidSoft.Loaylty.ProductCatalog.Tests.DataSources
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
        Justification = "Для тестов можно опустить.")]
    [TestClass]
    public class OrdersNotificationsRepositoryTests
    {
        [TestMethod]
        public void ShouldFillOrdersNotifications()
        {
            var repository = new OrdersNotificationsRepository();

            var etlSessionId = Guid.NewGuid().ToString();
            repository.FillOrdersNotifications(etlSessionId, 1000);
        }

        [TestMethod]
        public void ShouldFillOneOrderNotification()
        {
            // добавим один заказ
            var ordersDataSource = new OrdersDataSource();
            var order = RandomHelper.RandomOrder();
            order.Status = OrderStatuses.DeliveryWaiting;
            ordersDataSource.Insert(order);

            var repository = new OrdersNotificationsRepository();

            var etlSessionId = Guid.NewGuid().ToString();
            var count = repository.FillOrdersNotifications(etlSessionId, 1);

            Assert.AreEqual(count, 1);
        }

        [TestMethod]
        public void ShouldFillTwoOrdersNotifications()
        {
            // добавим два заказа
            var ordersDataSource = new OrdersDataSource();
            foreach (var order in Enumerable.Range(0, 2).Select(i => RandomHelper.RandomOrder()))
            {
                order.Status = OrderStatuses.DeliveryWaiting;
                ordersDataSource.Insert(order);
            }

            var repository = new OrdersNotificationsRepository();

            var etlSessionId = Guid.NewGuid().ToString();
            var count = repository.FillOrdersNotifications(etlSessionId, 2);

            Assert.AreEqual(count, 2);
        }

        [TestMethod]
        public void ShouldFillTwoOrdersNotificationsWithThreeOrders()
        {
            // добавим три заказа
            var ordersDataSource = new OrdersDataSource();
            foreach (var order in Enumerable.Range(0, 3).Select(i => RandomHelper.RandomOrder()))
            {
                order.Status = OrderStatuses.DeliveryWaiting;
                ordersDataSource.Insert(order);
            }

            var repository = new OrdersNotificationsRepository();

            var etlSessionId = Guid.NewGuid().ToString();
            var count = repository.FillOrdersNotifications(etlSessionId, 2);

            Assert.AreEqual(count, 2);
        }

        [TestMethod]
        public void ShouldReturnOrdersNotifications()
        {
            // добавим один заказ
            var ordersDataSource = new OrdersDataSource();
            var order = RandomHelper.RandomOrder();
            order.Status = OrderStatuses.DeliveryWaiting;
            var orderId = ordersDataSource.Insert(order);
            order = ordersDataSource.GetOrder(orderId);

            var repository = new OrdersNotificationsRepository();

            // создадим нотификации
            var etlSessionId = Guid.NewGuid().ToString();
            repository.FillOrdersNotifications(etlSessionId, 1000);

            // получим нотификации
            var notifications = repository.GetOrdersNotifications(etlSessionId);

            Assert.IsNotNull(notifications);
            Assert.IsTrue(notifications.Length > 0);

            var notification = notifications.Single(n => n.OrderId == orderId);

            Assert.AreEqual(order.InsertedDate, notification.CreateDate);
            Assert.AreEqual(order.ExternalOrderId, notification.ExternalOrderId);
            Assert.AreEqual(order.PartnerId, notification.PartnerId);
            Assert.AreEqual(order.Items[0].Product.ProductId, notification.Items[0].ProductId);
            Assert.AreEqual(order.Items[0].Product.Name, notification.Items[0].ProductName);
            Assert.AreEqual(order.Items[0].Amount, notification.Items[0].ProductQuantity);
            Assert.AreEqual(order.TotalCost, notification.TotalCost);
        }

        [TestMethod]
        public void ShouldSaveAndReturnEmail()
        {
            var etlSessionId = Guid.NewGuid().ToString("D");

            var email = new OrdersNotificationsEmail
            {
                EtlSessionId = etlSessionId,
                Subject = "test",
                Body = "test",
                Recipients = "a@b.c",
                Status = OrdersNotificationsEmailStatus.Error
            };

            var repository = new OrdersNotificationsRepository();

            repository.SaveEmails(new[] { email });

            var emails = repository.GetOrdersNotificationsEmails(etlSessionId);

            Assert.IsNotNull(emails);
            Assert.AreEqual(emails.Length, 1);
        }
    }
}
