using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Vtb24.Site.Infrastructure;
using Vtb24.Site.Models.Layout;
using Vtb24.Site.Models.Shared;
using Vtb24.Site.Models.Shared.Location;
using Vtb24.Site.Security.Models;
using Vtb24.Site.Services;
using Vtb24.Site.Services.ClientMessageService;
using Vtb24.Site.Services.Models;
using Vtb24.Site.Services.Profile.Models;
using IClientMessageService = Vtb24.Site.Services.IClientMessageService;

namespace Vtb24.Site.Controllers
{
    public class LayoutController : BaseController
    {
        public LayoutController(ClientPrincipal principal, IClientService client, IGiftShop catalog, IGeoService geo, IClientMessageService clientMessage)
        {
            _principal = principal;
            _client = client;
            _catalog = catalog;
            _geo = geo;
            _clientMessage = clientMessage;
        }

        private readonly ClientPrincipal _principal;
        private readonly IClientService _client;
        private readonly IGiftShop _catalog;
        private readonly IGeoService _geo;
        private readonly IClientMessageService _clientMessage;

        [ChildActionOnly]
        public ActionResult Header(string activeMenu, bool enableHeaderDropdown, bool hideRegionSelector, bool searchSiteInitial)
        {
            DumpActivationStatusToViewBag(_client); // TODO: удалить этот хак

            var isAuth = User.Identity.IsAuthenticated;

            var locationTask = Async(() => _client.GetUserLocation());
            var regionsTask = Async(()=>_geo.ListRegions());
            var profileTask = Async(() => isAuth ? _client.GetProfile() : null);
            var basketTask = Async(() => isAuth ? _catalog.GetBasket(PagingSettings.ByOffset(0, BasketController.MAX_BASKET_ITEMS_COUNT)) : null);
            var wishListTask = Async(() => isAuth ? _catalog.GetWishList(PagingSettings.ByOffset(0, WishListController.MAX_WISHLIST_ITEMS_COUNT)) : null);
            var messagesTask = Async(() => isAuth ? GetMessagesStatistics() : null);
            var balanceTask = Async(() => isAuth ? _client.GetBalance() : (decimal?) null);

            Task.WaitAll(
                locationTask,
                regionsTask,
                profileTask,
                basketTask,
                wishListTask,
                messagesTask,
                balanceTask
            );

            // удаляем Байконур из регионов (VTBPLK-2476: При выборе байконура в регионах не отображаются населеные пункты данного региона...)
            var regions = regionsTask.Result
                                     .Where(r => r.KladrCode != "9900000000000")
                                     .Select(LocationItem.FromGeoLocation)
                                     .ToArray();

            var location = locationTask.Result;
            var model = new HeaderModel
            {
                IsAuthenticated = isAuth,
                ActiveMenu = activeMenu,
                EnableHeaderDropdown = enableHeaderDropdown,
                HideRegionSelector = hideRegionSelector,
                SearchSiteInitial = searchSiteInitial,

                Regions = regions,
                UserLocationName = location.Title,
                UserLocationKladr = location.KladrCode
            };

            if (isAuth)
            {
                var profile = profileTask.Result;
                var basket = basketTask.Result;
                var wishList = wishListTask.Result;
                var messages = messagesTask.Result;
                var balance = balanceTask.Result ?? 0;

                model.UserFullName = profile.GetPoliteName();
                model.ShowMyInfoLink = profile.Status == ClientStatus.Activated; // только активированные пользователи видят страницу профиля
                model.Balance = balance;
                model.BasketCount = basket.TotalCount;
                model.WishListCount = wishList.TotalCount;
                model.UnreadMessagesCount = messages.UnreadThreadsCount;
            }

            return View("_Header", model);
        }

        [ChildActionOnly]
        [Authorize]
        public ActionResult ClientMenu(string activeMenu)
        {
            var menu = GetClientMenu().ToArray();

            var selected = menu.SingleOrDefault(m => m.Id == activeMenu);
            if (selected != null)
            {
                selected.IsActive = true;
            }

            return View("Secondary/_ClientMenu", menu);
        }

        // TODO: удалить, используется для теста ошибок сайта
        [HttpGet]
        public ActionResult ThrowException()
        {
            try
            {
                throw new InvalidOperationException("inner error message");
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception("error message", ex);
            }
        }

        private GetStatisticsResult GetMessagesStatistics()
        {
            return _clientMessage.GetStatistics(new GetStatisticsParameters
            {
                ClientId = _principal.ClientId
            });
        }

        private IEnumerable<MenuItemModel> GetClientMenu()
        {
            var statisticTask = GetMessagesStatistics();
            var isClientActivated = _client.GetStatus() == ClientStatus.Activated;

            yield return new MenuItemModel
            {
                Id = "LoyaltyPoints",
                IsRoot = true,
                Text = "Выписка",
                Url = Url.Action("Index", "MyPoints")
            };

            yield return new MenuItemModel
            {
                Id = "Orders",
                IsRoot = true,
                Text = "Заказы",
                Url = Url.Action("Index", "MyOrders")
            };

            yield return new MenuItemModel
            {
                Id = "Messages",
                IsRoot = true,
                Text = "Сообщения",
                BadgeNumber =
                    statisticTask.UnreadThreadsCount > 0
                        ? statisticTask.UnreadThreadsCount
                        : (int?) null,
                BadgeName = "unreads",
                Url = Url.Action("Index", "MyMessages")
            };

            if (isClientActivated)
            {
                yield return new MenuItemModel
                {
                    Id = "MyInformation",
                    IsRoot = true,
                    Text = "Мои данные",
                    Url = Url.Action("Index", "MyInfo")
                };
            }

            yield return new MenuItemModel
            {
                Id = "Calculator",
                IsRoot = true,
                Text = "Бонусный калькулятор",
                Url = Url.Action("Index", "Calculator")
            };
        }
    }
}
