using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Vtb24.Site.Content.Models;
using Vtb24.Site.Content.News;
using Vtb24.Site.Infrastructure;
using Vtb24.Site.Models.Catalog;
using Vtb24.Site.Models.Main;
using Vtb24.Site.Services;
using Vtb24.Site.Services.GiftShop.Catalog.Models.Inputs;
using System.Linq;
using Vtb24.Site.Content.Advertisements;

namespace Vtb24.Site.Controllers
{
    public class MainController : BaseController
    {
        private const int PRODUCTS_COUNT_PER_TAB = 4;
        private const int MAX_NEWS_COUNT = 50;

        public MainController(IGiftShop catalog, IClientService client, INews news, IAdvertisementsManagement advertismentManagement)
        {
            _catalog = catalog;
            _client = client;
            _news = news;
	        _advertisementsManagement = advertismentManagement;
        }

        private readonly IGiftShop _catalog;
        private readonly IClientService _client;
        private readonly INews _news;
	    private readonly IAdvertisementsManagement _advertisementsManagement;

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            DumpActivationStatusToViewBag(_client); // TODO: удалить этот хак

            var isAuth = User.Identity.IsAuthenticated;

            var mostViewedTask = Async(() => _catalog.GetPopularProducts(ProductPopularityType.MostViewed, PRODUCTS_COUNT_PER_TAB));
            var mostOrderedTask = Async(() => _catalog.GetPopularProducts(ProductPopularityType.MostOrdered, PRODUCTS_COUNT_PER_TAB));
            var mostWishedTask = Async(()=> _catalog.GetPopularProducts(ProductPopularityType.MostWished, PRODUCTS_COUNT_PER_TAB));
            var balanceTask = isAuth ? Async(() => (decimal?) _client.GetBalance()) : Task.FromResult((decimal?) null);
            var audiencesTask = isAuth ? Async(() => _client.GetMechanicsContext().ClientGroups) : Task.FromResult((string[]) null);
            var politeNameTask = isAuth ? Async(() => _client.GetProfile().GetPoliteNameWithAppeal()) : Task.FromResult((string) null);

            await Task.WhenAll(
                mostViewedTask,
                mostOrderedTask,
                mostWishedTask, 
                balanceTask, 
                audiencesTask,
                politeNameTask
            );

            var balance = balanceTask.Result;
            var audiences = audiencesTask.Result;
            var politeName = politeNameTask.Result;

            var mostOrdered = mostOrderedTask.Result.Select(x => (object) ListProductModel.Map(x, balance)).ToList();
            var mostViewed = mostViewedTask.Result.Select(x => (object)ListProductModel.Map(x, balance)).ToList();
            var mostWished = mostWishedTask.Result.Select(x => (object)ListProductModel.Map(x, balance)).ToList();

            _catalog.GetCustomPopularProducts(balance ?? 0).Select(CustomProductModel.Map).MixToList(mostOrdered);
            _catalog.GetCustomPopularProducts(balance ?? 0).Select(CustomProductModel.Map).MixToList(mostViewed);
            _catalog.GetCustomPopularProducts(balance ?? 0).Select(CustomProductModel.Map).MixToList(mostWished);

            var model = new MainModel
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                Balance = balance,
                PopularProductsByOrder = mostOrdered,
                PopularProductsByView = mostViewed,
                PopularProductsByWish = mostWished,
				ShowBonusBackBanner = false
            };

            if (isAuth)
            {
                var recommended = CatalogController.RecomendedProductModels(balance, _catalog)
                                                   .Select(r => (object) RecomendedProductModel.Map(r))
                                                   .ToList();

                _catalog.GetCustomRecommendedProducts(balance ?? 0)
                        .Select(CustomProductModel.Map)
                        .MixToList(recommended);

                model.PoliteName = politeName;
                model.RecomendedProducts = recommended;

                var advertisements = _advertisementsManagement
                    .GetAdvertisements(_client.GetProfile().ClientId, PagingSettings.ByOffset(0, 1))
                    .FirstOrDefault(a => !a.Advertisement.ShowUntil.HasValue || DateTime.Now < a.Advertisement.ShowUntil.Value);

	            model.ShowBonusBackBanner = advertisements != null;
            }

            var news = _news.GetNewsMessages(audiences, PagingSettings.ByOffset(0, MAX_NEWS_COUNT));
            model.News = news.Select(NewsMessageListModel.Map).ToArray();

            return View(model);
        }

        [HttpGet]
        public ActionResult PleaseRegister()
        {
            return View("PleaseRegister");
        }

        [HttpGet]
        public ActionResult Calculators()
        {
            return View("Calculators");
        }

    }
}
