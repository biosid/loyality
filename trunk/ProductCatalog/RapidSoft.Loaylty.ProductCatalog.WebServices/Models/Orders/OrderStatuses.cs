namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum OrderStatuses
    {
        /// <summary>
        /// Создан запрос на заказ
        /// </summary>
        [EnumMember]
        Draft = 0,

        /// <summary>
        /// Оформление заказа
        /// </summary>
        [EnumMember]
        Registration = 5,
        
        /// <summary>
        /// В обработке
        /// </summary>
        [EnumMember]
        Processing = 10,

        /// <summary>
        /// Аннулирован партнером
        /// </summary>
        [EnumMember]
        CancelledByPartner = 20,

        /// <summary>
        /// Требует доставки
        /// </summary>
        [EnumMember]
        DeliveryWaiting = 30,

        /// <summary>
        /// Доставка заказа
        /// </summary>
        [EnumMember]
        Delivery = 40,

        /// <summary>
        /// Заказ доставлен
        /// </summary>
        [EnumMember]
        Delivered = 50,

        /// <summary>
        /// Доставлен c задержкой
        /// </summary>
        [EnumMember]
        DeliveredWithDelay = 51,
        
        /// <summary>
        /// Не доставлен
        /// </summary>
        [EnumMember]
        NotDelivered = 60,
    }
}
