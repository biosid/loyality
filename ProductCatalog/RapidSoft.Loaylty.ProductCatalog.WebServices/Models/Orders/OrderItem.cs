namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders
{
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog;

    [DataContract]
    public class OrderItem
    {
        /// <summary>
        /// Количество заказанных экземпляров товара
        /// </summary>
        [DataMember]
        public int Amount { get; set; }

        /// <summary>
        /// Идентификатор элемента корзины заполняется в том случае, если товар присутствовал и был заказан с использованием корзины.
        /// </summary>
        [DataMember]
        public string BasketItemId { get; set; }

        /// <summary>
        /// Идентификатор товара
        /// </summary>
        [DataMember]
        public string ProductId { get; set; }

        /// <summary>
        /// Идентификатор товара в системе партнера
        /// </summary>
        [DataMember]
        public string PartnerProductId { get; set; }

        /// <summary>
        /// Название товара
        /// </summary>
        [DataMember]
        public string ProductName { get; set; }

        /// <summary>
        /// Цена позиции заказа в баллах с учетом доставки, используется для показа в UI.
        /// </summary>
        [DataMember]
        public decimal PriceTotal { get; set; }

        /// <summary>
        /// Цена позиции заказа в рублях без учета доставки, используется для передачи в систему партнеру.
        /// </summary>
        [DataMember]
        public decimal PriceRur { get; set; }

        [DataMember]
        public FixedPrice FixedPrice { get; set; }
    }
}
