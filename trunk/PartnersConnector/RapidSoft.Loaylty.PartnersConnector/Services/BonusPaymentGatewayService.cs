namespace RapidSoft.Loaylty.PartnersConnector.Services
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    using ProductCatalog.WsClients.OrderManagementService;

    using RapidSoft.Extensions;

    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.Logging.Interaction;
    using RapidSoft.Loaylty.PartnersConnector.Common.DTO.Online;
    using RapidSoft.Loaylty.PartnersConnector.Interfaces;
    using RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities;
    using RapidSoft.Loaylty.PartnersConnector.Services.CryptoServices;
    using RapidSoft.Loaylty.PartnersConnector.Services.Providers;

    using VTB24.Site.PublicProfileWebApi;

    using OrderStatuses = ProductCatalog.WsClients.OrderManagementService.OrderStatuses;

    /// <summary>
    /// Сервис-обработчик запросов от online партнеров
    /// </summary>
    public class BonusPaymentGatewayService
    {
        /// <summary>
        /// Статус заказа созданного в системе партнера, 
        /// при получении уведомления (<see cref="NotifyOrder"/>) со статусом 1 
        /// необходимо создать заказа в компонента "Каталог порадков (ворзнаграждений)". 
        /// </summary>
        private const sbyte OrderCreated = 1;

        private static readonly sbyte[] AllowedOrderStatuses = new sbyte[]
        {
            OrderCreated,
            (sbyte)OrderStatuses.CancelledByPartner,
            (sbyte)OrderStatuses.DeliveryWaiting,
            (sbyte)OrderStatuses.Delivery,
            (sbyte)OrderStatuses.Delivered,
            (sbyte)OrderStatuses.DeliveredWithDelay,
            (sbyte)OrderStatuses.NotDelivered
        };

        /// <summary>
        /// Lazy обертка провайдера компонента "SecurityWebServices"
        /// </summary>
        private readonly Lazy<ISecurityWebServicesProvider> securityWebServicesProvider;

        /// <summary>
        /// Lazy обертка провайдера компонента "Каталог порадков (вознаграждений)".
        /// </summary>
        private readonly Lazy<IProductCatalogProvider> catalogProvider;

        private readonly Lazy<IBonusGatewayProvider> bonusGatewayProvider;

        /// <summary>
        /// Фабрика криптосервисов.
        /// </summary>
        private readonly ICryptoServiceFactory cryptoServiceFactory;

        private static readonly ILog log = LogManager.GetLogger(typeof (BonusPaymentGatewayService));
        
        /// <summary>
        /// Initializes a new instance of the <see cref="BonusPaymentGatewayService"/> class.
        /// </summary>
        /// <param name="cryptoServiceFactory">
        /// Фабрика криптосервисов.
        /// </param>
        /// <param name="catalogProvider">
        /// Провайдер компонента "Каталог порадков (вознаграждений)".
        /// </param>
        /// <param name="securityWebServicesProvider">
        /// Провайдера публичного API профиля.
        /// </param>
        /// <param name="bonusGatewayProvider">
        /// Провайдер БПШ
        /// </param>
        public BonusPaymentGatewayService(
            ICryptoServiceFactory cryptoServiceFactory = null, 
            IProductCatalogProvider catalogProvider = null,
            ISecurityWebServicesProvider securityWebServicesProvider = null,
            IBonusGatewayProvider bonusGatewayProvider = null)
        {
            this.bonusGatewayProvider =
                new Lazy<IBonusGatewayProvider>(() => bonusGatewayProvider ?? new BonusGatewayProvider());
            this.securityWebServicesProvider =
                new Lazy<ISecurityWebServicesProvider>(
                    () => securityWebServicesProvider ?? new SecurityWebServicesProvider());
            this.catalogProvider =
                new Lazy<IProductCatalogProvider>(() => catalogProvider ?? new ProductCatalogProvider());
            this.cryptoServiceFactory = cryptoServiceFactory ?? new CryptoServiceFactory();
        }

        public string ValidateUser(string shopId, string userTicket, string signature, IInteractionLogEntry logEntry)
        {
            log.InfoFormat("Получение данных пользователя: shopId={0}, userTicket={1}, signature={2}.", shopId, userTicket, signature);
            var bankCryptoService = this.cryptoServiceFactory.GetBankCryptoService();

            try
            {
                CheckValidateUserParams(userTicket, shopId, signature);

                var partnerId = ConvertShopId(shopId, ResultCodes.InvalidUserTicket);

                logEntry.Info["PartnerId"] = partnerId;

                var parnterCryptoService = this.cryptoServiceFactory.GetParnterCryptoService(partnerId);

                // NOTE: Проверить подпись
                var checkValue = string.Concat(shopId, userTicket);
                var checkResult = parnterCryptoService.VerifyData(signature, checkValue);
                if (!checkResult)
                {
                    logEntry.NotSucceeded();
                    var mess = string.Format("Подпись не соответствует данным");
                    log.Error(mess);
                    var resultInvalidSign = ValidateUserResult.BuildFail(ResultCodes.InvalidSignature);
                    return resultInvalidSign.SerializeWithSing(bankCryptoService);
                }

                // NOTE: Получаем данные
                var profileResult = this.securityWebServicesProvider.Value.GetUserProfile(shopId, userTicket);

                // NOTE: Формируем ответ
                var retVal = BuildValidateUserResult(profileResult, parnterCryptoService);

                if (retVal.Status == 0)
                {
                    logEntry.Succeeded();
                }
                else
                {
                    logEntry.NotSucceeded();
                }

                return retVal.SerializeWithSing(bankCryptoService);
            }
            catch (OperationException ex)
            {
                logEntry.NotSucceeded();
                var mess = string.Format("Ошибка получения данных пользователя: {0}", ex.Message);
                log.Error(mess, ex);
                var resultInvalidSign = ValidateUserResult.BuildFail(ex.ResultCode);
                return resultInvalidSign.SerializeWithSing(bankCryptoService);
            }
            catch (Exception ex)
            {
                logEntry.NotSucceeded();
                var mess = string.Format("Ошибка получения данных пользователя: {0}", ex.Message);
                log.Error(mess, ex);
                var resultInvalidSign = ValidateUserResult.BuildFail(ResultCodes.InvalidUserTicket);
                return resultInvalidSign.SerializeWithSing(bankCryptoService);
            }
        }

        public string NotifyOrder(string notifyOrderXml, IInteractionLogEntry logEntry)
        {
            log.InfoFormat("Уведомление о статусе заказа: xml={0}.", notifyOrderXml);
            var bankCryptoService = this.cryptoServiceFactory.GetBankCryptoService();
            var orderId = string.Empty;
            try
            {
                var notifyOrder = Common.DTO.Online.NotifyOrder.DeserializeNotifyOrder(notifyOrderXml);

                CheckNotifyOrder(notifyOrder);

                orderId = notifyOrder.OrderId;

                var partnerId = ConvertShopId(notifyOrder.ShopId, ResultCodes.InvalidOrderUpdateStatus);

                logEntry.Info["PartnerId"] = partnerId;

                // NOTE: Проверяем подпись
                var sing = notifyOrder.Signature;
                log.InfoFormat("Уведомление о статусе заказа: signature={0}.", sing);

                var withoutSing = RemoveSing(notifyOrderXml);
                log.InfoFormat("Уведомление о статусе заказа: withoutSign={0}.", withoutSing);

                var cryptoService = this.cryptoServiceFactory.GetParnterCryptoService(partnerId);
                var verifyResult = cryptoService.VerifyData(sing, withoutSing);
                if (!verifyResult)
                {
                    logEntry.NotSucceeded();
                    const string Mess = "Подпись не соответствует запросу";
                    var resultInvalidOrder = NotifyOrderResult.BuildFail(ResultCodes.InvalidSignature, Mess, orderId);
                    return resultInvalidOrder.SerializeWithSing(bankCryptoService);
                }

                // NOTE: Обновляем статус
                var result = notifyOrder.OrderStatus == OrderCreated
                                 ? this.CreateOrder(partnerId, notifyOrder)
                                 : this.UpdateOrderStatus(partnerId, notifyOrder);

                if (result.Status == 0)
                {
                    logEntry.Succeeded();
                }
                else
                {
                    logEntry.NotSucceeded();
                }

                return result.SerializeWithSing(bankCryptoService);
            }
            catch (OperationException ex)
            {
                logEntry.NotSucceeded();
                var mess = string.Format("Ошибка обновления статуса заказа: {0}", ex.Message);
                log.Error(mess, ex);
                var resultInvalidSign = NotifyOrderResult.BuildFail(ex, orderId);
                return resultInvalidSign.SerializeWithSing(bankCryptoService);
            }
            catch (Exception ex)
            {
                logEntry.NotSucceeded();
                //REVIEW: г**нокод: привязывает все исключения к статусу ResultCodes.InvalidOrderUpdateStatus
                var mess = string.Format("Ошибка обновления статуса заказа: {0}", ex.Message);
                log.Error(mess, ex);
                var resultInvalidSign = NotifyOrderResult.BuildFail(ResultCodes.InvalidOrderUpdateStatus, mess, orderId);
                return resultInvalidSign.SerializeWithSing(bankCryptoService);
            }
        }

        public string GetPaymentStatus(string shopId, string orderId, string signature, IInteractionLogEntry logEntry)
        {
            log.InfoFormat("Получение статуса списания по заказу: shopId={0},  orderId={1}, signature={2}.", shopId, orderId, signature);
            var bankCryptoService = this.cryptoServiceFactory.GetBankCryptoService();

            try
            {
                CheckGetPaymentStatusParams(shopId, orderId, signature);

                var partnerId = ConvertShopId(shopId, ResultCodes.InvalidUserTicket);

                logEntry.Info["PartnerId"] = partnerId;

                var parnterCryptoService = this.cryptoServiceFactory.GetParnterCryptoService(partnerId);

                // NOTE: Проверить подпись
                var checkValue = string.Concat(shopId, orderId);
                var checkResult = parnterCryptoService.VerifyData(signature, checkValue);
                if (!checkResult)
                {
                    logEntry.NotSucceeded();
                    var mess = string.Format("Подпись не соответствует данным");
                    log.Error(mess);
                    var resultInvalidSign = new GetPaymentStatusResult(ResultCodes.InvalidSignature, orderId);
                    return resultInvalidSign.SerializeWithSing(bankCryptoService);
                }

                var orderResult = this.catalogProvider.Value.GetOrderByExternalId(new GetOrderByExternalIdParameters
                {
                    ExternalOrderId = orderId,
                    PartnerId = int.Parse(shopId)
                });

                // NOTE: Формируем ответ
                GetPaymentStatusResult retVal;
                if (!orderResult.Success)
                {
                    retVal = new GetPaymentStatusResult(ResultCodes.OrderNotFound, orderId);
                }
                else if (orderResult.Order.Status < OrderStatuses.Processing ||
                         orderResult.Order.Status == OrderStatuses.CancelledByPartner)
                {
                    retVal = new GetPaymentStatusResult(ResultCodes.OrderIsNotPaidOrCancelled, orderId);
                }
                else
                {
                    retVal = new GetPaymentStatusResult(ResultCodes.Success, orderId);
                }

                if (retVal.Status == 0)
                {
                    logEntry.Succeeded();
                }
                else
                {
                    logEntry.NotSucceeded();
                }

                return retVal.SerializeWithSing(bankCryptoService);
            }
            catch (Exception ex)
            {
                logEntry.NotSucceeded();
                var mess = string.Format("Ошибка получения заказа онлайн партнера: {0}", ex.Message);
                log.Error(mess, ex);
                var resultInvalidSign = new GetPaymentStatusResult(ResultCodes.UnknownGetPaymentStatusError, orderId);
                return resultInvalidSign.SerializeWithSing(bankCryptoService);
            }
        }

        private void CheckNotifyOrder(NotifyOrder notifyOrder)
        {
            CheckArg(string.IsNullOrEmpty(notifyOrder.UserTicket), "UserTicket is required");
            CheckArg(string.IsNullOrEmpty(notifyOrder.ShopId), "ShopId is required");
            CheckArg(string.IsNullOrEmpty(notifyOrder.OrderId), "OrderId is required");
            CheckArg(notifyOrder.TotalCost == 0, "TotalCost is 0");
            CheckArg(!AllowedOrderStatuses.Contains(notifyOrder.OrderStatus), "OrderStatus is invalid");
            CheckArg(notifyOrder.DateTime == DateTime.MinValue, "DateTime is required");
            CheckArg(notifyOrder.UtcDateTime == DateTime.MinValue, "UtcDateTime is required");
        }

        private static void CheckArg(bool predicate, string message)
        {
            if (predicate)
            {
                log.Error(string.Format(message));
                throw new OperationException((int) ResultCodes.InvalidArgument, string.Format(message));
            }
        }

        private static string RemoveSing(string notifyOrderXml)
        {
            var regex = new Regex(@"Signature="".*?""");
            var withoutSing = regex.Replace(notifyOrderXml, "Signature=\"\"");
            return withoutSing;
        }

        private static int ConvertShopId(string shopId, ResultCodes errorCode)
        {
            int retVal;
            try
            {
                retVal = Convert.ToInt32(shopId);
            }
            catch (Exception ex)
            {
                var mess = "Идентификатор магазина (ShopId) должен быть числом, но при конвертации возникла ошибка: "
                           + ex.Message;
                log.Error(mess);
                throw new OperationException((int)errorCode, mess);
            }

            return retVal;
        }

        private static decimal ConvertDiscountRefund(string discountRefund, ResultCodes errorCode)
        {
            if (string.IsNullOrWhiteSpace(discountRefund))
            {
                return 0m;
            }

            decimal refund;
            try
            {
                refund = Convert.ToDecimal(discountRefund, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                var mess =
                    "Сумма в рублях (ShopId) должен быть числом c разделителем дробной части – точка, но при конвертации возникла ошибка: "
                    + ex.Message;
                log.Error(mess);
                throw new OperationException((int)errorCode, mess);
            }

            return refund;
        }

        private static void CheckValidateUserParams(string userTicket, string shopId, string signature)
        {
            if (shopId.IsNullOrWhiteSpace() || shopId.Length > 50)
            {
                var mess = string.Format(
                    "Не допустимое значение идентификатора партнера (ShopId): {0}", shopId ?? "null");
                log.Error(mess);
                throw new OperationException((int)ResultCodes.InvalidUserTicket, mess);
            }

            if (userTicket.IsNullOrWhiteSpace() || userTicket.Length > 500)
            {
                var mess = string.Format(
                    "Не допустимое значение временного кода пользователя (UserTicket): {0}", userTicket ?? "null");
                log.Error(mess);
                throw new OperationException((int)ResultCodes.InvalidUserTicket, mess);
            }

            if (signature.IsNullOrWhiteSpace() || signature.Length > 500)
            {
                var mess = string.Format(
                    "Не допустимое значение подписи запроса (Signature): {0}", signature ?? "null");
                log.Error(mess);
                throw new OperationException((int)ResultCodes.InvalidSignature, mess);
            }
        }

        private static void CheckCancelPaymentParams(string shopId, string orderId, string discountRefund, string signature)
        {
            if (shopId.IsNullOrWhiteSpace() || shopId.Length > 50)
            {
                var mess = string.Format(
                    "Не допустимое значение идентификатора партнера (ShopId): {0}", shopId ?? "null");
                log.Error(mess);
                throw new OperationException((int)ResultCodes.UnknowCancelPaymentError, mess);
            }

            if (orderId.IsNullOrWhiteSpace() || orderId.Length > 50)
            {
                var mess = string.Format(
                    "Не допустимое значение идентификатора заказа (OrderId): {0}", orderId ?? "null");
                log.Error(mess);
                throw new OperationException((int)ResultCodes.UnknowCancelPaymentError, mess);
            }

            // NOTE: Максимум 10 знаков в целой части, 2 знака в дробной части, разделитель дробной части – точка
            if (!discountRefund.IsNullOrWhiteSpace() && discountRefund.Length > 10 + 1 + 2)
            {
                var mess = string.Format(
                    "Не допустимое значение cуммы в рублях (DiscountRefund): {0}", discountRefund);
                log.Error(mess);
                throw new OperationException((int)ResultCodes.UnknowCancelPaymentError, mess);
            }

            if (signature.IsNullOrWhiteSpace() || signature.Length > 500)
            {
                var mess = string.Format(
                    "Не допустимое значение подписи запроса (Signature): {0}", signature ?? "null");
                log.Error(mess);
                throw new OperationException((int)ResultCodes.InvalidSignature, mess);
            }
        }


        private static void CheckGetPaymentStatusParams(string shopId, string orderId, string signature)
        {
            // TODO: избавится от всей постыдной кописасты, которую тут развели нехорошие люди!
            if (shopId.IsNullOrWhiteSpace() || shopId.Length > 50)
            {
                var mess = string.Format(
                    "Не допустимое значение идентификатора партнера (ShopId): {0}", shopId ?? "null");
                log.Error(mess);
                throw new OperationException((int)ResultCodes.UnknowCancelPaymentError, mess);
            }

            if (orderId.IsNullOrWhiteSpace() || orderId.Length > 50)
            {
                var mess = string.Format(
                    "Не допустимое значение идентификатора заказа (OrderId): {0}", orderId ?? "null");
                log.Error(mess);
                throw new OperationException((int)ResultCodes.UnknowCancelPaymentError, mess);
            }

            if (signature.IsNullOrWhiteSpace() || signature.Length > 500)
            {
                var mess = string.Format(
                    "Не допустимое значение подписи запроса (Signature): {0}", signature ?? "null");
                log.Error(mess);
                throw new OperationException((int)ResultCodes.InvalidSignature, mess);
            }
        }

        private static ValidateUserResult BuildValidateUserResult(
            GetPublicProfileResult profileResult, ICryptoService parnterCryptoService)
        {
            var retVal = new ValidateUserResult
                             {
                                 Status = (sbyte)profileResult.Status,
                                 Email = parnterCryptoService.Encrypt(profileResult.Email),
                                 LastName = parnterCryptoService.Encrypt(profileResult.LastName),
                                 FirstName = parnterCryptoService.Encrypt(profileResult.FirstName),
                                 MiddleName = parnterCryptoService.Encrypt(profileResult.MiddleName),
                                 NameLang = profileResult.NameLang,
                                 City = profileResult.City,
                                 Balance = profileResult.Balance,
                                 BalanceSpecified = true,
                                 Rate = profileResult.Rate,
                                 Delta = profileResult.Delta,
                                 UtcDateTime = profileResult.UtcDateTime,
                                 Signature = string.Empty
                             };
            return retVal;
        }

        private NotifyOrderResult UpdateOrderStatus(int partnerId, NotifyOrder notifyOrder)
        {
            var clientId = this.GetClientId(notifyOrder);

            var message = new ExternalOrdersStatus
                              {
                                  OrderId = null,
                                  ExternalOrderId = notifyOrder.OrderId,
                                  PartnerId = partnerId,
                                  ClientId = clientId,
                                  OrderStatus = (OrderStatuses)notifyOrder.OrderStatus,
                                  OrderStatusDescription = notifyOrder.Description,
                                  ExternalOrderStatusCode = null,
                                  ExternalOrderStatusDateTime = notifyOrder.DateTime
                              };

            var updateStatusResult = this.catalogProvider.Value.ChangeOrdersStatuses(new[] { message });

            if (!updateStatusResult.Success)
            {
                var mess = string.IsNullOrEmpty(updateStatusResult.ResultDescription)
                               ? string.Empty
                               : updateStatusResult.ResultDescription.Substring(0, 255);
                return NotifyOrderResult.BuildFail(ResultCodes.InvalidOrderUpdateStatus, mess, notifyOrder.OrderId);
            }

            var orderResult =
                updateStatusResult.ChangeExternalOrderStatusResults.FirstOrDefault(
                    x => x.ExternalOrderId == notifyOrder.OrderId);

            if (orderResult == null)
            {
                const string Mess = "Не получен ответ от каталога подарков по заказу";
                return NotifyOrderResult.BuildFail(ResultCodes.InvalidOrderUpdateStatus, Mess, notifyOrder.OrderId);
            }

            if (!orderResult.Success)
            {
                if (orderResult.ResultCode == 55 &&
                    orderResult.OriginalStatus.HasValue &&
                    (int)orderResult.OriginalStatus.Value < 10)
                {

                    return NotifyOrderResult.BuildFail(ResultCodes.InvalidOrderUpdateStatus, "Клиент не подтвердил заказ, поэтому он не может быть переведен в другой статус", notifyOrder.OrderId);
                }

                var mess = string.IsNullOrEmpty(updateStatusResult.ResultDescription)
                               ? string.Empty
                               : updateStatusResult.ResultDescription.Substring(0, 255);
                return NotifyOrderResult.BuildFail(ResultCodes.InvalidOrderUpdateStatus, mess, notifyOrder.OrderId);
            }

            return NotifyOrderResult.BuildSuccess(notifyOrder.OrderId);
        }

        private NotifyOrderResult CreateOrder(int partnerId, NotifyOrder notifyOrder)
        {
            if (notifyOrder.Item.Any(x => x.Price <= 0 || x.BonusPrice <= 0))
            {
                const string Mess = "Один из элементов заказа содержит цену в рублях или балах меньше или равную 0, создание заказа невозможно.";
                var resultInvalidOrder = NotifyOrderResult.BuildFail(ResultCodes.InvalidOrderItemPrice, Mess, notifyOrder.OrderId);
                return resultInvalidOrder;
            }

            var clientId = this.GetClientId(notifyOrder);

            var items = notifyOrder.Item.Select(x => x.ConvertToCreateOrderFromOnlinePartnerItem()).ToArray();
            var parameter = new CreateOrderFromOnlinePartnerParameters
                                {
                                    ClientId = clientId,
                                    DateTime = notifyOrder.DateTime,
                                    Description = notifyOrder.Description,
                                    ExternalOrderId = notifyOrder.OrderId,
                                    PartnerId = partnerId,
                                    TotalCost = notifyOrder.TotalCost,
                                    UtcDateTime = notifyOrder.UtcDateTime,
                                    ExternalStatus =
                                        notifyOrder.OrderStatus.ToString(
                                            CultureInfo.InvariantCulture),
                                    Items = items
                                };

            var orderResult = this.catalogProvider.Value.CreateOrderForOnlinePartner(parameter);

            NotifyOrderResult retVal;
            if (orderResult.Success)
            {
                retVal = NotifyOrderResult.BuildSuccess(orderResult.Order.ExternalOrderId);
            }
            else
            {
                retVal = NotifyOrderResult.BuildFail(
                    ResultCodes.InvalidOrderUpdateStatus, orderResult.ResultDescription, notifyOrder.OrderId);
            }

            return retVal;
        }

        private string GetClientId(NotifyOrder notifyOrder)
        {
            log.Info("Определение идентификатора клиента");
            if (string.IsNullOrWhiteSpace(notifyOrder.UserTicket))
            {
                const string Mess = "Запрос не содержит пользовательский токен, выполнение операции невозможно.";
                throw new OperationException((int)ResultCodes.InvalidOrderUpdateStatus, Mess);
            }

            var securityTokenResult = this.securityWebServicesProvider.Value.GetClientId(notifyOrder.UserTicket);

            if (securityTokenResult == null || string.IsNullOrWhiteSpace(securityTokenResult.PrincipalId))
            {
                const string Mess =
                    "Не удалось получить идентификатор клиента по пользовательскому токену, выполнение операции невозможно.";
                throw new OperationException((int)ResultCodes.InvalidOrderUpdateStatus, Mess);
            }

            var clientId = securityTokenResult.PrincipalId;
            log.Info("Идентификатор клиента: " + clientId);
            return clientId;
        }
    }
}