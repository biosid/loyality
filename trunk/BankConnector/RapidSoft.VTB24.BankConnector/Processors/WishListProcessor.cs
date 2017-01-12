namespace RapidSoft.VTB24.BankConnector.Processors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Rapidsoft.Loyalty.NotificationSystem.WsClients.ClientMessageService;
    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.ProductCatalog.WsClients.WishListService;

    using RapidSoft.VTB24.BankConnector.Infrastructure.Configuration;

    using Notification = RapidSoft.Loaylty.ProductCatalog.WsClients.WishListService.Notification;

    public class WishListProcessor
    {
        private readonly ILog log = LogManager.GetLogger(typeof(WishListProcessor));

        private readonly IClientMessageService clientMessageService;

        private readonly IWishListService wishListService;

        public WishListProcessor(IClientMessageService clientMessageService, IWishListService wishListService)
        {
            this.clientMessageService = clientMessageService;
            this.wishListService = wishListService;
        }

        public void SendWishListNotifications()
        {           
            List<Notification> notificationsBatch;
            var listPart = new List<Notification>();

            while ((notificationsBatch = GetNotifications()).Any())
            {
                var lastClientId = notificationsBatch.Last().ClientId;
                notificationsBatch.AddRange(listPart);
                listPart = notificationsBatch.Where(x => x.ClientId == lastClientId).ToList();
                ProcessCompleteList(notificationsBatch.Where(x => x.ClientId != lastClientId).GroupBy(x => x.ClientId));
            }

            if (listPart.Any())
            {
                ProcessCompleteList(listPart.GroupBy(x => x.ClientId));
            }
        }

        private static string BuildMessageText(List<Notification> source)
        {
            if (!source.Any())
            {
                throw new Exception("Нет данных для построения уведомления клиента");
            }

            var template = new WishListTemplate();

            template.Session = new Dictionary<string, object> { { "NotificationList", source } };

            return template.TransformText();
        }

        private void ProcessCompleteList(IEnumerable<IGrouping<string, Notification>> clientItemsGroups)
        {
            if (!clientItemsGroups.Any())
            {
                return;
            }

            var request = clientItemsGroups.Select(
                x =>
                    new Rapidsoft.Loyalty.NotificationSystem.WsClients.ClientMessageService.Notification()
                    {
                        ClientId = x.First().ClientId,
                        Title = "Поздравляем! Заказывайте!",
                        Text = BuildMessageText(x.ToList()),
                    });

            NotifyClientsResult notifyClientsResult;
            try
            {
                var notifyClientsParameters = new NotifyClientsParameters()
                                              {
                                                  Notifications = request.ToArray()
                                              };

                notifyClientsResult = this.clientMessageService.Notify(notifyClientsParameters);

                if (!notifyClientsResult.Success)
                {
                    throw new ApplicationException(string.Format("{0} - {1}", notifyClientsResult.ResultCode, notifyClientsResult.ResultDescription));
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Ошибка при отправке оповещений в сервис ClientInbox: {0}", ex));
                throw;
            }

            foreach (var messageSendResult in notifyClientsResult.Threads)
            {
                log.Info(string.Format("Обработка оповещения для пользователя ({0}) в сервисе ClientInbox завершена успешно", messageSendResult.ClientId));
            }
        }

        private List<Notification> GetNotifications()
        {
            try
            {
                var param = new GetWishListNotificationsParameters
                {
                    CalcTotalCount = false,
                    CountToTake = ConfigHelper.BatchSize,
                    Rebuild = false
                };
                var result = wishListService.GetWishListNotifications(param);
                if (!result.Success)
                {
                    throw new ApplicationException(string.Format("{0} - {1}", result.ResultCode, result.ResultDescription));
                }

                return result.Notifications.ToList();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Ошибка при получении оповещения для рассылки от сервиса WishListService: {0}", ex));
                throw;
            }
        }
    }
}
