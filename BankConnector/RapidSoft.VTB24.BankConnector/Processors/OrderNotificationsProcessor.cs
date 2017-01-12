namespace RapidSoft.VTB24.BankConnector.Processors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Rapidsoft.Loyalty.NotificationSystem.WsClients.ClientMessageService;
    using RapidSoft.Loaylty.ClientProfile.ClientProfileService;
    using RapidSoft.Loaylty.ProductCatalog.WsClients.CatalogAdminService;
    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource;
    using RapidSoft.VTB24.BankConnector.Infrastructure.Configuration;
    using RapidSoft.VTB24.BankConnector.Service;

    public class OrderNotificationsProcessor : ProcessorBase
    {
        private static readonly OrderStatuses[] ExecutedOrderStatuses = new[]
        {
            OrderStatuses.DeliveryWaiting,
            OrderStatuses.Delivery,
            OrderStatuses.Delivered,
            OrderStatuses.DeliveredWithDelay,
            OrderStatuses.NotDelivered
        };

        private static readonly string BasketUrl = ConfigHelper.ClientSiteBasketUrl;
        private static readonly string OrderUrlTemplate = ConfigHelper.ClientSiteOrderUrlTemplate;

        private readonly IClientMessageService clientMessageService;
        private readonly ICatalogAdminService catalogAdminService;
        private readonly ClientProfileService clientProfileService;

        public OrderNotificationsProcessor(
            EtlLogger.EtlLogger logger,
            IUnitOfWork uow,
            IClientMessageService clientMessageService,
            ICatalogAdminService catalogAdminService,
            ClientProfileService clientProfileService)
            : base(logger, uow)
        {
            this.clientMessageService = clientMessageService;
            this.catalogAdminService = catalogAdminService;
            this.clientProfileService = clientProfileService;
        }

        public void NotifyIncompleteOrders(DateTime from)
        {
            var batchSize = ConfigHelper.BatchSize;

            var skip = 0;
            var take = batchSize;

            var count = 0;
            var skippedCount = 0;
            var errorCount = 0;

            string[] batch;

            Logger.Info("Отправка оповещений о неоформленных заказах начата");

            while ((batch = GetClientsWithIncompleteOdersBatch(from, skip, take)) != null)
            {
                skip += batch.Length;
                count += batch.Length;

                var notifications = batch
                    .Select(ToIncompleteOrderNotification)
                    .Where(n => n != null)
                    .ToArray();

                skippedCount += batch.Length - notifications.Length;

                Logger.InfoFormat("Отправка {0} сообщений", notifications.Length);

                if (!SendNotifications(notifications))
                {
                    errorCount += batch.Length;
                }
            }

            Logger.Info("Отправка оповещений о неоформленных заказах окончена");

            Logger.Counter("Сообщения", "всего", count);
            Logger.Counter("Сообщения", "не удалось сформировать", skippedCount);
            Logger.Counter("Сообщения", "не удалось отправить", errorCount);

            Logger.Info("Очистка реестра неоформленных заказов начата");

            try
            {
                Uow.OrderAttemptsRepository.ClearAll();
                Uow.Save();
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка очистки реестра неоформленных заказов", ex);
            }

            Logger.Info("Очистка реестра неоформленных заказов окончена");
        }

        public void NotifyExecutedOrders(DateTime fromDateTime, DateTime toDateTime, string surveyUrl)
        {
            var batchSize = ConfigHelper.BatchSize;

            var count = 0;
            var skippedCount = 0;
            var errorCount = 0;

            var skip = 0;
            var take = batchSize;

            Order[] batch;

            Logger.Info("Отправка предложений оценить сервис начата");

            while ((batch = GetExecutedOrdersBatch(fromDateTime, toDateTime, skip, take)) != null)
            {
                skip += batch.Length;
                count += batch.Length;

                var notifications = batch
                    .Select(o => ToExecutedOrderNotification(o, surveyUrl))
                    .Where(n => n != null)
                    .ToArray();

                skippedCount += batch.Length - notifications.Length;

                Logger.InfoFormat("Отправка {0} сообщений", batch.Length);

                if (!SendNotifications(notifications))
                {
                    errorCount += notifications.Length;
                }
            }

            Logger.Info("Отправка предложений оценить сервис окончена");

            Logger.Counter("Сообщения", "всего", count);
            Logger.Counter("Сообщения", "не удалось сформировать", skippedCount);
            Logger.Counter("Сообщения", "не удалось отправить", errorCount);
        }

        public void NotifyExecutedBankOfferOrders(List<OrderForPayment> orders)
        {
            Logger.Info("Отправка сообщений о выполнении заказа начата");

            var notifications = orders.Select(ToExecutedBankOfferOrderNotification).Where(n => n != null).ToArray();
            SendNotifications(notifications);

            Logger.Info("Отправка сообщений о выполнении заказа окончена");
        }

        private static Notification ToIncompleteOrderNotification(string clientId)
        {
            return new Notification
            {
                ClientId = clientId,

                Title = "Вы хотели сделать заказ на сайте «Коллекция»!",
                Text = string.Format(
                    @"Уважаемый клиент!

Сегодня Вы планировали сделать покупку на нашем сайте, но не оформили заказ. Выбранные Вами товары будут зарезервированы для вас только после оформления заказа. Вы можете перейти к оформлению заказа! {0}",
                    BasketUrl)
            };
        }

        private Notification ToExecutedOrderNotification(Order order, string surveyUrl)
        {
            var profile = GetClientProfile(order.ClientId);

            if (profile == null)
            {
                return null;
            }

            var politeName = profile.FirstName + " " + profile.MiddleName;

            return new Notification
            {
                ClientId = order.ClientId,

                Title = "Ваш заказ в программе «Коллекция» ВТБ24",
                Text = string.Format(
                    @"{0},

Спасибо за Ваш заказ в программе «Коллекция».

Мы будем Вам очень признательны, если Вы примете участие в опросе и расскажете о том, насколько Вы удовлетворены обслуживанием в Программе «Коллекция». Это займет не более 2 минут.
Ваши ответы помогут нам понять, все ли мы делаем правильно или нам нужно что-то изменить, чтобы повысить вашу удовлетворенность.
Для начала опроса перейдите по ссылке: {1}.

Команда Программы «Коллекция».",
                    politeName,
                    surveyUrl)
            };
        }

        private Notification ToExecutedBankOfferOrderNotification(OrderForPayment order)
        {
            return new Notification
            {
                ClientId = order.ClientId,
                Title = "Ваш заказ в программе «Коллекция» ВТБ24",
                Text = string.Format(
                    @"Уважаемый клиент! 

Ваш заказ №{0} на сумму {1} бонусов успешно осуществлен.
Спасибо, что пользуетесь услугами банка ВТБ24.", 
                                               order.OrderId, 
                                               order.OrderBonusCost.ToString("N"))
            };
        }

        private string[] GetClientsWithIncompleteOdersBatch(DateTime from, int skip, int take)
        {
            try
            {
                var clientIds = Uow.OrderAttemptsRepository.Get(from, skip, take);

                if (clientIds == null)
                {
                    Logger.Error("Некорректный ответ при получении клиентов с незавершенными заказами от OrderAttemptsRepository: clientIds = null");
                    return null;
                }

                return clientIds.Length > 0 ? clientIds : null;
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при получении пачки клиентов с незавершенными заказами", ex);
                return null;
            }
        }

        private bool SendNotifications(Notification[] notifications)
        {
            var parameters = new NotifyClientsParameters
            {
                Notifications = notifications
            };

            try
            {
                var response = clientMessageService.Notify(parameters);

                return AssertResponse(response);
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при отправке клиентам сообщений c предложением оценить сервис", ex);
                return false;
            }
        }

        private bool AssertResponse(NotifyClientsResult response)
        {
            if (response == null)
            {
                Logger.Error("Некорректный ответ при отправки оповещений в ClientMessageService: null");
                return false;
            }

            if (!response.Success)
            {
                Logger.ErrorFormat(
                    "Не удалось отправить клиентам сообщения c предложением оценить сервис: ClientMessageService [{0}] - {1}",
                    response.ResultCode,
                    response.ResultDescription);
                return false;
            }

            return true;
        }

        private Order[] GetExecutedOrdersBatch(DateTime fromDateTime, DateTime toDateTime, int skip, int take)
        {
            var parameters = new SearchOrdersParameters
            {
                CalcTotalCount = false,
                CountToSkip = skip,
                CountToTake = take,
                StartDate = fromDateTime,
                EndDate = toDateTime,
                Statuses = ExecutedOrderStatuses,
                UserId = ConfigHelper.VtbSystemUser
            };

            try
            {
                var response = catalogAdminService.SearchOrders(parameters);

                return AssertResponse(response) && response.Orders.Length > 0 ? response.Orders : null;
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при получении пачки оформленных заказов", ex);
                return null;
            }
        }

        private bool AssertResponse(SearchOrdersResult response)
        {
            if (response == null)
            {
                Logger.Error("Некорректный ответ при получении пачки заказов от CatalogAdminService: null");
                return false;
            }

            if (!response.Success)
            {
                Logger.ErrorFormat(
                    "Не удалось получить пачку оформленных заказов: CatalogAdminService [{0}] - {1}",
                    response.ResultCode,
                    response.ResultDescription);
                return false;
            }

            if (response.Orders == null)
            {
                Logger.Error("Некорректный ответ при получении пачки заказов от CatalogAdminService: Orders = null");
                return false;
            }

            return true;
        }

        private GetClientProfileFullResponseTypeClientProfile GetClientProfile(string clientId)
        {
            var parameters = new GetClientProfileFullRequestType
            {
                ClientId = clientId
            };

            try
            {
                var response = clientProfileService.GetClientProfileFull(new GetClientProfileFullRequest(parameters));

                return AssertResponse(response) ? response.Response.ClientProfile : null;
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при получении профиля клиента " + clientId, ex);
                return null;
            }
        }

        private bool AssertResponse(GetClientProfileFullResponse response)
        {
            if (response == null)
            {
                Logger.Error("Некорректный ответ при получении профиля клиента: null");
                return false;
            }

            if (response.Response == null)
            {
                Logger.Error("Некорректный ответ при получении профиля клиента: Response = null");
                return false;
            }

            if (response.Response.StatusCode != 0)
            {
                Logger.ErrorFormat(
                    "Не удалось получить профиль клиента: ClientProfileService [{0}] - {1}",
                    response.Response.StatusCode,
                    response.Response.Error);
                return false;
            }

            if (response.Response.ClientProfile == null)
            {
                Logger.Error("Некорректный ответ при получении профиля клиента: Response.ClientProfile = null");
                return false;
            }

            return true;
        }
    }
}
