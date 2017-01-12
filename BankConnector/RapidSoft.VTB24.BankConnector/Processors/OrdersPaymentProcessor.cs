namespace RapidSoft.VTB24.BankConnector.Processors
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using RapidSoft.Loaylty.BonusGateway.BonusGateway;
    using RapidSoft.Loaylty.ProductCatalog.WsClients.OrderManagementService;
    using RapidSoft.VTB24.BankConnector.Acquiring;
    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource;
    using RapidSoft.VTB24.BankConnector.Infrastructure.Configuration;
    using RapidSoft.VTB24.BankConnector.Service;

    using CatalogAdmin = RapidSoft.Loaylty.ProductCatalog.WsClients.CatalogAdminService;

    /// <summary>
    ///     Обработчик заказов
    /// </summary>
    public class OrdersPaymentProcessor : ProcessorBase
    {
        private readonly BonusGateway bonusGateway;
        private readonly IOrderManagementService orderManagementService;
        private readonly IUnitellerProvider unitellerProvider;
        private readonly OrderNotificationsProcessor notificationsProcessor;

        public OrdersPaymentProcessor(
            EtlLogger.EtlLogger logger,
            IUnitOfWork uow,
            IOrderManagementService orderManagementService,
            BonusGateway bonusGateway,
            IUnitellerProvider unitellerProvider,
            OrderNotificationsProcessor notificationsProcessor)
            : base(logger, uow)
        {
            this.orderManagementService = orderManagementService;
            this.bonusGateway = bonusGateway;
            this.unitellerProvider = unitellerProvider;
            this.notificationsProcessor = notificationsProcessor;
        }

        public void Execute()
        {
            var sessionId = Guid.Parse(Logger.EtlSessionId);

            var queryOrdersForPaymentResponses =
                Uow.OrderPaymentResponseRepository.GetAll().Where(op => op.EtlSessionId == sessionId);

            Logger.Counter("Ответы банка", "всего", queryOrdersForPaymentResponses.Count());

            var responses = queryOrdersForPaymentResponses
                .GroupJoin(
                    Uow.OrderForPaymentRepository.GetAll(),
                    orderForPaymentResponse => orderForPaymentResponse.OrderId,
                    orderForPayment => orderForPayment.OrderId,
                    (orderForPaymentResponse, ordersForPayment) =>
                    new ResponseInfo
                    {
                        OrderId = orderForPaymentResponse.OrderId,
                        BankStatus = orderForPaymentResponse.Status,
                        IsProcessed = false,
                        Request = ordersForPayment.OrderByDescending(item => item.InsertedDate).FirstOrDefault(),
                    })
                .OrderBy(info => info.OrderId);

            ProcessResponses(responses);
        }

        private void ProcessResponses(IQueryable<ResponseInfo> responses)
        {
            Logger.InfoFormat("Обработка заказов");

            var processed = 0;
            var notMatched = 0;
            var statusError = 0;
            var bankCancelled = 0;
            var paymentErrors = 0;

            var batchSize = ConfigHelper.BatchSize;

            var batch = responses.Take(batchSize).ToArray();

            while (batch.Length > 0)
            {
                notMatched += CheckRequests(batch.Where(r => !r.IsProcessed));

                statusError += GetPaymentStatuses(batch.Where(r => !r.IsProcessed));

                var responsesForPaymentStatusUpdate = batch.Where(r => !r.IsProcessed).ToArray();

                bankCancelled += ProcessCancelled(batch.Where(r => !r.IsProcessed));

                paymentErrors += PerformPayments(batch.Where(r => !r.IsProcessed));
                
                ProcessBankOffersOrders(batch.Where(r => r.IsProcessed));

                SetOrdersPaymentStatusesInOrderManagement(responsesForPaymentStatusUpdate);

                SaveResponses(batch);

                processed += batch.Length;

                Logger.InfoFormat(
                    "Количество обработанных заказов на текущую итерацию: {0}.",
                    processed);

                batch = responses.Skip(processed).Take(batchSize).ToArray();
            }

            Logger.Info("Обработка заказов завершена.");

            Logger.Counter("Обработанные ответы", "всего", processed);
            Logger.Counter("Обработанные ответы", "без запроса", notMatched);
            Logger.Counter("Обработанные ответы", "с ошибкой получения статусов", statusError);
            Logger.Counter("Обработанные ответы", "отменены банком", bankCancelled);
            Logger.Counter("Обработанные ответы", "с ошибкой оплаты", paymentErrors);
        }

        private void SetError(ResponseInfo response, string message)
        {
            response.ItemsPaymentStatus = OrderPaymentStatuses.Error;
            response.DeliveryPaymentStatus = OrderDeliveryPaymentStatus.Error;
            response.Message = message;
            response.IsProcessed = true;
        }

        private int SetError(IEnumerable<ResponseInfo> responses, string message)
        {
            var result = 0;

            foreach (var response in responses)
            {
                SetError(response, message);
                ++result;
            }

            return result;
        }

        private int CheckRequests(IEnumerable<ResponseInfo> responses)
        {
            Logger.Info("Проверка наличия отправленного запроса");

            var result = SetError(responses.Where(r => r.Request == null), "неизвестный заказ");

            return result;
        }

        private int GetPaymentStatuses(IEnumerable<ResponseInfo> responses)
        {
            Logger.Info("Получение статусов оплаты заказов из OrderManagementService");

            var responsesArray = responses.ToArray();

            var ordersIds = responsesArray.Select(o => o.OrderId).ToArray();

            GetOrderPaymentStatusesResult statusesResult;
            try
            {
                statusesResult = orderManagementService.GetOrderPaymentStatuses(ordersIds);
                if (!statusesResult.Success)
                {
                    Logger.ErrorFormat(
                        "Ошибка получения статусов оплаты заказов: [{0}] {1}",
                        statusesResult.ResultCode,
                        statusesResult.ResultDescription);
                    statusesResult = null;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка получения статусов оплаты заказов:", ex);
                statusesResult = null;
            }

            if (statusesResult == null)
            {
                return SetError(responsesArray, "ошибка обработки");
            }

            var join = responsesArray
                .GroupJoin(
                    statusesResult.OrderPaymentStatuses,
                    r => r.OrderId,
                    s => s.Id,
                    (response, statuses) => new { response, status = statuses.FirstOrDefault() })
                .ToArray();

            var result = 0;

            foreach (var j in join)
            {
                var response = j.response;
                var status = j.status;

                if (status == null)
                {
                    Logger.WarnFormat("Не получен статус оплаты по заказу {0}", response.OrderId);

                    SetError(response, "ошибка обработки");
                    ++result;
                }
                else
                {
                    response.ItemsPaymentStatus = status.PaymentStatus;
                    response.DeliveryPaymentStatus = status.DeliveryPaymentStatus;
                }
            }

            return result;
        }

        private int ProcessCancelled(IEnumerable<ResponseInfo> responses)
        {
            var result = 0;

            var cancelled = responses.Where(r => r.BankStatus != (int)ReceivedOrderStatus.Approved).ToArray();

            foreach (var response in cancelled)
            {
                response.ItemsPaymentStatus = OrderPaymentStatuses.BankCancelled;
                response.DeliveryPaymentStatus = OrderDeliveryPaymentStatus.BankCancelled;
                response.IsProcessed = true;
                ++result;
            }

            var bankProductOrders = cancelled.Where(r => r.Request.PartnerId == ConfigHelper.BankProductsPartnerId)
                                             .ToArray();
            if (bankProductOrders.Length > 0)
            {
                SetOrderStatus(bankProductOrders, OrderStatuses.CancelledByPartner);
            }

            return result;
        }

        private int PerformPayments(IEnumerable<ResponseInfo> responses)
        {
            var result = 0;

            foreach (var response in responses)
            {
                var doItemsPayment = NeedItemsPayment(response);
                var doDeliveryPayment = NeedDeliveryPayment(response);

                if (doItemsPayment)
                {
                    PerformItemsPayment(response);
                    if (response.ItemsPaymentStatus != OrderPaymentStatuses.Yes)
                    {
                        response.Message = "ошибка при выполнении оплаты позиций заказа";
                    }
                }
                else if (response.Request.PartnerId == ConfigHelper.BankProductsPartnerId)
                {
                    response.ItemsPaymentStatus = OrderPaymentStatuses.Yes;
                }

                if (doDeliveryPayment)
                {
                    PerformDeliveryPayment(response);
                    if (response.DeliveryPaymentStatus != OrderDeliveryPaymentStatus.Yes)
                    {
                        response.Message = (response.Message != null ? response.Message + "/" : string.Empty) +
                                           "ошибка при выполнении оплаты доставки заказа";
                    }
                }

                if (doItemsPayment || doDeliveryPayment || response.Request.PartnerId == ConfigHelper.BankProductsPartnerId)
                {
                    ConfirmInBonusGateway(response);
                }

                if ((doItemsPayment && response.ItemsPaymentStatus != OrderPaymentStatuses.Yes) ||
                    (doDeliveryPayment && response.DeliveryPaymentStatus != OrderDeliveryPaymentStatus.Yes))
                {
                    ++result;
                }

                response.IsProcessed = true;
            }

            return result;
        }

        /// <summary>
        /// Для подтверждённых заказов по продуктам банка проставляем статус "Доставлено" и отправляем сообщение клиенту
        /// </summary>
        /// <param name="responses">Подтверждённые запросы по заказам продуктов банка</param>
        private void ProcessBankOffersOrders(IEnumerable<ResponseInfo> responses)
        {
            var responsesToProcess = responses.Where(r =>
                                                     r.Request.PartnerId == ConfigHelper.BankProductsPartnerId &&
                                                     r.BankStatus == (int)ReceivedOrderStatus.Approved)
                                              .ToArray();

            if (responsesToProcess.Length == 0)
            {
                return; // Нет подтверждённых заказов по продуктам банка - выходим
            }

            var changeStatusResult = SetOrderStatus(responsesToProcess, OrderStatuses.Delivered);

            if (changeStatusResult != null && changeStatusResult.Success)
            {
                var successOrders = changeStatusResult.ChangeOrderStatusResults
                                                      .Where(r => r.Success && r.OrderId.HasValue)
                                                      .Select(r => r.OrderId.Value)
                                                      .ToArray();

                notificationsProcessor.NotifyExecutedBankOfferOrders(responsesToProcess.Where(r => successOrders.Contains(r.Request.OrderId)).Select(r => r.Request).ToList());
            }
        }

        private bool NeedItemsPayment(ResponseInfo response)
        {
            var status = response.ItemsPaymentStatus;

            return response.Request.OrderItemsCost != 0 &&
                   (status == OrderPaymentStatuses.No ||
                    status == OrderPaymentStatuses.Error ||
                    status == OrderPaymentStatuses.BankCancelled);
        }

        private bool NeedDeliveryPayment(ResponseInfo response)
        {
            var status = response.DeliveryPaymentStatus;

            return response.Request.OrderDeliveryCost != 0 &&
                   (status == OrderDeliveryPaymentStatus.No ||
                    status == OrderDeliveryPaymentStatus.Error ||
                    status == OrderDeliveryPaymentStatus.BankCancelled);
        }

        private void SetOrdersPaymentStatusesInOrderManagement(ResponseInfo[] responses)
        {
            Logger.InfoFormat("Установка статусов оплаты заказов в OrderManagementService.");

            SetOrdersItemPaymentStatusInOrderManagement(responses);

            SetOrdersDeliveryPaymentStatusInOrderManagement(responses);
        }

        private void PerformItemsPayment(ResponseInfo response)
        {
            var request = response.Request;

            try
            {
                unitellerProvider.Pay(
                    request.UnitellerItemsShopId,
                    request.OrderItemsCost,
                    request.ClientId.ToString(CultureInfo.InvariantCulture),
                    request.OrderId.ToString(CultureInfo.InvariantCulture));
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat(
                    "Ошибка при выполнении оплаты позиций заказа {0} в uniteller: {1}",
                    response.OrderId,
                    ex.ToString());

                response.ItemsPaymentStatus = OrderPaymentStatuses.Error;
                return;
            }

            Logger.InfoFormat(
                "Выполнение оплаты в uniteller выполнено для позиций заказа ({0}), сумма ({1}), клиент ({2}), идентификатор партнера ({3}), внешний id партнера ({4})",
                request.OrderId,
                request.OrderItemsCost,
                request.ClientId,
                request.PartnerId,
                request.UnitellerItemsShopId);

            response.ItemsPaymentStatus = OrderPaymentStatuses.Yes;
        }

        private void PerformDeliveryPayment(ResponseInfo response)
        {
            var request = response.Request;

            try
            {
                unitellerProvider.Pay(
                    request.UnitellerDeliveryShopId,
                    request.OrderDeliveryCost,
                    request.ClientId.ToString(CultureInfo.InvariantCulture),
                    request.OrderId.ToString(CultureInfo.InvariantCulture) + "/d");
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat(
                    "Ошибка при выполнении оплаты доставки заказа({0}) в uniteller: {1}",
                    response.OrderId,
                    ex.ToString());

                response.DeliveryPaymentStatus = OrderDeliveryPaymentStatus.Error;
                return;
            }

            Logger.InfoFormat(
                "Выполнение оплаты в uniteller выполнено для доставки заказа ({0}), сумма ({1}), клиент ({2}), идентификатор партнера ({3}), внешний id партнера ({4})",
                request.OrderId,
                request.OrderDeliveryCost,
                request.ClientId,
                request.CurrierId,
                request.UnitellerDeliveryShopId);

            response.DeliveryPaymentStatus = OrderDeliveryPaymentStatus.Yes;
        }

        private void ConfirmInBonusGateway(ResponseInfo response)
        {
            try
            {
                var confirmRequest = new ConfirmPointsRequest
                {
                    RequestId = Guid.NewGuid().ToString(),
                    OriginalRequestId = response.OrderId.ToString("D"),
                    RequestDateTime = response.Request.OrderDateTime,
                    PartnerId = ConfigHelper.LoyaltyPartnerId,
                    PosId = ConfigHelper.LoyaltyPosId,
                    TerminalId = ConfigHelper.LoyaltyTerminalId,
                };

                var confirmResponse = bonusGateway.ConfirmPoints(confirmRequest);

                if (confirmResponse.Status != 0)
                {
                    Logger.ErrorFormat(
                        "Ошибка при подтверждении списания заказа ({0}) в БПШ ({1} - {2})",
                        response.OrderId,
                        confirmResponse.Status,
                        confirmResponse.Error);

                    response.BonusGatewayStatus = BonusGatewayConfirmStatus.Error;
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat(ex, "Ошибка при выполнении заказа({0}) в БПШ", response.OrderId);

                response.BonusGatewayStatus = BonusGatewayConfirmStatus.Error;
                return;
            }

            Logger.InfoFormat(
                "Успешно подтверждено в БПШ orderId:{0} bonus:{2} partnerId:{1}",
                response.OrderId,
                response.Request.PartnerId,
                response.Request.OrderBonusCost);

            response.BonusGatewayStatus = BonusGatewayConfirmStatus.Confirmed;
        }

        private void SetOrdersItemPaymentStatusInOrderManagement(IEnumerable<ResponseInfo> responses)
        {
            var changeRequest =
                responses.Select(r => new OrdersPaymentStatus
                {
                    OrderId = r.OrderId,
                    ClientId = r.Request.ClientId,
                    PaymentStatus = r.ItemsPaymentStatus
                }).ToArray();

            try
            {
                var changeResponse = orderManagementService.ChangeOrdersPaymentStatuses(changeRequest);

                if (!changeResponse.Success)
                {
                    Logger.ErrorFormat(
                        "При установке статусов оплаты позиций заказов в OrderManagementService произошла ошибка: [{0}] {1}",
                        changeResponse.ResultCode,
                        changeResponse.ResultDescription);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("При установке статусов оплаты позиций заказов в OrderManagementService произошла ошибка", ex);
            }
        }

        private ChangeOrdersStatusesResult SetOrderStatus(IEnumerable<ResponseInfo> responses, OrderStatuses newStatus)
        {
            ChangeOrdersStatusesResult serviceResponse = null;
            try
            {
                serviceResponse = orderManagementService.ChangeOrdersStatuses(
                    responses.Select(
                        r =>
                            new OrdersStatus
                            {
                                ClientId = r.Request.ClientId,
                                OrderId = r.OrderId,
                                OrderStatus = newStatus
                            })
                        .ToArray());
                
                if (!serviceResponse.Success)
                {
                    Logger.ErrorFormat("При изменении статусов заказов в OrderManagementService произошла ошибка: {0}", serviceResponse.ResultDescription);    
                }

                foreach (var result in serviceResponse.ChangeOrderStatusResults.Where(cosr => !cosr.Success))
                {
                    Logger.WarnFormat("При изменении статуса заказа Id:{0} в OrderManagementService произошла ошибка: {1}", result.OrderId, result.ResultDescription);
                }
            }
            catch (Exception error)
            {
                Logger.Error("При изменении статусов заказов в OrderManagementService произошла ошибка", error);
            }

            return serviceResponse;
        }

        private void SetOrdersDeliveryPaymentStatusInOrderManagement(IEnumerable<ResponseInfo> responses)
        {
            var changeRequest =
                responses.Select(r => new OrdersDeliveryStatus
                {
                    OrderId = r.OrderId,
                    ClientId = r.Request.ClientId,
                    DeliveryStatus = r.DeliveryPaymentStatus
                }).ToArray();

            try
            {
                var changeResponse = orderManagementService.ChangeOrdersDeliveryStatuses(changeRequest);

                if (!changeResponse.Success)
                {
                    Logger.ErrorFormat(
                        "При установке статусов оплаты доставки заказов в OrderManagementService произошла ошибка: [{0}] {1}",
                        changeResponse.ResultCode,
                        changeResponse.ResultDescription);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("При установке статусов оплаты доставки заказов в OrderManagementService произошла ошибка", ex);
            }
        }

        private void SaveResponses(IEnumerable<ResponseInfo> responses)
        {
            var etlSessionId = Guid.Parse(Logger.EtlSessionId);

            foreach (var response in responses)
            {
                var response2 = new OrderPaymentResponse2
                {
                    OrderId = response.OrderId,
                    EtlSessionId = etlSessionId,
                    ItemPaymentStatus = (int)response.ItemsPaymentStatus,
                    DeliveryPaymentStatus = (int)response.DeliveryPaymentStatus,
                    Message = response.Message,
                    BonusGatewayStatus = (int?)response.BonusGatewayStatus
                };

                Uow.OrderPaymentResponse2Repository.Add(response2);
            }

            Uow.Save();
        }

        #region Nested type: ResponseInfo

        private class ResponseInfo
        {
            public int OrderId { get; set; }

            public int? BankStatus { get; set; }

            public bool IsProcessed { get; set; }

            public OrderForPayment Request { get; set; }

            public OrderPaymentStatuses ItemsPaymentStatus { get; set; }

            public OrderDeliveryPaymentStatus DeliveryPaymentStatus { get; set; }

            public string Message { get; set; }

            public BonusGatewayConfirmStatus? BonusGatewayStatus { get; set; }
        }

        #endregion
    }
}