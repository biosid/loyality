namespace RapidSoft.Loaylty.ProductCatalog.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using API;
    using API.Entities;
    using API.InputParameters;
    using API.OutputResults;

    using Configuration;

    using DataSources;
    using DataSources.Interfaces;
    using DataSources.Repositories;

    using Extensions;

    using Interfaces;

    using Logging;

    using Logging.Wcf;

    using Monitoring;

    using PartnersConnector.WsClients.PartnersOrderManagementService;

    using Settings;

    using VTB24.BankConnector.WsClients.BankConnectorService;

    /// <summary>
    ///     Сервис "Корзина клиента".
    /// </summary>
    [LoggingBehavior]
    public class BasketService : SupportService, IBasketService
    {
        private static readonly int SaveOrderAttemptMinHour = ConfigHelper.SaveOrderAttemptMinHour;
        private static readonly int SaveOrderAttemptMaxHour = ConfigHelper.SaveOrderAttemptMaxHour;

        private readonly IBasketItemRepository basketItemRepository;

        private readonly IProductsSearcher productsSearcher;

        private readonly IPartnerConnectorProvider partnerConnectorProvider;

        private readonly IMechanicsProvider mechanicsProvider;

        private readonly IPartnerRepository partnerRepository;

        private readonly ILog log = LogManager.GetLogger(typeof(BasketService));

        /// <summary>
        /// Initializes a new instance of the <see cref="BasketService" /> class.
        /// </summary>
        public BasketService()
        {
            basketItemRepository = new BasketItemRepository(DataSourceConfig.ConnectionString);
            productsSearcher = new ProductsSearcher();
            partnerConnectorProvider = new PartnerConnectorProvider();
            mechanicsProvider = new MechanicsProvider();
            partnerRepository = new PartnerRepository();
        }

        public BasketService(
            IBasketItemRepository basketItemRepository, 
            IProductsSearcher productsSearcher, 
            IPartnerConnectorProvider partnerConnectorProvider,
            IMechanicsProvider mechanicsProvider,
            IPartnerRepository partnerRepository)
        {
            basketItemRepository.ThrowIfNull("basketItemRepository");
            this.basketItemRepository = basketItemRepository;
            this.productsSearcher = productsSearcher;
            this.partnerConnectorProvider = partnerConnectorProvider;
            this.mechanicsProvider = mechanicsProvider;
            this.partnerRepository = partnerRepository;
        }

        #region IBasketService Members

        /// <summary>
        /// Добавление товара в корзину клиента.
        /// </summary>
        /// <param name="clientId">
        ///     Идентификатор клиента.
        /// </param>
        /// <param name="productId">
        ///     Внутренний идентификатор товара.
        /// </param>
        /// <param name="quantity">
        ///     Количество товаров
        /// </param>
        /// <param name="clientContext">Контекст клиента</param>
        /// <returns>
        /// Результат операции.
        /// </returns>
        public BasketManageResult Add(string clientId, string productId, int quantity, Dictionary<string, string> clientContext)
        {
            try
            {
                clientId.ThrowIfNullOrWhiteSpace("clientId");
                productId.ThrowIfNullOrWhiteSpace("productId");
                clientContext.ThrowIfNull("clientContext");
                Utils.CheckArgument(quantity < 1, "Кол-во элементов (quantity) не может быть меньше 1");

                var item = this.productsSearcher.GetProductById(productId, clientContext);
                if (item == null || item.Product == null)
                {
                    throw new OperationException(
                        ResultCodes.NOT_FOUND, 
                        string.Format("Товар с идентификатором {0} не найден", productId));
                }

                var exists = basketItemRepository.Get(clientId, productId);

                var partner = partnerRepository.GetById(item.Product.PartnerId);

                bool partnerHasMultiPositionOrders = partner != null &&
                                                     partner.Settings.GetMultiPositionOrdersSupported();

                quantity = exists == null ? quantity : quantity + exists.ProductsQuantity;

                var basketItemId = exists == null ? Guid.NewGuid() : exists.Id;

                var productPrice = FixPrice(clientId, item.Product.PartnerProductId, quantity, basketItemId, item.Product);

                SaveOrderAttempt(clientId);

                if (exists == null)
                {
                    var created = basketItemRepository.Add(clientId, productId, quantity, productPrice, basketItemId, partnerHasMultiPositionOrders ? (int?)partner.Id : null);
                    return BasketManageResult.BuildSuccess(created.Id);
                }

                basketItemRepository.Update(exists.ClientId, exists.ProductId, quantity, productPrice);
                return BasketManageResult.BuildSuccess(exists.Id);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка при добавлении товаров в корзину клиента", ex);
                return ServiceOperationResult.BuildErrorResult<BasketManageResult>(ex);
            }
        }

        /// <summary>
        /// Установка количества товаров в корзине клиента.
        /// </summary>
        /// <param name="clientId">
        ///     Идентификатор клиента.
        /// </param>
        /// <param name="productId">
        ///     Внутренний идентификатор товара.
        /// </param>
        /// <param name="quantity">
        ///     Количество товаров
        /// </param>
        /// <param name="clientContext">Контекст клиента</param>
        /// <returns>
        /// Результат операции.
        /// </returns>
        public BasketManageResult SetQuantity(string clientId, string productId, int quantity, Dictionary<string, string> clientContext)
        {
            try
            {
                clientId.ThrowIfNull("clientId");
                productId.ThrowIfNull("productId");
                clientContext.ThrowIfNull("clientContext");

                var item = this.productsSearcher.GetProductById(productId, clientContext);
                if (item == null || item.Product == null)
                {
                    throw new OperationException(
                        ResultCodes.NOT_FOUND, 
                        string.Format("Товар с идентификатором {0} не найден", productId));
                }

                if (quantity < 1)
                {
                    throw new ArgumentException(
                        "Кол-во элементов (quantity) должно быть указано и не может быть меньше 1");
                }

                var exists = basketItemRepository.Get(clientId, productId);

                var basketItemId = exists == null ? Guid.NewGuid() : exists.Id;

                var actualPrice = FixPrice(clientId, item.Product.PartnerProductId, quantity, basketItemId, item.Product);

                if (exists == null)
                {
                    exists = basketItemRepository.Add(clientId, productId, quantity, actualPrice, basketItemId);
                }
                else
                {
                    exists = basketItemRepository.Update(clientId, productId, quantity, actualPrice);
                    return BasketManageResult.BuildSuccess(exists.Id);
                }

                SaveOrderAttempt(clientId);

                return BasketManageResult.BuildSuccess(exists.Id);
            }
            catch (Exception ex)
            {
                log.Error("Необработанная ошибка при установки кол-ва товаров в корзине клиента", ex);
                return ServiceOperationResult.BuildErrorResult<BasketManageResult>(ex);
            }
        }

        /// <summary>
        /// Удаление товара из Корзины клиента
        /// </summary>
        /// <param name="clientId">
        /// Идентификатор клиента.
        /// </param>
        /// <param name="productId">
        /// Внутренний идентификатор товара.
        /// </param>
        /// <returns>
        /// Результат операции.
        /// </returns>
        public BasketManageResult Remove(string clientId, string productId)
        {
            try
            {
                clientId.ThrowIfNull("clientId");
                productId.ThrowIfNull("productId");

                basketItemRepository.Remove(clientId, productId);

                return BasketManageResult.BuildSuccess();
            }
            catch (Exception ex)
            {
                log.Error("Необработанная ошибка при удалении перечня товаров из корзины клиента", ex);
                return ServiceOperationResult.BuildErrorResult<BasketManageResult>(ex);
            }
        }

        /// <summary>
        /// Получение перечня товаров из Корзины клиента
        /// </summary>
        /// <param name="parameters">
        /// Параметры запроса.
        /// </param>
        /// <returns>
        /// Результат операции.
        /// </returns>
        public BasketResult GetBasket(GetBasketParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters");
                parameters.ClientId.ThrowIfNull("parameters.ClientId");
                parameters.ClientContext.ThrowIfNull("parameters.ClientContext");

                var clientId = parameters.ClientId;

                var takeCount = parameters.CountToTake.NormalizeByHeight(ApiSettings.MaxResultsCountBasketItems);
                var skipCount = parameters.CountToSkip.HasValue ? parameters.CountToSkip.Value : 0;

                var basketItems = basketItemRepository.GetByClientId(clientId, skipCount, takeCount).ToArray();

                int? allCount = null;
                if (parameters.CalcTotalCount.HasValue && parameters.CalcTotalCount.Value)
                {
                    allCount = basketItemRepository.GetCountByClientId(clientId);
                }

                var basketProductIds = basketItems.Select(i => i.ProductId).ToArray();

                var items = productsSearcher.GetProductsByIds(basketProductIds, parameters.ClientContext);

                var joins = basketItems
                    .Join(
                        items,
                        basketItem => basketItem.ProductId.ToLower(),
                        item => item.Product.ProductId.ToLower(),
                        (basketItem, item) =>
                            {
                                basketItem.Product = item.Product;
                                basketItem.AvailabilityStatus = item.AvailabilityStatus;
                                return basketItem;
                            })
                    .ToArray();

                var basketResult = new BasketResult
                {
                    ResultCode = ResultCodes.SUCCESS, 
                    Items = joins, 
                    TotalCount = allCount
                };

                foreach (var item in joins)
                {
                    PriceSpecification.FillBasketItemPrice(item, mechanicsProvider, parameters.ClientContext);
                }

                basketResult.TotalPrice = joins.Sum(i => i.TotalPrice);
                return basketResult;
            }
            catch (Exception ex)
            {
                log.Error("Необработанная ошибка при получении перечня товаров из корзины клиента", ex);
                return ServiceOperationResult.BuildErrorResult<BasketResult>(ex);
            }
        }

        public GetBasketItemResult GetBasketItem(
            string clientId, 
            string productId, 
            Dictionary<string, string> clientContext)
        {
            try
            {
                clientId.ThrowIfNull("clientId");
                productId.ThrowIfNull("productId");
                clientContext.ThrowIfNull("clientContext");

                var basketItem = basketItemRepository.Get(clientId, productId);

                if (basketItem == null)
                {
                    return new GetBasketItemResult
                    {
                        ResultCode = ResultCodes.BASKET_ITEM_NOT_FOUND,
                        ResultDescription = "Элемент корзины не найден"
                    };
                }

                var item = productsSearcher.GetProductById(productId, clientContext);

                if (item == null)
                {
                    return new GetBasketItemResult
                    {
                        ResultCode = ResultCodes.NOT_FOUND, 
                        ResultDescription = "Товар не найден"
                    };
                }

                basketItem.Product = item.Product;
                basketItem.AvailabilityStatus = item.AvailabilityStatus;

                PriceSpecification.FillBasketItemPrice(basketItem, mechanicsProvider, clientContext);

                return new GetBasketItemResult
                {
                    ResultCode = ResultCodes.SUCCESS, 
                    Item = basketItem
                };
            }
            catch (Exception ex)
            {
                log.Error("Необработанная ошибка при получении элемента корзины клиента", ex);
                return ServiceOperationResult.BuildErrorResult<GetBasketItemResult>(ex);
            }
        }

        public GetBasketItemsResult GetBasketItems(
            string clientId,
            string[] productIds,
            Dictionary<string, string> clientContext)
        {
            try
            {
                clientId.ThrowIfNull("clientId");
                productIds.ThrowIfNull("productIds");
                clientContext.ThrowIfNull("clientContext");

                var basketItems = basketItemRepository.Get(clientId, productIds);

                var items = productsSearcher.GetProductsByIds(productIds, clientContext);

                for (var i = 0; i < basketItems.Length; i++)
                {
                    var basketItem = basketItems[i];

                    if (basketItem == null)
                    {
                        continue;
                    }

                    var item = items == null ? null : items.FirstOrDefault(p => p.Product.ProductId == basketItem.ProductId);

                    if (item != null)
                    {
                        basketItem.Product = item.Product;
                        basketItem.AvailabilityStatus = item.AvailabilityStatus;
                        PriceSpecification.FillBasketItemPrice(basketItem, mechanicsProvider, clientContext);
                    }
                }

                return new GetBasketItemsResult
                {
                    ResultCode = ResultCodes.SUCCESS,
                    Items = basketItems
                };
            }
            catch (Exception ex)
            {
                log.Error("Необработанная ошибка при получении элемента корзины клиента", ex);
                return ServiceOperationResult.BuildErrorResult<GetBasketItemsResult>(ex);
            }
        }

        public GetBasketItemResult GetBasketItem(Guid basketItemId, Dictionary<string, string> clientContext)
        {
            var basketItem = basketItemRepository.Get(basketItemId);

            if (basketItem == null)
            {
                return new GetBasketItemResult
                {
                    ResultCode = ResultCodes.BASKET_ITEM_NOT_FOUND,
                    ResultDescription = "Элемент корзины не найден"
                };
            }

            return GetBasketItem(basketItem.ClientId, basketItem.ProductId, clientContext);
        }

        public GetBasketItemsResult GetBasketItems(Guid[] basketItemIds, Dictionary<string, string> clientContext)
        {
            var basketItems = basketItemRepository.Get(basketItemIds);

            if (basketItems.Length != basketItemIds.Length)
            {
                return new GetBasketItemsResult
                {
                    ResultCode = ResultCodes.BASKET_ITEM_NOT_FOUND,
                    ResultDescription = "Элемент корзины не найден"
                };
            }

            return GetBasketItems(basketItems[0].ClientId, basketItems.Select(b => b.ProductId).ToArray(), clientContext);
        }

        #endregion

        private FixedPrice FixPrice(string clientId, string productId, int quantity, Guid newBasketItemId, Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException("product");
            }

            var partner = partnerRepository.GetById(product.PartnerId);

            if (partner == null)
            {
                throw new OperationException(string.Format("Partner with id={0} not found", product.PartnerId), ResultCodes.NOT_FOUND);
            }

            if (!partner.Settings.GetFixPriceSupported())
            {
                return null;
            }

            var fixBasketItemPriceParam = new FixBasketItemPriceParam
                                              {
                                                  PartnerId = product.PartnerId,
                                                  Amount = quantity,
                                                  BasketItemId = newBasketItemId.ToString(),
                                                  ClientId = clientId,
                                                  OfferId = productId,
                                                  OfferName = product.Name,
                                                  Price = product.PriceRUR
                                              };

            var fixBasketItemPriceResult = partnerConnectorProvider.FixBasketItemPrice(fixBasketItemPriceParam);

            if (!fixBasketItemPriceResult.Success)
            {
                var message = string.Format(
                    "Ошибка фиксации цены ResultCode:{0} ResultDescription:{1}",
                    fixBasketItemPriceResult.ResultCode,
                    fixBasketItemPriceResult.ResultDescription);

                throw new OperationException(message, ResultCodes.PROVIDER_CONNECTOR_CALL_ERROR);
            }

            if (fixBasketItemPriceResult.Confirmed == 0)
            {
                var message = string.Format(
                    "Провайдер отклонил фиксацию цены товара Confirmed={0} ReasonCode:{1} Reason:{2}",
                    fixBasketItemPriceResult.Confirmed,
                    fixBasketItemPriceResult.ReasonCode,
                    fixBasketItemPriceResult.Reason);

                throw new OperationException(message, ResultCodes.FIX_PRICE_DECLINED);
            }

            if (!fixBasketItemPriceResult.ActualPrice.HasValue)
            {
                throw new OperationException("Провайдер не вернул ActualPrice", ResultCodes.PROVIDER_CONNECTOR_CALL_ERROR);
            }

            return new FixedPrice
                   {
                       PriceRUR = fixBasketItemPriceResult.ActualPrice.Value,
                       FixDate = DateTime.Now
                   };
        }

        private void SaveOrderAttempt(string clientId)
        {
            var currentHour = DateTime.Now.TimeOfDay.Hours;

            // сохраняем только попытки, сделанные в определенное время суток
            if (currentHour < SaveOrderAttemptMinHour || currentHour > SaveOrderAttemptMaxHour)
            {
                return;
            }

            try
            {
                using (var service = new BankConnectorClient())
                {
                    var response = service.SaveOrderAttempt(clientId);

                    if (!response.Success)
                    {
                        log.WarnFormat(
                            "Не удалось сохранить попытку клиента оформить заказ: [{0}] - {1}",
                            response.ResultCode,
                            response.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Warn("Ошибка при сохранении попытки клиента оформить заказ, ClientID = " + clientId, ex);
            }
        }
    }
}