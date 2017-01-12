using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Vtb24.Site.Filters;
using Vtb24.Site.Infrastructure;
using Vtb24.Site.Infrastructure.Caching;
using Vtb24.Site.Models.Basket;
using Vtb24.Site.Models.WishList;
using Vtb24.Site.Services;
using Vtb24.Site.Services.GiftShop.Basket.Models.Exceptions;
using Vtb24.Site.Services.GiftShop.Catalog.Models.Exceptions;
using Vtb24.Site.Services.GiftShop.Model;
using Vtb24.Site.Services.GiftShop.Wishlist.Models.Exceptions;
using Vtb24.Site.Services.Models;

namespace Vtb24.Site.Controllers
{
    [Authorize]
    public class WishListController : BaseController
    {
        public const int MAX_WISHLIST_ITEMS_COUNT = 100;

        public WishListController(IGiftShop catalog, IClientService client, ICacheCleaner cacheCleaner)
        {
            _catalog = catalog;
            _client = client;
            _cacheCleaner = cacheCleaner;
        }

        private readonly IGiftShop _catalog;
        private readonly IClientService _client;
        private readonly ICacheCleaner _cacheCleaner;

        [HttpGet]
        public ActionResult Index()
        {
            DumpActivationStatusToViewBag(_client); // TODO: удалить этот хак

            var wishesTask = Async(() => _catalog.GetWishList(PagingSettings.ByPage(1, MAX_WISHLIST_ITEMS_COUNT)));
            var balanceTask = Async(() => _client.GetBalance());

            Task.WaitAll(
                wishesTask,
                balanceTask
            );

            var wishes = wishesTask.Result;
            var balance = balanceTask.Result;

            var model = new WishListModel
            {
                Items = wishes.Select(p => BasketItemModel.Map(p, balance)).ToArray(),
                Balance = balance
            };

            return View("Index", model);
        }

        [HttpGet]
        public ActionResult Add(string id)
        {
            try
            {
                _catalog.AddToWishList(id);
                _catalog.UpdateWishListItemQuantity(id, 1); // TODO: перенести фикс в бэкенд: каждый товар должен существовать в одном экземпляре
            }
            catch (ProductNotFoundException)
            {
                TempData["error"] = "Извините, это вознаграждение временно недоступно.";
            }
            catch (WishListItemQuantityOverflowException)
            {
                TempData["error"] = "Вы можете заказать не более 9 экземпляров одного вознаграждения.";
            }

            //TODO удалить как только соберём статистику по вишлисту для акции
            LogWishlistStatistics(id);
            
            return RedirectToAction("Index", "WishList");
        }

        private void LogWishlistStatistics(string id)
        {
            //TODO удалить как только соберём статистику по вишлисту для акции
            try
            {
                var product = _catalog.GetProduct(id, false);
                var baseUrl = string.Format("{0}://{1}", Request.Url.Scheme, Request.Headers["host"]).TrimEnd('/');
                var query = string.Format(
                    "/Content/statistics.txt?type=wishlist&product={0}&price={1}&partner={2}&category={3}&balance={4}",
                    id,
                    product.Product.Price,
                    product.Product.PartnerId,
                    product.Product.CategoryId,
                    _client.GetBalance()
                );

                var client = new HttpClient();

                foreach (var header in Request.Headers.AllKeys)
                {
                    client.DefaultRequestHeaders.Add(header, Request.Headers[header]);
                }
                client.GetAsync(baseUrl + query).Wait();
            }
            catch (Exception e)
            {
                LogError(e);
            }
        }


        [HttpGet]
        public ActionResult Remove(string id, bool ajax = false)
        {
            _catalog.RemoveFromWishList(id);
            
            return ajax ? (ActionResult) new EmptyResult() : RedirectToAction("Index", "WishList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ClientActivated]
        public ActionResult MoveToBasket(params string[] product)
        {
            // если не переданы конкретные вознаграждения, значит пользователь пожелал заказать весть список желаемых
            if (product == null)
            {
                product = _catalog
                    .GetWishList(PagingSettings.ByPage(1, MAX_WISHLIST_ITEMS_COUNT))
                    .Where(i=>i.ProductStatus == ProductStatus.Available) // исключаем недоступные
                    .Select(i=>i.Product.Id).ToArray();
                // P.S. Сверку баланса с итоговой ценой оставим на откуп фронтенду
            }

            foreach (var id in product)
            {
                var errors = new List<string>();
                try
                {
                    _catalog.AddToBasket(id, 1); // TODO: можно вызвать паралельно
                }
                catch (BasketItemQuantityOverflowException)
                {
                    errors.Add("Вы можете заказать не более 9 экземпляров одного вознаграждения.");
                }
                catch (BasketItemPriceNotFixedException)
                {
                    _cacheCleaner.CleanWishList();
                    errors.Add("Некоторые вознаграждения временно недоступны.");
                }
                catch (ProductNotFoundException)
                {
                    _cacheCleaner.CleanWishList();
                    errors.Add("Некоторые вознаграждения временно недоступны.");
                }

                if (errors.Any())
                {
                    TempData["error"] = "Извините, в корзину добавились не все вознаграждения. " + string.Join(" ", errors.Distinct().ToArray());
                }
            }

            return RedirectToAction("Index", "Basket");
        }

    }
}
