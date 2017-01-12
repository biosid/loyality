namespace RapidSoft.Loaylty.ProductCatalog.Interfaces
{
    using API.InputParameters;
    using API.OutputResults;

    public interface ICategoriesSearcher
    {
        GetSubCategoriesResult GetPublicSubCategories(GetPublicSubCategoriesParameters parameters);

        GetSubCategoriesResult GetAllSubCategories(GetAllSubCategoriesParameters parameters);

        GetCategoryInfoResult GetCategoryInfo(GetCategoryInfoParameters parameters);
    }
}
