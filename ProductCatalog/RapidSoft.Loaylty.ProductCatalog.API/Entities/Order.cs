namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class Order
    {
        public Order()
        {
        }

        /// <summary>
        /// Идентификатор заказа
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор клиента (покупателя)
        /// </summary>
        [DataMember]
        public string ClientId { get; set; }

        /// <summary>
        /// Идентификатор заказа в информационной системе партнёра предоставленный партнёром во время подтверждения заказа.
        /// </summary>
        [DataMember]
        public string ExternalOrderId { get; set; }

        /// <summary>
        /// Идентификатор партнёра.
        /// </summary>
        [DataMember]
        public int PartnerId { get; set; }

        /// <summary>
        /// Идентификатор курьера.
        /// </summary>
        [DataMember]
        public int? CarrierId { get; set; }

        /// <summary>
        /// Статус оплаты
        /// </summary>
        [DataMember]
        public OrderPaymentStatuses PaymentStatus { get; set; }

        /// <summary>
        /// Статус доставки
        /// </summary>
        [DataMember]
        public OrderDeliveryPaymentStatus DeliveryPaymentStatus { get; set; }

        /// <summary>
        /// Статус заказа
        /// </summary>
        [DataMember]
        public OrderStatuses Status { get; set; }

        /// <summary>
        /// Код статуса в информационной системе поставщика. Предназначен для сверок. Не обязателен к заполнению.
        /// </summary>
        [DataMember]
        public string ExternalOrderStatusCode { get; set; }

        /// <summary>
        /// Текстовое пояснение статуса заказа
        /// </summary>
        [DataMember]
        public string OrderStatusDescription { get; set; }

        /// <summary>
        /// Дата и время изменения статуса в информационной системе поставщика.
        /// </summary>
        [DataMember]
        public DateTime? ExternalOrderStatusDateTime { get; set; }

        /// <summary>
        /// Дата и время последнего обновления статуса заказа
        /// </summary>
        [DataMember]
        public DateTime StatusChangedDate { get; set; }

        /// <summary>
        /// Дата и время создания заказа.
        /// </summary>
        [DataMember]
        public DateTime InsertedDate { get; set; }

        /// <summary>
        /// Имя пользователя в системе безопасности, который внес последнее изменение.
        /// </summary>
        [DataMember]
        public string UpdatedUserId { get; set; }

        /// <summary>
        /// Массив записей типа «Позиция заказа»
        /// </summary>
        [DataMember]
        public OrderItem[] Items { get; set; }

        [IgnoreDataMember]
        public string ItemsXml { get; set; }

        /// <summary>
        /// Сведения о доставке
        /// </summary>
        [DataMember]
        public DeliveryInfo DeliveryInfo { get; set; }

        [IgnoreDataMember]
        public string DeliveryInfoXml { get; set; }

        /// <summary>
        /// Инструкции для получения заказа, передаваемые клиенту от поставщика. Необязательное поле.
        /// </summary>
        [DataMember]
        public string DeliveryInstructions { get; set; }

        /// <summary>
        /// Вес заказа. Рассчитывается по формуле: Item.Weight * Item.Amount.
        /// </summary>
        [DataMember]
        public int TotalWeight { get; set; }

        #region Price
        
        /// <summary>
        /// Стоимость позиций заказа в рублях с учетом количества заказанных экземпляров, но без учета стоимости доставки.  Рассчитывается по формуле Item.Price * Item.Amount.
        /// </summary>
        [DataMember]
        public decimal ItemsCost { get; set; }

        /// <summary>
        /// Стоимость позиций в баллах
        /// </summary>
        [DataMember]
        public decimal BonusItemsCost { get; set; }

        /// <summary>
        /// Стоимость доставки заказа в рублях.
        /// </summary>
        [DataMember]
        public decimal DeliveryCost { get; set; }

        /// <summary>
        /// Стоимость доставки в баллах
        /// </summary>
        [DataMember]
        public decimal BonusDeliveryCost { get; set; }

        /// <summary>
        /// Аванс за доставку в рублях
        /// </summary>
        [DataMember]
        public decimal DeliveryAdvance { get; set; }

        /// <summary>
        /// Аванс за позиции в рублях
        /// </summary>
        [DataMember]
        public decimal ItemsAdvance { get; set; }

        /// <summary>
        /// Полная стоимость заказа в рублях с учетом доставки. Рассчитывается по формуле: ItemsCost + DeliveryCost. Если доставка не включена в заказ или не поддерживается поставщиком, то значение равно ItemsCost.
        /// </summary>
        [DataMember]
        public decimal TotalCost { get; set; }

        /// <summary>
        /// Полная стоимость заказа TotalCost в баллах
        /// </summary>
        [DataMember]
        public decimal BonusTotalCost { get; set; }

        /// <summary>
        /// Аванс за заказ в рублях с учетом доставки. Рассчитывается по формуле: ItemsAdvance + DeliveryAdvance. Если доставка не включена в заказ или не поддерживается поставщиком, то значение равно ItemsAdvance.
        /// </summary>
        [DataMember]
        public decimal TotalAdvance { get; set; }

        #endregion

        [DataMember]
        public PublicOrderStatuses PublicStatus { get; set; }

        public DateTime StatusUtcChangedDate { get; set; }

        public DateTime InsertedUtcDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public DateTime UpdatedUtcDate { get; set; }
    }
}