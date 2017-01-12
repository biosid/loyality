namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    /// <summary>
    /// Элемент, содержащий информацию о строке заказа. 
    /// </summary>
    public class CustomOrderItem
    {
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
        public decimal PriceRur { get; set; }

        /// <summary>
        /// Цена за единицу товара, в баллах.
        /// </summary>
        public decimal PriceBonus { get; set; }
    }
}