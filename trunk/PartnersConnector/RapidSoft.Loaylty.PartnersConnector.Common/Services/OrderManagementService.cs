namespace RapidSoft.Loaylty.PartnersConnector.Common.Services
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using AutoMapper;

    using DTO;
    using DTO.CheckOrder;
    using DTO.CommitOrder;
    using DTO.GetDeliveryVariants;

    using PartnersConnector.Services;

    using ProductCatalog.WsClients.CatalogAdminService;

    using RapidSoft.Loaylty.Monitoring;
    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.Logging.Interaction;
    using RapidSoft.Loaylty.Logging.Wcf;
    using RapidSoft.Loaylty.PartnersConnector.Interfaces;
    using RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities;
    using RapidSoft.Loaylty.PartnersConnector.Litres;
    using RapidSoft.Loaylty.PartnersConnector.Queue.Entities;
    using RapidSoft.Loaylty.PartnersConnector.Queue.Repository;
    using RapidSoft.Loaylty.PartnersConnector.Services.Providers;
    using RapidSoft.Loaylty.ProductCatalog.WsClients.OrderManagementService;
    using RapidSoft.Loyalty.Security;

    using ChangeExternalOrderStatusResult = ProductCatalog.WsClients.OrderManagementService.ChangeExternalOrderStatusResult;
    using CheckOrderResult = PartnersConnector.Interfaces.Entities.CheckOrderResult;
    using CommitOrderResult = PartnersConnector.Interfaces.Entities.CommitOrderResult;
    using DeliveryGroup = DTO.GetDeliveryVariants.DeliveryGroup;
    using DeliveryInfo = PartnersConnector.Interfaces.Entities.DeliveryInfo;
    using DeliveryVariant = DTO.GetDeliveryVariants.DeliveryVariant;
    using GetDeliveryVariantsResult = PartnersConnector.Interfaces.Entities.GetDeliveryVariantsResult;
    using IOrderManagementService = PartnersConnector.Interfaces.IOrderManagementService;
    using PickupPoint = DTO.GetDeliveryVariants.PickupPoint;
    using VariantsLocation = DTO.GetDeliveryVariants.VariantsLocation;

    /// <summary>
    /// Реализация сервиса для взаимодействия с поставщиком подарков(партнером).
    /// </summary>
    [LoggingBehavior]
    [Serializable]
    public class OrderManagementService : SupportService, IOrderManagementService
    {
        public const string RusCountryCode = "RU";
        public const string RusCountryName = "Россия";
        private const string InternalExceptionMessage = "Ошибка отправки заказа партнеру";

        #region Fields

        private readonly IProductCatalogProvider catalogProvider;
        private readonly ITextMessageDispatcher messageDispatcher;
        private readonly ICommitOrderQueueItemRepository queueItemRepository;
        private readonly ICatalogAdminServiceProvider catalogAdminServiceProvider;

        private readonly ILog log = LogManager.GetLogger(typeof(OrderManagementService));

        private static readonly Dictionary<string, Action<PartnersConnector.Interfaces.Entities.Order>> CustomCommitMethods;

        #endregion

        #region Constructors

        static OrderManagementService()
        {
            // check order
            Mapper.CreateMap<PartnersConnector.Interfaces.Entities.Order, DTO.CheckOrder.Order>()
                  .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id.ToString(CultureInfo.InvariantCulture)));

            Mapper.CreateMap<PartnersConnector.Interfaces.Entities.DeliveryInfo, DTO.CheckOrder.DeliveryInfo>()
                  .ForMember(dest => dest.ExternalLocationId, opt => opt.MapFrom(src => src.DeliveryVariantsLocation.ExternalLocationId))
                  .ForMember(dest => dest.Address, opt => opt.MapFrom(src => GetAddressText(src)))
                  .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => RusCountryCode))
                  .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => RusCountryName))
                  .ForMember(dest => dest.PostCode, opt => opt.MapFrom(src => GetPostCode(src)))
                  .ForMember(dest => dest.ExternalDeliveryVariantId, opt => opt.MapFrom(src => src.ExternalDeliveryVariantId))
                  .ForMember(dest => dest.ExternalPickupPointId, opt => opt.MapFrom(src => src.PickupPoint.ExternalPickupPointId));

            Mapper.CreateMap<PartnersConnector.Interfaces.Entities.OrderItem, DTO.CheckOrder.OrderItem>();

            Mapper.CreateMap<DTO.CheckOrder.CheckOrderResult, CheckOrderResult>();

            // commit order            
            Mapper.CreateMap<PartnersConnector.Interfaces.Entities.Order, DTO.CommitOrder.Order>()
                  .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id.ToString(CultureInfo.InvariantCulture)));
            
            Mapper.CreateMap<PartnersConnector.Interfaces.Entities.Contact, DTO.CommitOrder.Contact>()
                  .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone));

            Mapper.CreateMap<PartnersConnector.Interfaces.Entities.DeliveryInfo, DTO.CommitOrder.DeliveryInfo>()
                  .ForMember(dest => dest.ExternalLocationId, opt => opt.MapFrom(src => src.DeliveryVariantsLocation.ExternalLocationId))
                  .ForMember(dest => dest.Address, opt => opt.MapFrom(src => GetAddressText(src)))
                  .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => RusCountryCode))
                  .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => RusCountryName))
                  .ForMember(dest => dest.PostCode, opt => opt.MapFrom(src => GetPostCode(src)))
                  .ForMember(dest => dest.ExternalDeliveryVariantId, opt => opt.MapFrom(src => src.ExternalDeliveryVariantId))
                  .ForMember(dest => dest.ExternalPickupPointId, opt => opt.MapFrom(src => src.PickupPoint.ExternalPickupPointId))
                  .ForMember(dest => dest.Contacts, opt => opt.MapFrom(src => new[] { src.Contact }));

            Mapper.CreateMap<PartnersConnector.Interfaces.Entities.OrderItem, DTO.CommitOrder.OrderItem>();

            Mapper.CreateMap<DTO.CommitOrder.CommitOrderResult, PartnerOrderCommitment>()
              .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => int.Parse(src.OrderId)))
              .ForMember(dest => dest.ExternalOrderId, opt => opt.MapFrom(src => src.InternalOrderId));

            // UpdateOrdersStatuses
            Mapper.CreateMap<NotifyOrderMessage, ExternalOrdersStatus>()
                  .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                  .ForMember(dest => dest.ExternalOrderId, opt => opt.MapFrom(src => src.PartnerOrderId))
                  .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.Code))
                  .ForMember(dest => dest.OrderStatusDescription, opt => opt.MapFrom(src => src.StatusReason))
                  .ForMember(dest => dest.ExternalOrderStatusCode, opt => opt.MapFrom(src => src.PartnerCode))
                  .ForMember(dest => dest.ExternalOrderStatusDateTime, opt => opt.MapFrom(src => src.StatusDateTime))
                  .ForMember(dest => dest.PartnerId, opt => opt.MapFrom(src => src.PartnerId))
                  .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.ClientId));

            Mapper.CreateMap<ChangeExternalOrdersStatusesResult, UpdateOrdersStatusResult>()
                .ForMember(dest => dest.UpdateOrderStatusResults, opt => opt.MapFrom(src => src.ChangeExternalOrderStatusResults));
            Mapper.CreateMap<ChangeExternalOrderStatusResult, UpdateOrderStatusResult>();

            // FixBasketItemPrice
            Mapper.CreateMap<FixBasketItemPriceParam, FixPriceMessage>();

            // GetDeliveryVariants
            Mapper.CreateMap<GetDeliveryVariantsParam, GetDeliveryVariantsMessage>();

            Mapper.CreateMap<DTO.GetDeliveryVariants.GetDeliveryVariantsResult, GetDeliveryVariantsResult>()
                .ForMember(dest => dest.DeliveryGroups, opt => opt.MapFrom(src => MapArrayFromMessage(src.DeliveryGroups)))
                .ForMember(dest => dest.ResultDescription, opt => opt.MapFrom(src => src.Reason));
            Mapper.CreateMap<VariantsLocation, PartnersConnector.Interfaces.Entities.VariantsLocation>();
            Mapper.CreateMap<DeliveryGroup, PartnersConnector.Interfaces.Entities.DeliveryGroup>()
                .ForMember(dest => dest.DeliveryVariants, opt => opt.MapFrom(src => MapArrayFromMessage(src.DeliveryVariants)));
            Mapper.CreateMap<DeliveryVariant, PartnersConnector.Interfaces.Entities.DeliveryVariant>()
                .ForMember(dest => dest.PickupPoints, opt => opt.MapFrom(src => MapArrayFromMessage(src.PickupPoints)));
            Mapper.CreateMap<PickupPoint, PartnersConnector.Interfaces.Entities.PickupPoint>();
            Mapper.CreateMap<PartnersConnector.Interfaces.Entities.OrderItem, DTO.GetDeliveryVariants.OrderItem>();

            CustomCommitMethods = new Dictionary<string, Action<PartnersConnector.Interfaces.Entities.Order>>()
            {
                { "Litres", order => new LitresOrderManagement().CommitOrder(order) }
            };
        }

        public OrderManagementService() : this(null)
        {
            
        }

        public OrderManagementService(
            IProductCatalogProvider catalogProvider = null,
            ITextMessageDispatcher messageDispatcher = null,
            ICommitOrderQueueItemRepository queueItemRepository = null,
            ICatalogAdminServiceProvider catalogAdminServiceProvider = null)
        {
            this.catalogProvider = catalogProvider ?? new ProductCatalogProvider();
            this.queueItemRepository = queueItemRepository ?? new CommitOrderQueueItemRepository();
            this.catalogAdminServiceProvider = catalogAdminServiceProvider ?? new CatalogAdminServiceProvider();
            this.messageDispatcher = messageDispatcher;
        }

        private static string GetAddressText(DeliveryInfo src)
        {
            return src.Maybe(d => d.Address).Maybe(a => a.AddressText) ?? src.Maybe(d => d.PickupPoint).Maybe(a => a.Address.StripTags());
        }

        private static string GetPostCode(DeliveryInfo src)
        {
            if (src == null)
            {
                return null;
            }

            if (src.Address != null)
            {
                return src.Address.PostCode;
            }

            if (src.DeliveryVariantsLocation != null)
            {
                return src.DeliveryVariantsLocation.PostCode;
            }

            return null;
        }

        #endregion

        #region IOrderManagementService Members

        public GetDeliveryVariantsResult GetDeliveryVariants(GetDeliveryVariantsParam param)
        {
            try
            {
                param.ThrowIfNull("param");
                param.Items.ThrowIfNull("param.Items");

                var partnerId = param.PartnerId;

                if (partnerId < 0)
                {
                    return BuildFail<GetDeliveryVariantsResult>(ResultCodes.InvalidPartnerConfig, "param.PartnerId должен быть больше 0");
                }

                log.Info("Идентификатор партнера: " + partnerId);

                var partnerSettings = this.catalogAdminServiceProvider.GetPartnerSettings(partnerId);
               
                if (partnerSettings == null)
                {
                    var error = string.Format("Для партнера с идентификатором {0} не заданы настройки подключения", partnerId);
                    log.Error(error);
                    return BuildFail<GetDeliveryVariantsResult>(ResultCodes.InvalidPartnerConfig, error);
                }

                var dto = Mapper.Map<GetDeliveryVariantsParam, GetDeliveryVariantsMessage>(param);

                var data = dto.Serialize(Encoding.UTF8);

                var url = partnerSettings.GetDeliveryVariants;

                DTO.GetDeliveryVariants.GetDeliveryVariantsResult rawResult;

                var logEntry = StartInteraction.With("Partner").For("GetDeliveryVariants");
                logEntry.Info["PartnerId"] = partnerId;
                logEntry.Info["Request"] = data;

                try
                {
                    log.InfoFormat("Отправка запроса {0} в систему партнера, body: {1}", url, data);
                    var uri = new Uri(url);

                    var response = GetMessageDispatcher().Send(uri, data);

                    logEntry.Info["Response"] = response;

                    log.InfoFormat("Полученный ответ: {0}", response);

                    rawResult = response.Deserialize<DTO.GetDeliveryVariants.GetDeliveryVariantsResult>(Encoding.UTF8);

                    if (rawResult == null)
                    {
                        var error = string.Format("Ответ партнера с идентификатором {0} не содержит данные: {1}", partnerId, response);

                        logEntry.Failed("ответ не содержит данные", null);

                        return BuildFail<GetDeliveryVariantsResult>(ResultCodes.InvalidPartnerResponse, error);
                    }

                    if (rawResult.ResultCode == (int)ResultCodes.Success)
                    {
                        MessageValidator.Validate(rawResult);
                        logEntry.Succeeded();
                    }
                    else
                    {
                        logEntry.NotSucceeded();
                    }
                }
                catch (Exception e)
                {
                    logEntry.Failed("ошибка взаимодействия", e);
                    throw;
                }
                finally
                {
                    logEntry.FinishAndWrite(log);
                }

                var res = Mapper.Map<DTO.GetDeliveryVariants.GetDeliveryVariantsResult, GetDeliveryVariantsResult>(rawResult);
                return res;
            }
            catch (InvalidOperationException ex)
            {
                log.Error(InternalExceptionMessage, ex);
                return ServiceOperationResult.BuildErrorResult<GetDeliveryVariantsResult>(ex);
            }
            catch (Exception ex)
            {
                log.Error(InternalExceptionMessage, ex);
                return ServiceOperationResult.BuildErrorResult<GetDeliveryVariantsResult>(ex);
            }
        }

        private ITextMessageDispatcher GetMessageDispatcher()
        {
            return this.messageDispatcher ?? CreateMessageDispatcher();
        }

        private ITextMessageDispatcher CreateMessageDispatcher()
        {
            var thumbprint = ConfigurationManager.AppSettings["PartnersConnectorThumbprint"];
            var x509Certificate = new StoreCertificateProvider().GetByThumbprint(thumbprint);
            return new TextOverSslMessageDispatcher(x509Certificate);            
        }

        public FixBasketItemPriceResult FixBasketItemPrice(FixBasketItemPriceParam param)
        {
            string response = null;
            try
            {
                param.ThrowIfNull("param");
                if (param.PartnerId < 0)
                {
                    return new FixBasketItemPriceResult
                    {
                        ResultCode = (int)ResultCodes.InvalidPartnerConfig,
                        ResultDescription = "param.PartnerId должен быть больше 0"
                    };
                }

                log.Info("Идентификатор партнера: " + param.PartnerId);

                var partnerSettings = this.catalogAdminServiceProvider.GetPartnerSettings(param.PartnerId);

                if (partnerSettings == null)
                {
                    var error = string.Format("Для партнера с идентификатором {0} не заданы настройки подключения", param.PartnerId);
                    log.Error(error);
                    return new FixBasketItemPriceResult
                    {
                        ResultCode = (int)ResultCodes.InvalidPartnerConfig,
                        ResultDescription = error
                    };
                }

                var dto = Mapper.Map<FixBasketItemPriceParam, FixPriceMessage>(param);

                var data = dto.Serialize(Encoding.UTF8);

                var url = partnerSettings.FixBasketItemPrice;

                FixPriceResult fixResult;

                var logEntry = StartInteraction.With("Partner").For("FixPrice");
                logEntry.Info["PartnerId"] = param.PartnerId;
                logEntry.Info["Request"] = data;

                try
                {
                    log.InfoFormat("Отправка запроса {0} в систему партнера, body: {1}", url, data);

                    var uri = new Uri(url);

                    response = GetMessageDispatcher().Send(uri, data);

                    logEntry.Info["Response"] = response;

                    log.InfoFormat("Полученный ответ: {0}", response);

                    fixResult = response.Deserialize<FixPriceResult>(Encoding.UTF8);

                    if (fixResult == null)
                    {
                        var error = string.Format("Ответ партнера с идентификатором {0} не содержит данные: {1}",
                                                  param.PartnerId, response);
                        log.Error(error);

                        logEntry.Failed("ответ не содержит данные", null);

                        return new FixBasketItemPriceResult
                        {
                            ResultCode = (int) ResultCodes.InvalidPartnerResponse,
                            ResultDescription = error
                        };
                    }

                    MessageValidator.Validate(fixResult);

                    if (fixResult.Confirmed.HasValue && fixResult.Confirmed.Value == 1)
                    {
                        logEntry.Succeeded();
                    }
                    else
                    {
                        logEntry.NotSucceeded();
                    }
                }
                catch (Exception e)
                {
                    logEntry.Failed("ошибка взаимодействия", e);
                    throw;
                }
                finally
                {
                    logEntry.FinishAndWrite(log);
                }

                return new FixBasketItemPriceResult
                {
                    ResultCode = (int)ResultCodes.Success,
                    Confirmed = fixResult.Confirmed.Value,
                    ReasonCode = fixResult.ReasonCode,
                    Reason = fixResult.Reason,
                    ActualPrice = fixResult.ActualPrice
                };
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Source == "System.Xml")
                {
                    var error = string.Format("Ответ партнера с идентификатором {0} содержит не корректные данные: {1}", param.PartnerId, response);
                    log.Error(error, ex);
                    return new FixBasketItemPriceResult
                    {
                        ResultCode = (int)ResultCodes.InvalidPartnerResponse,
                        ResultDescription = error
                    };
                }

                log.Error(InternalExceptionMessage, ex);
                return ServiceOperationResult.BuildErrorResult<FixBasketItemPriceResult>(ex);
            }
            catch (Exception ex)
            {
                log.Error(InternalExceptionMessage, ex);
                return ServiceOperationResult.BuildErrorResult<FixBasketItemPriceResult>(ex);
            }
        }

        public CheckOrderResult CheckOrder(PartnersConnector.Interfaces.Entities.Order order)
        {
            string response = null;
            try
            {
                order.ThrowIfNull("order");

                log.Info("Идентификатор партнера: " + order.PartnerId);

                var partnerSettings = this.catalogAdminServiceProvider.GetPartnerSettings(order.PartnerId);

                if (partnerSettings == null)
                {
                    var error = string.Format("Для партнера с идентификатором {0} не заданы настройки подключения", order.PartnerId);
                    log.Error(error);
                    return CheckOrderResult.BuildFail(ResultCodes.InvalidPartnerConfig, error);
                }

                var dto = Mapper.Map<PartnersConnector.Interfaces.Entities.Order, DTO.CheckOrder.Order>(order);

                var message = new CheckOrderMessage { Order = dto };
                var data = message.Serialize(Encoding.UTF8);

                if (string.IsNullOrWhiteSpace(partnerSettings.OrderCheck))
                {
                    var error =
                        string.Format(
                            "Для партнера с идентификатором {0} не задан url проверки заказа, возможно партнер не поддерживает проверку.",
                            order.PartnerId);
                    log.Error(error);
                    return CheckOrderResult.BuildFail(ResultCodes.InvalidPartnerConfig, error);
                }

                var url = partnerSettings.OrderCheck;

                DTO.CheckOrder.CheckOrderResult checkResult;

                var logEntry = StartInteraction.With("Partner").For("CheckOrder");
                logEntry.Info["PartnerId"] = order.PartnerId;
                logEntry.Info["Request"] = data;

                try
                {
                    log.InfoFormat("Отправка запроса {0} в систему партнера, body: {1}", url, data);
                    var uri = new Uri(url);

                    response = GetMessageDispatcher().Send(uri, data);

                    logEntry.Info["Response"] = response;

                    log.InfoFormat("Полученный ответ: {0}", response);

                    checkResult = response.Deserialize<DTO.CheckOrder.CheckOrderResult>(Encoding.UTF8);

                    if (checkResult == null)
                    {
                        var error = string.Format("Ответ партнера с идентификатором {0} не содержит данные: {1}",
                                                  order.PartnerId, response);
                        log.Error(error);

                        logEntry.Failed("ответ не содержит данные", null);

                        return CheckOrderResult.BuildFail(ResultCodes.InvalidPartnerResponse, error);
                    }

                    if (checkResult.Checked.HasValue && checkResult.Checked.Value == 1)
                    {
                        logEntry.Succeeded();
                    }
                    else
                    {
                        logEntry.NotSucceeded();
                    }
                }
                catch (Exception e)
                {
                    logEntry.Failed("ошибка взаимодействия", e);
                    throw;
                }
                finally
                {
                    logEntry.FinishAndWrite(log);
                }

                return CheckOrderResult.BuildSuccess(checkResult.Checked.Value, checkResult.Reason, checkResult.ReasonCode);
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Source == "System.Xml")
                {
                    var error = string.Format("Ответ партнера с идентификатором {0} содержит не корректные данные: {1}", order.PartnerId, response);
                    log.Error(error, ex);
                    return CheckOrderResult.BuildFail(ResultCodes.InvalidPartnerResponse, error);
                }

                log.Error(InternalExceptionMessage, ex);
                return ServiceOperationResult.BuildErrorResult<CheckOrderResult>(ex);
            }
            catch (Exception ex)
            {
                log.Error(InternalExceptionMessage, ex);
                return ServiceOperationResult.BuildErrorResult<CheckOrderResult>(ex);
            }
        }

        public CommitOrderResult CommitOrder(PartnersConnector.Interfaces.Entities.Order order)
        {
            try
            {
                order.ThrowIfNull("order");

                log.Info("Идентификатор партнера: " + order.PartnerId);

                var connection = this.catalogAdminServiceProvider.GetPartnerSettings(order.PartnerId);

                if (connection == null)
                {
                    var error = string.Format("Для партнера с идентификатором {0} не заданы настройки подключения", order.PartnerId);
                    log.ErrorFormat(error);
                    return CommitOrderResult.BuildFail(ResultCodes.InvalidPartnerConfig, error);
                }

                if (connection.UseBatch)
                {
                    this.AddOrderToQueue(order.PartnerId, order);
                    return CommitOrderResult.BuildSuccess(ConfirmedStatuses.AddToQueue);
                }

                return this.InternalCommitOrder(order.PartnerId, connection.OrderConfirmation, order);
            }
            catch (Exception ex)
            {
                log.Error(InternalExceptionMessage, ex);
                return ServiceOperationResult.BuildErrorResult<CommitOrderResult>(ex);
            }
        }

        public PartnersConnector.Interfaces.Entities.ResultBase CustomCommitOrder(PartnersConnector.Interfaces.Entities.Order order, string methodName)
        {
            try
            {
                Action<PartnersConnector.Interfaces.Entities.Order> commitMethod;
                if (!CustomCommitMethods.TryGetValue(methodName, out commitMethod) || commitMethod == null)
                {
                    return new PartnersConnector.Interfaces.Entities.ResultBase
                    {
                        ResultCode = (int) ResultCodes.UnknownCustomCommitMethod
                    };
                }

                commitMethod(order);

                return new PartnersConnector.Interfaces.Entities.ResultBase
                {
                    ResultCode = (int) ResultCodes.Success
                };
            }
            catch (Exception ex)
            {
                log.Error("ошибка автоматического подтверждения заказа", ex);
                return new PartnersConnector.Interfaces.Entities.ResultBase
                {
                    ResultCode = (int) ResultCodes.CustomCommitFailed,
                    ResultDescription = "ошибка автоматического подтверждения заказа: " + ex.Message
                };
            }
        }

        public UpdateOrdersStatusResult UpdateOrdersStatuses(NotifyOrderMessage[] messages)
        {
            try
            {
                var ordersStatus = Mapper.Map<NotifyOrderMessage[], ExternalOrdersStatus[]>(messages);                

                var result = this.catalogProvider.ChangeOrdersStatuses(ordersStatus);

                var retVal = Mapper.Map<ChangeExternalOrdersStatusesResult, UpdateOrdersStatusResult>(result);
                return retVal;
            }
            catch (Exception ex)
            {
                log.Error("Ошибка передачи статуса заказа в ProductCatalog", ex);
                return ServiceOperationResult.BuildErrorResult<UpdateOrdersStatusResult>(ex);
            }
        }

        #endregion

        #region Methods
        
        private void AddOrderToQueue(int partnerId, PartnersConnector.Interfaces.Entities.Order order)
        {
            var clientId = order.ClientId;
            var dto = Mapper.Map<PartnersConnector.Interfaces.Entities.Order, DTO.CommitOrder.Order>(order);

            var queueItem = new CommitOrderQueueItem
                                {
                                    ClientId = clientId,
                                    PartnerId = partnerId,

                                    // NOTE: Так как сущность храниться в MS SQL базе в поле с типом xml который не признает utf-8, сериализуем в utf-16 (Encoding.Unicode)
                                    Order = dto.Serialize(Encoding.Unicode),
                                    ProcessStatus = Statuses.NotProcessed,
                                };

            this.queueItemRepository.Save(queueItem);
        }
        
        private CommitOrderResult InternalCommitOrder(int partnerId, string url, PartnersConnector.Interfaces.Entities.Order order)
        {
            string response = null;
            try
            {
                var dto = Mapper.Map<PartnersConnector.Interfaces.Entities.Order, DTO.CommitOrder.Order>(order);
                var message = new CommitOrderMessage { Order = dto };
                var data = message.Serialize(Encoding.UTF8);

                DTO.CommitOrder.CommitOrderResult commitResult;
                ConfirmedStatuses confirmed;

                var logEntry = StartInteraction.With("Partner").For("CommitOrder");
                logEntry.Info["PartnerId"] = order.PartnerId;
                logEntry.Info["Request"] = data;

                try
                {
                    log.InfoFormat("Отправка запроса {0} в систему партнера, body: {1}", url, data);

                    var uri = new Uri(url);

                    response = GetMessageDispatcher().Send(uri, data);

                    logEntry.Info["Response"] = response;

                    log.InfoFormat("Полученный ответ: {0}", response);

                    commitResult = response.Deserialize<DTO.CommitOrder.CommitOrderResult>(Encoding.UTF8);

                    if (commitResult == null)
                    {
                        var error = string.Format("Ответ партнера с идентификатором {0} не содержит данные: {1}",
                                                  order.PartnerId, response);
                        log.Error(error);

                        logEntry.Failed("ответ не содержит данные", null);

                        return CommitOrderResult.BuildFail(ResultCodes.InvalidPartnerResponse, error);
                    }

                    confirmed = (ConfirmedStatuses)commitResult.Confirmed;

                    if (confirmed == ConfirmedStatuses.Committed)
                    {
                        logEntry.Succeeded();
                    }
                    else
                    {
                        logEntry.NotSucceeded();
                    }
                }
                catch (Exception e)
                {
                    logEntry.Failed("ошибка взаимодействия", e);
                    throw;
                }
                finally
                {
                    logEntry.FinishAndWrite(log);
                }

                return CommitOrderResult.BuildSuccess(confirmed, commitResult.Reason, commitResult.ReasonCode, commitResult.InternalOrderId ?? dto.OrderId);
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Source == "System.Xml")
                {
                    var error = string.Format("Ответ партнера с идентификатором {0} содержит не корректные данные: {1}", partnerId, response);
                    log.Error(error, ex);
                    return CommitOrderResult.BuildFail(ResultCodes.InvalidPartnerResponse, error);
                }

                log.Error(InternalExceptionMessage, ex);
                return ServiceOperationResult.BuildErrorResult<CommitOrderResult>(ex);
            }
            catch (Exception ex)
            {
                log.Error(InternalExceptionMessage, ex);
                return ServiceOperationResult.BuildErrorResult<CommitOrderResult>(ex);
            }
        }

        private T BuildFail<T>(ResultCodes resultCodes, string message) where T : PartnersConnector.Interfaces.Entities.ResultBase, new()
        {
            return new T()
            {
                ResultCode = (int)resultCodes,
                ResultDescription = message
            };
        }

        private static IEnumerable<T> MapArrayFromMessage<T>(IEnumerable<T> src)
        {
            if (src == null)
            {
                return null;
            }

            return src.Maybe(g => g.Where(i => i != null));
        }

        #endregion
    }
}