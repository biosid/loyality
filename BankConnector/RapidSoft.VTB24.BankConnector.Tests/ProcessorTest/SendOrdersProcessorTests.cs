using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using RapidSoft.Etl.Logging;
using RapidSoft.Loaylty.ProductCatalog.WsClients.CatalogAdminService;
using RapidSoft.Loaylty.ProductCatalog.WsClients.OrderManagementService;
using RapidSoft.VTB24.BankConnector.API;
using RapidSoft.VTB24.BankConnector.API.Entities;
using RapidSoft.VTB24.BankConnector.DataModels;
using RapidSoft.VTB24.BankConnector.DataSource;
using RapidSoft.VTB24.BankConnector.DataSource.Interface;
using RapidSoft.VTB24.BankConnector.Processors;
using ChangeOrdersStatusesResult = RapidSoft.Loaylty.ProductCatalog.WsClients.OrderManagementService.ChangeOrdersStatusesResult;
using Contact = RapidSoft.Loaylty.ProductCatalog.WsClients.OrderManagementService.Contact;
using DeliveryAddress = RapidSoft.Loaylty.ProductCatalog.WsClients.OrderManagementService.DeliveryAddress;
using DeliveryInfo = RapidSoft.Loaylty.ProductCatalog.WsClients.OrderManagementService.DeliveryInfo;
using DeliveryTypes = RapidSoft.Loaylty.ProductCatalog.WsClients.OrderManagementService.DeliveryTypes;
using Order = RapidSoft.Loaylty.ProductCatalog.WsClients.OrderManagementService.Order;
using OrderItem = RapidSoft.Loaylty.ProductCatalog.WsClients.OrderManagementService.OrderItem;
using PhoneNumber = RapidSoft.Loaylty.ProductCatalog.WsClients.OrderManagementService.PhoneNumber;
using Product = RapidSoft.Loaylty.ProductCatalog.WsClients.OrderManagementService.Product;

namespace RapidSoft.VTB24.BankConnector.Tests.ProcessorTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SendOrdersProcessorTests
    {
        private IUnitOfWork _uow;
        private IOrderForPaymentRepository _orderForPaymentRepository;
        private IOrderItemsForPaymentRepository _orderItemsForPaymentRepository;
        private IOrderPaymentResponseRepository _orderPaymentResponseRepository;

        private IOrderManagementService _orderManagementService;
        private ICatalogAdminService _catalogAdminService;
        private IPaymentService _paymentService;

        private EtlLogger.EtlLogger _etlLogger;
        private Guid _etlPackageId;
        private Guid _etlSessionId;

        private SendOrdersProcessor _processor;

        private readonly List<Order> _orders = new List<Order>();
        private readonly List<OrderForPayment> _ordersForPayment = new List<OrderForPayment>();
        private readonly List<OrderItemsForPayment> _orderItemsForPayment = new List<OrderItemsForPayment>();

        [TestInitialize]
        public void Init()
        {
            _uow = Substitute.For<IUnitOfWork>();
            _orderForPaymentRepository = Substitute.For<IOrderForPaymentRepository>();
            _orderItemsForPaymentRepository = Substitute.For<IOrderItemsForPaymentRepository>();
            _orderPaymentResponseRepository = Substitute.For<IOrderPaymentResponseRepository>();

            _uow.OrderForPaymentRepository.Returns(_orderForPaymentRepository);
            _uow.OrderItemsForPaymentRepository.Returns(_orderItemsForPaymentRepository);
            _uow.OrderPaymentResponseRepository.Returns(_orderPaymentResponseRepository);

            _orderManagementService = Substitute.For<IOrderManagementService>();
            _catalogAdminService = Substitute.For<ICatalogAdminService>();
            _paymentService = Substitute.For<IPaymentService>();

            _etlPackageId = Guid.NewGuid();
            _etlSessionId = Guid.NewGuid();

            _etlLogger = new EtlLogger.EtlLogger(Substitute.For<IEtlLogger>(), "stub", _etlSessionId.ToString());

            _processor = new SendOrdersProcessor(
                _etlLogger,
                _uow,
                _orderManagementService,
                _catalogAdminService,
                _paymentService);

            var partners = new[]
            {
                new Partner
                {
                    Id = 1,
                    Type = PartnerType.Offline,
                    Settings = new Dictionary<string, string>
                    {
                        { SendOrdersProcessor.UnitellerShopIdPartnerKey, "shop_1" }
                    }
                }
            };

            _orderManagementService.ChangeOrdersStatusesBeforePayment()
                                   .Returns(new ChangeOrdersStatusesResult { Success = true });

            _orders.Clear();
            _orderManagementService.GetOrdersForPayment(Arg.Any<GetOrdersForPaymentParameters>())
                       .Returns(info => new GetOrdersHistoryResult { Success = true, Orders = _orders.ToArray() },
                                info => new GetOrdersHistoryResult { Success = true, Orders = new Order[0] });

            _catalogAdminService.GetPartners(Arg.Is<int[]>(x => x != null && x.OrderBy(y => y).SequenceEqual(new[] { 1 })), Arg.Any<string>())
                                .Returns(new GetPartnerResult { Success = true, Partners = partners });

            _paymentService.GetPaymentByOrderId(2)
                           .Returns(new GenericBankConnectorResponse<PaymentInfo>(new PaymentInfo { OrderId = 2, UnitellerBillNumber = "bill_1", UnitellerShopId = "ashop_1" }));

            _orderForPaymentRepository.GetAll()
                                      .Returns(new[]
                                      {
                                          new OrderForPayment { OrderId = 3, InsertedDate = new DateTime(2014, 9, 1) },
                                          new OrderForPayment { OrderId = 6, InsertedDate = new DateTime(2014, 9, 1) },
                                          new OrderForPayment { OrderId = 6, InsertedDate = new DateTime(2014, 9, 5) }
                                      }.AsQueryable());

            _orderPaymentResponseRepository.GetAll()
                                           .Returns(new[]
                                           {
                                               new OrderPaymentResponse { OrderId = 3, InsertedDate = new DateTime(2014, 9, 3) },
                                               new OrderPaymentResponse { OrderId = 6, InsertedDate = new DateTime(2014, 9, 3) }
                                           }.AsQueryable());

            _ordersForPayment.Clear();
            _orderForPaymentRepository.When(x => x.Add(Arg.Any<OrderForPayment>()))
                                      .Do(info => _ordersForPayment.Add(info.Arg<OrderForPayment>()));

            _orderItemsForPayment.Clear();
            _orderItemsForPaymentRepository.When(x => x.Add(Arg.Any<OrderItemsForPayment>()))
                                           .Do(info => _orderItemsForPayment.Add(info.Arg<OrderItemsForPayment>()));
        }

        [TestMethod]
        public void ShouldSaveOrderForPayment()
        {
            _orders.Add(new Order
            {
                Id = 1,
                PartnerId = 1,
                ClientId = "client_1",
                Items = new[]
                {
                    new OrderItem
                    {
                        Product = new Product { ProductId = "product_1", Name = "Product1" },
                        Amount = 1, AmountPriceRur = 200, AmountPriceBonus = 600, PriceRur = 200, PriceBonus = 600
                    }
                },
                DeliveryInfo = new DeliveryInfo
                {
                    DeliveryVariantName = "Delivery",
                    DeliveryType = DeliveryTypes.Delivery,
                    Address = new DeliveryAddress { RegionTitle = "Region", CityTitle = "City", AddressText = "Region, City, 1-1" },
                    Contact = new Contact
                    {
                        FirstName = "Jhon",
                        LastName = "Smith",
                        Phone = new PhoneNumber { CountryCode = "7", CityCode = "495", LocalNumber = "1234567" }
                    }
                },
                BonusTotalCost = 1000,
                TotalCost = 333,
                DeliveryCost = 133,
                ItemsCost = 200,
                InsertedDate = DateTime.Now
            });

            _processor.Execute();

            _orderManagementService.Received(1).ChangeOrdersStatusesBeforePayment();
            _orderManagementService.Received(2).GetOrdersForPayment(Arg.Any<GetOrdersForPaymentParameters>());
            _paymentService.Received(0).GetPaymentByOrderId(Arg.Any<int>());
            _orderForPaymentRepository.Received(1).GetAll();
            _orderPaymentResponseRepository.Received(0).GetAll();

            Assert.AreEqual(1, _ordersForPayment.Count);
            Assert.AreEqual(2, _orderItemsForPayment.Count, "Количество товаров в заказе + доставка");

            Assert.AreEqual(_ordersForPayment[0].OrderId, _orders[0].Id);
            Assert.AreEqual(_orderItemsForPayment[0].OrderId, _orders[0].Id);
            Assert.AreEqual(_orderItemsForPayment[1].OrderId, _orders[0].Id);
        }

        [TestMethod]
        public void ShouldSaveOrderForPayment_OrderWithAdvance()
        {
            _orders.Add(new Order
            {
                Id = 2,
                PartnerId = 1,
                ClientId = "client_2",
                Items = new[]
                {
                    new OrderItem
                    {
                        Product = new Product { ProductId = "product_1", Name = "Product1" },
                        Amount = 2, AmountPriceRur = 400, AmountPriceBonus = 1200, PriceRur = 200, PriceBonus = 600
                    }
                },
                DeliveryInfo = new DeliveryInfo
                {
                    DeliveryVariantName = "Delivery",
                    DeliveryType = DeliveryTypes.Delivery,
                    Address = new DeliveryAddress { RegionTitle = "Region", CityTitle = "City", AddressText = "Region, City, 1-1" },
                    Contact = new Contact
                    {
                        FirstName = "Ivan",
                        LastName = "Ivanov",
                        Phone = new PhoneNumber { CountryCode = "7", CityCode = "495", LocalNumber = "7654321" }
                    }
                },
                BonusTotalCost = 1600,
                TotalCost = 533,
                DeliveryCost = 133,
                ItemsCost = 400,
                DeliveryAdvance = 133,
                InsertedDate = DateTime.Now
            });

            _processor.Execute();

            _orderManagementService.Received(1).ChangeOrdersStatusesBeforePayment();
            _orderManagementService.Received(2).GetOrdersForPayment(Arg.Any<GetOrdersForPaymentParameters>());
            _paymentService.Received(1).GetPaymentByOrderId(_orders[0].Id);
            _orderForPaymentRepository.Received(1).GetAll();
            _orderPaymentResponseRepository.Received(0).GetAll();

            Assert.AreEqual(1, _ordersForPayment.Count);
            Assert.AreEqual(3, _orderItemsForPayment.Count, "Количество товаров в заказе + доставка + итого");

            Assert.AreEqual(_ordersForPayment[0].OrderId, _orders[0].Id);
            Assert.AreEqual(_orderItemsForPayment[0].OrderId, _orders[0].Id);
            Assert.AreEqual(_orderItemsForPayment[1].OrderId, _orders[0].Id);
        }

        [TestMethod]
        public void ShouldSaveOrderForPayment_ResendCancelledOrder()
        {
            _orders.Add(new Order
            {
                Id = 3,
                PartnerId = 1,
                ClientId = "client_3",
                Items = new[]
                    {
                        new OrderItem
                        {
                            Product = new Product { ProductId = "product_1", Name = "Product1" },
                            Amount = 3, AmountPriceRur = 600, AmountPriceBonus = 1800, PriceRur = 200, PriceBonus = 600
                        }
                    },
                DeliveryInfo = new DeliveryInfo
                {
                    DeliveryVariantName = "Delivery",
                    DeliveryType = DeliveryTypes.Delivery,
                    Address = new DeliveryAddress { RegionTitle = "Region", CityTitle = "City", AddressText = "Region, City, 1-1" },
                    Contact = new Contact
                    {
                        FirstName = "Eva",
                        LastName = "Grey",
                        Phone = new PhoneNumber { CountryCode = "7", CityCode = "495", LocalNumber = "5555555" }
                    }
                },
                BonusTotalCost = 2200,
                TotalCost = 733,
                DeliveryCost = 133,
                ItemsCost = 600,
                InsertedDate = DateTime.Now
            });

            _processor.Execute();

            _orderManagementService.Received(1).ChangeOrdersStatusesBeforePayment();
            _orderManagementService.Received(2).GetOrdersForPayment(Arg.Any<GetOrdersForPaymentParameters>());
            _paymentService.Received(0).GetPaymentByOrderId(Arg.Any<int>());
            _orderForPaymentRepository.Received(1).GetAll();
            _orderPaymentResponseRepository.Received(1).GetAll();

            Assert.AreEqual(1, _ordersForPayment.Count);
            Assert.AreEqual(2, _orderItemsForPayment.Count, "Количество товаров в заказе + доставка");

            Assert.AreEqual(_ordersForPayment[0].OrderId, _orders[0].Id);
            Assert.AreEqual(_orderItemsForPayment[0].OrderId, _orders[0].Id);
            Assert.AreEqual(_orderItemsForPayment[1].OrderId, _orders[0].Id);
        }

        [TestMethod]
        public void ShouldNotSaveOrderForPayment_InvalidOrder()
        {
            _orders.Add(new Order { Id = 4, PartnerId = 1 });

            _processor.Execute();

            _orderManagementService.Received(1).ChangeOrdersStatusesBeforePayment();
            _orderManagementService.Received(2).GetOrdersForPayment(Arg.Any<GetOrdersForPaymentParameters>());
            _paymentService.Received(0).GetPaymentByOrderId(Arg.Any<int>());
            _orderForPaymentRepository.Received(0).GetAll();
            _orderPaymentResponseRepository.Received(0).GetAll();

            Assert.AreEqual(0, _ordersForPayment.Count);
            Assert.AreEqual(0, _orderItemsForPayment.Count);
        }

        [TestMethod]
        public void ShouldNotSaveOrderForPayment_OrderWithAdvanceWithoutInfo()
        {
            _orders.Add(new Order
            {
                Id = 5,
                PartnerId = 1,
                ClientId = "client_2",
                Items = new[]
                    {
                        new OrderItem
                        {
                            Product = new Product { ProductId = "product_1", Name = "Product1" }
                        }
                    },
                DeliveryInfo = new DeliveryInfo
                {
                    DeliveryVariantName = "Delivery",
                    DeliveryType = DeliveryTypes.Delivery,
                    Address = new DeliveryAddress { RegionTitle = "Region", CityTitle = "City", AddressText = "Region, City, 1-1" },
                    Contact = new Contact
                    {
                        FirstName = "Ivan",
                        LastName = "Ivanov",
                        Phone = new PhoneNumber { CountryCode = "7", CityCode = "495", LocalNumber = "7654321s" }
                    }
                },
                BonusTotalCost = 1000,
                TotalCost = 333,
                DeliveryCost = 133,
                ItemsCost = 200,
                DeliveryAdvance = 133,
                InsertedDate = DateTime.Now
            });

            _processor.Execute();

            _orderManagementService.Received(1).ChangeOrdersStatusesBeforePayment();
            _orderManagementService.Received(2).GetOrdersForPayment(Arg.Any<GetOrdersForPaymentParameters>());
            _paymentService.Received(1).GetPaymentByOrderId(Arg.Any<int>());
            _orderForPaymentRepository.Received(0).GetAll();
            _orderPaymentResponseRepository.Received(0).GetAll();

            Assert.AreEqual(0, _ordersForPayment.Count);
            Assert.AreEqual(0, _orderItemsForPayment.Count);
        }

        [TestMethod]
        public void ShouldNotSaveOrderForPayment_WaitingReplyForOrder()
        {
            _orders.Add(new Order
            {
                Id = 6,
                PartnerId = 1,
                ClientId = "client_2",
                Items = new[]
                    {
                        new OrderItem
                        {
                            Product = new Product { ProductId = "product_1", Name = "Product1" }
                        }
                    },
                DeliveryInfo = new DeliveryInfo
                {
                    DeliveryVariantName = "Delivery",
                    DeliveryType = DeliveryTypes.Delivery,
                    Address = new DeliveryAddress { RegionTitle = "Region", CityTitle = "City", AddressText = "Region, City, 1-1" },
                    Contact = new Contact
                    {
                        FirstName = "Ivan",
                        LastName = "Ivanov",
                        Phone = new PhoneNumber { CountryCode = "7", CityCode = "495", LocalNumber = "7654321s" }
                    }
                },
                BonusTotalCost = 1000,
                TotalCost = 333,
                DeliveryCost = 133,
                ItemsCost = 200,
                InsertedDate = DateTime.Now
            });

            _processor.Execute();

            _orderManagementService.Received(1).ChangeOrdersStatusesBeforePayment();
            _orderManagementService.Received(2).GetOrdersForPayment(Arg.Any<GetOrdersForPaymentParameters>());
            _paymentService.Received(0).GetPaymentByOrderId(Arg.Any<int>());
            _orderForPaymentRepository.Received(1).GetAll();
            _orderPaymentResponseRepository.Received(1).GetAll();

            Assert.AreEqual(0, _ordersForPayment.Count);
            Assert.AreEqual(0, _orderItemsForPayment.Count);
        }
    }
}
