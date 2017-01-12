namespace RapidSoft.Loaylty.ProductCatalog.Entities
{
    /// <summary>
    /// Типы ошибок импорта продукта
    /// </summary>
    public enum ProductImportErrorCodes
    {
        /// <summary>
        /// Партнерская категория не найдена.
        /// </summary>
        PartnerCategoryNotFound = 0,

        /// <summary>
        /// Найдено несколько маппингов для партнерской категории.
        /// </summary>
        ManyMappings = 1,

        /// <summary>
        /// Маппинг не найден.
        /// </summary>
        MappingNotFound = 2,

        /// <summary>
        /// В задаче импорта указано что вес обязателен, но в YML-файле вес не указан.
        /// </summary>
        WeightRequiredButNotFound = 3,

        /// <summary>
        /// Предложение содержит более одного параметра Вес. Не известно какой из них брать.
        /// </summary>
        WeightDuplicated = 4
    }
}