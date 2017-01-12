namespace RapidSoft.Loaylty.ProductCatalog.Tests.DataSources
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Transactions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.API.InputParameters;
    using RapidSoft.Loaylty.ProductCatalog.API.OutputResults;
    using RapidSoft.Loaylty.ProductCatalog.DataSources;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories;
    using RapidSoft.Loaylty.ProductCatalog.Tests.Common;

    [TestClass]
    public class OrderHistoryRepositoryTests
    {
        [TestMethod]
        public void Insert()
        {
            var orderRepo = new OrdersDataSource();
            Order order = RandomHelper.RandomOrder();
            var orderId = orderRepo.Insert(order);
            var orderHistoryRepo = new OrdersHistoryRepository();
            var orders = orderHistoryRepo.GetOrderHistory(orderId, 0, 10, false);
            Assert.IsNotNull(orders.FirstOrDefault());
        }

        [TestMethod]
        public void ShouldReturnCorrectOrderHistoryAfterDifferentChanges()
        {
            var orderRepo = new OrdersDataSource();
            Order order = RandomHelper.RandomOrder();
            order.Status = OrderStatuses.Draft;
            var orderId = orderRepo.Insert(order);

            // NOTE: Эти изменения должны отображаться в истории как 1 строка
            orderRepo.UpdateOrdersStatuses(new[] { new OrdersStatus { OrderId = orderId, OrderStatus = OrderStatuses.Registration } }, "updatedUserId");
            orderRepo.UpdateOrdersStatuses(new[] { new OrdersStatus { OrderId = orderId, OrderStatus = OrderStatuses.Registration } }, "updatedUserId");
            Thread.Sleep(2);

            // NOTE: Эти изменения должны отображаться в истории как 3 строки
            orderRepo.UpdateOrdersPaymentStatuses(new[] { new OrdersPaymentStatus { OrderId = orderId, PaymentStatus = OrderPaymentStatuses.Yes } }, "updatedUserId");
            orderRepo.UpdateOrdersPaymentStatuses(new[] { new OrdersPaymentStatus { OrderId = orderId, PaymentStatus = OrderPaymentStatuses.Yes } }, "updatedUserId");
            orderRepo.UpdateOrdersDeliveryStatuses(new[] { new OrdersDeliveryStatus { OrderId = orderId, DeliveryStatus = OrderDeliveryPaymentStatus.Yes } }, "updatedUserId");
            Thread.Sleep(2);

            // NOTE: Эти изменения должны отображаться в истории как 1 строка
            orderRepo.UpdateOrdersStatuses(new[] { new OrdersStatus { OrderId = orderId, OrderStatus = OrderStatuses.Processing } }, "updatedUserId");
            orderRepo.UpdateOrdersStatuses(new[] { new OrdersStatus { OrderId = orderId, OrderStatus = OrderStatuses.Processing } }, "updatedUserId");

            var orderHistoryRepo = new OrdersHistoryRepository();
            OrderHistoryPage orders = orderHistoryRepo.GetOrderHistory(orderId, 0, 10, false);
            Assert.AreEqual(1 + 3 + 1, orders.Count());
        }

        [TestMethod]
        public void ChangeOrderStatusOnceGetTwoHistoryRecords()
        {
            const string statusDescription = "statusDescription";
            const string newStatusDescription = "new statusDescription";
            
            var orderRepo = new OrdersDataSource();
            Order order = RandomHelper.RandomOrder();
            order.OrderStatusDescription = statusDescription;
            
            var orderId = orderRepo.Insert(order);

            Thread.Sleep(1000);

            var orderStatuseses = new[] { new OrdersStatus { OrderId = orderId, OrderStatus = OrderStatuses.Registration, OrderStatusDescription = newStatusDescription } };
            orderRepo.UpdateOrdersStatuses(orderStatuseses, "updatedUserId");

            var orderHistoryRepo = new OrdersHistoryRepository();
            var orders = orderHistoryRepo.GetOrderHistory(orderId, 0, 10, false).OrderBy(h => h.UpdatedDate).ToList();

            Assert.AreEqual(2, orders.Count());
            Assert.IsTrue(orders[0].IsOrderStatusDescriptionChanged);
            Assert.IsTrue(orders[1].IsOrderStatusDescriptionChanged);

            Assert.IsNull(orders[0].OldOrderStatusDescription);
            Assert.AreEqual(statusDescription, orders[0].NewOrderStatusDescription);

            Assert.AreEqual(newStatusDescription, orders[1].NewOrderStatusDescription);
            Assert.AreEqual(statusDescription, orders[1].OldOrderStatusDescription);
        }

        [TestMethod]
        public void ChangeExternalOrderIdGetOnlyInsertHistoryRecord()
        {
            var orderRepo = new OrdersDataSource();
            var order = RandomHelper.RandomOrder();
            var orderId = orderRepo.Insert(order);

            var statuses = new[]
                           {
                               new ExternalOrdersStatus
                               {
                                   OrderId = orderId,
                                   ExternalOrderId = "test",
                                   ExternalOrderStatusCode = "test",
                                   OrderStatusDescription = "test",
                               }
                           };
            orderRepo.UpdateExternalOrdersStatuses(statuses, "updatedUserId");


            var orderHistoryRepo = new OrdersHistoryRepository();
            var orders = orderHistoryRepo.GetOrderHistory(orderId, 0, 10, false);

            Assert.AreEqual(1, orders.Count());
        }

        [TestMethod]
        public void ShouldNotChangeExternalOrderIdWhenInvalidNewStatus()
        {
            var orderRepo = new OrdersRepository();
            var ordersDataSource = new OrdersDataSource();
            var order = RandomHelper.RandomOrder();
            order.Status = OrderStatuses.Processing;
            var orderId = ordersDataSource.Insert(order);

            IList<OrderStatuses> l = orderRepo.GetNextOrderStatuses(order.Status);
            l.Add(order.Status);
            var all = Enum.GetValues(typeof(OrderStatuses)).Cast<OrderStatuses>();
            OrderStatuses notNext = all.Except(l).First();

            var externalOrdersStatus = new ExternalOrdersStatus
                                       {
                                           OrderId = orderId,
                                           ExternalOrderId = "test",
                                           ExternalOrderStatusCode = "test",
                                           OrderStatusDescription = "test",
                                           OrderStatus = notNext
                                       };

            ChangeExternalOrderStatusResult result = ordersDataSource.UpdateExternalOrdersStatuses(new[] { externalOrdersStatus }, "FSY").Single();

            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result.Success);
        }

        [TestMethod]
        public void ShouldNotChangeExternalOrderIdWhenNotOwner()
        {
            var orderRepo = new OrdersRepository();
            var ordersDataSource = new OrdersDataSource();
            var order = RandomHelper.RandomOrder();
            order.Status = OrderStatuses.Processing;
            var orderId = ordersDataSource.Insert(order);

            IList<OrderStatuses> l = orderRepo.GetNextOrderStatuses(order.Status);
            l.Add(order.Status);
            var all = Enum.GetValues(typeof(OrderStatuses)).Cast<OrderStatuses>();
            OrderStatuses notNext = all.Except(l).First();

            var otherCleintId = "Other" + order.ClientId;

            var externalOrdersStatus = new ExternalOrdersStatus
            {
                OrderId = orderId,
                ExternalOrderId = "test",
                ExternalOrderStatusCode = "test",
                OrderStatusDescription = "test",
                OrderStatus = notNext,
                ClientId = otherCleintId
            };

            ChangeExternalOrderStatusResult result = ordersDataSource.UpdateExternalOrdersStatuses(new[] { externalOrdersStatus }, "FSY").Single();

            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result.Success);
            Assert.IsTrue(result.ResultDescription.Contains("is not owner of order"));
        }

        [TestMethod]
        public void ChangeStatusThenPaymentInOrder()
        {
            var orderRepo = new OrdersDataSource();
            Order order = RandomHelper.RandomOrder();
            var orderId = orderRepo.Insert(order);

            var orderStatuseses = new[] { new OrdersStatus { OrderId = orderId, OrderStatus = OrderStatuses.Registration } };
            orderRepo.UpdateOrdersStatuses(orderStatuseses, "updatedUserId");

            var ordersPaymentStatuses = new[] { new OrdersPaymentStatus { OrderId = orderId, PaymentStatus = OrderPaymentStatuses.Yes } };
            orderRepo.UpdateOrdersPaymentStatuses(ordersPaymentStatuses, "updatedUserId");

            var orderHistoryRepo = new OrdersHistoryRepository();
            var orders = orderHistoryRepo.GetOrderHistory(orderId, 0, 10, false);

            Assert.AreEqual(3, orders.Count());
        }

        [TestMethod]
        public void ShouldNotChangePaymentStatusWhenNotOwner()
        {
            var orderRepo = new OrdersDataSource();
            Order order = RandomHelper.RandomOrder();
            var orderId = orderRepo.Insert(order);

            var otherCleintId = "Other" + order.ClientId;
            var ordersPaymentStatus = new OrdersPaymentStatus
                                      {
                                          OrderId = orderId,
                                          PaymentStatus = OrderPaymentStatuses.Yes,
                                          ClientId = otherCleintId
                                      };
            var ordersPaymentStatuses = new[] { ordersPaymentStatus };
            var result = orderRepo.UpdateOrdersPaymentStatuses(ordersPaymentStatuses, "updatedUserId");

            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result.First().Success);
            Assert.IsTrue(result.First().ResultDescription.Contains("is not owner of order"));
        }

        [TestMethod]
        public void ChangeStatusPaymentPaymentStatus()
        {
            var orderRepo = new OrdersDataSource();
            Order order = RandomHelper.RandomOrder();
            var orderId = orderRepo.Insert(order);

            var orderStatuseses = new OrdersStatus { OrderId = orderId, OrderStatus = OrderStatuses.Registration };
            var result = orderRepo.UpdateOrdersStatuses(new[] { orderStatuseses }, "OrderStatus").Single();
            Assert.AreEqual(true, result.Success);

            Thread.Sleep(10);
            var ordersPaymentStatuses = new[] { new OrdersPaymentStatus { OrderId = orderId, PaymentStatus = OrderPaymentStatuses.Yes } };
            var results1 = orderRepo.UpdateOrdersPaymentStatuses(ordersPaymentStatuses, "PaymentStatus");
            Assert.IsTrue(results1.All(x => x.Success));

            Thread.Sleep(10);
            var paymentStatuses = new[] { new OrdersPaymentStatus { OrderId = orderId, PaymentStatus = OrderPaymentStatuses.Error } };
            results1 = orderRepo.UpdateOrdersPaymentStatuses(paymentStatuses, "PaymentStatus");
            Assert.IsTrue(results1.All(x => x.Success));

            Thread.Sleep(10);
            var ordersStatus = new OrdersStatus { OrderId = orderId, OrderStatus = OrderStatuses.Processing };
            result = orderRepo.UpdateOrdersStatuses(new[] { ordersStatus }, "OrdersStatus").Single();
            Assert.AreEqual(true, result.Success);

            var orderHistoryRepo = new OrdersHistoryRepository();
            var orders = orderHistoryRepo.GetOrderHistory(orderId, 0, 10, false);

            Assert.AreEqual(5, orders.Count());
            Assert.AreNotEqual(orders[4].OldStatus, orders[4].NewStatus); // insert
            Assert.AreNotEqual(orders[3].OldStatus, orders[3].NewStatus); // first update
            Assert.AreNotEqual(orders[2].OldOrderPaymentStatus, orders[2].NewOrderPaymentStatus); // second update
            Assert.AreNotEqual(orders[1].OldOrderPaymentStatus, orders[1].NewOrderPaymentStatus);
            Assert.AreNotEqual(orders[0].OldStatus, orders[0].NewStatus);
        }
    }
}
