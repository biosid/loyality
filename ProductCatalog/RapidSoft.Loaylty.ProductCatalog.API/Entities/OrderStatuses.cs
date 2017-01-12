namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    public enum OrderStatuses
    {
        /// <summary>
        /// Создан запрос на заказ
        /// </summary>
        Draft = 0,

        /// <summary>
        /// Оформление заказа
        /// </summary>
        Registration = 5,
        
        /// <summary>
        /// В обработке
        /// </summary>
        Processing = 10,

        /// <summary>
        /// Аннулирован партнером
        /// </summary>
        CancelledByPartner = 20,

        /// <summary>
        /// Требует доставки
        /// </summary>
        DeliveryWaiting = 30,

        /// <summary>
        /// Доставка заказа
        /// </summary>
        Delivery = 40,

        /// <summary>
        /// Заказ доставлен
        /// </summary>
        Delivered = 50,

        /// <summary>
        /// Доставлен c задержкой
        /// </summary>
        DeliveredWithDelay = 51,
        
        /// <summary>
        /// Не доставлен
        /// </summary>
        NotDelivered = 60,
    }
}