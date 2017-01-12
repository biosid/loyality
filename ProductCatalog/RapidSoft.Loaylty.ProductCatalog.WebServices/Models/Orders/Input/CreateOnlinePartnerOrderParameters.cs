namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders.Input
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class CreateOnlinePartnerOrderParameters
    {
        [DataMember]
        public string ClientId { get; set; }

        /// <summary>
        /// Идентификатор Партнера
        /// </summary>
        [DataMember]
        public int PartnerId { get; set; }

        /// <summary>
        /// Идентификатор заказа в информационной системе Партнера
        /// </summary>
        [DataMember]
        public string ExternalOrderId { get; set; }

        /// <summary>
        /// Стоимость всего заказа, в рублях, с учетом доставки.
        /// </summary>
        [DataMember]
        public decimal TotalCost { get; set; }

        /// <summary>
        /// Код статуса заказа в информационной системе поставщика. 
        /// </summary>
        [DataMember]
        public string ExternalStatus { get; set; }

        /// <summary>
        /// Дата и время изменения статуса заказа, во временной зоне информационной системы Партнера.
        /// </summary>
        [DataMember]
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Дата и время изменения статуса заказа, во временной зоне UTC.
        /// </summary>
        [DataMember]
        public DateTime UtcDateTime { get; set; }

        /// <summary>
        /// Пояснение статуса заказа, предназначенное для отображения пользователю.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Элементы, содержащий информацию о строке заказа. 
        /// </summary>
        [DataMember]
        public OnlinePartnerOrderItem[] Items { get; set; }
    }
}
