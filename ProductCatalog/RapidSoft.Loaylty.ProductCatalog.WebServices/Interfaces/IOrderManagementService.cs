namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Interfaces
{
    using System.ServiceModel;

    using Monitoring;

    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Input;
    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders;
    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders.Input;
    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders.Output;
    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Output;

    [ServiceContract]
    public interface IOrderManagementService : ISupportService
    {
        /// <summary>
        /// Создание и предварительное подтверждение заказа
        /// </summary>
        [OperationContract]
        ValueResult<Order> CreateOrderFromBasketItem(CreateOrderFromBasketItemParameters parameters);

        /// <summary>
        /// Создание и предварительное подтверждение заказа для онлайн партнера
        /// </summary>
        [OperationContract]
        ValueResult<Order> CreateOnlinePartnerOrder(CreateOnlinePartnerOrderParameters parameters);

        /// <summary>
        /// Подтверждение заказа клиентом
        /// </summary>
        [OperationContract]
        ValueResult<Order> ClientCommitOrder(ClientCommitOrderParameters parameters);

        /// <summary>
        /// Выполняет поиск истории заказов пользователя/клиента.
        /// </summary>
        [OperationContract]
        PagedResult<Order> GetOrdersHistory(GetOrdersHistoryParameters parameters);

        [OperationContract]
        ValueResult<bool> HasNonterminatedOrders(HasNonterminatedOrdersParameters parameters);

        /// <summary>
        /// Выполняет поиск заказов на оплату.
        /// </summary>
        [OperationContract]
        PagedResult<Order> GetOrdersForPayment(PagingParameters parameters);

        [OperationContract]
        OrderResult GetOrderById(GetOrderByIdParameters parameters);

        [OperationContract]
        OrderResult GetOrderByExternalId(GetOrderByExternalIdParameters parameters);

        [OperationContract]
        ArrayResult<OrderPayments> GetOrderPaymentStatuses(GetOrderPaymentStatusesParameters parameters);

        /// <summary>
        /// Получение последних адресов доставки заказов
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ArrayResult<DeliveryAddress> GetLastDeliveryAddresses(GetLastDeliveryAddressesParameters parameters);

        /// <summary>
        /// Обновление статусов заказов
        /// </summary>
        [OperationContract]
        ArrayResult<string> ChangeExternalOrdersStatuses(ChangeExternalOrdersStatusesParameters parameters);

        /// <summary>
        /// Обновление статусов заказов
        /// </summary>
        [OperationContract]
        ArrayResult<int> ChangeOrdersStatuses(ChangeOrdersStatusesParameters parameters);

        /// <summary>
        /// Изменение описания статуса
        /// </summary>
        [OperationContract]
        ResultBase ChangeOrderStatusDescription(ChangeOrderStatusDescriptionParameters parameters);

        /// <summary>
        /// Переводит заказы партнеров с типом Direct в статус DeliveryWaiting из статуса Processing
        /// для того чтобы пока выполняется оплата партнер не перевел заказы в CancelledByPartner.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ArrayResult<int> ChangeOrdersStatusesBeforePayment();

        /// <summary>
        /// Обновление статусов оплаты заказов
        /// </summary>
        [OperationContract]
        ArrayResult<int> ChangeProductsPaymentStatuses(ChangePaymentStatusesParameters parameters);

        /// <summary>
        /// Обновление статусов доставки заказов
        /// </summary>
        [OperationContract]
        ArrayResult<int> ChangeDeliveryPaymentStatuses(ChangePaymentStatusesParameters parameters);

        [OperationContract]
        DeliveryVariantsResult GetDeliveryVariants(GetDeliveryVariantsParameters parameters);
    }
}
