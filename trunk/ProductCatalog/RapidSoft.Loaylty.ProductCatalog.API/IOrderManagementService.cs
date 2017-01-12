namespace RapidSoft.Loaylty.ProductCatalog.API
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;

    using Monitoring;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.API.InputParameters;
    using RapidSoft.Loaylty.ProductCatalog.API.OutputResults;

    [ServiceContract]
    public interface IOrderManagementService : ISupportService
    {
        /// <summary>
        /// Создание и предварительное подтверждение заказа
        /// </summary>
        [OperationContract]
        CreateOrderResult CreateOrderFromBasketItems(CreateOrderFromBasketItemsParameters parameters);

        /// <summary>
        /// Создание и предварительное подтверждение заказа для онлайн партнера
        /// </summary>
        [OperationContract]
        CreateOrderResult CreateOnlinePartnerOrder(CreateOrderFromOnlinePartnerParameters parameters);

        /// <summary>
        /// Создание кастомного заказа для директ партнера (используется для заказов продуктов банка)
        /// </summary>
        [OperationContract]
        CreateOrderResult CreateCustomOrder(CreateCustomOrderParameters parameters);

        /// <summary>
        /// Подтверждение заказа клиентом
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [OperationContract]
        ClientCommitOrderResult ClientCommitOrder(string clientId, int orderId);

        /// <summary>
        /// Выполняет поиск истории заказов пользователя/клиента.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Результат поиска заказов.
        /// </returns>
        [OperationContract]
        GetOrdersHistoryResult GetOrdersHistory(GetOrdersHistoryParameters parameters);

        [OperationContract]
        HasNonterminatedOrdersResult HasNonterminatedOrders(string clientId);

        /// <summary>
        /// Выполняет поиск заказов на оплату.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Результат поиска заказов.
        /// </returns>
        [OperationContract]
        GetOrdersHistoryResult GetOrdersForPayment(GetOrdersForPaymentParameters parameters);

        [OperationContract]
        GetOrderResult GetOrderById(int orderId, string clientId);

        [OperationContract]
        GetOrderResult GetOrderByExternalId(GetOrderByExternalIdParameters parameters);

        [OperationContract]
        GetOrderPaymentStatusesResult GetOrderPaymentStatuses(int[] orderIds);

        /// <summary>
        /// Получение последних адресов доставки заказов
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        GetLastDeliveryAddressesResult GetLastDeliveryAddresses(string clientId, bool excludeAddressesWithoutKladr, int? countToTake);

        /// <summary>
        /// Обновление статусов заказов
        /// </summary>
        /// <param name="externalOrdersStatuses"></param>
        /// <returns></returns>
        [OperationContract]
        ChangeExternalOrdersStatusesResult ChangeExternalOrdersStatuses(ExternalOrdersStatus[] externalOrdersStatuses);

        /// <summary>
        /// Обновление статусов заказов
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="ordersStatuses"></param>
        /// <returns></returns>
        [OperationContract]
        ChangeOrdersStatusesResult ChangeOrdersStatuses(OrdersStatus[] ordersStatuses);

        /// <summary>
        /// Изменение описания статуса
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderStatusDescription"></param>
        /// <returns></returns>
        [OperationContract]
        ResultBase ChangeOrderStatusDescription(int orderId, string orderStatusDescription);

        /// <summary>
        /// Переводит заказы партнеров с типом Direct в статус DeliveryWaiting из статуса Processing
        /// для того чтобы пока выполняется оплата партнер не перевел заказы в CancelledByPartner.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ChangeOrdersStatusesResult ChangeOrdersStatusesBeforePayment();

        /// <summary>
        /// Обновление статусов оплаты заказов
        /// </summary>
        /// <param name="statuses"></param>
        /// <returns></returns>
        [OperationContract]
        ChangeOrdersStatusesResult ChangeOrdersPaymentStatuses(OrdersPaymentStatus[] statuses);

        /// <summary>
        /// Обновление статусов доставки заказов
        /// </summary>
        /// <param name="statuses"></param>
        /// <returns></returns>
        [OperationContract]
        ChangeOrdersStatusesResult ChangeOrdersDeliveryStatuses(OrdersDeliveryStatus[] statuses);

        [OperationContract]
        GetDeliveryVariantsResult GetDeliveryVariants(GetDeliveryVariantsParameters parameters);
    }
}