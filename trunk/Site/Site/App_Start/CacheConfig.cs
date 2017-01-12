using System;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Vtb24.Logging;
using Vtb24.Site.Content.Advertisements;
using Vtb24.Site.Content.Pages;
using Vtb24.Site.Content.Pages.Models.Inputs;
using Vtb24.Site.Infrastructure.Caching;
using Vtb24.Site.Services;
using Vtb24.Site.Services.Buy.Models.Inputs;
using Vtb24.Site.Services.ClientMessageService;
using Vtb24.Site.Services.ClientTargeting;
using Vtb24.Site.Services.GeoService.Models.Inputs;
using Vtb24.Site.Services.GiftShop.Catalog.Models.Inputs;
using Vtb24.Site.Services.Infrastructure;
using Vtb24.Site.Services.Models;
using Vtb24.Site.Services.MyInfoService.Models.Inputs;
using IClientMessageService = Vtb24.Site.Services.IClientMessageService;

namespace Vtb24.Site.App_Start
{
    public static class CacheConfig
    {
        
        public static bool IsEnabled
        {
            get { return AppSettingsHelper.Bool("cache_enabled", true); }
        }

        public static TimeSpan ShortDuration
        {
            get { return TimeSpan.FromSeconds(AppSettingsHelper.Int("cache_duration_short_seconds", 5 * 60)); }
        }

        public static TimeSpan LongDuration
        {
            get { return TimeSpan.FromSeconds(AppSettingsHelper.Int("cache_duration_long_seconds", 15 * 60)); }
        }

        public static TimeSpan UberLongDuration
        {
            get { return TimeSpan.FromSeconds(AppSettingsHelper.Int("cache_duration_uber_long_seconds", 16 * 60 * 60)); }
        }

        public static void Configure(IUnityContainer container)
        {
            // Перед настройкой необходимо убедиться, что
            // в Unity добавлено расширение для перехвата
            // вызовов: container.AddNewExtension<Interception>();

            if (!IsEnabled)
            {
                return;
            }

            var memoryLimit = AppSettingsHelper.Int("cache_memory_limit_mb", 0);
            var config = Caching.Configure(container, memoryLimit);

            ConfigureClientService(config);
            ConfigureClientMessage(config);
            ConfigureGiftShop(config);
            ConfigurePagesManagement(config);
            ConfigureGeoService(config);
        }

        #region Конфигурации сервисов

        /// <summary>
        /// Геобаза
        /// </summary>
        private static void ConfigureGeoService(Caching config)
        {
            // (поиск опасно кэшировать)

            config.Add<IGeoService>(s => s.ListRegions(), LongDuration);
            config.Add<IGeoService>(s => s.GetLocationByUserIp(new GetLocationByIpParams()), LongDuration);
        }

        /// <summary>
        /// Управление контентом
        /// </summary>
        private static void ConfigurePagesManagement(Caching config)
        {
            // страницы
            config.Add<IPagesManagement>(s => s.GetPageByUrl("", new GetPlainPagesOptions()), ShortDuration);
        }

        /// <summary>
        /// Магазин подарков
        /// </summary>
        private static void ConfigureGiftShop(Caching config)
        {
            var preload = TimeSpan.FromSeconds(60);

            // (поиск опасно кэшировать)

            // корзина
            config.Add<IGiftShop>(s => s.GetBasket(new PagingSettings()), LongDuration, VaryByUserAndLocation,
                new EvictTriggers
                {
                    (IClientService s) => s.SetUserLocation(""),
                    (IGiftShop s) => s.AddToBasket("", 0),
                    (IGiftShop s) => s.RemoveFromBasket(""),
                    (IGiftShop s) => s.UpdateBasketItemQuantity("", 0),
                    (IBuy s) => s.ConfirmOrder(null),
                    (ICacheCleaner s) => s.CleanBasket()
                }
            );

            // отложенные
            config.Add<IGiftShop>(s => s.GetWishList(new PagingSettings()), LongDuration, VaryByUserAndLocation,
                new EvictTriggers
                {
                    (IClientService s) => s.SetUserLocation(""),
                    (IGiftShop s) => s.AddToWishList("", 0),
                    (IGiftShop s) => s.RemoveFromWishList(""),
                    (IGiftShop s) => s.UpdateWishListItemQuantity("", 0),
                    (ICacheCleaner s) => s.CleanWishList()
                }
            );

            // список категорий
            config.Add<IGiftShop>(s => s.GetCategories(0, 0, new PagingSettings()), 
                duration: UberLongDuration, 
                expiration: LongDuration, 
                delay: ShortDuration,
                preloadOffset: preload,
                varyBy:VaryByCatalogContext,
                onSilentException: LogSilentError
            );
            
            // категория
            config.Add<IGiftShop>(s => s.GetCategoryInfo(0), 
                duration: UberLongDuration,
                expiration: LongDuration,
                delay: ShortDuration,
                preloadOffset: preload,
                varyBy: VaryByCatalogContext,
                onSilentException: LogSilentError
            );

            // популярные товары
            config.Add<IGiftShop>(s => s.GetPopularProducts(ProductPopularityType.Unknown, 0), 
                duration: UberLongDuration,
                expiration: LongDuration,
                delay: ShortDuration,
                preloadOffset: preload,
                varyBy: VaryByCatalogContext,
                onSilentException: LogSilentError
            );

            // рекомендуемые товары на главной
            // т.к. запрос получается тяжёлым, поставим сюда особенный период утсаревания
            // https://jira.rapidsoft.ru/browse/MLVTBPLK-205
            config.Add<IGiftShop>(s => s.GetRecommendedProductsForPriceRange(0,0,0),
                duration: UberLongDuration,
                expiration: TimeSpan.FromMinutes(10),
                delay: TimeSpan.FromMinutes(20), // в случае беды, используем долгий кэш
                preloadOffset: preload,
                varyBy: VaryByCatalogContext,
                onSilentException: LogSilentError
            );

            // рекомендуемые товары на странице вознаграждения
            config.Add<IGiftShop>(s => s.GetRecommendedProductsForCategory(0, 0),
                duration: UberLongDuration,
                expiration: LongDuration,
                delay: ShortDuration,
                preloadOffset: preload,
                varyBy: VaryByCatalogContext,
                onSilentException: LogSilentError
            );

            // каталог
            config.Add<IGiftShop>(s => s.Search(new SearchCriteria(), new PagingSettings()), ShortDuration, VaryByCatalogContext);

            // товар
            config.Add<IGiftShop>(s => s.GetProduct("", false), 
                duration: UberLongDuration,
                expiration: LongDuration,
                delay: ShortDuration,
                preloadOffset: preload,
                varyBy: VaryByCatalogContext,
                onSilentException: LogSilentError
            );
        }

        /// <summary>
        /// Мои сообщения
        /// </summary>
        private static void ConfigureClientMessage(Caching config)
        {
            // Личные сообщения
            config.Add<IClientMessageService>(s => s.GetStatistics(new GetStatisticsParameters()), ShortDuration, VaryByUser,
                new EvictTriggers
                {
                    (IClientMessageService s) => s.MarkThreadAsRead(new MarkThreadAsReadParameters()),
                    (IClientMessageService s) => s.Notify(new NotifyClientsParameters()),
                    (IClientMessageService s) => s.Reply(new ClientReplyParameters()),
                    (IClientMessageService s) => s.Delete(new DeleteThreadParameters())
                }
            );
        }

        /// <summary>
        /// Личный кабинет и пользователь
        /// </summary>
        private static void ConfigureClientService(Caching config)
        {
            // балланс
            config.Add<IClientService>(s => s.GetBalance(), LongDuration, VaryByUser,
                new EvictTriggers
                {
                    (IBuy s) => s.ConfirmOrder(new ConfirmOrderParams()),
                    (IBuy s) => s.ConfirmBankProductOrder(new ConfirmOrderParams())
                }
            );

            // профиль
            config.Add<IClientService>(s => s.GetProfile(), LongDuration, VaryByUser,
                new EvictTriggers
                {
                    (IClientService s) => s.SetUserLocation(""),
                    (IMyInfoService s) => s.UpdateMyInfo(new UpdateMyInfoParams()),
                    (IClientService s) => s.SetEmail("")
                }
            );

            // статус клиента
            config.Add<IClientService>(s => s.GetStatus(), LongDuration, VaryByUser);

            /***
             *  ВАЖНО: IClientService.GetMechanicsContext нельзя кэшировать с VaryByUser
             */

            // целевые аудитории
            config.Add<IClientTargeting>(s => s.GetClientGroups(""), LongDuration, VaryByUser);

			// баннер BonusBack на главной
            config.Add<IAdvertisementsManagement>(s => s.GetAdvertisements("", new Content.Models.PagingSettings()),
                                                  LongDuration, VaryByUser);
        }

        #endregion


        #region VaryBy

        /****   ВАЖНО: 
        *       Используя сервисы в VaryBy функциях, тщательно следите за циклическими зависимостями
        ****/

        /// <summary>
        /// Кэширование для пользователя
        /// </summary>
        private static string VaryByUser(VaryByContext context)
        {
            return "cl:" + ClaimsPrincipal.Current.Identity.Name;
        }

        /// <summary>
        /// Кэширование для пользователя + его местоположение
        /// </summary>
        private static string VaryByUserAndLocation(VaryByContext context)
        {
            var client = DependencyResolver.Current.GetService<IClientService>();
            return "cll:" + ClaimsPrincipal.Current.Identity.Name + ":" + client.GetUserLocation().KladrCode;
        }

        /// <summary>
        /// Кэширование по контексту механик (для каталога)
        /// </summary>
        private static string VaryByCatalogContext(VaryByContext context)
        {
            context.SuppressEvictTriggers = true; // Не сбрасывать кэш

            var cache = HttpContext.Current.Items["VaryByCatalogContext"] as string;
            if (cache == null)
            {
                var client = DependencyResolver.Current.GetService<IClientService>();
                var mechContext = client.GetMechanicsContext();

                cache = string.Format("cat:{0}:{1}",
                    mechContext.ClientLocationKladr,
                    mechContext.ClientGroups == null ? null : string.Join(",", mechContext.ClientGroups)
                );
                HttpContext.Current.Items["VaryByCatalogContext"] = cache;
            }

            return cache;
        }

        /// <summary>
        /// Логирование фоновых ошибок для расширенного кэша
        /// </summary>
        private static void LogSilentError(Exception e)
        {
            SerilogLoggers.MainLogger.LogError("Logged_Cache_Error", e, null);
        }

        #endregion
    }
}