namespace RapidSoft.Loaylty.ProductCatalog.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using API.Entities;

    using API.InputParameters;

    using API.OutputResults;

    using DataSources;

    using DataSources.Interfaces;

    using DataSources.Repositories;

    using Interfaces;

    using RapidSoft.Loaylty.ProductCatalog.Entities;

    using Settings;

    internal class ProductsSearcher : IProductsSearcher
    {
        private readonly IProductAttributeRepository attributeRepository;
        private readonly IMechanicsProvider mechanicsProvider;
        private readonly IProductsDataSource productsDataSource;
        private readonly IPartnerRepository partnerRepository;
        private readonly IProductCategoriesRepository productCategoriesRepository;
        private readonly ICategoryPermissionRepository categoryPermissionRepository;
        private readonly IDeliveryRatesRepository deliveryRatesRepository;
        private readonly IProductsRepository productsRepository;

        public ProductsSearcher(
            IProductsDataSource productsDataSource = null,
            IProductAttributeRepository attributeRepository = null,
            IMechanicsProvider mechanicsProvider = null,
            IPartnerRepository partnerRepository = null,
            IProductCategoriesRepository productCategoriesRepository = null,
            ICategoryPermissionRepository categoryPermissionRepository = null,
            IDeliveryRatesRepository deliveryRatesRepository = null,
            IProductsRepository productsRepository = null)
        {
            this.productsDataSource = productsDataSource ?? new ProductsDataSource();
            this.attributeRepository = attributeRepository ?? new ProductAttributeRepository();
            this.mechanicsProvider = mechanicsProvider ?? new MechanicsProvider();
            this.partnerRepository = partnerRepository ?? new PartnerRepository();
            this.productCategoriesRepository = productCategoriesRepository ?? new ProductCategoryRepository();
            this.categoryPermissionRepository = categoryPermissionRepository ?? new CategoryPermissionRepository();
            this.deliveryRatesRepository = deliveryRatesRepository ?? new DeliveryRatesRepository();
            this.productsRepository = productsRepository ?? new ProductsRepository();
        }

        #region IProductsSearcher Members

        public SearchProductsResult AdminSearchProducts(AdminSearchProductsParameters parameters)
        {
            return productsDataSource.AdminSearchProducts(parameters);
        }

        public SearchProductsResult SearchPublicProducts(SearchProductsParameters parameters)
        {
            parameters.CountToTake = parameters.CountToTake.NormalizeByHeight(ApiSettings.MaxResultsCountProducts);
            
            var priceSql = mechanicsProvider.GetPriceSql(parameters.ClientContext);

            return productsDataSource.SearchPublicProducts(parameters, priceSql);
        }

        public PopularProduct[] GetPopularProducts(GetPopularProductParameters parameters)
        {
            var priceSql = mechanicsProvider.GetPriceSql(parameters.ClientContext);

            var productsParameters = new SearchProductsParameters
            {
                CountToTake = parameters.CountToTake,
                ClientContext = parameters.ClientContext,
                PopularProductType = parameters.PopularProductType,
                SortType = parameters.SortType
            };

            var res = productsDataSource.SearchPublicProducts(productsParameters, priceSql);

            if (!res.Success)
            {
                throw new Exception(string.Format("Ошибка поиска товаров:{0}", res.ResultDescription));
            }

            return res.Products.Select(MapPopularProduct).ToArray();
        }

        public string[] GetListOfAttributeValues(ProductAttributes attribute, int? categoryId)
        {
            return attributeRepository.GetAll(attribute, categoryId, ProductModerationStatuses.Applied);
        }

        public GetProductByIdItem GetProductById(string productId, Dictionary<string, string> clientContext)
        {
            return GetProductsByIds(new[] { productId }, clientContext).FirstOrDefault();
        }

        public GetProductByIdItem[] GetProductsByIds(string[] productIds, Dictionary<string, string> clientContext)
        {
            if (productIds == null)
            {
                throw new ArgumentNullException("productIds");
            }

            if (productIds.Length == 0)
            {
                return new GetProductByIdItem[0];
            }

            // ищем все продукты не вычисляя их цены
            var result = productsDataSource.AdminSearchProducts(
                new AdminSearchProductsParameters
                {
                    ProductIds = productIds,
                    CountToTake = ApiSettings.MaxResultsCountProducts,
                    UserId = ApiSettings.ClientSiteUserName
                });

            if (!result.Success)
            {
                throw new OperationException(string.Format("Ошибка GetProductsByIds {0}", result.ResultDescription), ResultCodes.UNKNOWN_ERROR);
            }

            if (result.Products.Length == 0)
            {
                return new GetProductByIdItem[0];
            }

            // определяем статус доступности найденных продуктов
            var resultItems = FillAvailabilityStatuses(result.Products, clientContext).ToArray();

            // вычисляем цены для продуктов
            var priceSql = mechanicsProvider.GetPriceSql(clientContext);
            
            var prices = productsDataSource.CalculateProductsPrices(resultItems.Select(item => item.Product.ProductId).ToArray(), priceSql);

            foreach (var item in resultItems)
            {
                var productId = item.Product.ProductId;

                var itemPrices = prices.FirstOrDefault(p => p.ProductId == productId);
                if (itemPrices == null)
                {
                    throw new OperationException(string.Format("Ошибка GetProductsByIds: цена для продукта {0} не вычислена", productId), ResultCodes.UNKNOWN_ERROR);
                }

                item.Product.PriceBase = itemPrices.PriceBase;
                item.Product.Price = itemPrices.PricesAction;

                // проверяем скидку
                item.Product.IsActionPrice = itemPrices.PriceBase - itemPrices.PricesAction >= 1;
            }

            return resultItems;
        }

        #endregion

        private PopularProduct MapPopularProduct(Product product)
        {
            var result = new PopularProduct
            {
                Product = product,
                PopularType = product.PopularType,
                ProductRate = product.ProductRate
            };

            result.ProductId = result.Product.ProductId;

            return result;
        }

        private IEnumerable<GetProductByIdItem> FillAvailabilityStatuses(IEnumerable<Product> products, Dictionary<string, string> clientContext)
        {
            var contextParser = new ClientContextParser();

            var kladr = contextParser.GetLocationKladrCode(clientContext);
            var targetAudiences = contextParser.GetAudienceIds(clientContext);
            var targetAudienceIds = string.IsNullOrEmpty(targetAudiences)
                                        ? null
                                        : targetAudiences.Split(';');

            var result = products.Select(p => new GetProductByIdItem
            {
                Product = p,
                AvailabilityStatus = ProductAvailabilityStatuses.Available
            })
                                 .ToArray();

            // 1. активность вознаграждений
            var items = FillProductIsNotActiveAvailabilityStatuses(result);

            // 2. статус модерации
            items = FillModerationNotAppliedAvailabilityStatuses(items);

            // 3. активность партнеров
            items = FillPartnerIsNotActiveAvailabilityStatuses(items);

            // 4. активность категории
            items = FillCategoryIsNotActiveAvailabilityStatuses(items);

            // 5. наличие доступа партнера к категории
            items = FillCategoryPermissionNotFoundAvailabilityStatuses(items);

            // 6. целевый аудитории вознаграждений
            items = FillTargetAudienceNotFoundAvailabilityStatuses(items, targetAudienceIds);

            // 7. возможность доставки вознаграждений партнером
            FillDeliveryRateNotFoundAvailabilityStatuses(items, kladr).ToArray();

            return result;
        }

        private IEnumerable<GetProductByIdItem> FillAvailabilityStatuses(IEnumerable<GetProductByIdItem> items, Predicate<Product> predicate, ProductAvailabilityStatuses status)
        {
            foreach (var item in items)
            {
                if (predicate(item.Product))
                {
                    item.AvailabilityStatus = status;
                }
                else
                {
                    yield return item;
                }
            }
        }

        private IEnumerable<GetProductByIdItem> FillProductIsNotActiveAvailabilityStatuses(IEnumerable<GetProductByIdItem> items)
        {
            return FillAvailabilityStatuses(
                items,
                p => p.Status != ProductStatuses.Active,
                ProductAvailabilityStatuses.ProductIsNotActive);
        }

        private IEnumerable<GetProductByIdItem> FillModerationNotAppliedAvailabilityStatuses(IEnumerable<GetProductByIdItem> items)
        {
            return FillAvailabilityStatuses(
                items,
                p => p.ModerationStatus != ProductModerationStatuses.Applied,
                ProductAvailabilityStatuses.ModerationNotApplied);
        }

        private IEnumerable<GetProductByIdItem> FillPartnerIsNotActiveAvailabilityStatuses(IEnumerable<GetProductByIdItem> items)
        {
            var itemsArray = items.ToArray();
            if (itemsArray.Length == 0)
            {
                return Enumerable.Empty<GetProductByIdItem>();
            }

            var partnerIds = itemsArray.Select(item => item.Product.PartnerId)
                                       .Distinct()
                                       .ToArray();
            var activePartnerIds = partnerRepository.FilterPartners(partnerIds, p => p.Status == PartnerStatus.Active);

            return FillAvailabilityStatuses(
                itemsArray,
                p => !activePartnerIds.Contains(p.PartnerId),
                ProductAvailabilityStatuses.PartnerIsNotActive);
        }

        private IEnumerable<GetProductByIdItem> FillCategoryIsNotActiveAvailabilityStatuses(IEnumerable<GetProductByIdItem> items)
        {
            var itemsArray = items.ToArray();
            if (itemsArray.Length == 0)
            {
                return Enumerable.Empty<GetProductByIdItem>();
            }

            var categoryIds = itemsArray.Select(item => item.Product.CategoryId)
                                        .Distinct()
                                        .ToArray();
            var deactivatedCategoryIds = productCategoriesRepository.GetDeactivated(categoryIds);

            return FillAvailabilityStatuses(
                itemsArray,
                p => deactivatedCategoryIds.Contains(p.CategoryId),
                ProductAvailabilityStatuses.CategoryIsNotActive);
        }

        private IEnumerable<GetProductByIdItem> FillCategoryPermissionNotFoundAvailabilityStatuses(IEnumerable<GetProductByIdItem> items)
        {
            var itemsArray = items.ToArray();
            if (itemsArray.Length == 0)
            {
                return Enumerable.Empty<GetProductByIdItem>();
            }

            var productIds = itemsArray.Select(item => item.Product.ProductId).ToArray();
            productIds = categoryPermissionRepository.GetProductIdsHavingPermissions(productIds);

            return FillAvailabilityStatuses(
                itemsArray,
                p => !productIds.Contains(p.ProductId),
                ProductAvailabilityStatuses.CategoryPermissionNotFound);
        }

        private IEnumerable<GetProductByIdItem> FillTargetAudienceNotFoundAvailabilityStatuses(IEnumerable<GetProductByIdItem> items, string[] targetAudienceIds)
        {
            var itemsArray = items.ToArray();
            if (itemsArray.Length == 0)
            {
                return Enumerable.Empty<GetProductByIdItem>();
            }

            var productIds = itemsArray.Select(item => item.Product.ProductId).ToArray();
            productIds = productsRepository.FilterByTargetAudiences(productIds, targetAudienceIds);

            return FillAvailabilityStatuses(
                itemsArray,
                p => !productIds.Contains(p.ProductId),
                ProductAvailabilityStatuses.TargetAudienceNotFound);
        }

        private IEnumerable<GetProductByIdItem> FillDeliveryRateNotFoundAvailabilityStatuses(IEnumerable<GetProductByIdItem> items, string kladr)
        {
            // Для товаров с доставкой по e-mail не следует выполнять проверку доступности доставки
            var itemsArray = items.Where(i => !i.Product.IsDeliveredByEmail).ToArray();
            if (itemsArray.Length == 0)
            {
                return Enumerable.Empty<GetProductByIdItem>();
            }

            var partnerIds = deliveryRatesRepository.GetDeliveringPartnerIds(kladr);

            return FillAvailabilityStatuses(
                itemsArray,
                p => !partnerIds.Contains(p.PartnerId),
                ProductAvailabilityStatuses.DeliveryRateNotFound);
        }
    }
}
