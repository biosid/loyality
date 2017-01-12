namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Элемент корзины.
    /// </summary>
    [DataContract]
    public class ClientItem
    {
        /// <summary>
        /// Идентификатор элемента корзины.
        /// </summary>
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// Физический товар.
        /// </summary>
        [DataMember]
        public ClientItemProduct Product { get; set; }

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
        public decimal? TotalPrice { get; set; }

        /// <summary>
        /// стоимость элемента корзины в баллах
        /// </summary>
        [DataMember]
        public decimal? ItemPrice { get; set; }
    }
}
