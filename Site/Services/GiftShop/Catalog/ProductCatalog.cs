using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Vtb24.Site.Security.Models;
using Vtb24.Site.Services.GiftShop.Catalog.Models;
using Vtb24.Site.Services.GiftShop.Catalog.Models.Exceptions;
using Vtb24.Site.Services.GiftShop.Catalog.Models.Inputs;
using Vtb24.Site.Services.GiftShop.Catalog.Models.Outputs;
using Vtb24.Site.Services.Infrastructure;
using Vtb24.Site.Services.Infrastructure.CatalogParametersConfig;
using Vtb24.Site.Services.Infrastructure.CustomRecommedationsConfig;
using Vtb24.Site.Services.Models;
using Vtb24.Site.Services.ProductCatalogSearcherService;

namespace Vtb24.Site.Services.GiftShop.Catalog
{
    internal class ProductCatalog
    {
        static ProductCatalog()
        {
            var parametersSection = (CatalogParametersConfigSection)ConfigurationManager.GetSection("catalog_parameters");
            ParameterDefinitions = parametersSection.ParametersCollection
                                                     .Cast<CatalogParameterElement>()
                                                     .Select(p => new CatalogParameterDefinition
                                                     {
                                                         Name = p.Name,
                                                         NameInUrl = p.UrlArgument,
                                                         Unit = p.Unit
                                                     })
                                                     .ToDictionary(p => p.Name);

            var customRecommendationsSection = (CustomRecommendationsConfigSection)ConfigurationManager.GetSection("custom_recommendations");
            CustomRecommendations = customRecommendationsSection.ItemsCollection
                                                                .Cast<CustomRecommendationElement>()
                                                                .Select(e => new CustomRecommendation
                                                                {
                                                                    Kind = e.Kind,
                                                                    ProbabilityPercentage = Math.Min(Math.Max(e.ProbabilityPercentage, 0), 100),
                                                                    MinBalance = Math.Max(e.MinBalance, 0),
                                                                    Name = e.Name.Trim(),
                                                                    Url = e.Url,
                                                                    ImageUrl = e.ImageUrl.Trim(),
                                                                    PriceText = e.PriceText.Trim(),
                                                                })
                                                                .ToArray();
        }

        private static readonly Dictionary<string, CatalogParameterDefinition> ParameterDefinitions;
        private static readonly CustomRecommendation[] CustomRecommendations;

        public ProductCatalog(ClientPrincipal principal, IClientService client)
        {
            _principal = principal;
            _client = client;
        }

        private readonly ClientPrincipal _principal;
        private readonly IClientService _client;

        public CategoriesResult GetCategories(int? parentCategoryId, int? depth, PagingSettings paging)
        {
            using (var service = new CatalogSearcherClient())
            {
                var attributes = _client.GetMechanicsContext();
                
                var options = new GetPublicSubCategoriesParameters
                {
                    ParentId = parentCategoryId,
                    NestingLevel = depth,
                    CalcTotalCount = true,
                    CountToTake = paging.Take,
                    CountToSkip = paging.Skip,
                    ClientContext = attributes
                };

                var response = service.GetPublicSubCategories(options);

                AssertResponse(response.ResultCode, response.ResultDescription);

                var categories = response.Categories.Select(MappingsFromService.ToCatalogCategory).ToArray();
                var result = new CategoriesResult(categories, response.ChildrenCount, response.TotalCount ?? 0, paging);

                return result;
            }
        }

        public IEnumerable<CatalogProduct> GetPopularProducts(ProductPopularityType type, int? countToTake)
        {
            countToTake = countToTake ?? 30;

            using (var service = new CatalogSearcherClient())
            {
                var attributes = _client.GetMechanicsContext();
                var popularityType = MappingsToService.ToPopularProductTypes(type);

                var response = service.GetPopularProducts(_principal.ClientId, popularityType, attributes, countToTake);

                AssertResponse(response.ResultCode, response.ResultDescription);

                var products = response.PopularProducts.Select(MappingsFromService.ToCatalogProduct).ToArray();
                return products;
            }
        }

        public IEnumerable<CustomProduct> GetCustomPopularProducts(decimal balance)
        {
            return GetCustomRecommendations(CustomRecommendationKind.Popular, balance).ToArray();
        }

        public IEnumerable<CatalogProduct> GetRecommendedProducts(int? countToTake)
        {
            countToTake = countToTake ?? 30;

            using (var service = new CatalogSearcherClient())
            {
                var attributes = _client.GetMechanicsContext();

                var response = service.GetRecomendedProducts(attributes, countToTake);

                AssertResponse(response.ResultCode, response.ResultDescription);

                var products = response.Products.Select(MappingsFromService.ToCatalogProduct).ToArray();
                return products;
            }
        }

        public IEnumerable<CatalogProduct> GetRecommendedProductsForPriceRange(decimal minPrice, decimal maxPrice, int? countToTake)
        {
            countToTake = countToTake ?? 30;

            using (var service = new CatalogSearcherClient())
            {
                var attributes = _client.GetMechanicsContext();

                var response = service.GetRecomendedProductsByPriceRange(minPrice, maxPrice, attributes, countToTake);

                AssertResponse(response.ResultCode, response.ResultDescription);

                var products = response.Products.Select(MappingsFromService.ToCatalogProduct).ToArray();
                return products;
            }
        }

        public IEnumerable<CatalogProduct> GetRecommendedProductsForCategory(int categoryId, int? countToTake)
        {
            countToTake = countToTake ?? 30;

            using (var service = new CatalogSearcherClient())
            {
                var attributes = _client.GetMechanicsContext();

                var response = service.GetRecomendedProductsByCategoryId(categoryId, attributes, countToTake);

                AssertResponse(response.ResultCode, response.ResultDescription);

                var products = response.Products.Select(MappingsFromService.ToCatalogProduct).ToArray();
                return products;
            }
        }

        public IEnumerable<CustomProduct> GetCustomRecommendedProducts(decimal balance)
        {
            return GetCustomRecommendations(CustomRecommendationKind.Recommended, balance).ToArray();
        }

        public CatalogCategoryInfo GetCategoryInfo(int categoryId)
        {
            using (var service = new CatalogSearcherClient())
            {
                var attributes = _client.GetMechanicsContext();

                var options = new GetCategoryInfoParameters
                {
                    CategoryId = categoryId,
                    ClientContext = attributes
                };

                var response = service.GetCategoryInfo(options);

                AssertResponse(response.ResultCode, response.ResultDescription);

                return MappingsFromService.ToCatalogCategoryInfo(response);
            }
        }

        public CatalogProduct GetProduct(string productId, bool countAsView = false)
        {            
            using (var service = new CatalogSearcherClient())
            {
                var attributes = _client.GetMechanicsContext();

                var options = new GetProductByIdParameters
                {
                    ProductId = productId,
                    ClientContext = attributes,
                    ClientId = _principal.ClientId,
                    RegisterView = countAsView
                };

                var response = service.GetProductById(options);

                AssertResponse(response.ResultCode, response.ResultDescription);

                var product = MappingsFromService.ToCatalogProduct(response.Product, response.AvailabilityStatus, response.ViewsCount);

                return product;
            }
        }


        public SearchResult Search(SearchCriteria criteria, PagingSettings paging)
        {
            using (var service = new CatalogSearcherClient())
            {
                var attributes = _client.GetMechanicsContext();

                var options = new SearchPublicProductsParameters
                {
                    // критерий поиска
                    SearchTerm = criteria.SearchTerm,
                    SortType = MappingsToService.ToSortTypes(criteria.Sorting),
                    IncludeSubCategory = true,
                    MaxPrice = criteria.MaxPrice,
                    MinPrice = criteria.MinPrice,
                    ParentCategories = criteria.Categories.MaybeToArray(),
                    PartnerIds = criteria.CatalogPartners.MaybeToArray(),
                    Vendors = criteria.Vendors.MaybeToArray(),
                    IsActionPrice = criteria.IsActionPrice,
                    ReturnEmptyVendorProducts = criteria.ReturnEmptyVendorProducts,
                    MinInsertedDate = criteria.MinAddedToCatalogDate,
                    ProductParams = criteria.Parameters.MaybeSelect(MappingsToService.ToProductParam).MaybeToArray(),

                    // контекст пользователя
                    ClientContext = attributes,

                    // пейджинг
                    CountToTake = paging.Take,
                    CountToSkip = paging.Skip
                };

                var response = service.SearchPublicProducts(options);

                AssertResponse(response.ResultCode, response.ResultDescription);

                var result = new SearchResult(
                    response.Products.Select(MappingsFromService.ToCatalogProduct).ToArray(),
                    (int)(response.TotalCount ?? 0),
                    response.MaxPrice ?? 0,
                    paging
                    );

                return result;
            }
        }

        public SubCatalogInfo GetFilterMetaData(int? parentCategoryId = null)
        {
            using (var service = new CatalogSearcherClient())
            {
                var attributes = _client.GetMechanicsContext();

                var options = new GetFilterMetaDataParameters
                {
                    Attribute = ProductAttributes.Vendor,
                    CategoryId = parentCategoryId,
                    ClientContext = attributes
                };

                // REVIEW: MaxPrice самое место быть тут.
                var response = service.GetFilterMetaData(options);

                AssertResponse(response.ResultCode, response.ResultDescription);

                var result = new SubCatalogInfo
                {
                    Vendors = response.Attributes
                };

                return result;
            }
        }

        public CatalogParameter[] GetCategoryParameters(int categoryId)
        {
            using (var service = new CatalogSearcherClient())
            {
                var attributes = _client.GetMechanicsContext();

                var parameters = new CategoryProductParamsParameters
                {
                    CategoryId = categoryId,
                    ClientContext = attributes,
                    ProductParams = ParameterDefinitions.Values
                                                        .Select(MappingsToService.ToCategoryProductParamsParameter)
                                                        .ToArray()
                };

                var response = service.GetCategoryProductParams(parameters);

                AssertResponse(response.ResultCode, response.ResultDescription);

                return MappingsFromService.ToCatalogParameters(response.ProductParamResult, ParameterDefinitions)
                                          .ToArray();
            }
        }

        public CatalogPartner[] GetPartners()
        {
            using (var service = new CatalogSearcherClient())
            {
                var response = service.GetAllPartners();

                AssertResponse(response.ResultCode, response.ResultDescription);

                return response.Partners.MaybeSelect(MappingsFromService.ToCatalogPartner).MaybeToArray();
            }
        }

        private static readonly Random Random = new Random();
        private static readonly object RandomLock = new object();

        private static int RandomNext(int maxValue)
        {
            lock (RandomLock)
            {
                return Random.Next(maxValue);
            }
        }

        private static IEnumerable<CustomProduct> GetCustomRecommendations(CustomRecommendationKind kind, decimal balance)
        {
            var products = CustomRecommendations.Where(r => r.Kind == kind &&
                                                            (r.MinBalance == 0 || balance >= r.MinBalance) &&
                                                            RandomNext(100) < r.ProbabilityPercentage)
                                                .Select(r => new CustomProduct
                                                {
                                                    Name = r.Name,
                                                    Url = r.Url,
                                                    ImageUrl = r.ImageUrl,
                                                    PriceText = r.PriceText
                                                });

            return products;
        }

        private static void AssertResponse(int code, string message)
        {
            switch (code)
            {
                case 0:
                    return;
                case 2 :
                    throw new ProductNotFoundException(code, message);
                case 500:
                    throw new CategoryNotFoundException(code, message);
            }
            throw new CatalogServiceException(code, message);
        }
    }
}