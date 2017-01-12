namespace RapidSoft.Loaylty.ProductCatalog.Entities
{
    /// <summary>
    /// Проекция маппинга партнерских категорий на категории рубрикатора с учетом разрешний.
    /// </summary>
    public class CategoryMappingProjection
    {
        /// <summary>
        /// Идентификатор категории рубрикатора.
        /// </summary>
        public int? ProductCategoryId { get; set; }

        /// <summary>
        /// Наличие разрешения на категории рубрикатора.
        /// </summary>
        public bool Permission { get; set; }

        /// <summary>
        /// Идентификатор категории партнера.
        /// </summary>
        public string PartnerCategoryId { get; set; }

        /// <summary>
        /// Название категории партнера.
        /// </summary>
        public string PartnerCategoryName { get; set; }

        /// <summary>
        /// Путь категории партнера.
        /// </summary>
        public string PartnerCategoryNamePath { get; set; }
    }
}