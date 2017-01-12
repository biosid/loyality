namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    public enum CategoryPositionTypes
    {
        /// <summary>
        /// Вставить в категорию последней
        /// </summary>
        Append = 0,

        /// <summary>
        /// Вставить в категорию первой
        /// </summary>
        Prepend = 1,

        /// <summary>
        /// Вставить перед категорией
        /// </summary>
        Before = 2,

        /// <summary>
        /// Вставить после категории
        /// </summary>
        After = 3
    }
}