namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System;
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    [DataContract]
    public class WishListItem
    {
        /// <summary>
        /// Товар элемента WishList.
        /// </summary>
        [DataMember]
        public Product Product { get; set; }

        /// <summary>
        /// Дата добавления товара в WishList.
        /// </summary>
        [DataMember]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Общая стоимость в баллах.
        /// </summary>
        [DataMember]
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Количество товаров в корзине
        /// </summary>
        [DataMember]
        public int ProductsQuantity { get; set; }

        /// <summary>
        /// Стоимость единицы в баллах
        /// </summary>
        [DataMember]
        public decimal ItemPrice { get; set; }

        /// <summary>
        /// Статус доступности элемента козины для заказа
        /// </summary>
        [DataMember]
        public ProductAvailabilityStatuses AvailabilityStatus { get; set; }
    }
}