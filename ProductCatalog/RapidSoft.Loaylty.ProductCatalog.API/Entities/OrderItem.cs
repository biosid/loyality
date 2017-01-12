namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    using System.Runtime.Serialization;

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
        /// Товар
        /// </summary>
        [DataMember]
        public Product Product { get; set; }

        /// <summary>
        /// Цена за единицу товара в рублях
        /// </summary>
        [DataMember]
        public decimal PriceRur { get; set; }

        /// <summary>
        /// Цена за единицу товара в бонусах
        /// </summary>
        [DataMember]
        public decimal PriceBonus { get; set; }

        /// <summary>
        /// Цена элемента корзины в рублях (цена за единицу, умноженная на количество)
        /// </summary>
        [DataMember]
        public decimal AmountPriceRur { get; set; }

        /// <summary>
        /// Цена элемента корзины в бонусах (цена за единицу, умноженная на количество)
        /// </summary>
        [DataMember]
        public decimal AmountPriceBonus { get; set; }

        public FixedPrice FixedPrice
        {
            get;
            set;
        }
    }
}