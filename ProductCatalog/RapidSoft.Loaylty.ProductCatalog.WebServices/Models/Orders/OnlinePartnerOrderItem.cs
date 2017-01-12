namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Orders
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Элемент, содержащий информацию о строке заказа. 
    /// </summary>
    [DataContract]
    public class OnlinePartnerOrderItem
    {
        /// <summary>
        /// Идентификатор строки заказа.
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Идентификатор товара/услуги в информационной системе Партнера.
        /// </summary>
        [DataMember]
        public string ArticleId { get; set; }

        /// <summary>
        /// Наименование товара/услуги.
        /// </summary>
        [DataMember]
        public string ArticleName { get; set; }

        /// <summary>
        /// Количество данного товара.
        /// </summary>
        [DataMember]
        public int Amount { get; set; }

        /// <summary>
        /// Цена за единицу товара, в рублях.
        /// </summary>
        [DataMember]
        public decimal Price { get; set; }

        /// <summary>
        /// Цена за единицу товара, в баллах.
        /// </summary>
        [DataMember]
        public int BonusPrice { get; set; }

        /// <summary>
        /// Вес единицы товара, в граммах.
        /// </summary>
        [DataMember]
        public int Weight { get; set; }

        /// <summary>
        /// Комментарий к строке заказа для отображения пользователю.
        /// </summary>
        [DataMember]
        public string Comment { get; set; }
    }
}
