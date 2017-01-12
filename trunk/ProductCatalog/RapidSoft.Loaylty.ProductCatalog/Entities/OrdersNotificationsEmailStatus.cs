namespace RapidSoft.Loaylty.ProductCatalog.Entities
{
    /// <summary>
    /// Статусы писем с нотификациями по заказам
    /// </summary>
    public enum OrdersNotificationsEmailStatus
    {
        /// <summary>
        /// Письмо готово к отправке
        /// </summary>
        ReadyToSend = 0,

        /// <summary>
        /// Письмо успешно отправлено
        /// </summary>
        Sent = 1,

        /// <summary>
        /// При отправке письма произошла ошибка
        /// </summary>
        Error = 2
    }
}
