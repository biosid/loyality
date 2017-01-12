namespace RapidSoft.Loaylty.IntegrationTests.ProductCatalog
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.ProductCatalog.WsClients.BasketService;
    using RapidSoft.Loaylty.ProductCatalog.WsClients.CatalogAdminService;
    using RapidSoft.Loaylty.ProductCatalog.WsClients.OrderManagementService;

    using Contact = RapidSoft.Loaylty.ProductCatalog.WsClients.OrderManagementService.Contact;
    using DeliveryAddress = Loaylty.ProductCatalog.WsClients.OrderManagementService.DeliveryAddress;
    using DeliveryInfo = RapidSoft.Loaylty.ProductCatalog.WsClients.OrderManagementService.DeliveryInfo;
    using DeliveryTypes = Loaylty.ProductCatalog.WsClients.OrderManagementService.DeliveryTypes;
    using Order = Loaylty.ProductCatalog.WsClients.OrderManagementService.Order;
    using OrderStatuses = RapidSoft.Loaylty.ProductCatalog.WsClients.OrderManagementService.OrderStatuses;
    using PhoneNumber = RapidSoft.Loaylty.ProductCatalog.WsClients.OrderManagementService.PhoneNumber;
    using PickupPoint = Loaylty.ProductCatalog.WsClients.OrderManagementService.PickupPoint;
    using PublicOrderStatuses = RapidSoft.Loaylty.ProductCatalog.WsClients.OrderManagementService.PublicOrderStatuses;

    [TestClass]
    public class OrderTest
    {
        private readonly string clientId = "intUser";

        private readonly Dictionary<string, string> clientContext = TestHelper.GetClientContext();
        
        [TestMethod]
        public void CanCreateOzonDeliveryOrder()
        {
            var productId = TestUtils.FindProduct(TestHelper.OzonPartnerId).ProductId;
            var basketItem = TestUtils.AddBasketItem(productId);

            var deliveryDto = new DeliveryDto
            {
                ExternalDeliveryVariantId = "55",
                Address = new DeliveryAddress
                {
                    PostCode = TestHelper.PostCode,
                    TownTitle = "TownTitle",
                    StreetTitle = "StreetTitle",
                    House = "22б"
                },
                Contact = TestHelper.GetContact(),
                Comment = "без опозданий",
                DeliveryType = DeliveryTypes.Delivery,
                DeliveryVariantLocation = new Location
                {
                    KladrCode = "7700000000000",
                    PostCode = TestHelper.PostCode
                }
            };

            // Создаём заказ
            var draftOrder = CreateOrder(basketItem, deliveryDto, productId);

            // Клиент подтверждает заказ
            ClientCommitOrder(draftOrder.Id);

            Thread.Sleep(1100);

            // Партнёр обновляет статус заказа
            ChangeExternalOrdersStatus(draftOrder.Id);

            AssertOrder(draftOrder);
        }

        [TestMethod]
        public void CanCreateOzonPickUpOrder()
        {
            var partner = TestUtils.FindPartner(p => p.Name == "Озон");
            var product = TestUtils.FindProduct(partner.Id);
            var productId = product.ProductId;
            var basketItem = TestUtils.AddBasketItem(product.ProductId);

            var deliveryDto = new DeliveryDto
            {
                ExternalDeliveryVariantId = "1205",
                PickupPoint = new PickupPoint()
                {
                    ExternalPickupPointId = "569891"
                },
                Contact = TestHelper.GetContact(),
                Comment = "без опозданий",
                DeliveryType = DeliveryTypes.Pickup,
                DeliveryVariantLocation = new Location
                {
                    PostCode = TestHelper.PostCode
                }
            };

            // Создаём заказ
            var draftOrder = CreateOrder(basketItem, deliveryDto, productId);

            // Клиент подтверждает заказ
            ClientCommitOrder(draftOrder.Id);

            Thread.Sleep(1100);

            // Партнёр обновляет статус заказа
            ChangeExternalOrdersStatus(draftOrder.Id);

            AssertOrder(draftOrder);
        }

        #region Methods

        private void AssertOrder(Order draftOrder)
        {
            using (var client = new OrderManagementServiceClient())
            {
                var order = client.GetOrderById(draftOrder.Id, clientId);

                Assert.IsNotNull(order);
                Assert.AreEqual(order.Success, true);
                Assert.AreEqual(order.Order.ExternalOrderId, "778");
                Assert.AreEqual(order.Order.ExternalOrderStatusCode, "Бух");
                Assert.AreEqual(order.Order.OrderStatusDescription, "Успешно доставлен");
                Assert.AreNotEqual(order.Order.OrderStatusDescription, draftOrder.ExternalOrderStatusDateTime);
            }
        }

        private static void ChangeExternalOrdersStatus(int orderId)
        {
            ChangeExternalOrdersStatusesResult changeOrdersStatusesResult;
            using (var client = new OrderManagementServiceClient())
            {
                changeOrdersStatusesResult =
                    client.ChangeExternalOrdersStatuses(
                        new[]
                        {
                            new ExternalOrdersStatus
                            {
                                OrderId = orderId,
                                OrderStatus = OrderStatuses.Delivered,
                                ExternalOrderId = 778.ToString(CultureInfo.InvariantCulture),
                                ExternalOrderStatusCode = "Бух",
                                OrderStatusDescription = "Успешно доставлен"
                            }
                        });
            }

            Assert.IsNotNull(changeOrdersStatusesResult);
            Assert.IsTrue(changeOrdersStatusesResult.Success, changeOrdersStatusesResult.ResultDescription);
        }

        private ClientCommitOrderResult ClientCommitOrder(int orderId)
        {
            ClientCommitOrderResult commitClientOrder;

            using (var client = new OrderManagementServiceClient())
            {
                commitClientOrder = client.ClientCommitOrder(clientId, orderId);

                Assert.IsNotNull(commitClientOrder);
                Assert.IsTrue(commitClientOrder.Success, commitClientOrder.ResultDescription);
                var order = client.GetOrderById(orderId, clientId);

                Assert.IsNotNull(order);
                Assert.AreEqual(order.Success, true);
                Assert.IsNotNull(order.Order.ExternalOrderId, "Идентфикатор заказа в системе партнера должен быть заполнен либо ид от партнера, либо подставить наш ид.");
                Assert.IsNotNull(order.Order.ExternalOrderStatusDateTime);
            }

            return commitClientOrder;
        }

        private Order CreateOrder(GetBasketItemResult basketItem, DeliveryDto deliveryDto, string productId)
        {
            CreateOrderResult createOrderResult;

            using (var client = new OrderManagementServiceClient())
            {
                var parameters = new CreateOrderFromBasketItemsParameters()
                {
                    BasketItems = new[]
                    {
                        basketItem.Item.Id
                    },
                    ClientContext = clientContext,
                    ClientId = clientId,
                    Delivery = deliveryDto
                };

                createOrderResult = client.CreateOrderFromBasketItems(parameters);
            }

            Assert.IsNotNull(createOrderResult);
            Assert.IsTrue(createOrderResult.Success, string.Format("{0} {1}", createOrderResult.ResultCode, createOrderResult.ResultDescription));
            Assert.IsNotNull(createOrderResult.Order);

            var draftOrder = createOrderResult.Order;

            Assert.AreEqual(draftOrder.PublicStatus, PublicOrderStatuses.Registration);
            Assert.AreEqual(draftOrder.Status, OrderStatuses.Registration);

            Assert.IsNotNull(draftOrder.Items);
            Assert.AreEqual(1, draftOrder.Items.Count());
            Assert.IsTrue(draftOrder.Items.Any());
            Assert.AreEqual(productId, draftOrder.Items[0].Product.ProductId);

            return createOrderResult.Order;
        }

        #endregion
    }
}