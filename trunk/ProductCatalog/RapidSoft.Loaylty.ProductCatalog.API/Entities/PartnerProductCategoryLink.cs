namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    public class PartnerProductCategoryLink
    {
        public PartnerProductCategoryLink()
        {
        }

        public PartnerProductCategoryLink(int partnerId, int productCategoryId, CategoryPath[] paths)
        {
            this.PartnerId = partnerId;
            this.ProductCategoryId = productCategoryId;
            this.Paths = paths;
        }

        /// <summary>
        /// Идентфикатор партнёра привязанной категории
        /// </summary>
        public int PartnerId { get; set; }

        /// <summary>
        /// Идентификатор привязанной категории рубрикатора
        /// </summary>
        public int ProductCategoryId { get; set; }

        /// <summary>
        /// Путь привязанной категории партнёра
        /// </summary>
        public CategoryPath[] Paths { get; set; }
    }
}