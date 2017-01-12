namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    /// <summary>
    /// Публичные/пользовательские статусы заказа
    /// </summary>
    public enum PublicOrderStatuses
    {
        /// <summary>
        /// Статус "В обработке".
        /// </summary>
        Processing = 10,

        /// <summary>
        /// Статус "Заказ подтверждён".
        /// </summary>
        DeliveryWaiting = 30,

        /// <summary>
        /// Статус "Доставка".
        /// </summary>
        Delivery = 40,

        /// <summary>
        /// Статус "Доставлен".
        /// </summary>
        Delivered = 50,

        /// <summary>
        /// Статус "Не доставлен".
        /// </summary>
        NotDelivered = 60,

        /// <summary>
        /// Статус "Аннулирован".
        /// </summary>
        Cancelled = 20,

        /// <summary>
        /// Статус "Оформление"
        /// </summary>
        Registration = 0
    }
}