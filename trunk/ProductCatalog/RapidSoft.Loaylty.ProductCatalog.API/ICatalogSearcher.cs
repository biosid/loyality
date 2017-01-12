namespace RapidSoft.Loaylty.ProductCatalog.API
{
    using System.Collections.Generic;
    using System.ServiceModel;

    using Monitoring;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.API.InputParameters;
    using RapidSoft.Loaylty.ProductCatalog.API.OutputResults;

    [ServiceContract]
    public interface ICatalogSearcher : ISupportService
    {
        /// <summary>
        /// Получение всех партнеров-поставщиков
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        GetAllPartnersResult GetAllPartners();

        /// <summary>
        /// Получение публичных категорий
        /// </summary>
        /// <param name="parameters">Критерии поиска</param>
        /// <returns></returns>
        [OperationContract]
        GetSubCategoriesResult GetPublicSubCategories(GetPublicSubCategoriesParameters parameters);

        /// <summary>
        /// Получение категорий
        /// </summary>
        /// <param name="parameters">Критерии поиска</param>
        /// <returns></returns>
        [OperationContract]
        GetCategoryInfoResult GetCategoryInfo(GetCategoryInfoParameters parameters);

        /// <summary>
        /// Поиск публичных товаров
        /// </summary>
        /// <param name="parameters">Критерии поиска</param>
        /// <returns></returns>
        [OperationContract]
        SearchProductsResult SearchPublicProducts(SearchPublicProductsParameters parameters);

        /// <summary>
        /// Поиск всех товаров
        /// </summary>
        /// <param name="parameters">Контекст поиска товара по Id</param>
        /// <returns></returns>
        [OperationContract]
        GetProductResult GetProductById(GetProductByIdParameters parameters);

        /// <summary>
        /// Получение перечня значений атрибутов
        /// </summary>
        /// <param name="parameters">Контекст выбора данных для фильтра</param>
        /// <returns></returns>
        [OperationContract]
        GetFilterMetaDataResult GetFilterMetaData(GetFilterMetaDataParameters parameters);

        /// <summary>
        /// Получение популярных товаров
        /// </summary>
        /// <param name="clientId">Идентификатор клиента</param>
        /// <param name="popularProductType">Вид популярности</param>
        /// <param name="clientContext">Контекст клиента</param>
        /// <param name="countToTake"></param>
        /// <returns></returns>
        [OperationContract]
        GetPopularProductsResult GetPopularProducts(
            string clientId,
            PopularProductTypes popularProductType,
            Dictionary<string, string> clientContext,
            int? countToTake = null);

        /// <summary>
        /// Получение списка дополнительных аттрибутов продуктов для категории
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        CategoryProductParamsResult GetCategoryProductParams(CategoryProductParamsParameters parameters);

        /// <summary>
        /// Получение рекомендуемых товаров
        /// </summary>
        [OperationContract]
        GetRecomendedProductsResult GetRecomendedProducts(
            Dictionary<string, string> clientContext, int? countToTake);

        /// <summary>
        /// Получение рекомендуемых товаров для данного диапазона цен
        /// </summary>
        [OperationContract]
        GetRecomendedProductsResult GetRecomendedProductsByPriceRange(
            decimal minPrice, decimal maxPrice, Dictionary<string, string> clientContext, int? countToTake);

        /// <summary>
        /// Получение рекомендуемых товаров для данной категории
        /// </summary>
        [OperationContract]
        GetRecomendedProductsResult GetRecomendedProductsByCategoryId(
            int categoryId, Dictionary<string, string> clientContext, int? countToTake);
    }
}
