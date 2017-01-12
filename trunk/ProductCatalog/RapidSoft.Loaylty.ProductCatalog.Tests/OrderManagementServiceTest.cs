using RapidSoft.Loaylty.ProductCatalog.Services.Delivery;

namespace RapidSoft.Loaylty.ProductCatalog.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlTypes;
    using System.Diagnostics;
    using System.Linq;
    using System.Text.RegularExpressions;

    using API.InputParameters;

    using Common;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using PartnersConnector.WsClients.PartnersOrderManagementService;

    using RapidSoft.Etl.LogSender;
    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.ProductCatalog.API;
    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.API.OutputResults;
    using RapidSoft.Loaylty.ProductCatalog.DataSources;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories;
    using RapidSoft.Loaylty.ProductCatalog.Interfaces;
    using RapidSoft.Loaylty.ProductCatalog.Services;
    using RapidSoft.VTB24.ArmSecurity;

    using CreateOrderFromOnlinePartnerParameters = API.InputParameters.CreateOrderFromOnlinePartnerParameters;
    using DeliveryInfo = RapidSoft.Loaylty.ProductCatalog.API.Entities.DeliveryInfo;
    using ExternalOrdersStatus = API.InputParameters.ExternalOrdersStatus;
    using GetOrderByExternalIdParameters = API.InputParameters.GetOrderByExternalIdParameters;
    using GetOrdersHistoryParameters = API.InputParameters.GetOrdersHistoryParameters;
    using IOrderManagementService = RapidSoft.Loaylty.ProductCatalog.API.IOrderManagementService;
    using Order = RapidSoft.Loaylty.ProductCatalog.API.Entities.Order;
    using OrderStatuses = RapidSoft.Loaylty.ProductCatalog.API.Entities.OrderStatuses;
    using PartnerOrderCommitment = API.InputParameters.PartnerOrderCommitment;
    using RapidSoft.Loaylty.ProductCatalog.Fake;
    using RapidSoft.Loaylty.PromoAction.WsClients.MechanicsService;

    [TestClass]
    public class OrderManagementServiceTest
    {       
        private readonly IOrderManagementService orderManagementService = new OrderManagementService();

        private readonly Dictionary<string, string> clientContext = TestDataStore.GetClientContext();
        private readonly int partnerId = TestDataStore.OzonPartnerId;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var mock = MockFactory.GetUserService();
            var service = mock.Object;
            ArmSecurity.UserServiceCreator = () => service;
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            ArmSecurity.UserServiceCreator = null;
        }

        [TestMethod]
        public void CanCreateOrderTest()
        {
            // Создать заказ
            var clientId = TestDataStore.TestClientId;
            string productId = TestHelper.GetAnyProductId();
            Assert.IsTrue(productId.StartsWith(partnerId + "_"), "Тестовый продукт должен быт от тестового партнера");

            var partner = new PartnerRepository().GetById(partnerId);
            Assert.AreEqual(PartnerType.Offline, partner.Type, "В этом тесте тестовый партнер должен быть типа " + PartnerType.Offline);

            var basketService = GetBasketService();
            var res = basketService.Add(clientId, productId, 1, clientContext);

            Assert.IsNotNull(res);
            Assert.IsTrue(res.Success, res.ResultCode + " " + res.ResultDescription);

            var partnerConnectorProvider = MockFactory.GetPartnerConnectorProvider(1, ConfirmedStatuses.AddToQueue);

            var service = GetService(basketService, partnerConnectorProvider);

            ICatalogAdminService adminService = MockFactory.GetCatalogAdminService();

            var deliveryDto = TestDataStore.GetDeliveryDto();

            var createOrderResult = CreateOrder(service, deliveryDto, res.BasketItemId, clientId);

            Assert.IsNotNull(createOrderResult);
            Assert.IsTrue(createOrderResult.Success, createOrderResult.ResultDescription);
            Assert.IsNotNull(createOrderResult.Order);
            Assert.IsTrue(!string.IsNullOrEmpty(deliveryDto.Address.AddressText));

            var draftOrder = createOrderResult.Order;
            Assert.AreEqual(OrderStatuses.Registration, draftOrder.Status);
            
            Assert.IsNotNull(draftOrder.Items);
            Assert.AreEqual(1, draftOrder.Items.Length);
            Assert.IsNotNull(draftOrder.Items[0].Product);
            Assert.AreEqual(draftOrder.Items[0].Product.ProductId, productId);
            Assert.IsTrue(draftOrder.Items[0].PriceRur > 0, "Цена Items[0].PriceRUR должна быть больше нуля");
            Assert.IsTrue(draftOrder.Items[0].PriceBonus > 0, "Цена Items[0].PriceBonus должна быть больше нуля");
            Assert.IsTrue(draftOrder.Items[0].AmountPriceRur > 0, "Цена Items[0].AmountPriceRur должна быть больше нуля");
            Assert.IsTrue(draftOrder.Items[0].AmountPriceBonus > 0, "Цена Items[0].AmountPriceBonus должна быть больше нуля");

            // Клиент подтверждает заказ
            var commitClientOrder = service.ClientCommitOrder(clientId, createOrderResult.Order.Id);

            Assert.IsNotNull(commitClientOrder);
            Assert.IsTrue(commitClientOrder.Success, commitClientOrder.ResultDescription);

            // Партнёр подтверждает заказ
            var partnerOrderCommitment = new PartnerOrderCommitment
                                         {
                                             OrderId = createOrderResult.Order.Id,
                                             IsConfirmed = true,
                                             ExternalOrderId = "ABCD" + createOrderResult.Order.Id
                                         };
            var commitPartnerResult = adminService.PartnerCommitOrder(
                TestDataStore.TestUserId, partnerId, new[] { partnerOrderCommitment });

            Assert.IsNotNull(commitPartnerResult);
            Assert.IsTrue(commitPartnerResult.Success, commitPartnerResult.ResultDescription);

            // NOTE: Партнёр обновляет статус заказа, причем перескакивает через 
            var externalOrdersStatus = new ExternalOrdersStatus
                                       {
                                           OrderId = commitClientOrder.Order.Id,
                                           ClientId = clientId,
                                           OrderStatus = OrderStatuses.Delivery,
                                           OrderStatusDescription = "Доставлен с задержкой"
                                       };

            var changeOrdersStatusesResult = new OrderManagementService().ChangeExternalOrderStatus(externalOrdersStatus);

            Assert.IsNotNull(changeOrdersStatusesResult);
            Assert.IsTrue(changeOrdersStatusesResult.Success, changeOrdersStatusesResult.ResultDescription);

            partnerConnectorProvider.Verify(s => s.CheckOrder(It.IsAny<Order>()), Times.Once());
            partnerConnectorProvider.Verify(s => s.CommitOrder(It.IsAny<Order>()), Times.Once());            
        }

        [TestMethod]
        public void ShouldNotChangeOrderStatuses()
        {
            var result = new OrderManagementService().ChangeExternalOrdersStatuses(null);

            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void ShouldCreateOrderFromOnlinePartner()
        {
            var clientId = TestDataStore.TestClientId;
            var partnerId = TestDataStore.BiletixPartnerID;
            var externalOrderId = Guid.NewGuid().ToString();

            var service = new OrderManagementService(mechanicsProvider: MockFactory.GetMechanicsProvider().Object);
            var items = TestDataStore.GetCreateOrderFromOnlinePartnerItems();
            var createParameters = new CreateOrderFromOnlinePartnerParameters
                             {
                                 ClientId = clientId,
                                 PartnerId = partnerId,
                                 ExternalOrderId = externalOrderId,
                                 Items = items
                             };
            var result = service.CreateOnlinePartnerOrder(createParameters);

            Assert.IsTrue(result.Success, result.ResultDescription);
            Assert.IsNotNull(result.Order);

            var getParameters = new GetOrderByExternalIdParameters
                                {
                                    ClientId = clientId,
                                    ExternalOrderId = externalOrderId,
                                    PartnerId = partnerId
                                };
            var orderByExternalId = service.GetOrderByExternalId(getParameters);

            Assert.IsNotNull(orderByExternalId, "Заказ онлайн партнёра должен быть получен по ExternalId");
            Assert.IsNotNull(orderByExternalId.Order, "Заказ онлайн партнёра должен быть получен по ExternalId");
            Assert.IsNull(orderByExternalId.Order.DeliveryInfo, "Заказ онлайн партнёра должен быть без DeliveryInfo");

            new OrdersRepository().Delete(result.Order);
        }

        [TestMethod]
        public void ShouldReturnOrderPaymentInfo()
        {
            var order = RandomHelper.RandomOrder();
            order.DeliveryPaymentStatus = OrderDeliveryPaymentStatus.Error;
            order.PaymentStatus = OrderPaymentStatuses.Yes;
            var orderId = new OrdersDataSource().Insert(order);

            var service = new OrderManagementService();

            var result = service.GetOrderPaymentStatuses(orderId.MakeArray());

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Success);
            var statusesInfo = result.OrderPaymentStatuses.FirstOrDefault();
            Assert.IsNotNull(statusesInfo);
            Assert.AreEqual(order.PaymentStatus, statusesInfo.PaymentStatus);
            Assert.AreEqual(order.DeliveryPaymentStatus, statusesInfo.DeliveryPaymentStatus);

            var repo = new OrdersRepository();
            order = repo.GetById(orderId);
            repo.Delete(order);
        }

        [TestMethod]
        public void ShouldNotCreateOrderFromBasketItem()
        {
            var orderManagmentService = new OrderManagementService();

            var result = orderManagmentService.CreateOrderFromBasketItems(new CreateOrderFromBasketItemsParameters());

            Assert.IsFalse(result.Success);
            Assert.AreEqual(ResultCodes.INVALID_PARAMETER_VALUE, result.ResultCode);

            Assert.IsFalse(Regex.IsMatch("23323kjk43", @"^\d*$"));
            Assert.IsTrue(Regex.IsMatch("2332343", @"^\d*$"));
        }
        
        [TestMethod]
        public void ShouldSearchOrders()
        {
            var clientId = TestDataStore.TestClientId;
            var param = new GetOrdersHistoryParameters(
                clientId, SqlDateTime.MinValue.Value, SqlDateTime.MaxValue.Value, null, int.MinValue, int.MaxValue, true);

            var result = this.orderManagementService.GetOrdersHistory(param);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Success, true);
            Assert.IsNotNull(result.TotalCount);

            param = new GetOrdersHistoryParameters(
                clientId,
                SqlDateTime.MinValue.Value,
                SqlDateTime.MaxValue.Value,
                null,
                int.MinValue,
                int.MaxValue,
                false);

            result = this.orderManagementService.GetOrdersHistory(param);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Success, true);
            Assert.IsNull(result.TotalCount);
        }

        [TestMethod]
        public void ShouldNotGetOrdersHistory()
        {
            const string UserId = "99999";
            var param = new GetOrdersHistoryParameters(
                UserId, SqlDateTime.MinValue.Value, SqlDateTime.MaxValue.Value, null, int.MinValue, int.MaxValue, true);

            var result = this.orderManagementService.GetOrdersHistory(param);

            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void ShouldDeleteBasketItemAfterCommitForOfflinePartner()
        {
            var partnerConnectorProvider = MockFactory.GetPartnerConnectorProvider();

            var basketService = GetBasketService();

            var service = GetService(basketService, partnerConnectorProvider);

            var clientId = Guid.NewGuid().ToString();
            var testProductId = TestHelper.GetAnyProductId();
            Assert.IsTrue(testProductId.StartsWith(partnerId + "_"), "Тестовый продукт должен быт от тестового партнера");

            var partner = new PartnerRepository().GetById(partnerId);
            Assert.AreEqual(PartnerType.Offline, partner.Type, "В этом тесте тестовый партнер должен быть типа " + PartnerType.Offline);

            var basketItemRepository = new BasketItemRepository();

            basketItemRepository.Add(clientId: clientId, productId: testProductId, quantity: 1, newId: Guid.NewGuid());

            var deliveryInfo = TestDataStore.GetDeliveryDto();

            var res = basketService.Add(clientId, testProductId, 1, clientContext);

            Assert.IsNotNull(res);
            Assert.IsTrue(res.Success, res.ResultCode + " " + res.ResultDescription);

            var createOrderResult = CreateOrder(service, deliveryInfo, res.BasketItemId, clientId);
            
            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                // NOTE: Элемент корзины есть
                Assert.IsTrue(ctx.BasketItems.Any(x => x.ProductId == testProductId && x.ClientId == clientId));
            }

            service.ClientCommitOrder(clientId, createOrderResult.Order.Id);

            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                // NOTE: Элемент корзины НЭ эсть :)
                Assert.IsFalse(ctx.BasketItems.Any(x => x.ProductId == testProductId && x.ClientId == clientId));
            }
        }

        [TestMethod]
        public void ShouldDeleteBasketItemAfterCommitForDirectPartner()
        {
            var partnerId = 555;
            var partner = new Partner
                          {
                              Id = partnerId,
                              Type = PartnerType.Direct
                          };
            var productId = "DirectPartnerProduct";
            var orderId = 500;
            var orderItem = new API.Entities.OrderItem
                            {
                                Product = new Product()
                                {
                                   ProductId  = productId
                                } 
                            };
            var testClentId = Guid.NewGuid().ToString();
            var order = new Order
                        {
                            ClientId = testClentId,
                            Id = orderId,
                            PartnerId = partnerId,
                            Items = new[] { orderItem },
                        };

            var basketServiceMock = new Mock<IBasketService>();
            basketServiceMock.Setup(x => x.Remove(It.IsAny<string>(), It.IsAny<string>()))
                .Callback(
                        (string client, string product) =>
                        {
                            Assert.AreEqual(
                                productId,
                                product,
                                "В данном случаем удаление должно выполниться для известного productId = DirectPartnerProduct");
                            Assert.AreEqual(
                                testClentId, client, "В данном случаем удаление должно выполниться для известного clientId");
                        });

            var partnerRepoMock = new Mock<IPartnerRepository>();
            partnerRepoMock.Setup(x => x.GetById(partnerId)).Returns(partner);

            var ordersDataSourceMock = new Mock<IOrdersDataSource>();
            ordersDataSourceMock.Setup(x => x.GetOrder(orderId, testClentId)).Returns(order);
            ordersDataSourceMock.Setup(
                x => x.UpdateExternalOrdersStatuses(It.IsAny<ExternalOrdersStatus[]>(), It.IsAny<string>()))
                                .Returns(new[] { new ChangeExternalOrderStatusResult { ResultCode = ResultCodes.SUCCESS, Success = true } });

            IOrderManagementService managementService =
                new OrderManagementService(
                    basketService: basketServiceMock.Object, partnerRepository: partnerRepoMock.Object, ordersDataSource: ordersDataSourceMock.Object);

            managementService.ClientCommitOrder(testClentId, orderId);

            basketServiceMock.Verify(x => x.Remove(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ShouldNotCommitOrder()
        {
            var service = new OrderManagementService();
            var result = service.ClientCommitOrder(string.Empty, 11111);

            Assert.IsFalse(result.Success);

            var result2 = service.ClientCommitOrder("testUser1", 1);

            Assert.IsFalse(result2.Success);
        }

        [TestMethod]
        public void ShouldReturnZeroWhenOrdersNotFound()
        {
            var param = new GetOrdersHistoryParameters(
                Guid.NewGuid().ToString(),
                SqlDateTime.MinValue.Value,
                SqlDateTime.MaxValue.Value,
                null,
                int.MinValue,
                int.MaxValue,
                true);

            var result = this.orderManagementService.GetOrdersHistory(param);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.TotalCount, 0);
        }

        [TestMethod]
        public void CanGetDeliveryAddressesTest()
        {
            var clientId = TestDataStore.TestClientId;

            var ds = new OrdersDataSource();
            ds.Insert(TestDataStore.GetOrder());

            var deliveryAddresses = this.orderManagementService.GetLastDeliveryAddresses(clientId, false, 10);

            Debug.WriteLine(deliveryAddresses.ResultCode);
            Debug.WriteLine(deliveryAddresses.ResultDescription);
            Assert.IsTrue(deliveryAddresses.Success);
            Assert.IsNotNull(deliveryAddresses);
            Assert.IsNotNull(deliveryAddresses.Addresses);
            Assert.IsTrue(deliveryAddresses.Addresses.Length > 0);

            var errorResult = this.orderManagementService.GetLastDeliveryAddresses(null, false, 10);

            Assert.AreEqual(ResultCodes.INVALID_PARAMETER_VALUE, errorResult.ResultCode);
            Assert.IsFalse(errorResult.Success);
        }

        [TestMethod]
        public void ShouldNotCancelOrderIfOrderCheckDeclined()
        {
            var kladr = "7700000000000";

            var delivery = TestDataStore.GetDeliveryDto(kladr, DeliveryVariantsProvider.DeliveryMatrixVariantId);

            // NOTE: "Мочим" получение партнера: партнер поддерживает проверку заказа, партнер не поддерживает получение вариантов доставки
            var partner = new Partner
            {
                Settings =
                    new Dictionary<string, string>
                    {
                        { PartnerSettingsExtension.CheckOrderUrlKey, "check_order_url_mock" },
                        { PartnerSettingsExtension.GetDeliveryVariantsKey, null }
                    }
            };

            // NOTE: "Мочим" взаимодействие с партнером: метод CheckOrder должен "проваливать" проверку заказа
            var partnerConnProvMock = new Mock<IPartnerConnectorProvider>();
            partnerConnProvMock.Setup(x => x.CheckOrder(It.IsAny<Order>())).Returns(
                new CheckOrderResult
                {
                    Success = true,
                    Checked = 0
                });

            var partnerRepoMock = MockFactory.GetPartnersRepository(partner);

            var basketService = MockFactory.GetBasketService(55).Object;

            var service = GetService(basketService, partnerConnProvMock, partnerRepoMock.Object);

            var basketItemId = Guid.NewGuid();

            var clientId = Guid.NewGuid().ToString();

            // NOTE: Создаем заказ
            var result = CreateOrder(service, delivery, basketItemId, clientId);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(ResultCodes.PROVIDER_CHECK_ORDER_DECLINED, result.ResultCode);
        }

        [TestMethod]
        public void ShouldCreateOrderForDirectPartner()
        {
            string kladr = TestDataStore.KladrCode;

            var delivery = TestDataStore.GetDeliveryDto(kladr, DeliveryVariantsProvider.DeliveryMatrixVariantId);

            // NOTE: "Мочим" получение партнера: партнер не поддерживает получение вариантов доставки и проверку заказа
            var partner = new Partner
                              {
                                  Settings =
                                      new Dictionary<string, string>
                                          {
                                              { PartnerSettingsExtension.CheckOrderUrlKey, null },
                                              { PartnerSettingsExtension.GetDeliveryVariantsKey, null }
                                          }
                              };

            // NOTE: "Мочим" взаимодействие с партнером: метод CheckOrder не должен вызываться
            var partnerConnProvMock = MockFactory.GetPartnerConnectorProvider();
            //partnerConnProvMock.Setup(x => x.CheckOrder(It.IsAny<Order>())).Returns(
            //    new CheckOrderResult
            //    {
            //        Success = true
            //    });

            var partnerRepoMock = MockFactory.GetPartnersRepository(partner);

            var basketService = MockFactory.GetBasketService(55).Object;

            var service = GetService(basketService, partnerConnProvMock, partnerRepoMock.Object);

            var basketItemId = Guid.NewGuid();
            
            var clientId = Guid.NewGuid().ToString();
            
            // NOTE: Создаем заказ
            var result = CreateOrder(service, delivery, basketItemId, clientId);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success, result.ResultDescription);

            partnerConnProvMock.Verify(
                x => x.CheckOrder(It.IsAny<Order>()),
                Times.Never(),
                "Так как партнер не поддерживает проверку заказа, метод не должен вызываться");
        }

        [TestMethod]
        public void ShouldNotCreateOrderWithNotDeliveredProduct()
        {
            var clientId = Guid.NewGuid().ToString();

            var deliveryInfo = TestDataStore.GetDeliveryDto();

            var service = GetService(MockFactory.GetBasketService(123, ProductAvailabilityStatuses.DeliveryRateNotFound).Object);            

            var result = CreateOrder(service, deliveryInfo, Guid.NewGuid(), clientId);

            Assert.IsNotNull(result);
            Assert.AreEqual(ResultCodes.PRODUCT_NOT_DELIVERED, result.ResultCode, result.ResultDescription);
        }

        [TestMethod]
        public void PhoneIsWellFormed()
        {
            var phone = TestHelper.BuildDeliveryInfo("2500000100300").Contact.Phone; // г. Владивосток, Русский Остров - партнер 111 не имеет туда доставки.

            phone.CountryCode = "+7";

            Assert.IsTrue(phone.IsWellFormed());

            phone.CountryCode = "-723";

            Assert.IsTrue(phone.IsWellFormed());

            phone.CountryCode = "723";

            Assert.IsTrue(phone.IsWellFormed());

            phone.CountryCode = "(723)";

            Assert.IsFalse(phone.IsWellFormed());
        }

        [TestMethod]
        public void ShouldNotCreateOrderWithInvalidProductStatus()
        {
            var clientId = TestDataStore.TestClientId;
            var deliveryInfo = TestDataStore.GetDeliveryDto();

            var service = GetService(MockFactory.GetBasketService(123, ProductAvailabilityStatuses.ProductIsNotActive).Object);

            var result = CreateOrder(service, deliveryInfo, Guid.NewGuid(), clientId);

            Assert.IsNotNull(result);
            Assert.AreEqual(ResultCodes.PRODUCT_INVALID_STATUS, result.ResultCode, result.ResultDescription);
        }

        [TestMethod]
        public void ShouldNotFoundGetOrderById()
        {
            var clientId = TestDataStore.TestClientId;
            var result = new OrderManagementService().GetOrderById(999999, clientId);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(ResultCodes.NOT_FOUND, result.ResultCode);
        }
        
        [TestMethod]
        public void ShouldGetOrderByExternalId()
        {
            using (var ctx = new LoyaltyDBEntities(DataSourceConfig.ConnectionString))
            {
                var order = ctx.Orders.FirstOrDefault(o => !string.IsNullOrEmpty(o.ExternalOrderId));

                if (order == null)
                {
                    return;
                }

                var result =
                    orderManagementService.GetOrderByExternalId(
                        new GetOrderByExternalIdParameters() { ExternalOrderId = order.ExternalOrderId });

                Assert.IsTrue(result.Success);
                Assert.AreEqual(order.Id, result.Order.Id);
                Assert.AreEqual(order.ExternalOrderId, result.Order.ExternalOrderId);
                Assert.IsNotNull(result.NextOrderStatuses);
            }
        }

        [TestMethod]
        public void ShouldNotCommitOrderWithNotUniqueExternalOrderId()
        {
            var orderId = -55;
            var otherOrderId = orderId - 1;
            var extOrderId = "ABCD";

            var mock1 = new Mock<IOrdersRepository>();
            mock1.Setup(x => x.Exists(orderId, extOrderId)).Returns(true);
            mock1.Setup(x => x.Get(partnerId, extOrderId)).Returns(
                new Order
                {
                    Id = otherOrderId,
                    ExternalOrderId = extOrderId
                });

            var emailSenderMock = new Mock<ILogEmailSender>();

            var service = new CatalogAdminService(ordersRepository: mock1.Object, logEmailSender: emailSenderMock.Object);

            var partnerOrderCommitment = new PartnerOrderCommitment
                                         {
                                             OrderId = orderId,
                                             ExternalOrderId = "ABCD",
                                             IsConfirmed = true,
                                             Reason = null,
                                             ReasonCode = null,
                                         };
            var commitments = new[]
                              {
                                  partnerOrderCommitment
                              };

            var result = service.PartnerCommitOrder(TestDataStore.TestUserId, partnerId, commitments);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(ResultCodes.NOT_UNIQUE_EXTERNAL_ORDER_ID, result.ResultCode);
            Assert.IsNotNull(result.ResultDescription);
        }

        [TestMethod]
        public void ShouldNotCommitOrderWithNotUniqueExternalOrderIdInBatch()
        {
            var orderId = -55;
            var otherOrderId = -56;
            var extOrderId = "ABCD";

            var mock1 = new Mock<IOrdersRepository>();
            mock1.Setup(x => x.Exists(It.IsAny<int?>(), extOrderId)).Returns(true);
            mock1.Setup(x => x.Get(partnerId, extOrderId)).Returns((Order)null);

            var emailSenderMock = new Mock<ILogEmailSender>();

            var service = new CatalogAdminService(ordersRepository: mock1.Object, logEmailSender: emailSenderMock.Object);

            var partnerOrderCommitment1 = new PartnerOrderCommitment
                                         {
                                             OrderId = orderId,
                                             ExternalOrderId = "ABCD",
                                             IsConfirmed = true,
                                             Reason = null,
                                             ReasonCode = null,
                                         };
            var partnerOrderCommitment2 = new PartnerOrderCommitment
                                          {
                                              OrderId = otherOrderId,
                                              ExternalOrderId = "ABCD",
                                              IsConfirmed = true,
                                              Reason = null,
                                              ReasonCode = null,
                                          };
            var commitments = new[]
                              {
                                  partnerOrderCommitment1, partnerOrderCommitment2
                              };

            var result = service.PartnerCommitOrder(TestDataStore.TestUserId, partnerId, commitments);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(ResultCodes.NOT_UNIQUE_EXTERNAL_ORDER_ID, result.ResultCode);
            Assert.IsNotNull(result.ResultDescription);
        }

        [TestMethod]
        public void ShouldReturnHasNonterminatedOrders()
        {
            var clientId = TestDataStore.TestClientId;
            var service = new OrderManagementService();

            var result1 = service.HasNonterminatedOrders(clientId);

            Assert.IsNotNull(result1);
            Assert.AreEqual(true, result1.Success);
        }

        [TestMethod]
        public void ShouldChangeOrdersStatusesBeforePayment()
        {
            var service = new OrderManagementService();

            var result = service.ChangeOrdersStatusesBeforePayment();

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Success);
        }

        /// <summary>
        /// Если оффлайн провайдер отклонил заказ то должен возвращатся соотв. код PROVIDER_CONFIRM_ORDER_DECLINED
        /// </summary>
        [TestMethod]
        public void CanReturnNotSuccessWhenProviderDeclinePrecommitTest()
        {
            var service = new OrderManagementService(
                MockFactory.OrdersDataSource(TestDataStore.GetOrder()).Object,
                MockFactory.GetBasketService().Object,
                MockFactory.GetPartnerConnectorProvider(1, ConfirmedStatuses.Rejected).Object,
                MockFactory.GetOrdersRepository(TestDataStore.GetOrder()).Object,
                MockFactory.GetPartnersRepository(TestDataStore.GetOZONPartner()).Object);

            var result = service.ClientCommitOrder(
                TestDataStore.TestClientId, 
                1);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success, "Операция должна быть не успешна. Так как провайдер отклонил заказ");
            Assert.AreEqual(ResultCodes.PROVIDER_CONFIRM_ORDER_DECLINED, result.ResultCode);
        }

        [TestMethod]
        public void ChangeOrdersStatusDescription()
        {
            Order order = RandomHelper.RandomOrder();
            order.Status = OrderStatuses.Draft;
            order.OrderStatusDescription = "description";

            const string description = "changed description";

            var orderId = new OrdersDataSource().Insert(order);

            var repo = new OrdersRepository();

            var resOrder = repo.GetById(orderId);

            Assert.AreEqual(order.OrderStatusDescription, resOrder.OrderStatusDescription);

            resOrder.OrderStatusDescription = description;

           var result = new OrderManagementService().ChangeOrderStatusDescription(resOrder.Id, description);

           Assert.IsTrue(result.Success);

           resOrder = repo.GetById(orderId);

            Assert.IsNotNull(resOrder);
            Assert.AreEqual(orderId, resOrder.Id);
            Assert.AreEqual(description, resOrder.OrderStatusDescription);
        }

        [TestMethod]
        public void ShouldReturnSetOfOrders()
        {
            var order = RandomHelper.RandomOrder();
            var orderId = new OrdersDataSource().Insert(order);
            var repo = new OrdersRepository();

            var ids = new[] { RandomHelper.RandomNumber(0, 500), orderId, RandomHelper.RandomNumber(0, 500) };

            var orders = repo.GetOrders(ids);

            Assert.IsTrue(orders.Any(x => x.Id == orderId));

            order = repo.GetById(orderId);
            new OrdersRepository().Delete(order);
        }

        [TestMethod]
        public void ShouldGetDeliveryVariantsTest()
        {
            var clientGuid = Guid.NewGuid().ToString();

            var basket = GetBasketService();

            var basketManageResult = basket.Add(clientGuid, TestDataStore.ProductID, 1, clientContext);

            TestHelper.AssertResult(basketManageResult);

            var basketItemId = basketManageResult.BasketItemId;

            var service = GetService(basket);

            var result = service.GetDeliveryVariants(new GetDeliveryVariantsParameters
            {
                ClientId = TestDataStore.TestClientId,
                ClientContext = clientContext,
                BasketItems = new[] { basketItemId },
                Location = new API.InputParameters.Location
                {
                    KladrCode = TestDataStore.KladrCode,
                    PostCode = "123456"
                }
            });

            Assert.IsTrue(result.Success, result.ResultDescription);
            Assert.IsNotNull(result.Location);
        }

        [TestMethod]
        public void ShouldNotGetDeliveryVariantsTest()
        {
            var clientGuid = Guid.NewGuid().ToString();

            var partnerId1 = 111;
            var partner1 = new Partner
            {
                Id = TestDataStore.OzonPartnerId,
                Settings =
                    new Dictionary<string, string>
                    {
                        { PartnerSettingsExtension.GetDeliveryVariantsKey, null }
                    }
            };

            var product1 = TestDataStore.GetProduct();
            product1.PartnerId = partnerId1;
            product1.Weight = 99999999;

            var partnerRepo = MockFactory.GetPartnersRepository(partner1).Object;

            var basket = new BasketService(new BasketItemRepository(),
                                           MockFactory.GetProductsSearcher(new[] { product1 }).Object,
                                           MockFactory.GetPartnerConnectorProvider().Object,
                                           MockFactory.GetMechanicsProvider().Object,
                                           partnerRepo);

            var basketManageResult = basket.Add(clientGuid, product1.ProductId, 1, clientContext);

            TestHelper.AssertResult(basketManageResult);

            var basketItemId = basketManageResult.BasketItemId;

            var service = GetService(basketService: basket, partnerRepo: partnerRepo);

            var result = service.GetDeliveryVariants(new GetDeliveryVariantsParameters
            {
                ClientId = TestDataStore.TestClientId,
                ClientContext = clientContext,
                BasketItems = new[] { basketItemId },
                Location = new API.InputParameters.Location
                {
                    KladrCode = TestDataStore.KladrCode,
                    PostCode = "123456"
                }
            });

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.DeliveryGroups.Length == 0);

        }

        [TestMethod]
        public void ShouldNotCreateOrderFromDifferentClients()
        {
            var clientGuid1 = Guid.NewGuid().ToString();
            var clientGuid2 = Guid.NewGuid().ToString();

            var partnerId1 = 111;
            var partner1 = new Partner
            {
                Id = partnerId1,
                Type = PartnerType.Direct,
                Settings = new Dictionary<string, string>() { {PartnerSettingsExtension.MultiPositionOrdersKey, "true"} }
            };

            var product1 = TestDataStore.GetProduct();
            product1.PartnerId = partnerId1;
            var product2 = TestDataStore.GetOtherProduct();
            product2.PartnerId = partnerId1;

            var partnerRepo = MockFactory.GetPartnersRepository(partner1).Object;

            var basket = new BasketService(new BasketItemRepository(),
                                           MockFactory.GetProductsSearcher(new[] {product1, product2}).Object,
                                           MockFactory.GetPartnerConnectorProvider().Object,
                                           MockFactory.GetMechanicsProvider().Object,
                                           partnerRepo);

            var basketManageResult1 = basket.Add(clientGuid1, TestDataStore.ProductID, 1, clientContext);
            var basketManageResult2 = basket.Add(clientGuid2, TestDataStore.GetOtherProduct().ProductId, 1, clientContext);

            var service = GetService(basketService: basket, partnerRepo: partnerRepo);

            var deliveryDto = TestDataStore.GetDeliveryDto();

            var createOrderResult = CreateOrder(service, deliveryDto, new[] { basketManageResult1.BasketItemId, basketManageResult2.BasketItemId }, clientGuid1);

            Assert.IsNotNull(createOrderResult);
            Assert.IsFalse(createOrderResult.Success);
            Assert.IsNull(createOrderResult.Order);
        }

        [TestMethod]
        public void ShouldCalculateDeliveryPriceInMultiPositionOrder()
        {
            var clientGuid1 = Guid.NewGuid().ToString();

            var partnerId1 = 111;
            var partner1 = new Partner
            {
                Id = partnerId1,
                Type = PartnerType.Direct,
                Settings = new Dictionary<string, string>()
                    {
                        {PartnerSettingsExtension.GetDeliveryVariantsKey, "true"},
                        { PartnerSettingsExtension.MultiPositionOrdersKey, "true" }
                    }
            };

            var product1 = TestDataStore.GetProduct();
            product1.PartnerId = partnerId1;
            product1.Weight = 1000;
            var product2 = TestDataStore.GetOtherProduct();
            product2.PartnerId = partnerId1;
            product2.Weight = 10000;

            var partnerRepo = MockFactory.GetPartnersRepository(partner1).Object;

            var mechMock = MockFactory.GetMechanicsProvider();

            var basket = new BasketService(new BasketItemRepository(),
                                           MockFactory.GetProductsSearcher(new[] { product1, product2 }).Object,
                                           MockFactory.GetPartnerConnectorProvider().Object,
                                           mechMock.Object,
                                           partnerRepo);

            var basketManageResult1 = basket.Add(clientGuid1, TestDataStore.ProductID, 1, clientContext);
            var basketManageResult2 = basket.Add(clientGuid1, TestDataStore.GetOtherProduct().ProductId, 1, clientContext);

            var service = GetService(basketService: basket, partnerRepo: partnerRepo, mechMock:mechMock.Object);

            var deliveryDto = TestDataStore.GetDeliveryDto();

            var createOrderResult = CreateOrder(service, deliveryDto, new[] { basketManageResult1.BasketItemId, basketManageResult2.BasketItemId }, clientGuid1);

            Assert.IsNotNull(createOrderResult);
            Assert.IsTrue(createOrderResult.Success);
            Assert.AreEqual(createOrderResult.Order.DeliveryCost, 500);
        }

        [TestMethod]
        public void ShouldNotCreateOrderFromDifferentPartners()
        {
            var clientGuid1 = Guid.NewGuid().ToString();

            var partnerId1 = 111;
            var partner1 = new Partner
            {
                Id = partnerId1,
                Type = PartnerType.Direct,
                Settings = new Dictionary<string, string>() { { PartnerSettingsExtension.MultiPositionOrdersKey, "true" } }
            };

            var partnerId2 = 555;
            var partner2 = new Partner
            {
                Id = partnerId2,
                Type = PartnerType.Direct
            };

            var product1 = TestDataStore.GetProduct();
            product1.PartnerId = partnerId1;
            var product2 = TestDataStore.GetOtherProduct();
            product2.PartnerId = partnerId2;

            var partnerRepo = MockFactory.GetPartnersRepository(partner1, partner2).Object;

            var basket = new BasketService(new BasketItemRepository(),
                                           MockFactory.GetProductsSearcher(new[] { product1, product2 }).Object,
                                           MockFactory.GetPartnerConnectorProvider().Object,
                                           MockFactory.GetMechanicsProvider().Object,
                                           partnerRepo);

            var basketManageResult1 = basket.Add(clientGuid1, TestDataStore.ProductID, 1, clientContext);
            var basketManageResult2 = basket.Add(clientGuid1, TestDataStore.GetOtherProduct().ProductId, 1, clientContext);

            var service = GetService(basketService: basket, partnerRepo: partnerRepo);

            var deliveryDto = TestDataStore.GetDeliveryDto();

            var createOrderResult = CreateOrder(service, deliveryDto, new[] { basketManageResult1.BasketItemId, basketManageResult2.BasketItemId }, clientGuid1);

            Assert.IsNotNull(createOrderResult);
            Assert.IsFalse(createOrderResult.Success);
            Assert.IsNull(createOrderResult.Order);
        }

        private static BasketService GetBasketService()
        {
            var mechMock = MockFactory.GetMechanicsProvider();

            var basketService = new BasketService(
                new BasketItemRepository(),
                new ProductsSearcher(new ProductsDataSource(), new ProductAttributeRepository(), mechMock.Object),
                MockFactory.GetPartnerConnectorProvider(1, ConfirmedStatuses.Committed).Object, 
                mechMock.Object,
                new PartnerRepository());
            return basketService;
        }

        private static OrderManagementService GetService(IBasketService basketService = null, Mock<IPartnerConnectorProvider> partnerConnectorProvider = null, IPartnerRepository partnerRepo = null, IMechanicsProvider mechMock = null)
        {
            basketService = basketService ?? MockFactory.GetBasketService().Object;
            var mechanicsProvider = MockFactory.GetMechanicsProvider();

            partnerConnectorProvider = partnerConnectorProvider ?? MockFactory.GetPartnerConnectorProvider(1, ConfirmedStatuses.Committed);

            var connectorProvider = partnerConnectorProvider.Object;

            //connectorProvider = null;

            var service = new OrderManagementService(
                partnerRepository: partnerRepo ?? new PartnerRepository(),
                basketService: basketService,
                deliveryVariantsProvider: new DeliveryVariantsProvider(connectorProvider, partnerRepo),
                geoPointProvider: MockFactory.GetGeoPointProvider().Object,
                partnerConnectorProvider: connectorProvider,
                mechanicsProvider: mechMock ?? mechanicsProvider.Object);
            return service;
        }

        private CreateOrderResult CreateOrder(IOrderManagementService service, DeliveryDto deliveryInfo, Guid basketItemId, string clientId)
        {
            return CreateOrder(service, deliveryInfo, new[] {basketItemId}, clientId);
        }

        private CreateOrderResult CreateOrder(IOrderManagementService service, DeliveryDto deliveryInfo, Guid[] basketItemIds, string clientId)
        {
            var createOrderResult = service.CreateOrderFromBasketItems(new CreateOrderFromBasketItemsParameters()
            {
                BasketItems = basketItemIds,
                ClientContext = clientContext,
                ClientId = clientId,
                Delivery = deliveryInfo,
                TotalAdvance = 0
            });

            return createOrderResult;
        }
    }
}