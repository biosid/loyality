using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Vtb24.Site.Filters;
using Vtb24.Site.Helpers;
using Vtb24.Site.Infrastructure;
using Vtb24.Site.Infrastructure.Caching;
using Vtb24.Site.Models.Basket;
using Vtb24.Site.Services;
using Vtb24.Site.Services.GiftShop.Basket.Models.Exceptions;
using Vtb24.Site.Services.GiftShop.Catalog.Models.Exceptions;
using Vtb24.Site.Services.Models;

namespace Vtb24.Site.Controllers
{
    [Authorize]
    [ClientActivated]
    public class BasketController : BaseController
    {
        public const int MAX_BASKET_ITEMS_COUNT = 100;

        public BasketController(IGiftShop catalog, IClientService client, ICacheCleaner cacheCleaner)
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
            if (TempData.ContainsKey("error"))
            {
                _cacheCleaner.CleanBasket();
            }

            var basketTask = Async(() => _catalog.GetBasket(PagingSettings.ByOffset(0, MAX_BASKET_ITEMS_COUNT)));
            var balanceTask = Async(() => _client.GetBalance());

            Task.WaitAll(basketTask, balanceTask);

            var basket = basketTask.Result;
            var balance = balanceTask.Result;

            var model = new BasketModel
            {
                Balance = balance,
                MaxOrderCost = ProductHelpers.MaxCost(balance),
                Items = basket.Select(i => BasketItemModel.Map(i, balance)).ToArray()
            };
            return View("Index", model);
        }

        [HttpGet]
        public ActionResult Add(string id)
        {
            try
            {
                var product = _catalog.GetProduct(id, false);
                _catalog.AddToBasket(product.Product.Id);
            }
            catch (BasketItemQuantityOverflowException)
            {
                TempData["error"] = "Вы можете заказать не более 9 экземпляров одного вознаграждения.";
            }
            catch (BasketItemPriceNotFixedException)
            {
                TempData["error"] = "Извините, это вознаграждение временно недоступно.";
            }
            catch (ProductNotFoundException)
            {
                TempData["error"] = "Извините, это вознаграждение временно недоступно.";
            }
            catch (BasketServiceException)
            {
                TempData["error"] = "Извините, это вознаграждение временно недоступно.";
            }
            
            return RedirectToAction("Index", "Basket");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Update(string id, int quantity = 1, bool ajax = false)
        {
            if (quantity <= 0)
            {
                quantity = 1;
            }

            try
            {
                _catalog.UpdateBasketItemQuantity(id, quantity);
            }
            catch (ProductNotFoundException)
            {
                TempData["error"] = "Извините, это вознаграждение временно недоступно.";
            }

            if (ajax)
            {
                var basket = _catalog.GetBasket(PagingSettings.ByOffset(0, MAX_BASKET_ITEMS_COUNT));
                var item = basket.First(i => i.Product.Id == id);

                return Json(new { itemPrice = item.ItemPrice, itemTotal = item.TotalQuantityPrice }, JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Index", "Basket");
        }

        [HttpPost]
        public ActionResult Buy(string[] id, int[] quantity)
        {
            var items = _catalog.GetBasketItems(id);

            // актуализируем количество товаров в позициях (пользователь мог поменять количество не нажав "Обновить")
            var itemsToUpdateQuantity = items
                .Zip(quantity, (p, q) => new { Item = p, UserQuantity = q < 1 ? 1 : q })
                .Where(i => i.Item != null && i.UserQuantity != i.Item.Quantity);
            foreach (var itemToUpdate in itemsToUpdateQuantity)
            {
                try
                {
                    _catalog.UpdateBasketItemQuantity(itemToUpdate.Item.Product.Id, itemToUpdate.UserQuantity);
                }
                catch (Exception e)
                {
                    LogError(e);
                }
            }

            return Redirect(Url.Action("Items", "Buy") + "?" + string.Join("&", id.Select(x => "id=" + x)));
        }

        [HttpGet]
        public ActionResult Remove(string id, bool ajax=false)
        {
            _catalog.RemoveFromBasket(id);

            return ajax ? (ActionResult) new EmptyResult() : RedirectToAction("Index", "Basket");
        }
    }
}
