namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces
{
    using API.Entities;
    using API.InputParameters;
    using API.OutputResults;

    /// <summary>
    /// Интефейс репозитория категории товаров.
    /// </summary>
    public interface IProductCategoriesRepository
    {
        ResultBase ChangeCategoriesStatus(string userId, int[] categoryIds, ProductCategoryStatuses status);

        ResultBase DeleteCategory(string userId, int id);

        ProductCategory Add(ProductCategory category);

        ResultBase MoveCategory(MoveCategoryParameters parameters);

        ProductCategory GetById(int id);

        void UpdateParent(string userId, int categoryId, int? newParentId);

        int[] GetDeactivated(int[] ids);
    }
}
