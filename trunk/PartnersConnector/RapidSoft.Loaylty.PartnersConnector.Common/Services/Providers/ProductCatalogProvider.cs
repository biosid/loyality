namespace RapidSoft.Loaylty.PartnersConnector.Services.Providers
{
    using System;
    using System.Linq;

    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.PartnersConnector.Interfaces;
    using RapidSoft.Loaylty.ProductCatalog.WsClients;
    using RapidSoft.Loaylty.ProductCatalog.WsClients.OrderManagementService;

    [Serializable]
    public class ProductCatalogProvider : IProductCatalogProvider
    {
        private readonly ILog log = LogManager.GetLogger(typeof (ProductCatalogProvider));

        public ChangeExternalOrdersStatusesResult ChangeOrdersStatuses(ExternalOrdersStatus[] ordersStatus)
        {
            log.Info("Обновление статуса в компоненте \"Каталог товаров\"");

            if (ordersStatus.Select(x => x.PartnerId).Distinct().Count() > 1)
            {
                const string Mess = "Нельзя обновить статусы по нескольким партнерам за раз";
                log.Info(Mess);
                throw new NotSupportedException(Mess);
            }

            var retVal =
                WebClientCaller.CallService<OrderManagementServiceClient, ChangeExternalOrdersStatusesResult>(
                    cl => cl.ChangeExternalOrdersStatuses(ordersStatus));

            log.Info("Полученный ответ на обновление статуса: " + retVal.Serialize());
            return retVal;
        }
        
        public CreateOrderResult CreateOrderForOnlinePartner(CreateOrderFromOnlinePartnerParameters parameters)
        {
            log.Info("Создание заказа для online партнера");

            var retVal =
                WebClientCaller.CallService<OrderManagementServiceClient, CreateOrderResult>(
                    cl => cl.CreateOnlinePartnerOrder(parameters));
            log.Info("Полученный ответ на создание заказа для online партнера: " + retVal.Serialize());
            return retVal;
        }

        public GetOrderResult GetOrderByExternalId(GetOrderByExternalIdParameters parameters)
        {
            log.Info("Получение заказа для online партрера");
            var retVal =
                WebClientCaller.CallService<OrderManagementServiceClient, GetOrderResult>(
                    cl => cl.GetOrderByExternalId(parameters));
            log.Info("Полученный ответ на запрос заказа для online партнера: " + retVal.Serialize());
            return retVal;
        }
    }
}