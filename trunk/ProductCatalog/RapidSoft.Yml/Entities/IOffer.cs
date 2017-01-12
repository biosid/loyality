namespace RapidSoft.YML.Entities
{
    public interface IOffer
    {
        string Id { get; set; }

        string DisplayName { get; }

        /// <summary>
        /// Ссылка на картинку соответствующего товарного предложения.
        /// Недопустимо давать ссылку на "заглушку", т.е. на картинку где написано 
        /// "картинка отсутствует" или на логотип магазина
        /// </summary>
        string[] Picture { get; set; }

        /// <summary>
        /// URL-адрес страницы товара
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// Цена товара
        /// </summary>
        decimal Price { get; set; }

        /// <summary>
        /// Категории предложения
        /// </summary>
        string[] Categories { get; set; }

        /// <summary>
        /// Описание товарного предложения
        /// </summary>
        string Description { get; set; }
    }
}