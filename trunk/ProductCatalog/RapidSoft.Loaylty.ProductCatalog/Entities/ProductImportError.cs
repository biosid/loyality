namespace RapidSoft.Loaylty.ProductCatalog.Entities
{
    /// <summary>
    /// Ошибка импорта продукта
    /// </summary>
    public class ProductImportError
    {
        public ProductImportError(string partnerProductId, ProductImportErrorCodes code, string partnerCategoryId)
        {
            this.PartnerProductId = partnerProductId;
            this.Code = code;
            this.PartnerCategoryId = partnerCategoryId;
        }

        /// <summary>
        /// Идентификатор продукта в yml-файле.
        /// </summary>
        public string PartnerProductId { get; set; }

        /// <summary>
        /// Код ошибки
        /// </summary>
        public ProductImportErrorCodes Code { get; set; }

        /// <summary>
        /// Идентификатор партнерской категории.
        /// </summary>
        public string PartnerCategoryId { get; set; }
    }
}