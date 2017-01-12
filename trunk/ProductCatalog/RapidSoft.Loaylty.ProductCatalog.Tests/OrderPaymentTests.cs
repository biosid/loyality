using NSubstitute;
using RapidSoft.Loaylty.ProductCatalog.API.Entities;
using RapidSoft.Loaylty.ProductCatalog.API.InputParameters;
using RapidSoft.Loaylty.ProductCatalog.API.OutputResults;

namespace RapidSoft.Loaylty.ProductCatalog.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.ProductCatalog.API;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces;
    using RapidSoft.Loaylty.ProductCatalog.Interfaces;
    using RapidSoft.Loaylty.ProductCatalog.Services;

    [TestClass]
    public class OrderPaymentTests
    {
        [TestMethod]
        public void ShouldCancelBonusPaymentWhenChangeOrderStatus()
        {
            var request = new[]
            {
                new OrdersStatus { OrderId = 1, OrderStatus = OrderStatuses.CancelledByPartner },
                new OrdersStatus { OrderId = 2, OrderStatus = OrderStatuses.CancelledByPartner }
            };

            var response = new[]
            {
                new ChangeOrderStatusResult { ResultCode = 0, OrderId = 1, OriginalStatus = OrderStatuses.Processing },
                new ChangeOrderStatusResult { ResultCode = 1, OrderId = 2, OriginalStatus = OrderStatuses.Processing }
            };

            var ordersDataSource = Substitute.For<IOrdersDataSource>();

            ordersDataSource.UpdateOrdersStatuses(Arg.Any<OrdersStatus[]>(), Arg.Any<string>())
                            .Returns(response);

            var bonusGatewayProvider = Substitute.For<IBonusGatewayProvider>();

            var service = CreateOrderManagementService(
                ordersDataSource: ordersDataSource,
                bonusGatewayProvider: bonusGatewayProvider);

            service.ChangeOrdersStatuses(request);

            bonusGatewayProvider.Received(1).CancelPayment(Arg.Any<string>());
            bonusGatewayProvider.Received(1).CancelPayment("1");
        }

        [TestMethod]
        public void ShouldCancelAdvancePaymentWhenChangeOrderStatus()
        {
            var request = new[]
            {
                new OrdersStatus { OrderId = 1, OrderStatus = OrderStatuses.CancelledByPartner },
                new OrdersStatus { OrderId = 2, OrderStatus = OrderStatuses.CancelledByPartner },
                new OrdersStatus { OrderId = 3, OrderStatus = OrderStatuses.CancelledByPartner }
            };

            var response = new[]
            {
                new ChangeOrderStatusResult { ResultCode = 0, OrderId = 1, OriginalStatus = OrderStatuses.Processing },
                new ChangeOrderStatusResult { ResultCode = 0, OrderId = 2, OriginalStatus = OrderStatuses.Processing },
                new ChangeOrderStatusResult { ResultCode = 1, OrderId = 3, OriginalStatus = OrderStatuses.Processing }
            };

            var ordersDataSource = Substitute.For<IOrdersDataSource>();
            ordersDataSource.UpdateOrdersStatuses(Arg.Any<OrdersStatus[]>(), Arg.Any<string>())
                            .Returns(response);

            var ordersRepository = Substitute.For<IOrdersRepository>();
            ordersRepository.GetOrdersWithAdvancePayments(Arg.Any<int[]>())
                            .Returns(new[] { 1 });

            var advancePaymentProvider = Substitute.For<IAdvancePaymentProvider>();

            var service = CreateOrderManagementService(
                ordersDataSource: ordersDataSource,
                ordersRepository: ordersRepository,
                advancePaymentProvider: advancePaymentProvider);

            service.ChangeOrdersStatuses(request);

            advancePaymentProvider.Received(1).CancelPayment(Arg.Any<int>());
            advancePaymentProvider.Received(1).CancelPayment(1);
        }

        [TestMethod]
        public void ShouldConfirmAdvancePaymentWhenChangeOrderStatus()
        {
            var request = new[]
            {
                new OrdersStatus { OrderId = 1, OrderStatus = OrderStatuses.DeliveryWaiting },
                new OrdersStatus { OrderId = 2, OrderStatus = OrderStatuses.Delivery },
                new OrdersStatus { OrderId = 3, OrderStatus = OrderStatuses.Delivered },
                new OrdersStatus { OrderId = 4, OrderStatus = OrderStatuses.DeliveredWithDelay },
                new OrdersStatus { OrderId = 5, OrderStatus = OrderStatuses.NotDelivered },
                new OrdersStatus { OrderId = 6, OrderStatus = OrderStatuses.CancelledByPartner },
                new OrdersStatus { OrderId = 7, OrderStatus = OrderStatuses.Delivery },
                new OrdersStatus { OrderId = 8, OrderStatus = OrderStatuses.DeliveryWaiting }
            };

            var response = new[]
            {
                new ChangeOrderStatusResult { ResultCode = 0, OrderId = 1, OriginalStatus = OrderStatuses.Processing },
                new ChangeOrderStatusResult { ResultCode = 0, OrderId = 2, OriginalStatus = OrderStatuses.Processing },
                new ChangeOrderStatusResult { ResultCode = 0, OrderId = 3, OriginalStatus = OrderStatuses.Processing },
                new ChangeOrderStatusResult { ResultCode = 0, OrderId = 4, OriginalStatus = OrderStatuses.Processing },
                new ChangeOrderStatusResult { ResultCode = 0, OrderId = 5, OriginalStatus = OrderStatuses.Processing },
                new ChangeOrderStatusResult { ResultCode = 0, OrderId = 6, OriginalStatus = OrderStatuses.Processing },
                new ChangeOrderStatusResult { ResultCode = 1, OrderId = 7, OriginalStatus = OrderStatuses.Processing },
                new ChangeOrderStatusResult { ResultCode = 1, OrderId = 8, OriginalStatus = OrderStatuses.Processing }
            };

            var ordersDataSource = Substitute.For<IOrdersDataSource>();
            ordersDataSource.UpdateOrdersStatuses(Arg.Any<OrdersStatus[]>(), Arg.Any<string>())
                            .Returns(response);

            var ordersRepository = Substitute.For<IOrdersRepository>();
            ordersRepository.GetOrdersWithAdvancePayments(Arg.Any<int[]>())
                            .Returns(new[] { 1, 3 });

            var advancePaymentProvider = Substitute.For<IAdvancePaymentProvider>();

            var service = CreateOrderManagementService(
                ordersDataSource: ordersDataSource,
                ordersRepository: ordersRepository,
                advancePaymentProvider: advancePaymentProvider);

            service.ChangeOrdersStatuses(request);

            advancePaymentProvider.Received(2).ConfirmPayment(Arg.Any<int>());
            advancePaymentProvider.Received(1).ConfirmPayment(1);
            advancePaymentProvider.Received(1).ConfirmPayment(3);
        }

        private OrderManagementService CreateOrderManagementService(
            IOrdersDataSource ordersDataSource = null,
            IBasketService basketService = null,
            IPartnerConnectorProvider partnerConnectorProvider = null,
            IOrdersRepository ordersRepository = null,
            IPartnerRepository partnerRepository = null,
            IBonusGatewayProvider bonusGatewayProvider = null,
            IAdvancePaymentProvider advancePaymentProvider = null,
            IDeliveryVariantsProvider deliveryVariantsProvider = null,
            IPriceSpecification priceSpecification = null,
            IMechanicsProvider mechanicsProvider = null,
            IGeoPointProvider geoPointProvider = null)
        {
            return new OrderManagementService(
                ordersDataSource ?? Substitute.For<IOrdersDataSource>(),
                basketService ?? Substitute.For<IBasketService>(),
                partnerConnectorProvider ?? Substitute.For<IPartnerConnectorProvider>(),
                ordersRepository ?? Substitute.For<IOrdersRepository>(),
                partnerRepository ?? Substitute.For<IPartnerRepository>(),
                bonusGatewayProvider ?? Substitute.For<IBonusGatewayProvider>(),
                advancePaymentProvider ?? Substitute.For<IAdvancePaymentProvider>(),
                deliveryVariantsProvider ?? Substitute.For<IDeliveryVariantsProvider>(),
                priceSpecification ?? Substitute.For<IPriceSpecification>(),
                mechanicsProvider ?? Substitute.For<IMechanicsProvider>(),
                geoPointProvider ?? Substitute.For<IGeoPointProvider>());
        }
    }
}
