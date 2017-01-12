using System.Collections.Generic;
using Vtb24.Site.Services.GiftShop.Basket.Models.Outputs;
using Vtb24.Site.Services.GiftShop.Catalog.Models;
using Vtb24.Site.Services.GiftShop.Catalog.Models.Inputs;
using Vtb24.Site.Services.GiftShop.Catalog.Models.Outputs;
using Vtb24.Site.Services.GiftShop.Model;
using Vtb24.Site.Services.GiftShop.Orders.Models;
using Vtb24.Site.Services.GiftShop.Orders.Models.Outputs;
using Vtb24.Site.Services.GiftShop.Wishlist.Models.Outputs;
using Vtb24.Site.Services.Infrastructure;
using Vtb24.Site.Services.Models;

namespace Vtb24.Site.Services
{
    /// <summary>
    /// Фасад для Магазина подарков (сервисы для клиентского сайта)
    /// </summary>
    public interface IGiftShop
    {
        #region Каталог

        /// <summary>
        /// Получить ветвь каталога
        /// </summary>
        CategoriesResult GetCategories(int? parentCategoryId, int? depth, PagingSettings paging);

        /// <summary>
        /// Получить информацию о категории и её предках по Id
        /// </summary>
        CatalogCategoryInfo GetCategoryInfo(int categoryId);

        /// <summary>
        /// Получить товар
        /// </summary>
        CatalogProduct GetProduct(string productId, bool countAsView = false);

        /// <summary>
        /// Поиск товаров
        /// </summary>
        SearchResult Search(SearchCriteria criteria, PagingSettings paging);

        /// <summary>
        /// Получить дополнительную информацию о категории
        /// </summary>
        SubCatalogInfo GetFilterMetaData(int? parentCategoryId = null);

        /// <summary>
        /// Получить расширенные параметры вознаграждений в категории
        /// </summary>
        CatalogParameter[] GetCategoryParameters(int categoryId);

        /// <summary>
        /// Получить популярные товары
        /// </summary>
        IEnumerable<CatalogProduct> GetPopularProducts(ProductPopularityType type, int? countToTake);

        /// <summary>
        /// Получить "виртуальные" популярные товары
        /// </summary>
        IEnumerable<CustomProduct> GetCustomPopularProducts(decimal balance);

        /// <summary>
        /// Получить рекомендуемые товары
        /// </summary>
        IEnumerable<CatalogProduct> GetRecommendedProducts(int? countToTake);

        /// <summary>
        /// Получить рекомендуемые товары для диапазона цен
        /// </summary>
        IEnumerable<CatalogProduct> GetRecommendedProductsForPriceRange(decimal minPrice, decimal maxPrice, int? countToTake);

        /// <summary>
        /// Получить рекомендуемые товары для категории
        /// </summary>
        IEnumerable<CatalogProduct> GetRecommendedProductsForCategory(int categoryId, int? countToTake);

        /// <summary>
        /// Получить "виртуальные" рекомендуемые товары
        /// </summary>
        IEnumerable<CustomProduct> GetCustomRecommendedProducts(decimal balance);

        /// <summary>
        /// Получить всех партнеров
        /// </summary>
        CatalogPartner[] GetPartners();
        
        #endregion


        #region Список отложенных товаров (WishList)

        WishListResult GetWishList(PagingSettings paging);

        ReservedProductItem GetWishedProduct(string productId);

        void AddToWishList(string productId, int quantity = 1);

        void RemoveFromWishList(string productId);

        void UpdateWishListItemQuantity(string productId, int quantity);

        #endregion


        #region Корзина

        BasketResult GetBasket(PagingSettings paging);

        ReservedProductItem GetBasketItem(string productId, string locationKladr = null);

        ReservedProductItem[] GetBasketItems(string[] productIds, string locationKladr = null);

        void AddToBasket(string productId, int quantity = 1);

        void RemoveFromBasket(string productId);

        void UpdateBasketItemQuantity(string productId, int quantity);

        #endregion


        #region Заказы

        GiftShopOrder GetOrder(int orderId);

        GiftShopOrder GetOrderByExternalId(int partnerId, string externalOrderId);

        DeliveryAddressesResult GetDeliveryAddresses(bool excludeAddressesWithoutKladr);

        GiftShopOrdersResult GetOrdersHistory(OrderStatus[] statuses, DateTimeRange range, PagingSettings paging);

        #endregion
    }
}