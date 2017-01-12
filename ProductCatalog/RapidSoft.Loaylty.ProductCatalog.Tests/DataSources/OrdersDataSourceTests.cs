namespace RapidSoft.Loaylty.ProductCatalog.Tests.DataSources
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;

    using Extensions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.API.InputParameters;
    using RapidSoft.Loaylty.ProductCatalog.DataSources;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories;
    using RapidSoft.Loaylty.ProductCatalog.Tests.Common;

    [TestClass]
    public class OrdersDataSourceTests
    {
        private string clientId = TestDataStore.TestClientId;
		
        [TestMethod]
        public void ChangeOrderStatus()
        {
            var orderRepo = new OrdersDataSource();
            
            Order order = RandomHelper.RandomOrder();
            order.Status = OrderStatuses.Draft;
            order.OrderStatusDescription = "description";

            const string description = "changed description";
            
            var orderId = orderRepo.Insert(order);

            var orderStatuseses = new[] { new OrdersStatus { OrderId = orderId, OrderStatus = OrderStatuses.Registration, OrderStatusDescription = description } };
            orderRepo.UpdateOrdersStatuses(orderStatuseses, "updatedUserId");

            var savedOrder = orderRepo.GetOrder(orderId);
            Assert.AreEqual(OrderStatuses.Registration, savedOrder.Status);
            Assert.AreEqual(description, savedOrder.OrderStatusDescription);
        }

        [TestMethod]
        public void ShouldNotChangeOrderStatusWhenInvalidNewStatus()
        {
            var orderRepo = new OrdersDataSource();
            Order order = RandomHelper.RandomOrder();
            order.Status = OrderStatuses.DeliveredWithDelay;
            var orderId = orderRepo.Insert(order);

            var orderStatuseses = new OrdersStatus { OrderId = orderId, OrderStatus = OrderStatuses.Processing };
            orderRepo.UpdateOrdersStatuses(new[] { orderStatuseses }, "updatedUserId");

            var savedOrder = orderRepo.GetOrder(orderId);
            Assert.AreEqual(OrderStatuses.DeliveredWithDelay, savedOrder.Status);
        }

        [TestMethod]
        public void ShouldNotChangeOrderStatusWhenNotOwner()
        {
            var orderRepo = new OrdersDataSource();
            Order order = RandomHelper.RandomOrder();
            order.Status = OrderStatuses.DeliveredWithDelay;
            var orderId = orderRepo.Insert(order);

            var otherCleintId = "Other" + order.ClientId;
            var orderStatuseses = new OrdersStatus
                                  {
                                      OrderId = orderId,
                                      OrderStatus = OrderStatuses.Processing,
                                      ClientId = otherCleintId
                                  };

            var result = orderRepo.UpdateOrdersStatuses(new[] { orderStatuseses }, "updatedUserId").Single();

            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result.Success);
            Assert.IsTrue(result.ResultDescription.Contains("is not owner of order"));

            var savedOrder = orderRepo.GetOrder(orderId);
            Assert.AreEqual(OrderStatuses.DeliveredWithDelay, savedOrder.Status);
        }

        [TestMethod]
        public void ChangeOrderPaymentStatus()
        {
            var orderRepo = new OrdersDataSource();
            Order order = RandomHelper.RandomOrder();
            order.PaymentStatus = OrderPaymentStatuses.No;
            var orderId = orderRepo.Insert(order);

            orderRepo.UpdateOrdersPaymentStatuses(new[]
            {
                new OrdersPaymentStatus
                {
                    OrderId = orderId,
                    PaymentStatus = OrderPaymentStatuses.Yes
                }
            }, "updatedUserId");

            var savedOrder = orderRepo.GetOrder(orderId);
            Assert.AreEqual(OrderPaymentStatuses.Yes, savedOrder.PaymentStatus);
        }

        [TestMethod]
        public void ChangeOrderDeliveryStatus()
        {
            var orderRepo = new OrdersDataSource();
            Order order = RandomHelper.RandomOrder();
            order.DeliveryPaymentStatus = OrderDeliveryPaymentStatus.No;
            var orderId = orderRepo.Insert(order);

            var ordersDeliveryStatus = new OrdersDeliveryStatus
                                       {
                                           OrderId = orderId,
                                           DeliveryStatus = OrderDeliveryPaymentStatus.Yes
                                       };
            orderRepo.UpdateOrdersDeliveryStatuses(new[] { ordersDeliveryStatus }, "updatedUserId");

            var savedOrder = orderRepo.GetOrder(orderId);
            Assert.AreEqual(OrderDeliveryPaymentStatus.Yes, savedOrder.DeliveryPaymentStatus);
        }

        [TestMethod]
        public void ShouldNotChangeDeliveryStatusWhenNotOwner()
        {
            var orderRepo = new OrdersDataSource();
            Order order = RandomHelper.RandomOrder();
            order.DeliveryPaymentStatus = OrderDeliveryPaymentStatus.No;
            var orderId = orderRepo.Insert(order);

            var otherCleintId = "Other" + order.ClientId;
            var ordersDeliveryStatus = new OrdersDeliveryStatus
                                       {
                                           OrderId = orderId,
                                           DeliveryStatus = OrderDeliveryPaymentStatus.Yes,
                                           ClientId = otherCleintId
                                       };
            var result = orderRepo.UpdateOrdersDeliveryStatuses(new[] { ordersDeliveryStatus }, "updatedUserId");

            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result.First().Success);
            Assert.IsTrue(result.First().ResultDescription.Contains("is not owner of order"));
        }

        [TestMethod]
        public void SearchOrders()
        {
            var dt = DateTime.Now;
            var ds = new OrdersDataSource();
            var phone = new PhoneNumber { CityCode = "123", CountryCode = "RU", LocalNumber = "4563423" };

            var order = CreateOrder(phone, OrderPaymentStatuses.No);

            var orders = ds.SearchOrders(dt.AddMinutes(-1), dt.AddMinutes(1), null, null, null, null, null, 0, 10, false);
            Assert.IsNotNull(orders.FirstOrDefault());
        }

        [TestMethod]
        public void SearchOrdersWithCarrierIdFilter()
        {
            var carrierId = new List<int> { 1 };

            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                var order = ctx.Orders.FirstOrDefault();

                if (order == null)
                {
                    throw new AssertInconclusiveException("No orders found");
                }

                order.Items = new OrderItem[]
                    {
                        new OrderItem()
                        {
                            Product = new Product()
                            {                                
                            }
                        }
                    };
                
                order.CarrierId = carrierId[0];

                new OrdersRepository().UpdateOrderInternal(order);
            }

            var dt = DateTime.Now;

            var orders = new OrdersDataSource().SearchOrders(dt.AddYears(-1), dt.AddYears(1), null, null, null, null, null, 0, 10, false, null, carrierId.ToArray());

            foreach (var order1 in orders)
            {
                Assert.IsTrue(order1.CarrierId.HasValue && order1.CarrierId == carrierId[0]);
            }
        }

        [TestMethod]
        public void SearchOnlyPaymentStatusYesOrders()
        {
            var dt = DateTime.Now;
            var ds = new OrdersDataSource();
            var phone = new PhoneNumber { CityCode = "123", CountryCode = "RU", LocalNumber = "4563423" };

            var firstId = CreateOrder(phone, OrderPaymentStatuses.No);

            var second = CreateOrder(phone, OrderPaymentStatuses.Yes);

            var orders = ds.SearchOrders(dt.AddMinutes(-1), dt.AddMinutes(1), null, null, new[] { OrderPaymentStatuses.Yes }, null, null, 0, 10, false);

            Assert.IsNull(orders.FirstOrDefault(t => t.Id == firstId));
        }

        private static int CreateOrder(PhoneNumber phone, OrderPaymentStatuses orderPayStat)
        {
            var first = RandomHelper.RandomOrder();
            first.DeliveryInfo = new DeliveryInfo
            {
                Contact = new Contact
                {
                    Phone = phone
                }
            };
            first.ClientId = RandomHelper.RandomString(10);
            first.Items = TestDataStore.GetOrderItems();
            first.PaymentStatus = orderPayStat;
            var firstId = new OrdersDataSource().Insert(first);
            return firstId;
        }

        [TestMethod]
        public void SearchOrdersByPartnerId()
        {
            var dt = DateTime.Now;
            var ds = new OrdersDataSource();
            var phone = new PhoneNumber { CityCode = "123", CountryCode = "RU", LocalNumber = "4563423" };

            var first = CreateOrder(phone, OrderPaymentStatuses.No);

            var orders = ds.SearchOrders(dt.AddMinutes(-1), dt.AddMinutes(1), null, null, null, null, new[] { TestDataStore.PartnerId }, 0, 10, false);

            Assert.IsNotNull(orders.FirstOrDefault(t => t.Id == first));
        }

        [TestMethod]
        public void ShouldReturnOrderById()
        {
            var now = DateTime.Now;
            var ds = new OrdersDataSource();

            ds.Insert(TestDataStore.GetOrder());

            var list = ds.GetOrders(this.clientId, now.AddYears(-50), now.AddYears(1), null, null, 0, 10, false);

            Assert.IsTrue(list.Any());

            var orderId = list.First().Id;

            var order = ds.GetOrder(orderId);

            Assert.IsNotNull(order);
        }

        [TestMethod]
        public void ShouldReturnOrdersWithStatuses()
        {
            var now = DateTime.Now;
            var ds = new OrdersDataSource();

            ds.Insert(TestDataStore.GetOrder());

            var list = ds.GetOrders(this.clientId, now.AddYears(-50), now.AddDays(10), null, null, 0, 10, false);

            var status = list.Select(x => x.Status).Distinct().First();

            var listWithSkip = ds.GetOrders(this.clientId, now.AddYears(-50), now, null, new[] { status }, 0, 10, false);

            Assert.IsTrue(listWithSkip.All(x => x.Status != status));

            var listWithTake = ds.GetOrders(this.clientId, now.AddYears(-50), now, new[] { status }, null, 0, 10, false);

            Assert.IsTrue(listWithTake.All(x => x.Status == status));

            var listEmpty = ds.GetOrders(
                this.clientId, now.AddYears(-50), now, new[] { status }, new[] { status }, 0, 10, false);

            Assert.AreEqual(listEmpty.Count, 0);
        }

        [TestMethod]
        public void ShouldReturnLastDeliveryAddresses()
        {
            var testClientId = Guid.NewGuid().ToString();
            var orderDs = new OrdersDataSource();

            Order order1 = RandomHelper.RandomOrder();
            order1.DeliveryInfo = null;
            order1.ClientId = testClientId;
            var order1Id = orderDs.Insert(order1);

            Order order2 = RandomHelper.RandomOrder();
            order2.DeliveryInfo = TestDataStore.GetDeliveryInfo();
            order2.ClientId = testClientId;
            var order2Id = orderDs.Insert(order2);

            Order order3 = RandomHelper.RandomOrder();
            order3.DeliveryInfo = TestDataStore.GetDeliveryInfo();
            order3.ClientId = testClientId;
            var order3Id = orderDs.Insert(order3);

            var adresses = orderDs.GetLastDeliveryAddresses(testClientId, false);

            Assert.AreEqual(1, adresses.Count());

            var repo = new OrdersRepository();
            repo.Delete(orderDs.GetOrder(order1Id));
            repo.Delete(orderDs.GetOrder(order2Id));
            repo.Delete(orderDs.GetOrder(order3Id));
        }

        [TestMethod]
        public void ShouldDeserealizeOrderTest()
        {
            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                foreach (var order in ctx.Orders)
                {
                    var items = XmlSerializer.Deserialize<OrderItem[]>(order.ItemsXml);
                    var deliveryInfo = XmlSerializer.Deserialize<DeliveryInfo>(order.DeliveryInfoXml);
                }
            }
        }
    }
}
