namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System;

    public class CreateOrderFromOnlinePartnerParameters
    {
        public string ClientId { get; set; }

        /// <summary>
        /// Идентификатор Партнера
        /// </summary>
        public int PartnerId { get; set; }

        /// <summary>
        /// Идентификатор заказа в информационной системе Партнера
        /// </summary>
        public string ExternalOrderId { get; set; }

        /// <summary>
        /// Стоимость всего заказа, в рублях, с учетом доставки.
        /// </summary>
        public decimal TotalCost { get; set; }

        /// <summary>
        /// Код статуса заказа в информационной системе поставщика. 
        /// </summary>
        public string ExternalStatus { get; set; }

        /// <summary>
        /// Дата и время изменения статуса заказа, во временной зоне информационной системы Партнера.
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Дата и время изменения статуса заказа, во временной зоне UTC.
        /// </summary>
        public DateTime UtcDateTime { get; set; }

        /// <summary>
        /// Пояснение статуса заказа, предназначенное для отображения пользователю.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Элементы, содержащий информацию о строке заказа. 
        /// </summary>
        public CreateOrderFromOnlinePartnerItem[] Items { get; set; }
    }
}
