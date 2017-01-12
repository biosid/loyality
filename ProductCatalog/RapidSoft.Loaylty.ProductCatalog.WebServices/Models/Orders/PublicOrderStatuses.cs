namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Публичные/пользовательские статусы заказа
    /// </summary>
    [DataContract]
    public enum PublicOrderStatuses
    {
        /// <summary>
        /// Статус "Оформление"
        /// </summary>
        [EnumMember]
        Registration = 0,

        /// <summary>
        /// Статус "В обработке".
        /// </summary>
        [EnumMember]
        Processing = 10,

        /// <summary>
        /// Статус "Аннулирован".
        /// </summary>
        [EnumMember]
        Cancelled = 20,

        /// <summary>
        /// Статус "Заказ подтверждён".
        /// </summary>
        [EnumMember]
        DeliveryWaiting = 30,

        /// <summary>
        /// Статус "Доставка".
        /// </summary>
        [EnumMember]
        Delivery = 40,

        /// <summary>
        /// Статус "Доставлен".
        /// </summary>
        [EnumMember]
        Delivered = 50,

        /// <summary>
        /// Статус "Не доставлен".
        /// </summary>
        [EnumMember]
        NotDelivered = 60
    }
}
