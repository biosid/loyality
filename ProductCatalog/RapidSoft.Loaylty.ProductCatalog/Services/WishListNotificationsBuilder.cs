namespace RapidSoft.Loaylty.ProductCatalog.Services
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using API.Entities;
    using API.InputParameters;

    using ClientProfile.ClientProfileService;

    using DataSources;
    using DataSources.Interfaces;
    using DataSources.Repositories;

    using Interfaces;

    using Logging;

    using Settings;

    public class WishListNotificationsBuilder
    {
        private readonly ILog log = LogManager.GetLogger(typeof(WishListNotificationsBuilder));
        private readonly IWishListRepository wishListRepository;
        private readonly IProcessingProvider processingProvider;
        private readonly IClientProfileProvider clientProfileProvider;
        private readonly IProductsSearcher productsSearcher;
        private readonly IMechanicsProvider mechanicsProvider;
        private Dictionary<string, Tuple<Product, Price>> productCache;

        public WishListNotificationsBuilder(
            IWishListRepository wishListRepository = null,
            IProcessingProvider processingProvider = null,
            IClientProfileProvider clientProfileProvider = null,
            IProductsSearcher productsSearcher = null,
            IMechanicsProvider mechanicsProvider = null)
        {
            this.mechanicsProvider = mechanicsProvider ?? new MechanicsProvider();
            this.wishListRepository = wishListRepository ?? new WishListRepository();
            this.productsSearcher = productsSearcher ?? new ProductsSearcher();
            this.clientProfileProvider = clientProfileProvider ?? new ClientProfileProvider();
            this.processingProvider = processingProvider ?? new ProcessingProvider();
        }

        public void MakeWishListNotifications()
        {
            log.Debug("Очистка не отправленных нотификаций");
            wishListRepository.CleanUpNonSentNotifications();

            log.Debug("Получение нотификаций");
            var itemsNotifications = wishListRepository.GetWishListToNotify();

            var clientIds = itemsNotifications.Select(i => i.ClientId).Distinct().ToList();

            log.Info(string.Format("Клиентов для воможного оповещения:{0}", clientIds.Count));
            var profiles = new List<GetClientProfileFullResponseTypeClientProfile>();

            foreach (var clientId in clientIds)
            {
                var clientProfile = GetClientProfile(clientId);

                if (clientProfile != null)
                {
                    profiles.Add(clientProfile);
                }
            }

            log.Debug("Инициализация нотификаций");
            var notifications = GetNotifications(itemsNotifications, profiles);

            log.Debug("Создание нотификаций");
            log.Info(string.Format("Всего нотификаций:{0}", notifications.Count()));
            wishListRepository.AddNotifications(notifications);
        }

        private List<WishListItemNotification> GetNotifications(List<WishListItemNotification> itemsNotifications, List<GetClientProfileFullResponseTypeClientProfile> profiles)
        {
            var profilesWithLocations = profiles.Where(x => x.ClientStatus == (int)ClientProfileStatuses.Active && !string.IsNullOrEmpty(x.ClientLocationKladr));
            var balances = profilesWithLocations.ToDictionary(x => x.ClientId, x => GetUserBalance(x, x.ClientId));

            var itemsWithClients = from i in itemsNotifications
                                   join b in balances on i.ClientId equals b.Key
                                   join l in profilesWithLocations on i.ClientId equals l.ClientId
                                   select new { Notification = i, Profile = l, Balance = b };

            var notifications = new List<WishListItemNotification>();

            productCache = new Dictionary<string, Tuple<Product, Price>>();

            foreach (var item in itemsWithClients)
            {
                var tuple = GetNotification(item.Notification.ProductId, item.Profile.ClientLocationKladr);
                
                var product = tuple.Item1;

                if (!NotificationProductValid(product, item.Notification.ProductId, item.Profile.ClientLocationKladr))
                {
                    continue;
                }

                var totalBonusCost = PriceSpecification.GetProductsBonusPrice(product, item.Notification.ProductsQuantity);
                var totalBonusCostWithDelivery = totalBonusCost + tuple.Item2.Bonus;

                if (!item.Balance.Value.HasValue || item.Balance.Value < totalBonusCostWithDelivery)
                {
                    log.Info(string.Format("Не достаточно средств productId:{0} balance:{1} totalBonusCost:{2}", product.ProductId, item.Balance.Value, totalBonusCost));
                    continue;
                }

                var notification = item.Notification;

                notification.TotalBonusCost = totalBonusCost;
                notification.ItemBonusCost = product.Price;
                notification.FirstName = item.Profile.FirstName;
                notification.MiddleName = item.Profile.MiddleName;
                notification.ClientBalance = item.Balance.Value.Value;

                notifications.Add(notification);
            }

            return notifications;
        }

        private GetClientProfileFullResponseTypeClientProfile GetClientProfile(string clientId)
        {
            try
            {
                return clientProfileProvider.GetClientProfile(clientId);
            }
            catch (Exception e)
            {
                log.Error(string.Format("Не удалось получить профиль клиента {0}", clientId), e);
                return null;
            }
        }

        private decimal? GetUserBalance(GetClientProfileFullResponseTypeClientProfile x, string clientId)
        {
            try
            {
                return processingProvider.GetUserBalance(clientId);
            }
            catch (Exception e)
            {
                log.Error(string.Format("Не удалось получить баланс клиента {0}", clientId), e);
                return null;
            }
        }

        private Tuple<Product, Price> GetNotification(string productId, string clientLocationKladr)
        {
            var clientContext = new Dictionary<string, string> { { ClientContextParser.LocationKladrCodeKey, clientLocationKladr } };

            var param = new SearchProductsParameters()
            {
                ClientContext = clientContext,
                ProductIds = new[] { productId }
            };

            var hashParam = string.Format("{0}{1}", param.ClientContext[ClientContextParser.LocationKladrCodeKey], param.ProductIds[0]);

            if (productCache.ContainsKey(hashParam))
            {
                return productCache[hashParam];
            }

            var product = SearchProduct(productId, param);

            Price deliveryPrice = null;

            if (product != null)
            {
                // https://jira.rapidsoft.ru/browse/VTBPLK-3525 -- цену доставки не учитываем
                deliveryPrice = new Price { Bonus = 0, Rur = 0 };
            }

            var tuple = Tuple.Create(product, deliveryPrice);

            productCache.Add(hashParam, tuple);
            
            return tuple;
        }

        private bool NotificationProductValid(Product product, string productId, string kladr)
        {
            if (product == null)
            {
                log.Info(string.Format("Товар для нотификации не найден id:{0} kladr:{1}", productId, kladr));
                return false;
            }

            return true;
        }

        private Product SearchProduct(string productId, SearchProductsParameters param)
        {
            var searchResult = productsSearcher.SearchPublicProducts(param);

            if (!searchResult.Success)
            {
                log.Error(string.Format("Ошибка поиска продукта productId:{0} success:{1} description:{2}", productId, searchResult.Success, searchResult.ResultDescription));
                return null;
            }

            if (searchResult.Products.Length > 1)
            {
                log.Error(string.Format("Ошибка поска продукта productId:{0} найдено единиц:{1}", productId, searchResult.Products.Length));
                return null;
            }

            if (searchResult.Products.Length == 0)
            {
                return null;
            }

            return searchResult.Products.First();
        }
    }
}