using System;
using System.Collections.Generic;
using Vtb24.Site.Security.Models;
using Vtb24.Site.Services.GiftShop.Basket;
using Vtb24.Site.Services.GiftShop.Basket.Models.Outputs;
using Vtb24.Site.Services.GiftShop.Catalog;
using Vtb24.Site.Services.GiftShop.Catalog.Models;
using Vtb24.Site.Services.GiftShop.Catalog.Models.Inputs;
using Vtb24.Site.Services.GiftShop.Catalog.Models.Outputs;
using Vtb24.Site.Services.GiftShop.Model;
using Vtb24.Site.Services.GiftShop.Orders.Models;
using Vtb24.Site.Services.GiftShop.Orders.Models.Outputs;
using Vtb24.Site.Services.GiftShop.Wishlist;
using Vtb24.Site.Services.GiftShop.Wishlist.Models.Outputs;
using Vtb24.Site.Services.Infrastructure;
using Vtb24.Site.Services.Models;

namespace Vtb24.Site.Services.GiftShop
{
    public class GiftShop : IGiftShop, IDisposable
    {
        private readonly ClientPrincipal _principal;
        private readonly ProductCatalog _catalog;
        private readonly GiftShopWishList _wishlist;
        private readonly GiftShopBasket _basket;
        private readonly IGiftShopOrders _orders;

        public GiftShop(ClientPrincipal principal, IClientService client, IGiftShopOrders orders)
        {
            _principal = principal;
            _catalog = new ProductCatalog(principal, client);
            _wishlist = new GiftShopWishList(principal, client);
            _basket = new GiftShopBasket(principal, client);
            _orders = orders;
        }

        public CategoriesResult GetCategories(int? parentCategoryId, int? depth, PagingSettings paging)
        {
            return _catalog.GetCategories(parentCategoryId, depth, paging);
        }

        public CatalogCategoryInfo GetCategoryInfo(int categoryId)
        {
            return _catalog.GetCategoryInfo(categoryId);
        }

        public CatalogProduct GetProduct(string productId, bool countAsView = false)
        {
            return _catalog.GetProduct(productId, countAsView);
        }

        public SearchResult Search(SearchCriteria criteria, PagingSettings paging)
        {
            return _catalog.Search(criteria, paging);
        }

        public SubCatalogInfo GetFilterMetaData(int? parentCategoryId = null)
        {
            return _catalog.GetFilterMetaData(parentCategoryId);
        }

        public CatalogParameter[] GetCategoryParameters(int categoryId)
        {
            return _catalog.GetCategoryParameters(categoryId);
        }

        public IEnumerable<CatalogProduct> GetPopularProducts(ProductPopularityType type, int? countToTake)
        {
            return _catalog.GetPopularProducts(type, countToTake);
        }

        public IEnumerable<CustomProduct> GetCustomPopularProducts(decimal balance)
        {
            return _catalog.GetCustomPopularProducts(balance);
        }

        public IEnumerable<CatalogProduct> GetRecommendedProducts(int? countToTake)
        {
            return _catalog.GetRecommendedProducts(countToTake);
        }

        public IEnumerable<CatalogProduct> GetRecommendedProductsForPriceRange(decimal minPrice, decimal maxPrice, int? countToTake)
        {
            return _catalog.GetRecommendedProductsForPriceRange(minPrice, maxPrice, countToTake);
        }

        public IEnumerable<CatalogProduct> GetRecommendedProductsForCategory(int categoryId, int? countToTake)
        {
            return _catalog.GetRecommendedProductsForCategory(categoryId, countToTake);
        }

        public IEnumerable<CustomProduct> GetCustomRecommendedProducts(decimal balance)
        {
            return _catalog.GetCustomRecommendedProducts(balance);
        }

        public CatalogPartner[] GetPartners()
        {
            return _catalog.GetPartners();
        }

        public WishListResult GetWishList(PagingSettings paging)
        {
            return _wishlist.GetWishList(paging);
        }

        public ReservedProductItem GetWishedProduct(string productId)
        {
            return _wishlist.GetWishedProduct(productId);
        }

        public void AddToWishList(string productId, int quantity = 1)
        {
            _wishlist.AddToWishList(productId, quantity);
        }

        public void RemoveFromWishList(string productId)
        {
            _wishlist.RemoveFromWishList(productId);
        }

        public void UpdateWishListItemQuantity(string productId, int quantity)
        {
            _wishlist.UpdateWishListItemQuantity(productId, quantity);
        }

        public BasketResult GetBasket(PagingSettings paging)
        {
            return _basket.GetBasket(paging);
        }

        public ReservedProductItem GetBasketItem(string productId, string locationKladr = null)
        {
            return _basket.GetBasketItem(productId, locationKladr);
        }

        public ReservedProductItem[] GetBasketItems(string[] productIds, string locationKladr = null)
        {
            return _basket.GetBasketItems(productIds, locationKladr);
        }

        public void AddToBasket(string productId, int quantity = 1)
        {
            _basket.AddToBasket(productId, quantity);
        }

        public void RemoveFromBasket(string productId)
        {
            _basket.RemoveFromBasket(productId);
        }

        public void UpdateBasketItemQuantity(string productId, int quantity)
        {
            _basket.UpdateBasketItemQuantity(productId, quantity);
        }

        public GiftShopOrder GetOrder(int orderId)
        {
            AssertUser("Невозможно получить заказ");

            return _orders.GetOrder(_principal.ClientId, orderId);
        }

        public GiftShopOrder GetOrderByExternalId(int partnerId, string externalOrderId)
        {
            AssertUser("Невозможно получить заказ онлайн партнёра");

            return _orders.GetOrderByExternalId(_principal.ClientId, partnerId, externalOrderId);
        }

        public DeliveryAddressesResult GetDeliveryAddresses(bool excludeAddressesWithoutKladr)
        {
            AssertUser("Невозможно получить возможные адреса доставки");

            return _orders.GetDeliveryAddresses(_principal.ClientId, excludeAddressesWithoutKladr);
        }

        public GiftShopOrdersResult GetOrdersHistory(OrderStatus[] statuses, DateTimeRange range, PagingSettings paging)
        {
            AssertUser("Невозможно получить историю заказов");

            return _orders.GetOrdersHistory(_principal.ClientId, statuses, range, paging);
        }

        public void Dispose()
        {
            // Do nothing
        }

        private void AssertUser(string message)
        {
            if (!_principal.IsAuthenticated)
            {
                throw new InvalidOperationException("Пользователь не аутентифицирован. " + message);
            }
        }
    }
}
