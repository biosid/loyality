namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    /// <summary>
    /// Элемент, содержащий информацию о строке заказа. 
    /// </summary>
    public class CreateOrderFromOnlinePartnerItem
    {
        /// <summary>
        /// Идентификатор строки заказа.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Идентификатор товара/услуги в информационной системе Партнера.
        /// </summary>
        public string ArticleId { get; set; }

        /// <summary>
        /// Наименование товара/услуги.
        /// </summary>
        public string ArticleName { get; set; }

        /// <summary>
        /// Количество данного товара.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Цена за единицу товара, в рублях.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Цена за единицу товара, в баллах.
        /// </summary>
        public int BonusPrice { get; set; }

        /// <summary>
        /// Вес единицы товара, в граммах.
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// Комментарий к строке заказа для отображения пользователю.
        /// </summary>
        public string Comment { get; set; }
    }
}