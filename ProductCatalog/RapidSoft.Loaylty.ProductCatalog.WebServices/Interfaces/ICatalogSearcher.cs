namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Interfaces
{
    using System.ServiceModel;

    using Monitoring;

    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog;
    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Input;
    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog.Output;
    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Output;

    [ServiceContract]
    public interface ICatalogSearcher : ISupportService
    {
        /// <summary>
        /// Получение публичных категорий
        /// </summary>
        [OperationContract]
        SubCategoriesResult GetPublicSubCategories(GetPublicSubCategoriesParameters parameters);

        /// <summary>
        /// Получение категорий
        /// </summary>
        [OperationContract]
        CategoryInfoResult GetCategoryInfo(GetCategoryInfoParameters parameters);

        /// <summary>
        /// Поиск публичных товаров
        /// </summary>
        [OperationContract]
        PublicProductsResult SearchPublicProducts(SearchPublicProductsParameters parameters);

        /// <summary>
        /// Поиск всех товаров
        /// </summary>
        [OperationContract]
        ValueResult<PublicProduct> GetProductById(GetProductByIdParameters parameters);

        /// <summary>
        /// Получение перечня значений атрибутов
        /// </summary>
        [OperationContract]
        ArrayResult<string> GetFilterMetaData(GetFilterMetaDataParameters parameters);

        /// <summary>
        /// Получение списка дополнительных аттрибутов продуктов для категории
        /// </summary>
        [OperationContract]
        ArrayResult<ProductParameterValues> GetCategoryProductParams(GetCategoryProductParamsParameters parameters);

        /// <summary>
        /// Получение популярных товаров
        /// </summary>
        [OperationContract]
        ArrayResult<PopularProduct> GetPopularProducts(GetPopularProductsParameters parameters);

        /// <summary>
        /// Получение рекомендуемых товаров
        /// </summary>
        [OperationContract]
        ArrayResult<PublicProduct> GetRecommendedProducts(GetRecommendedProductsParameters parameters);
    }
}
