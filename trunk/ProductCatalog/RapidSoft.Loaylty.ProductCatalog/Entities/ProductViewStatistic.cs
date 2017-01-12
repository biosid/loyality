namespace RapidSoft.Loaylty.ProductCatalog.Entities
{
    using System;

    public class ProductViewStatistic
    {
        /// <summary>
        /// Идентификатор продукта.
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Количество просмотров данного товара, заданным клиентом
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// Дата последнего обновления записи
        /// </summary>
        public DateTime UpdatedDate
        {
            get;
            set;
        }
    }
}