namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces
{
    using API.InputParameters;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    /// <summary>
    /// The ProductAttributeRepository interface.
    /// </summary>
    public interface IProductAttributeRepository
    {
        /// <summary>
        /// The get all.
        /// </summary>
        /// <param name="attribute">
        /// The attribute.
        /// </param>
        /// <param name="categoryId">
        /// The category id.
        /// </param>
        /// <param name="moderationStatus">
        /// The moderation Statuses.
        /// </param>
        /// <returns>
        /// The <see cref="string[]"/>.
        /// </returns>
        string[] GetAll(ProductAttributes attribute, int? categoryId, ProductModerationStatuses? moderationStatus);
    }
}