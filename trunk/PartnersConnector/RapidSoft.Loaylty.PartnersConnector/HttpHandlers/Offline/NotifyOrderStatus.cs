namespace RapidSoft.Loaylty.PartnersConnector.HttpHandlers.Offline
{
    using System;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Web;

    using AutoMapper;

    using Common.DTO.NotifyOrderStatus;
    using Common.Services;

    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.Logging.Interaction;
    using RapidSoft.Loaylty.PartnersConnector.Interfaces;
    using RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities;
    using RapidSoft.Loaylty.PartnersConnector.Services.Providers;

    /// <summary>
    /// Обработчик http-запросов об изменении статуса заказа.
    /// </summary>
    public class NotifyOrderStatus : PartnerInteractionHttpHandler
    {
        private readonly ICatalogAdminServiceProvider catalogAdminServiceProvider;
        private readonly ILog log = LogManager.GetLogger(typeof (NotifyOrderStatus));

        static NotifyOrderStatus()
        {
            Mapper.CreateMap<NotifyOrderStatusMessageOrder, NotifyOrderMessage>()
                  .ForMember(dest => dest.PartnerOrderId, opt => opt.MapFrom(src => src.InternalOrderId))
                  .ForMember(dest => dest.Code, opt => opt.MapFrom(src => (OrderStatuses)src.StatusCode))
                  .ForMember(dest => dest.PartnerCode, opt => opt.MapFrom(src => src.InternalStatusCode))
                  .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.ClientId));

            Mapper.CreateMap<UpdateOrdersStatusResult, NotifyOrdersResult>()
                    .ForMember(dest => dest.Orders, opt => opt.MapFrom(src => src.UpdateOrderStatusResults));

            Mapper.CreateMap<UpdateOrderStatusResult, NotifyOrdersResultOrder>()
                  .ForMember(dest => dest.InternalOrderId, opt => opt.MapFrom(src => src.ExternalOrderId))
                  .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
                  .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.ResultDescription))
                  .ForMember(
                      dest => dest.ResultCode,
                      opt => opt.ResolveUsing(
                          src =>
                              {
                                  switch (src.ResultCode)
                                  {
                                          // NOTE: ResultCodes.NOT_FOUND = 2
                                      case 2:
                                          return 2; /* не найден заказ; */

                                          // NOTE: ResultCodes.WRONG_WORKFLOW = 55
                                      case 55:
                                          return 3; /* статус не может обновлен по workflow */

                                          // NOTE: ResultCodes.SUCCESS = 0;
                                      case 0:
                                          return 0; /* статус заказа успешно обновлен; */
                                      default:
                                          return 1; /* неизвестная ошибка при обновлении статуса заказа; */
                                  }
                              }));
        }

        public NotifyOrderStatus()
            : base("NotifyOrderStatus")
        {
            catalogAdminServiceProvider = new CatalogAdminServiceProvider();
        }

        public NotifyOrderStatus(ICatalogAdminServiceProvider catalogAdminServiceProvider = null)
            : base("NotifyOrderStatus")
        {
            this.catalogAdminServiceProvider = catalogAdminServiceProvider ?? new CatalogAdminServiceProvider();
        }

        protected override void ProcessRequest(HttpContext context, IInteractionLogEntry logEntry)
        {
            log.Info("Начало обработки уведомления об изменении статуса заказа.");
            var request = context.Request;

            try
            {
                var body = request.ReadBody();

                logEntry.Info["Request"] = body;

                log.Info(string.Format("NotifyOrderStatus request:{0}", body));

                if (request.ClientCertificate.Certificate.Length == 0)
                {
                    log.Error("В запросе изменения статуса отсутствует клиентский сертификат.");
                    context.Response.BadRequestResponse();

                    logEntry.Failed("отсутствует клиентский сертификат", null);
                    return;
                }
                
                var cert = new X509Certificate2(request.ClientCertificate.Certificate);

                var partnerSettings = catalogAdminServiceProvider.GetPartnerSettingsByCertificateThumbprint(cert.Thumbprint);

                if (partnerSettings == null)
                {
                    log.Error("Не удалось найти партнёра используя CertificateThumbprint " + cert.Thumbprint);
                    context.Response.InternalServerErrorResponse();

                    logEntry.Failed("не удалось найти партнера по клиентскому сертификату", null);
                    return;
                }

                logEntry.Info["PartnerId"] = partnerSettings.PartnerId;

                if (string.IsNullOrWhiteSpace(body))
                {
                    log.Error("Ошибка: сообщение не содержит данных.");
                    context.Response.BadRequestResponse();

                    logEntry.Failed("пустой запрос", null);
                    return;
                }

                var data = body.Deserialize<NotifyOrderStatusMessage>(Encoding.UTF8);

                if (data.Orders == null || data.Orders.Length == 0)
                {
                    log.Warn("Ошибка: уведомление не содержит данных о заказе.");
                    var empty = new NotifyOrdersResult { Orders = null };
                    context.Response.OkXmlResponse(empty.Serialize(Encoding.UTF8, true));

                    logEntry.Succeeded();
                    return;
                }

                MessageValidator.Validate(data);

                NotifyOrderMessage[] destinations;

                try
                {
                    destinations = Mapper.Map<NotifyOrderStatusMessageOrder[], NotifyOrderMessage[]>(data.Orders);
                }
                catch (AutoMapperMappingException e)
                {
                    log.Error("Ошибка мапинга входящего сообщения.");
                    context.Response.BadRequestResponse();

                    logEntry.Failed("ошибка маппинга запроса", e);
                    return;
                }

                foreach (var notifyOrderMessage in destinations)
                {
                    notifyOrderMessage.PartnerId = partnerSettings.PartnerId;
                }

                var service = new OrderManagementService();

                var result = service.UpdateOrdersStatuses(destinations);
                if (!result.Success)
                {
                    log.ErrorFormat("Не удалось обновить статус: {0}", result.ResultDescription);
                    context.Response.InternalServerErrorResponse();

                    logEntry.Failed("не удалось обновить статус: " + result.ResultDescription, null);
                    return;
                }

                var retVal = Mapper.Map<UpdateOrdersStatusResult, NotifyOrdersResult>(result);

                var resltBody = retVal.Serialize(Encoding.UTF8, true);

                logEntry.Info["Response"] = resltBody;

                context.Response.OkXmlResponse(resltBody);

                logEntry.Succeeded();
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Source == "System.Xml")
                {
                    log.Error("Ошибка разбора xml.", ex);
                    context.Response.BadRequestResponse();

                    logEntry.Failed("ошибка разбора XML", ex);
                }
                else
                {
                    log.Error("Ошибка обработки запроса об изменении статуса заказа.", ex);
                    context.Response.InternalServerErrorResponse();

                    logEntry.Failed("ошибка обработки запроса", ex);
                }
            }
            catch (Exception ex)
            {
                log.Error("Ошибка обработки запроса об изменении статуса заказа.", ex);
                context.Response.InternalServerErrorResponse();

                logEntry.Failed("ошибка обработки запроса", ex);
            }
        }
    }
}