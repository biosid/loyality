namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Элемент корзины.
    /// </summary>
    [DataContract]
    public class BasketItem
    {
        /// <summary>
        /// Идентификатор элемента корзины.
        /// </summary>
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// Идентификатор клиента.
        /// </summary>
        [DataMember]
        public string ClientId { get; set; }

        /// <summary>
        /// Идентификатор группы элементов корзины.
        /// </summary>
        [DataMember]
        public int? BasketItemGroupId { get; set; }

        /// <summary>
        /// Внутренний идентификатор товара.
        /// </summary>
        [DataMember]
        public string ProductId { get; set; }

        /// <summary>
        /// Физический товар.
        /// </summary>
        [DataMember]
        public Product Product { get; set; }

        /// <summary>
        /// Дата добавления товара в корзину
        /// </summary>
        [DataMember]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Количество товаров в корзине
        /// </summary>
        [DataMember]
        public int ProductsQuantity { get; set; }

        /// <summary>
        /// Общая стоимость элемента корзины в баллах
        /// </summary>
        [DataMember]
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Общая стоимость элемента корзины в рублях
        /// </summary>
        [DataMember]
        public decimal TotalPriceRur { get; set; }

        /// <summary>
        /// стоимость элемента корзины в баллах
        /// </summary>
        [DataMember]
        public decimal ItemPrice { get; set; }

        /// <summary>
        /// Актуальная цена на единицу товара в элементе корзины в рублях
        /// </summary>
        [DataMember]
        public string FixedPrice { get; set; }

        /// <summary>
        /// Статус доступности элемента козины для заказа
        /// </summary>
        [DataMember]
        public ProductAvailabilityStatuses AvailabilityStatus { get; set; }
    }
}