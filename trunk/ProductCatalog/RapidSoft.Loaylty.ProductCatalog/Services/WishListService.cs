namespace RapidSoft.Loaylty.ProductCatalog.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using API;

    using API.Entities;

    using API.InputParameters;

    using API.OutputResults;

    using DataSources;
    using DataSources.Repositories;

    using Extensions;

    using Interfaces;

    using Logging;

    using Logging.Wcf;

    using Monitoring;

    using RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces;

    using Settings;

    using WishListItem = API.OutputResults.WishListItem;

    /// <summary>
    ///     The wish list service.
    /// </summary>
    [LoggingBehavior]
    public class WishListService : SupportService, IWishListService
    {
        private readonly IProductsSearcher productsSearcher;
        private readonly IWishListRepository wishRepository;
        private readonly ILog log = LogManager.GetLogger(typeof(WishListService));

        private readonly IClientProfileProvider clientProfileProvider;
        private readonly IProcessingProvider processingProvider;

        public WishListService() : this(null)
        {
        }

        public WishListService(
            IWishListRepository wishRepository = null, 
            IProductsSearcher productsSearcher = null, 
            IClientProfileProvider clientProfileProvider = null,
            IProcessingProvider processingProvider = null)
        {
            this.wishRepository = wishRepository ?? new WishListRepository();
            this.productsSearcher = productsSearcher ?? new ProductsSearcher();
            this.clientProfileProvider = clientProfileProvider ?? new ClientProfileProvider();
            this.processingProvider = processingProvider ?? new ProcessingProvider();
        }

        #region IWishListService Members

        public WishListResult Add(string clientId, string productId, int quantity = 1, Dictionary<string, string> clientContext = null)
        {
            try
            {
                clientId.ThrowIfNull("clientId");
                productId.ThrowIfNull("productId");
                clientContext.ThrowIfNull("clientContext");

                var product = this.productsSearcher.GetProductById(productId, clientContext);
                if (product == null)
                {
                    throw new OperationException(
                        ResultCodes.NOT_FOUND, 
                        string.Format("Товар с идентификатором {0} не найден", productId));
                }

                if (quantity < 1)
                {
                    throw new ArgumentException("Кол-во элементов (quantity) не может быть меньше 1");
                }

                wishRepository.IncreaseQuantityOrCreate(productId, clientId, quantity);
                return WishListResult.BuildSuccess();
            }
            catch (Exception ex)
            {
                log.ErrorFormat(
                    "Ошибка добавления товара ({0}) в wishlist клиента {1} (кол-во {2})", 
                    productId, 
                    clientId, 
                    quantity);
                return ServiceOperationResult.BuildErrorResult<WishListResult>(ex);
            }
        }

        public WishListResult SetQuantity(string clientId, string productId, int quantity, Dictionary<string, string> clientContext = null)
        {
            try
            {
                clientId.ThrowIfNull("clientId");
                productId.ThrowIfNull("productId");
                clientContext.ThrowIfNull("clientContext");

                var product = this.productsSearcher.GetProductById(productId, clientContext);
                if (product == null)
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

                wishRepository.CreateOrUpdate(productId, clientId, quantity);
                return WishListResult.BuildSuccess();
            }
            catch (Exception ex)
            {
                log.ErrorFormat(
                    "Ошибка установки кол-ва товара ({0}) в wishlist'е клиента {1} (кол-во {2})", 
                    productId, 
                    clientId, 
                    quantity);
                return ServiceOperationResult.BuildErrorResult<WishListResult>(ex);
            }
        }

        public WishListResult Remove(string clientId, string productId)
        {
            try
            {
                clientId.ThrowIfNull("clientId");
                productId.ThrowIfNull("productId");

                wishRepository.Remove(productId, clientId);
                return WishListResult.BuildSuccess();
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Ошибка удаления товара ({0}) из wishlist'а клиента {1}", productId, clientId);
                return ServiceOperationResult.BuildErrorResult<WishListResult>(ex);
            }
        }

        public GetWishListItemResult GetWishListItem(
            string clientId, 
            string productId, 
            Dictionary<string, string> clientContext)
        {
            try
            {
                clientId.ThrowIfNull("clientId");
                productId.ThrowIfNull("productId");

                var wishlistItem = wishRepository.Get(productId, clientId);

                if (wishlistItem == null)
                {
                    return BuildSuccess();
                }

                // NOTE: Неизвестно лежит ли в контексте клиентский ид или нет, поэтому для надежности сделаем свою копию.
                var context = new Dictionary<string, string>(clientContext);
                context[ClientContextParser.ClientIdKey] = clientId;

                var item = this.productsSearcher.GetProductById(productId, clientContext);

                var result = new WishListItem
                {
                    Product = item.Product,
                    AvailabilityStatus = item.AvailabilityStatus,
                    CreatedDate = wishlistItem.CreatedDate,
                    ProductsQuantity = wishlistItem.ProductsQuantity,
                };

                PriceSpecification.FillWishListItemPrice(result);

                return BuildSuccess(result);
            }
            catch (Exception ex)
            {
                log.ErrorFormat(
                    "Ошибка получения элемента wishlist'а клиента {0} с товаром {1}", 
                    clientId, 
                    productId);
                return ServiceOperationResult.BuildErrorResult<GetWishListItemResult>(ex);
            }
        }

        public GetWishListResult GetWishList(GetWishListParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters");
                parameters.ClientId.ThrowIfNull("parameters.ClientId");

                var internalCountToTake = parameters.CountToTake.NormalizeByHeight(ApiSettings.MaxResultsCountWishList);

                var list = wishRepository.GetWishList(
                    parameters.ClientId,
                    parameters.SortType,
                    parameters.SortDirect,
                    parameters.CountToSkip,
                    internalCountToTake);

                if (list.Count == 0)
                {
                    return GetWishListResult.BuildSuccess(internalCountToTake);
                }

                var wishlistProductIds = list.Select(item => item.ProductId).ToArray();

                var items = productsSearcher.GetProductsByIds(wishlistProductIds, parameters.ClientContext);

                if (list.Count != items.Length)
                {
                    // NOTE: Поиск и удаление "мертвых" элементов wishlist'а.
                    var badProductIds = wishlistProductIds.Except(items.Select(i => i.Product.ProductId)).ToArray();
                    foreach (var id in badProductIds)
                    {
                        wishRepository.Remove(id, parameters.ClientId);
                    }

                    // Если нет мёртвых элементов то идём дальше иначе зациклимся если нет товара в таблице
                    if (badProductIds.Length > 0)
                    {
                        return GetWishList(parameters);
                    }
                }

                var resultList = list.Join(
                    items,
                    i => i.ProductId.ToLower(),
                    p => p.Product.ProductId.ToLower(),
                    (i, p) =>
                    {
                        var item = new WishListItem
                                   {
                                       Product = p.Product,
                                       AvailabilityStatus = p.AvailabilityStatus,
                                       CreatedDate = i.CreatedDate,
                                       ProductsQuantity = i.ProductsQuantity
                                   };
                        PriceSpecification.FillWishListItemPrice(item);
                        return item;
                    });

                var itemsCount = (parameters.CalcTotalCount.HasValue && parameters.CalcTotalCount.Value)
                    ? wishRepository.GetCountByClientId(parameters.ClientId)
                    : (int?)null;

                return GetWishListResult.BuildSuccess(internalCountToTake, itemsCount, resultList);
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Ошибка получения wishlist'а клиента {0}", parameters.ClientId);
                return ServiceOperationResult.BuildErrorResult<GetWishListResult>(ex);
            }
        }

        public ResultBase MakeWishListNotifications()
        {
            try
            {
                var notificationsBuilder = new WishListNotificationsBuilder(wishRepository, processingProvider, clientProfileProvider, productsSearcher);
                notificationsBuilder.MakeWishListNotifications();

                return ResultBase.BuildSuccess();
            }
            catch (Exception ex)
            {
                log.Error("Ошибка при обновлении списка оповещений WishList пользователей", ex);
                return ServiceOperationResult.BuildErrorResult<ResultBase>(ex);
            }
        }
        
        public GetWishListNotificationsResult GetWishListNotifications(GetWishListNotificationsParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters");

                if (parameters.Rebuild.HasValue && parameters.Rebuild.Value)
                {
                    var rebuildResult = MakeWishListNotifications();

                    if (!rebuildResult.Success)
                    {
                        return GetWishListNotificationsResult.BuildFail(rebuildResult);
                    }
                }

                var noti = this.wishRepository.GetItemsToNotify(
                    parameters.ClientId, parameters.CountToTake, parameters.CalcTotalCount);

                return GetWishListNotificationsResult.BuildSuccess(noti, noti.TotalCount);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка получения списка оповещений WishList пользователей");
                return ServiceOperationResult.BuildErrorResult<GetWishListNotificationsResult>(ex);
            }
        }

        #endregion

        private GetWishListItemResult BuildSuccess(WishListItem item = null)
        {
            return new GetWishListItemResult
            {
                ResultCode = ResultCodes.SUCCESS,
                ResultDescription = null,
                Item = item
            };
        }
    }
}