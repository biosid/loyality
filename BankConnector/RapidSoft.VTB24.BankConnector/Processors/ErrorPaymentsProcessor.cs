namespace RapidSoft.VTB24.BankConnector.Processors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RapidSoft.Loaylty.ProductCatalog.WsClients.CatalogAdminService;
    using RapidSoft.VTB24.BankConnector.Acquiring;
    using RapidSoft.VTB24.BankConnector.DataSource;
    using RapidSoft.VTB24.BankConnector.Infrastructure.Configuration;
    using RapidSoft.VTB24.BankConnector.Service;

    public class ErrorPaymentsProcessor : ProcessorBase
    {
        private const int MaximumPaymentsCount = 1000;

        private readonly ICatalogAdminService catalogAdminService;
        private readonly IUnitellerProvider unitellerProvider;

        public ErrorPaymentsProcessor(
            EtlLogger.EtlLogger logger,
            IUnitOfWork uow,
            ICatalogAdminService catalogAdminService,
            IUnitellerProvider unitellerProvider)
            : base(logger, uow)
        {
            this.catalogAdminService = catalogAdminService;
            this.unitellerProvider = unitellerProvider;
        }

        private delegate void UpdatePaymentStatusesBatch(int[] orderIds, out int errorsCount);

        public bool HasErrors { get; private set; }

        public void Execute()
        {
            HasErrors = false;

            ExecuteItemsPayments();
            ExecuteDeliveryPayments();
        }

        public void ExecuteItemsPayments()
        {
            var succeededOrderIds = new List<int>();

            Logger.Info("Получение заказов с ошибками оплаты позиций");

            var payments = GetItemsPayments();

            Logger.Counter("Заказы с ошибками оплаты позиций", "всего", payments.Length);

            long shopIdErrors = 0, paymentErrors = 0;

            Logger.Info("Проведение оплаты позиций заказов");

            foreach (var payment in payments)
            {
                if (payment.ShopId == null)
                {
                    ++shopIdErrors;
                    Logger.ErrorFormat(
                        "не удалось получить ShopID для выполнения оплаты позиций заказа {0} (партнер {1})",
                        payment.OrderId,
                        payment.PartnerId);
                    continue;
                }

                if (PerformPayment(payment.ShopId, payment.Amount, payment.ClientId, payment.OrderId.ToString("D")))
                {
                    succeededOrderIds.Add(payment.OrderId);
                }
                else
                {
                    ++paymentErrors;
                }
            }

            Logger.Counter("Заказы с ошибками оплаты позиций", "не удалось получить ShopID", shopIdErrors);
            Logger.Counter("Заказы с ошибками оплаты позиций", "не удалось провести оплату", paymentErrors);

            Logger.Info("Обновление статусов оплаты позиций заказов");

            int statusErrors;
            UpdatePaymentStatuses(succeededOrderIds.ToArray(), UpdateItemsPaymentStatuses, out statusErrors);

            Logger.Counter("Заказы с ошибками оплаты позиций", "не удалось обновить статус", statusErrors);

            if (shopIdErrors > 0 || paymentErrors > 0 || statusErrors > 0)
            {
                HasErrors = true;
            }
        }

        public void ExecuteDeliveryPayments()
        {
            var succeededOrderIds = new List<int>();

            Logger.Info("Получение заказов с ошибками оплаты доставки");

            var payments = GetDeliveryPayments();

            Logger.Counter("Заказы с ошибками оплаты доставки", "всего", payments.Length);

            long shopIdErrors = 0, paymentErrors = 0;

            Logger.Info("Проведение оплаты доставки заказов");

            foreach (var payment in payments)
            {
                if (payment.ShopId == null)
                {
                    ++shopIdErrors;
                    Logger.ErrorFormat(
                        "не удалось получить ShopID для выполнения оплаты доставки заказа {0} (партнер {1})",
                        payment.OrderId,
                        payment.PartnerId);
                    continue;
                }

                if (PerformPayment(payment.ShopId, payment.Amount, payment.ClientId, payment.OrderId.ToString("D") + "/d"))
                {
                    succeededOrderIds.Add(payment.OrderId);
                }
                else
                {
                    ++paymentErrors;
                }
            }

            Logger.Counter("Заказы с ошибками оплаты доставки", "не удалось получить ShopID", shopIdErrors);
            Logger.Counter("Заказы с ошибками оплаты доставки", "не удалось провести оплату", paymentErrors);

            Logger.Info("Обновление статусов оплаты доставки заказов");

            int statusErrors;
            UpdatePaymentStatuses(succeededOrderIds.ToArray(), UpdateDeliveryPaymentStatuses, out statusErrors);

            Logger.Counter("Заказы с ошибками оплаты доставки", "не удалось обновить статус", statusErrors);

            if (shopIdErrors > 0 || paymentErrors > 0 || statusErrors > 0)
            {
                HasErrors = true;
            }
        }

        private string GetShopId(Partner partner)
        {
            string shopId;

            if (partner.Settings != null &&
                partner.Settings.TryGetValue(SendOrdersProcessor.UnitellerShopIdPartnerKey, out shopId) &&
                !string.IsNullOrWhiteSpace(shopId))
            {
                return shopId;
            }

            return null;
        }

        private PaymentInfo[] GetItemsPayments()
        {
            var payments = GetOrders(new[] { OrderPaymentStatuses.Error }, null)
                .Select(order => new PaymentInfo
                {
                    OrderId = order.Id,
                    PartnerId = order.PartnerId,
                    Amount = order.ItemsCost,
                    ClientId = order.ClientId
                })
                .Take(MaximumPaymentsCount)
                .ToArray();

            FillShopIds(payments);

            return payments;
        }

        private PaymentInfo[] GetDeliveryPayments()
        {
            var payments = GetOrders(null, new[] { OrderDeliveryPaymentStatus.Error })
                .Select(order => new PaymentInfo
                {
                    OrderId = order.Id,
                    PartnerId = order.CarrierId ?? order.PartnerId,
                    Amount = order.DeliveryAdvance > 0 ? 0 : order.DeliveryCost,
                    ClientId = order.ClientId
                })
                .Where(payment => payment.Amount > 0)
                .Take(MaximumPaymentsCount)
                .ToArray();

            FillShopIds(payments);

            return payments;
        }

        private IEnumerable<Order> GetOrders(OrderPaymentStatuses[] itemsPaymentStatuses, OrderDeliveryPaymentStatus[] deliveryPaymentStatuses)
        {
            var skip = 0;
            var take = ConfigHelper.BatchSize;

            Order[] ordersBatch;

            while ((ordersBatch = GetOrdersBatch(itemsPaymentStatuses, deliveryPaymentStatuses, skip, take)) != null
                   && ordersBatch.Length > 0)
            {
                skip += ordersBatch.Length;

                foreach (var order in ordersBatch)
                {
                    yield return order;
                }
            }
        }

        private Order[] GetOrdersBatch(OrderPaymentStatuses[] itemsPaymentStatuses, OrderDeliveryPaymentStatus[] deliveryPaymentStatuses, int skip, int take)
        {
            try
            {
                var response = catalogAdminService.SearchOrders(new SearchOrdersParameters
                {
                    OrderPaymentStatuses = itemsPaymentStatuses,
                    OrderDeliveryPaymentStatus = deliveryPaymentStatuses,
                    CountToSkip = skip,
                    CountToTake = take,
                    UserId = ConfigHelper.VtbSystemUser
                });

                if (!response.Success)
                {
                    Logger.ErrorFormat(
                        "Не удалось получить пачку заказов: CatalogAdminService [{0}] - {1}",
                        response.ResultCode,
                        response.ResultDescription);
                    return null;
                }

                if (response.Orders == null)
                {
                    Logger.Error("Не удалось получить пачку заказов: Orders = null");
                    return null;
                }

                return response.Orders;
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при получении пачки заказов", ex);
                return null;
            }
        }

        private void FillShopIds(PaymentInfo[] payments)
        {
            var partnerIds = payments.Select(p => p.PartnerId).Distinct().ToArray();

            var shopIds = GetShopIds(partnerIds);

            if (shopIds == null)
            {
                return;
            }

            foreach (var payment in payments)
            {
                string shopId;
                if (shopIds.TryGetValue(payment.PartnerId, out shopId))
                {
                    payment.ShopId = shopId;
                }
            }
        }

        private Dictionary<int, string> GetShopIds(int[] partnerIds)
        {
            try
            {
                var response = catalogAdminService.GetPartners(partnerIds, ConfigHelper.VtbSystemUser);

                if (!response.Success)
                {
                    Logger.ErrorFormat(
                        "Не удалось получить список партнеров: CatalogAdminService [{0}] - {1}",
                        response.ResultCode,
                        response.ResultDescription);
                    return null;
                }

                return response.Partners
                               .Select(p => new { id = p.Id, shopId = GetShopId(p) })
                               .Where(i => i.shopId != null)
                               .ToDictionary(i => i.id, i => i.shopId);
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при получении списка партнеров", ex);
                return null;
            }
        }

        private bool PerformPayment(string shopId, decimal amount, string clientId, string orderId)
        {
            try
            {
                unitellerProvider.Pay(shopId, amount, clientId, orderId);
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat(
                    ex,
                    "Ошибка при выполнении оплаты в uniteller (shopId = '{0}', sum = '{1}', clientId = '{2}', orderId = '{3}'",
                    shopId,
                    amount,
                    clientId,
                    orderId);
                return false;
            }
        }

        private void UpdatePaymentStatuses(int[] orderIds, UpdatePaymentStatusesBatch updateBatch, out int errorsCount)
        {
            var skip = 0;
            var take = ConfigHelper.BatchSize;

            errorsCount = 0;
            int[] ids;

            while ((ids = orderIds.Skip(skip).Take(take).ToArray()).Length > 0)
            {
                int errorsCountInBatch;

                updateBatch(ids, out errorsCountInBatch);

                errorsCount += errorsCountInBatch;

                skip += ids.Length;
            }
        }

        private void UpdateItemsPaymentStatuses(int[] orderIds, out int errorsCount)
        {
            try
            {
                var statuses = orderIds.Select(id => new OrdersPaymentStatus
                {
                    OrderId = id,
                    PaymentStatus = OrderPaymentStatuses.Yes
                }).ToArray();

                var response = catalogAdminService.ChangeOrdersPaymentStatuses(ConfigHelper.VtbSystemUser, statuses);

                if (!response.Success)
                {
                    Logger.ErrorFormat(
                        "Не удалось обновить статусы оплаты позиций заказов: CatalogAdminService [{0}] - {1}",
                        response.ResultCode,
                        response.ResultDescription);

                    errorsCount = orderIds.Length;
                    return;
                }

                if (response.ChangeOrderStatusResults == null)
                {
                    Logger.Error("Не удалось обновить статусы оплаты позиций заказов: ChangeOrderStatusResult = null");

                    errorsCount = orderIds.Length;
                    return;
                }

                errorsCount = response.ChangeOrderStatusResults.Count(r => !r.Success);

                if (errorsCount > 0)
                {
                    Logger.Error("Не удалось обновить статусы оплаты позиций некоторых заказов");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при обновлении статусов оплаты позиций заказов", ex);

                errorsCount = orderIds.Length;
            }
        }

        private void UpdateDeliveryPaymentStatuses(int[] orderIds, out int errorsCount)
        {
            try
            {
                var statuses = orderIds.Select(id => new OrdersDeliveryStatus
                {
                    OrderId = id,
                    DeliveryStatus = OrderDeliveryPaymentStatus.Yes
                }).ToArray();

                var response = catalogAdminService.ChangeOrdersDeliveryStatuses(ConfigHelper.VtbSystemUser, statuses);

                if (!response.Success)
                {
                    Logger.ErrorFormat(
                        "Не удалось обновить статусы оплаты доставки заказов: CatalogAdminService [{0}] - {1}",
                        response.ResultCode,
                        response.ResultDescription);

                    errorsCount = orderIds.Length;
                    return;
                }

                if (response.ChangeOrderStatusResults == null)
                {
                    Logger.Error("Не удалось обновить статусы оплаты доставки заказов: ChangeOrderStatusResult = null");

                    errorsCount = orderIds.Length;
                    return;
                }

                errorsCount = response.ChangeOrderStatusResults.Count(r => !r.Success);

                if (errorsCount > 0)
                {
                    Logger.Error("Не удалось обновить статусы оплаты доставки некоторых заказов");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при обновлении статусов оплаты доставки заказов", ex);

                errorsCount = orderIds.Length;
            }
        }

        private class PaymentInfo
        {
            public int OrderId { get; set; }

            public int PartnerId { get; set; }

            public decimal Amount { get; set; }

            public string ClientId { get; set; }

            public string ShopId { get; set; }
        }
    }
}
