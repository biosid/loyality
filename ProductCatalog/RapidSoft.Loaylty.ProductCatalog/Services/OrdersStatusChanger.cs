namespace RapidSoft.Loaylty.ProductCatalog.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.API.InputParameters;
    using RapidSoft.Loaylty.ProductCatalog.API.OutputResults;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces;
    using RapidSoft.Loaylty.ProductCatalog.Entities;
    using RapidSoft.Loaylty.ProductCatalog.Interfaces;

    internal class OrdersStatusChanger
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(OrdersStatusChanger));

        private static readonly OrderStatuses[] StatusesToConfirmAdancePayment = new[]
        {
            OrderStatuses.DeliveryWaiting,
            OrderStatuses.Delivery,
            OrderStatuses.Delivered,
            OrderStatuses.DeliveredWithDelay
        };

        private readonly IOrdersDataSource ordersDataSource;
        private readonly IOrdersRepository ordersRepository;
        private readonly IBonusGatewayProvider bonusGatewayProvider;
        private readonly IAdvancePaymentProvider advancePaymentProvider;

        public OrdersStatusChanger(
            IOrdersDataSource ordersDataSource,
            IOrdersRepository ordersRepository,
            IBonusGatewayProvider bonusGatewayProvider,
            IAdvancePaymentProvider advancePaymentProvider)
        {
            this.ordersDataSource = ordersDataSource;
            this.ordersRepository = ordersRepository;
            this.bonusGatewayProvider = bonusGatewayProvider;
            this.advancePaymentProvider = advancePaymentProvider;
        }

        public ChangeOrderStatusResult UpdateOrderStatus(OrdersStatus orderStatuseses, string updatedUserId)
        {
            return UpdateOrdersStatuses(new[] { orderStatuseses }, updatedUserId).Single();
        }

        public ChangeExternalOrderStatusResult UpdateExternalOrderStatus(ExternalOrdersStatus orderStatuseses, string updatedUserId)
        {
            return UpdateExternalOrdersStatuses(new[] { orderStatuseses }, updatedUserId).Single();
        }

        public ChangeOrderStatusResult[] UpdateOrdersStatuses(OrdersStatus[] orderStatuseses, string updatedUserId)
        {
            var results = ordersDataSource.UpdateOrdersStatuses(orderStatuseses, updatedUserId);

            var facts = CollectFacts(orderStatuseses, results);

            Handle(facts);

            return results;
        }

        public ChangeExternalOrderStatusResult[] UpdateExternalOrdersStatuses(ExternalOrdersStatus[] orderStatuseses, string updatedUserId)
        {
            var results = ordersDataSource.UpdateExternalOrdersStatuses(orderStatuseses, updatedUserId);

            var facts = CollectFacts(orderStatuseses.Cast<OrdersStatus>().ToArray(), results.Cast<ChangeOrderStatusResult>().ToArray());

            Handle(facts);

            return results;
        }

        private static OrderStatusChangeFact[] CollectFacts(OrdersStatus[] requests, ChangeOrderStatusResult[] results)
        {
            if (requests.Length != results.Length)
            {
                Log.Error("количество запросов и ответов на смену статуса заказов не совпадает");
            }

            var facts = requests.Zip(results, CollectFact).Where(fact => fact != null).ToArray();

            return facts;
        }

        private static OrderStatusChangeFact CollectFact(OrdersStatus request, ChangeOrderStatusResult result)
        {
            if (!result.Success)
            {
                return null;
            }

            if (!request.OrderId.HasValue || !request.OrderStatus.HasValue)
            {
                Log.Error("обнаружен некорректный запрос на смену статуса заказа");
                return null;
            }

            if (!result.OrderId.HasValue || !result.OriginalStatus.HasValue)
            {
                Log.Error("обнаружен некорректный ответ на смену статуса заказа");
                return null;
            }

            if (request.OrderId.Value != result.OrderId.Value)
            {
                Log.Error("обнаружен запрос/ответ на смену статуса заказа с разными Id заказа");
                return null;
            }

            return new OrderStatusChangeFact
            {
                OrderId = request.OrderId.Value,
                OriginalStatus = result.OriginalStatus.Value,
                NewStatus = request.OrderStatus.Value
            };
        }

        private void Handle(OrderStatusChangeFact[] changes)
        {
            CancelBonusPayments(changes);
            CancelAdvancePayments(changes);
            ConfirmAdvancePayments(changes);
        }

        // отмена бонусного платежа в случае анулирвоания заказа
        private void CancelBonusPayments(IEnumerable<OrderStatusChangeFact> changes)
        {
            foreach (var paymentRequestId in GetBonusPaymentsToCancel(changes))
            {
                bonusGatewayProvider.CancelPayment(paymentRequestId);
            }
        }

        private IEnumerable<string> GetBonusPaymentsToCancel(IEnumerable<OrderStatusChangeFact> changes)
        {
            return changes.Where(c => c.NewStatus == OrderStatuses.CancelledByPartner &&
                                      c.OriginalStatus != OrderStatuses.CancelledByPartner)
                          .Select(c => c.OrderId.ToString("D"));
        }

        // отмена преавторизации платежа картой в случае анулирования заказа
        private void CancelAdvancePayments(IEnumerable<OrderStatusChangeFact> changes)
        {
            foreach (var orderId in GetOrderIdsToCancelAdvance(changes))
            {
                advancePaymentProvider.CancelPayment(orderId);
            }
        }

        private IEnumerable<int> GetOrderIdsToCancelAdvance(IEnumerable<OrderStatusChangeFact> changes)
        {
            var ids = changes.Where(c => c.NewStatus == OrderStatuses.CancelledByPartner &&
                                         c.OriginalStatus != OrderStatuses.CancelledByPartner)
                             .Select(c => c.OrderId)
                             .ToArray();

            return ordersRepository.GetOrdersWithAdvancePayments(ids);
        }

        // подтверждение преавтроризации платежа в случае подтверждения заказа
        private void ConfirmAdvancePayments(IEnumerable<OrderStatusChangeFact> changes)
        {
            foreach (var orderId in GetOrderIdsToConfirmAdvance(changes))
            {
                advancePaymentProvider.ConfirmPayment(orderId);
            }
        }

        private IEnumerable<int> GetOrderIdsToConfirmAdvance(IEnumerable<OrderStatusChangeFact> changes)
        {
            var ids = changes.Where(c => StatusesToConfirmAdancePayment.Contains(c.NewStatus) &&
                                         c.OriginalStatus == OrderStatuses.Processing)
                             .Select(c => c.OrderId)
                             .ToArray();

            return ordersRepository.GetOrdersWithAdvancePayments(ids);
        }
    }
}
