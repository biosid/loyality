namespace RapidSoft.Loaylty.ProductCatalog.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using API;

    using API.Entities;

    using API.InputParameters;

    using API.OutputResults;

    using AutoMapper;

    using DataSources;
    using DataSources.Repositories;

    using Interfaces;

    using Logging;

    using Logging.Wcf;

    using Monitoring;

    using RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces;

    using Settings;
    using Vtb24.Common.Configuration;

    /// <summary>
    ///     Фасад для поисковых методов
    /// </summary>
    [LoggingBehavior]
    public class CatalogSearcher : SupportService, ICatalogSearcher
    {
        private readonly IProductViewStatisticRepository productViewStatisticDataSource;
        private readonly IPopularProductsRepository popularProductsRepository;
        private readonly ICategoriesSearcher categoriesSearcher;
        private readonly IPartnersSearcher partnersSearcher;
        private readonly IProductsSearcher productsSearcher;
        private readonly IProcessingProvider processingProvider;

        private readonly ILog log = LogManager.GetLogger(typeof(CatalogSearcher));

        static CatalogSearcher()
        {
            Mapper.CreateMap<SearchPublicProductsParameters, SearchProductsParameters>();
        }

        public CatalogSearcher(
            IProductsSearcher productsSearcher = null,
            IPartnersSearcher partnersSearcher = null,
            ICategoriesSearcher categoriesSearcher = null,
            IProductViewStatisticRepository productViewStatisticDataSource = null,
            IPopularProductsRepository popularProductsRepository = null,
            IProcessingProvider processingProvider = null)
        {
            this.productsSearcher = productsSearcher ?? new ProductsSearcher();
            this.partnersSearcher = partnersSearcher ?? new PartnersSearcher();
            this.categoriesSearcher = categoriesSearcher ?? new CategoriesSearcher();
            this.productViewStatisticDataSource = productViewStatisticDataSource ?? new ProductViewStatisticRepository();
            this.popularProductsRepository = popularProductsRepository ?? new PopularProductsRepository();
            this.processingProvider = processingProvider ?? new ProcessingProvider();
        }

        public CatalogSearcher()
        {
            productViewStatisticDataSource = new ProductViewStatisticRepository();
            popularProductsRepository = new PopularProductsRepository();
            productsSearcher = new ProductsSearcher();
            categoriesSearcher = new CategoriesSearcher();
            partnersSearcher = new PartnersSearcher();
            processingProvider = new ProcessingProvider();
        }

        #region ICatalogSearcher Members

        public GetAllPartnersResult GetAllPartners()
        {
            try
            {
                var partners = partnersSearcher.Search();

                var result = new GetAllPartnersResult();
                result.Partners = partners;
                result.ResultCode = partners == null ? ResultCodes.UNKNOWN_ERROR : ResultCodes.SUCCESS;

                if (result.ResultCode == ResultCodes.UNKNOWN_ERROR)
                {
                    result.ResultDescription = "Произошла непредвиденная ошибка";
                }

                return result;
            }
            catch (Exception e)
            {
                log.Error("Ошибка при получении партнеров", e);
                return ServiceOperationResult.BuildErrorResult<GetAllPartnersResult>(e);
            }
        }

        public GetSubCategoriesResult GetPublicSubCategories(GetPublicSubCategoriesParameters parameters)
        {
            try
            {
                return categoriesSearcher.GetPublicSubCategories(parameters);
            }
            catch (Exception e)
            {
                log.Error("Ошибка при получении публичных суб-категорий", e);
                return ServiceOperationResult.BuildErrorResult<GetSubCategoriesResult>(e);
            }
        }

        public GetCategoryInfoResult GetCategoryInfo(GetCategoryInfoParameters parameters)
        {
            try
            {
                return categoriesSearcher.GetCategoryInfo(parameters);
            }
            catch (Exception e)
            {
                log.Error("Ошибка при получении информации о категорий", e);
                return ServiceOperationResult.BuildErrorResult<GetCategoryInfoResult>(e);
            }
        }

        public SearchProductsResult SearchPublicProducts(SearchPublicProductsParameters parameters)
        {
            try
            {
                Utils.CheckArgument(parameters == null, "parameters");
                Utils.CheckArgument(parameters.ClientContext == null, "parameters.ClientContext");
                Utils.CheckArgument(
                    !parameters.ClientContext.ContainsKey(ClientContextParser.LocationKladrCodeKey), 
                    string.Format("{0} not found", ClientContextParser.LocationKladrCodeKey));

                var productsParameters = Mapper.Map<SearchPublicProductsParameters, SearchProductsParameters>(parameters);
                productsParameters.CalcTotalCount = true;

                return productsSearcher.SearchPublicProducts(productsParameters);
            }
            catch (Exception e)
            {
                log.Error("Ошибка при поиске публичных продуктов/товаров", e);
                return ServiceOperationResult.BuildErrorResult<SearchProductsResult>(e);
            }
        }

        public GetProductResult GetProductById(GetProductByIdParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            if (parameters.ProductId == null)
            {
                throw new ArgumentNullException("parameters.ProductId");
            }

            try
            {
                var item = productsSearcher.GetProductById(parameters.ProductId, parameters.ClientContext);

                if (item == null || item.Product == null)
                {
                    return new GetProductResult
                    {
                        ResultCode = ResultCodes.NOT_FOUND,
                        ResultDescription = "Предложение не найдено"
                    };
                }

                var viewsCount = popularProductsRepository.GetRate(PopularProductTypes.MostViewedOfAllTime, parameters.ProductId);

                var result = new GetProductResult
                {
                    ResultCode = ResultCodes.SUCCESS,
                    Product = item.Product,
                    AvailabilityStatus = item.AvailabilityStatus,
                    ViewsCount = viewsCount
                };

                // NOTE: статистика посещений сейчас рассчитывается по логам IIS отдельным сервисом
                // if (parameters.RegisterView.HasValue && parameters.RegisterView.Value && parameters.ClientId != null)
                // {
                //     productViewStatisticDataSource.RegisterProductView(parameters.ClientId, parameters.ProductId);
                // }
                return result;
            }
            catch (Exception e)
            {
                log.Error("Ошибка при получении продукта/товара", e);
                return ServiceOperationResult.BuildErrorResult<GetProductResult>(e);
            }
        }

        public GetPopularProductsResult GetPopularProducts(
           string clientId,
           PopularProductTypes popularProductType,
           Dictionary<string, string> clientContext,
           int? countToTake)
        {
            try
            {
                var countToTakeInternal = countToTake.NormalizeByHeight(ApiSettings.MaxResultsCountPopularProducts);

                var @params = new GetPopularProductParameters()
                {
                    CountToTake = countToTakeInternal,
                    ClientContext = clientContext,
                    PopularProductType = popularProductType,
                    SortType = SortTypes.ByPopularityDesc,
                };

                var products = productsSearcher.GetPopularProducts(@params);

                return new GetPopularProductsResult
                {
                    ResultCode = ResultCodes.SUCCESS,
                    PopularProducts = products
                };
            }
            catch (Exception e)
            {
                log.Error("Ошибка GetPopularProducts", e);
                return ServiceOperationResult.BuildErrorResult<GetPopularProductsResult>(e);
            }
        }

        public GetRecomendedProductsResult GetRecomendedProducts(
           Dictionary<string, string> clientContext,
           int? countToTake)
        {
            try
            {
                var countToTakeInternal = countToTake.NormalizeByHeight(ApiSettings.MaxResultsCountRecommendedProducts);

                var result = new GetRecomendedProductsResult();

                var searchProductsParameters = new SearchProductsParameters
                {
                    CalcTotalCount = false,
                    ClientContext = clientContext,
                    SortType = SortTypes.Recommended,
                    CountToTake = countToTakeInternal
                };

                var searchResult = productsSearcher.SearchPublicProducts(searchProductsParameters);

                result.Products = searchResult.Products;
                return result;
            }
            catch (Exception e)
            {
                log.Error("Ошибка при получении рекомендуемых продуктов ", e);
                return ServiceOperationResult.BuildErrorResult<GetRecomendedProductsResult>(e);
            }
        }

        public GetRecomendedProductsResult GetRecomendedProductsByPriceRange(
            decimal minPrice,
            decimal maxPrice,
            Dictionary<string, string> clientContext,
            int? countToTake)
        {
            if (minPrice >= maxPrice)
            {
                throw new ArgumentException("minPrice must be less then maxPrice");
            }

            try
            {
                var countToTakeInternal = countToTake.NormalizeByHeight(ApiSettings.MaxResultsCountRecommendedProducts);

                var result = new GetRecomendedProductsResult();

                var searchProductsParameters = new SearchProductsParameters
                {
                    SortType = SortTypes.RecommendedByPriceRange,
                    MinRecommendedPrice = minPrice,
                    MaxRecommendedPrice = maxPrice,
                    CalcTotalCount = false,
                    ClientContext = clientContext,
                    CountToTake = countToTakeInternal
                };

                var searchResult = productsSearcher.SearchPublicProducts(searchProductsParameters);

                result.Products = searchResult.Products;
                return result;
            }
            catch (Exception e)
            {
                log.Error("Ошибка при получении рекомендуемых продуктов ", e);
                return ServiceOperationResult.BuildErrorResult<GetRecomendedProductsResult>(e);
            }
        }

        public GetRecomendedProductsResult GetRecomendedProductsByCategoryId(
            int categoryId,
            Dictionary<string, string> clientContext,
            int? countToTake)
        {
            try
            {
                var countToTakeInternal = countToTake.NormalizeByHeight(ApiSettings.MaxResultsCountRecommendedProducts);

                var result = new GetRecomendedProductsResult();

                var searchProductsParameters = new SearchProductsParameters
                {
                    CalcTotalCount = false,
                    ClientContext = clientContext,
                    SortType = SortTypes.RecommendedByCategory,
                    CategoryIdToRecommendFor = categoryId,
                    CountToTake = countToTakeInternal
                };

                var searchResult = productsSearcher.SearchPublicProducts(searchProductsParameters);

                result.Products = searchResult.Products;
                return result;
            }
            catch (Exception e)
            {
                log.Error("Ошибка при получении рекомендуемых продуктов для категории", e);
                return ServiceOperationResult.BuildErrorResult<GetRecomendedProductsResult>(e);
            }
        }

        public GetFilterMetaDataResult GetFilterMetaData(GetFilterMetaDataParameters parameters)
        {
            try
            {
                var result = new GetFilterMetaDataResult();

                result.Attributes = productsSearcher.GetListOfAttributeValues(
                    parameters.Attribute,
                    parameters.CategoryId);
                result.ResultCode = ResultCodes.SUCCESS;

                return result;
            }
            catch (Exception e)
            {
                log.Error(
                    "Ошибка при получении списка возможных значений атрибутов вознаграждений для дальнейшего использования в фильтрации ",
                    e);
                return ServiceOperationResult.BuildErrorResult<GetFilterMetaDataResult>(e);
            }
        }

        public CategoryProductParamsResult GetCategoryProductParams(CategoryProductParamsParameters parameters)
        {
            try
            {
                Utils.CheckArgument(parameters.CategoryId <= 0, "invalid category id");
                Utils.CheckArgument(parameters.ClientContext == null, "parameters.ClientContext");

                Utils.CheckArgument(
                    !parameters.ClientContext.ContainsKey(ClientContextParser.LocationKladrCodeKey),
                    string.Format("{0} not found", ClientContextParser.LocationKladrCodeKey));

                return GetCategoryProductParamsInternals(parameters);
            }
            catch (Exception e)
            {
                log.Error("Ошибка при получении дополнительных аттрубутов для категории", e);
                return ServiceOperationResult.BuildErrorResult<CategoryProductParamsResult>(e);
            }
        }

        #endregion

        private CategoryProductParamsResult GetCategoryProductParamsInternals(CategoryProductParamsParameters parameters)
        {
            var result = new CategoryProductParamsResult()
            {
                ResultCode = ResultCodes.SUCCESS
            };

            var searchResult = productsSearcher.SearchPublicProducts(new SearchProductsParameters()
            {
                ParentCategories = new[]
                    {
                        parameters.CategoryId
                    },
                ClientContext = parameters.ClientContext
            });

            var @params = searchResult.Products.Where(p => p.Param != null).SelectMany(p => p.Param);

            if (parameters.ProductParams != null && parameters.ProductParams.Any())
            {
                if (parameters.ProductParams.Any(p => !string.IsNullOrEmpty(p.Name)))
                {
                    var filterNames = parameters.ProductParams.Select(p => p.Name);
                    @params = @params.Where(p => filterNames.Contains(p.Name));
                }

                if (parameters.ProductParams.Any(p => !string.IsNullOrEmpty(p.Unit)))
                {
                    var filterUnits = parameters.ProductParams.Select(p => p.Unit);
                    @params = @params.Where(p => filterUnits.Contains(p.Unit));
                }
            }

            var res = @params.Distinct(new ProductParamComparer()).ToLookup(e => e.Name, e => e.Value);

            var list = new List<ProductParamResult>();

            foreach (var r in res.Select(r => r.Key))
            {
                list.Add(new ProductParamResult()
                {
                    Name = r,
                    Values = res[r].ToArray()
                });
            }

            result.ProductParamResult = list.ToArray();
            return result;
        }
    }
}