namespace RapidSoft.Loaylty.PartnersConnector.Interfaces
{
    using System.ServiceModel;

    using Monitoring;

    using PartnersConnector.Interfaces.Entities;

    /// <summary>
    /// Интерфейс сервиса для взаимодействия с поставщиком подарков(партнером). 
    /// С сервисам должны взаимодействовать компоненты системы, из вне сервис не должен быть доступен.
    /// </summary>
    [ServiceContract]
    public interface IOrderManagementService : ISupportService
    {
        /// <summary>
        /// Фиксация цены на товар добавленный в корзину
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [OperationContract]
        FixBasketItemPriceResult FixBasketItemPrice(FixBasketItemPriceParam param);

        /// <summary>
        /// Проверка заказа.
        /// </summary>
        /// <param name="order">
        /// The order.
        /// </param>
        /// <returns>
        /// The <see cref="CheckOrderResult"/>.
        /// </returns>
        [OperationContract]
        CheckOrderResult CheckOrder(Order order);

        /// <summary>
        /// Подтверждение заказа у партнера.
        /// </summary>
        /// <param name="order">
        /// Подтверждаемый заказ.
        /// </param>
        /// <returns>
        /// The <see cref="CommitOrderResult"/>.
        /// </returns>
        [OperationContract]
        CommitOrderResult CommitOrder(Order order);

        /// <summary>
        /// Кастомное подтверждение заказа.
        /// </summary>
        [OperationContract]
        ResultBase CustomCommitOrder(Order order, string methodName);


        UpdateOrdersStatusResult UpdateOrdersStatuses(NotifyOrderMessage[] messages);

        /// <summary>
        /// Получение доступных способов доставки для партнёра
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [OperationContract]
        GetDeliveryVariantsResult GetDeliveryVariants(GetDeliveryVariantsParam param);
    }
}