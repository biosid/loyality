namespace RapidSoft.Loaylty.PartnersConnector.Tests.Service
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;

    using AutoMapper;

    using Common.DTO;
    using Common.Services;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using ProductCatalog.WsClients.OrderManagementService;

    using RapidSoft.Extensions;

    using RapidSoft.Loaylty.PartnersConnector.Interfaces;
    using RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities;
    using RapidSoft.Loaylty.PartnersConnector.Interfaces.Settings;
    using RapidSoft.Loaylty.PartnersConnector.Queue.Entities;
    using RapidSoft.Loaylty.PartnersConnector.Queue.Repository;
    using RapidSoft.Loaylty.PartnersConnector.Services;
    using RapidSoft.Loyalty.Security;

    using CheckOrderResult = Common.DTO.CheckOrder.CheckOrderResult;
    using CommitOrderResult = Common.DTO.CommitOrder.CommitOrderResult;
    using ConfirmedStatuses = Interfaces.Entities.ConfirmedStatuses;
    using Contact = RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities.Contact;
    using DeliveryInfo = Interfaces.Entities.DeliveryInfo;
    using Location = Interfaces.Entities.Location;
    using MockFactory = Tests.MockFactory;
    using Order = Interfaces.Entities.Order;
    using OrderItem = Interfaces.Entities.OrderItem;
    using OrderStatuses = RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities.OrderStatuses;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
        Justification = "Для тестов можно отключить.")]
    [TestClass]
    public class OrderManagementServiceTests
    {
        public OrderManagementServiceTests()
        {
            // Необходимо для того чтобы инициализировать маппинг в статическом конструкторе OrderManagementService
            new OrderManagementService();
        }

        #region Tests

        #region Mapping

        [TestMethod]
        public void ShouldMapContactToDTOContact()
        {
            var contact = new Contact
            {
                Email = "Email@Email.com",
                FirstName = "FirstName",
                LastName = "LastName",
                MiddleName = "MiddleName",
                Phone = TestHelper.GetTestPhoneNumber()
            };

            var dtoContact = Mapper.Map<Contact, Common.DTO.CommitOrder.Contact>(contact);

            Assert.IsNotNull(dtoContact);
            Assert.AreEqual(dtoContact.Email, "Email@Email.com");
            Assert.AreEqual(dtoContact.FirstName, "FirstName");
            Assert.AreEqual(dtoContact.LastName, "LastName");
            Assert.AreEqual(dtoContact.MiddleName, "MiddleName");
            Assert.AreEqual(dtoContact.PhoneNumber, 999881234567890);
        }

        [TestMethod]
        public void ShouldMapDeliveryInfoToDTODeliveryInfo()
        {
            var now = DateTime.Now;

            var deliveryInfo = TestHelper.GetDeliveryInfo(now);
            var dtoDeliveryInfo = Mapper.Map<DeliveryInfo, Common.DTO.CommitOrder.DeliveryInfo>(deliveryInfo);

            Assert.IsNotNull(dtoDeliveryInfo);
            Assert.AreEqual(TestHelper.AddressText, dtoDeliveryInfo.Address);
            Assert.AreEqual("Comment", dtoDeliveryInfo.Comment);
            Assert.AreEqual(OrderManagementService.RusCountryCode, dtoDeliveryInfo.CountryCode);
            Assert.AreEqual(OrderManagementService.RusCountryName, dtoDeliveryInfo.CountryName);
            Assert.AreEqual("197755", dtoDeliveryInfo.PostCode);
            Assert.AreEqual(999881234567890, dtoDeliveryInfo.Contacts.Single().PhoneNumber);
            Assert.AreEqual("ExternalDeliveryVariantId", dtoDeliveryInfo.ExternalDeliveryVariantId);
            Assert.AreEqual("ExternalPickupPointId", dtoDeliveryInfo.ExternalPickupPointId);
            Assert.AreEqual("ExternalLocationId", dtoDeliveryInfo.ExternalLocationId);
        }

        [TestMethod]
        public void ShouldMapOrderItemToDTOOrderItem()
        {
            var orderItem = new OrderItem
            {
                Amount = 1,
                BasketItemId = "2",
                OfferId = "OfferId",
                OfferName = "OfferName",
                Price = 5.5m,
                Weight = 111
            };
            var dtoOrderItem = Mapper.Map<OrderItem, Common.DTO.CommitOrder.OrderItem>(orderItem);
            Assert.IsNotNull(dtoOrderItem);
            Assert.AreEqual(dtoOrderItem.Amount, 1);
            Assert.AreEqual(dtoOrderItem.OfferId, "OfferId");
            Assert.AreEqual(dtoOrderItem.OfferName, "OfferName");
            Assert.AreEqual(dtoOrderItem.Price, 5.5m);
            Assert.AreEqual(dtoOrderItem.Weight, 111);
        }

        [TestMethod]
        public void ShouldMapOrderToDTOOrder()
        {
            var partnerId = TestHelper.FirstTestPartnerId;

            var order = TestHelper.GetOrder(partnerId);

            var dtoOrder = Mapper.Map<Order, Common.DTO.CommitOrder.Order>(order);
            Assert.IsNotNull(dtoOrder);
            Assert.AreEqual(300, dtoOrder.DeliveryCost);
            Assert.AreEqual("BBC: Акулы (4 DVD)", dtoOrder.Items.First().OfferName);
            Assert.AreEqual(709.123456m, dtoOrder.ItemsCost);
            Assert.AreEqual("-555", dtoOrder.OrderId);
            Assert.AreEqual(1009.123456m, dtoOrder.TotalCost);
            Assert.AreEqual(111, dtoOrder.TotalWeight);
            Assert.AreEqual("г. Москва, тестовый адрес", dtoOrder.DeliveryInfo.Address);
            Assert.AreEqual(999881234567890, dtoOrder.DeliveryInfo.Contacts.Single().PhoneNumber);
        }

        #endregion

        [TestMethod]
        public void FixPriceShouldFixOkTest()
        {
            var sendResult = new FixPriceResult()
            {
                ActualPrice = 123,
                Confirmed = 1
            };

            var serializedResult = sendResult.Serialize(Encoding.UTF8);

            var orderItem = TestHelper.GetOrderItem();

            var param = new FixBasketItemPriceParam()
            {
                BasketItemId = Guid.NewGuid().ToString(),
                OfferId = orderItem.OfferId,
                OfferName = orderItem.OfferName,
                ClientId = "123",
                PartnerId = 1,
                Price = orderItem.Price,
                Amount = 1
            };

            var result = GetService(serializedResult).FixBasketItemPrice(param);

            Assert.IsTrue(result.Success, result.ResultDescription);
        }

        [TestMethod]
        public void CheckOrder_ShouldCheck()
        {
            var partnerId = TestHelper.FirstTestPartnerId;

            var order = TestHelper.GetOrder(partnerId);

            var commitOrderResult = new CheckOrderResult
            {
                Checked = 1
            };

            var service = GetService(commitOrderResult.Serialize(Encoding.UTF8));

            var result = service.CheckOrder(order);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(1, result.Checked, string.Format("ResultDescription:{0} Reason:{1}", result.ResultDescription, result.Reason));
        }

        [TestMethod]
        public void CheckOrder_ShouldNotCheck()
        {
            var partnerId = TestHelper.FirstTestPartnerId;

            var order = TestHelper.GetOrder(partnerId);

            var commitOrderResult = new CheckOrderResult { Checked = 0, Reason = "Не хочу!" };
            var service = GetService(commitOrderResult.Serialize(Encoding.UTF8));
            
            var result = service.CheckOrder(order);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.Checked, 0);
            Assert.AreEqual(result.Reason, "Не хочу!");
        }

        [TestMethod]
        public void CheckOrder_ShouldProcessInvalidXml()
        {
            var partnerId = TestHelper.FirstTestPartnerId;

            var order = TestHelper.GetOrder(partnerId);

            var service = GetService(@"CheckOrderResult><Checked>0</Checked><Reason>Не хочу!</Reason></CheckOrderResult>");

            var result = service.CheckOrder(order);

            Assert.AreEqual(result.Success, false);
            Assert.AreEqual(result.Checked, 0);
            Assert.IsNotNull(result.ResultDescription);
        }

        [TestMethod]
        public void CommitOrder_ShouldCommit()
        {
            var partnerId = TestHelper.FirstTestPartnerId; // <== Из настроек, НЕ поддерживает пакетное.
            var order = TestHelper.GetOrder(partnerId);

            var commitOrderResult = new CommitOrderResult
            {
                Confirmed = 1,
                OrderId = order.Id.ToString(CultureInfo.InvariantCulture),
                InternalOrderId = order.Id.ToString(CultureInfo.InvariantCulture),
            };

            var serializedCommitOrderResult = commitOrderResult.Serialize(Encoding.UTF8);

            var result = GetService(serializedCommitOrderResult).CommitOrder(order);

            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(result.Confirmed, ConfirmedStatuses.Committed);
        }

        [TestMethod]
        public void CommitOrder_ShouldNotConfirm()
        {
            var partnerId = TestHelper.FirstTestPartnerId; // <== Из настроек, НЕ поддерживает пакетное.
            var order = TestHelper.GetOrder(partnerId);

            var commitOrderResult = new CommitOrderResult
            {
                Confirmed = 0,
                OrderId = order.Id.ToString(CultureInfo.InvariantCulture),
                InternalOrderId = order.Id.ToString(CultureInfo.InvariantCulture),
            };
            var serializedCommitOrderResult = commitOrderResult.Serialize(Encoding.UTF8);

            var result = GetService(serializedCommitOrderResult).CommitOrder(order);

            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(result.Confirmed, ConfirmedStatuses.Rejected);
        }

        [TestMethod]
        public void CommitOrder_ShouldAddOrderToQueue()
        {
            var partnerId = TestHelper.SecondTestPartnerId; // <== Из настроек, поддерживает пакетное.
            var order = TestHelper.GetOrder(partnerId);

            var queueMock = new Mock<ICommitOrderQueueItemRepository>();
            var queueRepo = queueMock.Object;

            var providerMock = new Mock<ICatalogAdminServiceProvider>();
            providerMock.Setup(x => x.GetPartnerSettings(partnerId, It.IsAny<string>()))
                        .Returns(new PartnerSettings(partnerId, TestHelper.GetSettings(true)));

            var service = new OrderManagementService(
                queueItemRepository: queueRepo, catalogAdminServiceProvider: providerMock.Object);

            var result = service.CommitOrder(order);

            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(result.Confirmed, ConfirmedStatuses.AddToQueue);
            queueMock.Verify(x => x.Save(It.IsAny<CommitOrderQueueItem>()), Times.Once());
        }

        [TestMethod]
        public void CommitOrder_ShouldProcessInvalidXml()
        {
            var partnerId = TestHelper.FirstTestPartnerId; // <== Из настроек, НЕ поддерживает пакетное.
            var order = TestHelper.GetOrder(partnerId);
            
            var result = GetService(@"<InternalOrderId>-555</InternalOrderId><Confirmed>0</Confirmed></CommitOrderResult>").CommitOrder(order);

            Assert.AreEqual(result.Success, false);
            Assert.AreEqual(result.Confirmed, ConfirmedStatuses.Rejected);
        }

        [TestMethod]
        public void UpdateOrdersStatuses_ShouldSendUpdateStatus()
        {
            var mock2 = new Mock<IProductCatalogProvider>();
            mock2.Setup(x => x.ChangeOrdersStatuses(It.IsAny<ExternalOrdersStatus[]>()))
                 .Returns(() => new ChangeExternalOrdersStatusesResult
                 {
                     ResultCode = 0,
                     Success = true
                 });
            var productCatalogProvider = mock2.Object;

            var mock3 = new Mock<ITextMessageDispatcher>();
            var messageDispatcher = mock3.Object;

            var service = new OrderManagementService(productCatalogProvider, messageDispatcher);

            var mess = new NotifyOrderMessage
                           {
                               Code = OrderStatuses.CancelledByPartner,
                               PartnerOrderId = "PartnerOrderId",
                               StatusDateTime = DateTime.Now,
                               StatusReason = "StatusReason",
                               PartnerCode = "PartnerCode"
                           };

            var result = service.UpdateOrdersStatuses(new[] { mess });

            Assert.AreEqual(result.Success, true);

            mock2.Verify(x => x.ChangeOrdersStatuses(It.IsAny<ExternalOrdersStatus[]>()), Times.Once());
        }

        [TestMethod]
        public void GetDeliveryVariants_ShouldGetDeliveryVariants()
        {
            var partnerId = TestHelper.FirstTestPartnerId;

            var order = TestHelper.GetOrder(partnerId);

            var sendResult = TestHelper.GetDeliveryVariantsResult();
            
            var serializedResult = sendResult.Serialize(Encoding.UTF8);

            var variantsParam = new GetDeliveryVariantsParam()
            {
                ClientId = "123",
                PartnerId = partnerId,
                Items = order.Items,
                Location = new Location()
                {
                    PostCode = order.DeliveryInfo.Address.PostCode
                }
            };

            var result = GetService(serializedResult).GetDeliveryVariants(variantsParam);

            Assert.IsTrue(result.Success, result.ResultDescription);
            Assert.AreEqual(1, result.DeliveryGroups[0].DeliveryVariants[0].PickupPoints.Length);
        }

        [TestMethod]
        public void GetDeliveryVariants_ShouldGetEmptyDeliveryVariants()
        {
            var partnerId = TestHelper.FirstTestPartnerId;

            var order = TestHelper.GetOrder(partnerId);

            var sendResult = TestHelper.GetEmptyDeliveryVariantsResult();

            var serializedResult = sendResult.Serialize(Encoding.UTF8);

            var variantsParam = new GetDeliveryVariantsParam()
            {
                PartnerId = partnerId,
                Items = order.Items,
                Location = new Location()
                {
                    PostCode = "000000"
                }
            };

            var result = GetService(serializedResult).GetDeliveryVariants(variantsParam);

            Assert.IsTrue(result.Success, result.ResultDescription);
        }

        #endregion

        #region Methods

        private static OrderManagementService GetService(string result)
        {
            var messageDispatcher = new Mock<ITextMessageDispatcher>();
            messageDispatcher.Setup(m => m.Send(It.IsAny<Uri>(), It.IsAny<string>())).Returns(result);

            var queueMock = new Mock<ICommitOrderQueueItemRepository>();

            var productCatalogProvider = MockFactory.GetProductCatalogProvider();
            var catalogAdminServiceProvider = MockFactory.GetCatalogAdminServiceProvider();

            var dispatcher = messageDispatcher.Object;
            //dispatcher = null;

            var service = new OrderManagementService(
                productCatalogProvider.Object,
                dispatcher,
                catalogAdminServiceProvider: catalogAdminServiceProvider.Object,
                queueItemRepository: queueMock.Object);
            return service;
        }

        #endregion
    }
}