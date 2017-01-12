using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Vtb24.Site.Content.Pages;
using Vtb24.Site.Content.Pages.Models;
using Vtb24.Site.Content.Pages.Models.Inputs;
using Vtb24.Site.Filters;
using Vtb24.Site.Infrastructure;
using Vtb24.Site.Models.Catalog;
using Vtb24.Site.Models.Catalog.Helpers;
using Vtb24.Site.Models.Main;
using Vtb24.Site.Models.Shared;
using Vtb24.Site.Services;
using Vtb24.Site.Services.GiftShop.Catalog.Models;
using Vtb24.Site.Services.GiftShop.Catalog.Models.Exceptions;
using Vtb24.Site.Services.GiftShop.Catalog.Models.Inputs;
using Vtb24.Site.Services.GiftShop.Catalog.Models.Outputs;
using Vtb24.Site.Services.Models;
using Vtb24.Site.Services.Profile.Models;

namespace Vtb24.Site.Controllers
{
    public class CatalogController : BaseController
    {
        public const int PAGE_SIZE = 30;

        public const int RECOMMENDED_PRODUCTS_COUNT = 6;

        public const int RECOMMENDED_PRODUCT_TITLE = 40;

        private const int MAX_CATEGORIES_COUNT = 500;

        private const string SEARCH_CATEGORY_TITLE = "Поиск";
        private const string SALE_CATEGORY_TITLE = "Распродажа до 50%";
        private const string NOVELTY_CATEGORY_TITLE = "Новинки";
        private const string BANK_PORODUCTS_CATEGORY_TITLE = "Продукты банка";

        private const string SALE_CATEGORY_ICON = "sale.png";
        private const string NOVELTY_CATEGORY_ICON = null;
        private const string BANK_PORODUCTS_CATEGORY_ICON = "card.png";

        private const string MAIN_CRUMB_TITLE = "Главная";
        private const string CATALOG_CRUMB_TITLE = "Каталог";

        public CatalogController(IGiftShop catalog, IClientService client, IOnlineCategoryService online, IPagesManagement pages)
        {
            _catalog = catalog;
            _client = client;
            _online = online;
            _pages = pages;
        }

        private readonly IGiftShop _catalog;
        private readonly IClientService _client;
        private readonly IOnlineCategoryService _online;
        private readonly IPagesManagement _pages;

        private static readonly GetPlainPagesOptions GetOnlineCategoryPreviewPageOptions = new GetPlainPagesOptions
        {
            IsBuiltin = false,
            Status = PageStatus.Active
        };

        private static readonly BalanceGroup[] BalanceGroups;

        private static readonly Dictionary<int, string> CategoryIcons;

        private static readonly Dictionary<string, string> BankProductsThumbnails;

        static CatalogController()
        {
            var balanceGroupsStr = ConfigurationManager.AppSettings["recommended_balance_groups"];
            BalanceGroups = balanceGroupsStr.Split(';')
                                            .Select(g => g.Split(','))
                                            .Select(g => new BalanceGroup
                                            {
                                                Balance = decimal.Parse(g[0], CultureInfo.InvariantCulture),
                                                MinPrice = decimal.Parse(g[1], CultureInfo.InvariantCulture),
                                                MaxPrice = decimal.Parse(g[2], CultureInfo.InvariantCulture)
                                            })
                                            .ToArray();

            var categoryIconsStr = ConfigurationManager.AppSettings["catalog_category_icons"];
            CategoryIcons = categoryIconsStr.Split(';')
                                            .Where(i => !string.IsNullOrWhiteSpace(i))
                                            .Select(i => i.Split(','))
                                            .ToDictionary(i => int.Parse(i[0]), i => i[1]);

            var bankProductsThumbnailsStr = ConfigurationManager.AppSettings["catalog_bank_products_thumbnails"];
            BankProductsThumbnails = bankProductsThumbnailsStr.Split(';')
                                                              .Where(t => !string.IsNullOrWhiteSpace(t))
                                                              .Select(t => t.Split(','))
                                                              .ToDictionary(t => t[0], t => t[1]);
        }

        #region Actions

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Index(int? id, CatalogQueryModel query, int page = 1)
        {
            DumpActivationStatusToViewBag(_client); // TODO: удалить этот хак

            var isAuth = User.Identity.IsAuthenticated;

            var balanceTask = Async(() => isAuth ? _client.GetBalance() : (decimal?) null);
            var categoryTask = Async(() => id.HasValue ? _catalog.GetCategoryInfo(id.Value) : null);
            var categoriesTask = Async(GetCategories);

            try
            {
                Task.WaitAll(
                    balanceTask,
                    categoryTask,
                    categoriesTask
                );
            }
            catch (AggregateException ae)
            {
                if (ae.InnerException is CategoryNotFoundException)
                {
                    throw new HttpException(404, "категория не найдена", ae.InnerException);
                }
                throw;
            }

            var balance = balanceTask.Result;
            var category = categoryTask.Result;
            var categories = categoriesTask.Result.ToArray();

            // если выбрана категория, в которой нет товаров, а есть только вложенные онлайн-категории,
            // делаем редирект на первую онлайн категорию
            // НО если пользователь не активирован И есть статическая страница-заглушка для такой категори -- показываем её
            if (id.HasValue && category.Category.ProductsCount == 0)
            {
                var onlineChild = categories.FirstOrDefault(c => c.ParentId == id.Value && c.CategoryType == CategoryType.Online);
                if (onlineChild != null)
                {
                    var showPreview = !isAuth || !ClientActivatedAttribute.IsClientActivated();
                    var previewUrl = GetOnlineCategoryPreviewUrl(id.Value);

                    if (showPreview && _pages.HasPageWithUrl(previewUrl, GetOnlineCategoryPreviewPageOptions))
                    {
                        return Redirect(previewUrl);
                    }

                    return RedirectToAction("OnlineCategory", "Catalog", new { id = onlineChild.Id });
                }
            }

            var catalogSections = GetCatalogSections(categories).ToArray();

            var breadCrumbs = GetCategoryBreadCrumbs(category, query.sale, query.is_new).ToArray();

            var showFilters = category != null && category.Category.Depth >= 2;

            var parameters = showFilters ? GetCategoryParameters(category.Category.Id) : null;
            var parametersCriteria = parameters != null ? GetParametersCriteria(parameters).ToArray() : null;

            var products = FindProducts(id, query, page, parametersCriteria); // синхронно
            var usePostMethod = parameters != null && parameters.Length > 0;
            var filters = CreateFilters(category, usePostMethod, products.MaxPrice, query, parameters);

            var model = new ProductCatalogModel
            {
                CatalogSections = catalogSections,
                BreadCrumbs = breadCrumbs,
                Products = products.Select(x => ListProductModel.Map(x, balance)).ToArray(),
                Filters = filters,
                Total = products.TotalCount,
                Query = query,
                TotalPages = products.TotalPages,
                Page = page,
                Keywords = id.HasValue && category != null && !string.IsNullOrWhiteSpace(category.Category.Title)
                               ? GetCategoryKeywords(category.Category.Title)
                               : CatalogKeywords
            };

            if (model.Page > model.TotalPages || model.Page < 1)
            {
                throw new HttpException(404, "страница каталога не найдена");
            }
            return View("Category", model);
        }

        [HttpGet]
        public ActionResult Actions(CatalogQueryModel query, int page = 1)
        {
            query.sale = true;
            return Index(null, query, page);
        }

        [HttpGet]
        public ActionResult New(CatalogQueryModel query, int page = 1)
        {
            query.is_new = true;
            return Index(null, query, page);
        }

        [HttpGet]
        public ActionResult Search(CatalogQueryModel query, int page = 1)
        {
            DumpActivationStatusToViewBag(_client); // TODO: удалить этот хак

            var isAuth = User.Identity.IsAuthenticated;

            var balanceTask = Async(() => isAuth ? _client.GetBalance() : (decimal?)null);
            var categoryTask = Async(() => query.category.HasValue ? _catalog.GetCategoryInfo(query.category.Value) : null);
            var productsTask = Async(() => FindProducts(query.category, query, page, null));
            var catalogTask = Async(GetCategories);

            Task.WaitAll(
                balanceTask,
                categoryTask,
                productsTask,
                catalogTask
            );

            var balance = balanceTask.Result;
            var category = categoryTask.Result;
            var products = productsTask.Result;
            var rootCategories = catalogTask.Result
                                            .Where(c => c.ParentId == null &&
                                                        c.ProductsCount > 0 &&
                                                        c.CategoryType == CategoryType.Static)
                                            .Select(c =>
                                                    new SelectListItem
                                                    {
                                                        Text = c.Title,
                                                        Value = c.Id.ToString(CultureInfo.InvariantCulture),
                                                        Selected = c.Id == query.category
                                                    })
                                            .ToArray();

            var breadCrumbs = GetStartBreadCrumbs()
                .Concat(Enumerable.Repeat(new BreadCrumbModel { Text = SEARCH_CATEGORY_TITLE }, 1))
                .ToArray();

            var filters = CreateFilters(category, false, products.MaxPrice, query, null);

            var model = new SearchModel
            {
                BreadCrumbs = breadCrumbs,
                Products = products.Select(x => ListProductModel.Map(x, balance)).ToArray(),
                Filters = filters,
                Total = products.TotalCount,
                Query = query,
                TotalPages = products.TotalPages,
                Page = page,
                Categories = new[] { new SelectListItem { Selected = true } }.Concat(rootCategories).ToArray()
            };
            return View("Search", model);
        }

        [HttpGet]
        public ActionResult Product(string id)
        {
            DumpActivationStatusToViewBag(_client); // TODO: удалить этот хак

            if (string.IsNullOrEmpty(id))
            {
                throw new HttpException(404, "не указан id вознаграждения");
            }

            var isAuth = User.Identity.IsAuthenticated;

            CatalogProduct product;
            CatalogCategoryInfo category;

            try
            {
                product = _catalog.GetProduct(id, countAsView: true);

                if (product == null)
                {
                    return ProductNotFound();
                }

                category = _catalog.GetCategoryInfo(product.Product.CategoryId); // синхронно
            }
            catch (ProductNotFoundException)
            {
                return ProductNotFound();
            }
            catch (CategoryNotFoundException)
            {
                return ProductNotFound();
            }

            var balanceTask = Async(() => isAuth ? _client.GetBalance() : (decimal?) null);
            var recomendedTask = Async(() => _catalog.GetRecommendedProductsForCategory(product.Product.CategoryId, RECOMMENDED_PRODUCTS_COUNT + 1));

            Task.WaitAll(balanceTask, recomendedTask);

            var balance = balanceTask.Result;

            // Перемешиваем рекомендуемые ПОСЛЕ того, как исключили показываемое вознаграждение.
            // В противном случае если количество рекомендуемых в каталоге меньше RECOMMENDED_PRODUCTS_COUNT,
            // то есть вероятность исключить рекомендуемый и показать случайный.
            var recommended = recomendedTask.Result.Any()
                                  ? recomendedTask.Result
                                                  .Where(p => p.Product.Id != id)
                                                  .Take(RECOMMENDED_PRODUCTS_COUNT)
                                                  .Select(RecomendedProductModel.Map)
                                                  .ToArray()
                                                  .Shuffle()
                                                  .ToArray()
                                  : null;

            if (recommended != null)
            {
                foreach (var rec in recommended)
                {
                    rec.Title = rec.Title.Truncate(RECOMMENDED_PRODUCT_TITLE, TruncateOptions.IncludeEllipsis | 
                                                                              TruncateOptions.FinishWord);
                }
            }

            var model = ProductModel.Map(product, balance);
            model.BreadCrumbs = GetCategoryBreadCrumbs(category).ToArray();
            model.RecomendedProducts = recommended;
            model.Keywords = GetProductKeywords(model.Title);

            return View("Product", model);
        }

        [ChildActionOnly]
        public ActionResult ProductNotFound()
        {
            var isAuth = User.Identity.IsAuthenticated;
            var balance = isAuth ? _client.GetBalance() : (decimal?) null;
            var recommended = RecomendedProductModels(balance, _catalog);
            var products = recommended.Select(p => ListProductModel.Map(p, balance)).ToArray();
           
            return View("ProductNotFound", products);
        }

        [ChildActionOnly]
        public ActionResult HeaderDropdownMenu()
        {
            var categories = GetCategories().ToArray();
            var catalogSections = GetCatalogSections(categories).ToArray();

            return PartialView("_CatalogDropDown", catalogSections);
        }

        [HttpGet]
        public ActionResult DummyOnlineCategory()
        {
            return View("DummyOnlineCategory");
        }

        [HttpGet]
        public ActionResult OnlineCategory(int? id)
        {
            if (!id.HasValue)
            {
                throw new HttpException(404, "не указан id онлайн-категории");
            }

            if (!User.Identity.IsAuthenticated)
            {
                var previewUrl = GetOnlineCategoryPreviewUrl(id.Value);

                if (_pages.HasPageWithUrl(previewUrl, GetOnlineCategoryPreviewPageOptions))
                {
                    return Redirect(previewUrl);
                }

                return RedirectToAction("PleaseRegister", "Main");
            }

            if (!ClientActivatedAttribute.IsClientActivated())
            {
                var previewUrl = GetOnlineCategoryPreviewUrl(id.Value);

                if (_pages.HasPageWithUrl(previewUrl, GetOnlineCategoryPreviewPageOptions))
                {
                    return Redirect(previewUrl);
                }

                return RedirectToAction("Index", "ActivationRequired");
            }


            var categoryTask = Async(() => _catalog.GetCategoryInfo(id.Value));
            var urlTask = Async(() => _online.GetIframeUrl(id.Value));

            try
            {
                Task.WaitAll(categoryTask, urlTask);
            }
            catch (AggregateException ae)
            {
                if (ae.InnerException is CategoryNotFoundException)
                {
                    throw new HttpException(404, "категория не найдена", ae.InnerException);
                }
                throw;
            }

            var category = categoryTask.Result;
            var url = urlTask.Result;

            var model = new OnlineCategoryModel
            {
                Name = category.Category.Title,
                CategoryUrl = url
            };
            return View("OnlineCategory", model);
        }

        [HttpGet]
        public ActionResult BankProducts(int page = 1)
        {
            if (!User.Identity.IsAuthenticated)
            {
                var guestModel = new GuestBankProductsModel
                {
                    CatalogSections = GetCatalogSections(GetCategories().ToArray()).ToArray(),
                    BreadCrumbs = GetBankProductsBreadCrumbs().ToArray()
                };

                return View("BankProductsForGuest", guestModel);
            }

            var products = _client.GetBankProducts(PagingSettings.ByPage(page, PAGE_SIZE));

            if (products.TotalCount == 0)
            {
                return View("BankProductsEmpty");
            }

            var balanceTask = Async(() => _client.GetBalance());
            var categoriesTask = Async(GetCategories);
            var statusTask = Async(() => _client.GetStatus());

            Task.WaitAll(balanceTask, categoriesTask, statusTask);

            var balance = balanceTask.Result;
            var categories = categoriesTask.Result.ToArray();
            var isUserActivated = statusTask.Result == ClientStatus.Activated;

            if (page > products.TotalPages || page < 1)
            {
                throw new HttpException(404, "страница не найдена");
            }

            var catalogSections = GetCatalogSections(categories).ToArray();

            var breadCrumbs = GetBankProductsBreadCrumbs().ToArray();

            var model = new BankProductsModel
            {
                BreadCrumbs = breadCrumbs,
                CatalogSections = catalogSections,
                Products = products
                    .Select(p => new ListBankProductModel
                    {
                        Id = p.Id,
                        Title = p.Description,
                        Thumbnail = GetBankProductThumbnail(p.ProductId),
                        ExpirationDate = p.ExpirationDate,
                        Price = p.Cost,
                        CanRedeem = isUserActivated && p.Cost <= balance,
                        RedeemDenyReason = !isUserActivated
                                               ? "Для заказа необходимо активироваться"
                                               : (p.Cost > balance
                                                      ? "Для заказа недостаточно бонусов"
                                                      : null)
                    })
                    .ToArray(),
                IsUserActivated = isUserActivated,
                Page = page,
                TotalPages = products.TotalPages
            };

            return View("BankProducts", model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult BankProduct(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new HttpException(404, "не указан id продукта банка");
            }

            var product = _client.GetBankProduct(id);

            if (product == null)
            {
                throw new HttpException(404, "не найден продукт банка");
            }

            var balanceTask = Async(() => _client.GetBalance());
            var statusTask = Async(() => _client.GetStatus());

            Task.WaitAll(balanceTask, statusTask);

            var balance = balanceTask.Result;

            var isUserActivated = statusTask.Result == ClientStatus.Activated;

            var model = BankProductModel.Map(product);

            model.IsUserActivated = isUserActivated;
            model.Thumbnail = GetBankProductThumbnail(product.ProductId);
            model.BreadCrumbs = GetBankProductsBreadCrumbs().ToArray();
            model.CanRedeem = isUserActivated && balance >= model.Price;

            var hasDeficit = isUserActivated && balance < model.Price;

            model.PointsDeficit = hasDeficit ? model.Price - balance : (decimal?) null;
            model.PointsProgress = hasDeficit && model.Price != 0m
                                       ? Math.Min(Math.Round(100 * balance / model.Price), 100)
                                       : 0m;

            return View("BankProduct", model);
        }

        #endregion

        private class BalanceGroup
        {
            public decimal Balance { get; set; }

            public decimal MinPrice { get; set; }

            public decimal MaxPrice { get; set; }
        }

        public static CatalogProduct[] RecomendedProductModels(decimal? balance, IGiftShop catalog)
        {
            var group = BalanceGroups.First(g => g.Balance > (balance ?? 0));

            var recommended = catalog.GetRecommendedProductsForPriceRange(group.MinPrice, group.MaxPrice, MainModel.RECOMENDED_PRODUCTS_COUNT)
                .Shuffle()
                .ToArray();
            return recommended;
        }


        #region Приватные методы

        private SearchResult FindProducts(int? categoryId, CatalogQueryModel query, int page, CatalogParameterCriteria[] parameters)
        {
            var criteria = CreateSearchCriteria(categoryId, query, parameters);

            var products = _catalog.Search(criteria, PagingSettings.ByPage(page, PAGE_SIZE));
            return products;
        }

        private static SearchCriteria CreateSearchCriteria(int? categoryId, CatalogQueryModel query, CatalogParameterCriteria[] parameters)
        {
            // нормализуем query
            if (query.min_price > query.max_price)
            {
                var min = query.min_price;
                query.min_price = query.max_price;
                query.max_price = min;
            }

            var criteria = new SearchCriteria
            {
                Categories = categoryId.HasValue ? new[] { categoryId.Value } : null,
                Sorting = query.sort.Map(),
                MinPrice = query.min_price,
                MaxPrice = query.max_price,
                SearchTerm = query.term,
                IsActionPrice = query.sale,
                MinAddedToCatalogDate = query.is_new ? ProductNoveltyCriteria.GetMinNoveltyDate() : (DateTime?)null,
                Parameters = parameters
            };
            if (query.vendor != null && query.vendor.Any())
            {
                criteria.Vendors = query.vendor
                                        .Where(v => !string.IsNullOrEmpty(v))
                                        .Select(v => v == FilterModel.VENDOR_NOT_SET ? "" : v)
                                        .Distinct()
                                        .ToArray();
            }

            return criteria;
        }

        private IEnumerable<CatalogSectionModel> GetCatalogSections(CatalogCategory[] catalog)
        {
            // Пояснение:
            //   Section - раздел каталога (категория самого верхнего уровня)
            //   Rubric - рубрика каталога (подкатегории разделов)
            //   SubRubric - подрубрики каталога (подкатегории рубрик)
            // Данный метод возвращает для каждого раздела все его рубрики, разбитые на группы - "колонки".
            // При этом:
            // - число "колонок" = 3;
            // - порядок рубрик не нарушается, то есть первая рубрика находится в начале первой "колонки", последняя рубрика - в конце последней;
            // - размер "колонки" - общее число рубрик и подрубрик, входящих в нее;
            // - в общем случае существует несколько возможных разбиений рубрик на "колонки", удовлетворящих предыдущим условиям.
            // Для выбора разбиения происходит перебор всех вариантов в следующем порядке:
            // - сначала вариант одной "колонкой";
            // - потом все разбиения на две "колонки", в порядке уменьшения размера первой "колонки";
            // - потом все разбиения на три "колонки", в порядке уменьшения размера сначала первой, а потом второй "колонки".
            // Выбирается вариант, удовлетворящий следующим условиям:
            // - если максимальный размер "колонки" очередного разбиения менее параметра MinColumnSize, то выбирается он;
            // - среди остальных разбиений ищется первое, имеющее минимальное значение максимального размера "колонки".

            catalog = GetMenuCategories(catalog).ToArray();

            var sectionsCount = catalog.Count(c => !c.ParentId.HasValue);

            // считаем, что минимальный размер подменю примерно равен <кол-во разделов каталога> * 4/3
            // так как разделы меню на верстке занимают немного больше места
            var minColumnSize = sectionsCount * 4 / 3;

            var sections = catalog.Where(c => !c.ParentId.HasValue)
                                  .Select(c => new CatalogSectionModel
                                  {
                                      Id = c.Id,
                                      Text = c.Title,
                                      Url = GetCategoryUrl(c),
                                      IconName = CategoryIcons.ContainsKey(c.Id)
                                                     ? CategoryIcons[c.Id]
                                                     : null
                                  });

            // Категория АКЦИИ
            if (ConfigurationManager.AppSettings["catalog_actions_category_enaled"] == "true")
            {
                ++sectionsCount;
                sections = Enumerable.Repeat(CreateSaleSection(), 1)
                                     .Concat(sections);
            }

            // Категория Продукты банка
            if (ConfigurationManager.AppSettings["catalog_bank_products_category_enabled"] == "true")
            {
                ++sectionsCount;
                sections = Enumerable.Repeat(CreateBankProductsSection(), 1)
                                     .Concat((sections));
            }

            // Категория НОВИНКИ
            if (ConfigurationManager.AppSettings["catalog_new_category_enaled"] == "true")
            {
                ++sectionsCount;
                sections = sections.Concat(Enumerable.Repeat(CreateNoveltySection(), 1));
            }

            foreach (var section in sections)
            {
                section.RubricsColumns = section.Id >= 0
                                             ? GetCatalogRubricsColumns(section.Id, catalog, minColumnSize)
                                             : null;
                yield return section;
            }
        }

        private CatalogSectionModel CreateBankProductsSection()
        {
            return new CatalogSectionModel
            {
                Id = -1,
                Text = BANK_PORODUCTS_CATEGORY_TITLE,
                IconName = BANK_PORODUCTS_CATEGORY_ICON,
                Url = Url.Action("BankProducts", "Catalog")
            };
        }

        private CatalogSectionModel CreateSaleSection()
        {
            return new CatalogSectionModel
            {
                Id = -1,
                Text = SALE_CATEGORY_TITLE,
                IconName = SALE_CATEGORY_ICON,
                Url = Url.Action("Actions", "Catalog")
            };
        }


        private CatalogSectionModel CreateNoveltySection()
        {
            return new CatalogSectionModel
            {
                Id = -1,
                Text = NOVELTY_CATEGORY_TITLE,
                IconName = NOVELTY_CATEGORY_ICON,
                Url = Url.Action("New", "Catalog")
            };
        }

        private CatalogRubricModel[][] GetCatalogRubricsColumns(int catalogSectionId, CatalogCategory[] catalog, int minColumnSize)
        {
            var rubrics = GetCatalogRubrics(catalogSectionId, catalog).ToArray();

            return rubrics.Length > 0
                       ? rubrics.SplitToColumns(minColumnSize)
                       : null;
        }

        private IEnumerable<CatalogRubricModel> GetCatalogRubrics(int catalogSectionId, CatalogCategory[] catalog)
        {
            return catalog.Where(c => c.ParentId.HasValue && c.ParentId.Value == catalogSectionId)
                          .Select(c => new CatalogRubricModel
                          {
                              Id = c.Id,
                              Text = c.Title,
                              Url = GetCategoryUrl(c),
                              SubRubrics = GetCatalogSubRubrics(c.Id, catalog).ToArray()
                          });
        }

        private IEnumerable<CatalogCategoryModel> GetCatalogSubRubrics(int catalogRubricId, IEnumerable<CatalogCategory> catalog)
        {
            return catalog.Where(c => c.ParentId.HasValue && c.ParentId.Value == catalogRubricId)
                          .Select(c => new CatalogRubricModel
                          {
                              Id = c.Id,
                              Text = c.Title,
                              Url = GetCategoryUrl(c)
                          });
        }

        private string GetCategoryUrl(CatalogCategory category)
        {
            return category.CategoryType == CategoryType.Online
                       ? Url.Action("OnlineCategory", "Catalog", new { id = category.Id })
                       : Url.Action("Index", "Catalog", new { id = category.Id });
        }

        private static IEnumerable<CatalogCategory> GetMenuCategories(CatalogCategory[] catalog)
        {
            var onlineCategories = catalog.Where(c => c.CategoryType == CategoryType.Online).ToArray();

            return catalog.Where(c => IsMenuCategory(c, onlineCategories));
        }

        private static bool IsMenuCategory(CatalogCategory category, IEnumerable<CatalogCategory> onlineCategories)
        {
            return category.Depth <= 3 &&
                   (category.CategoryType == CategoryType.Online ||
                    (category.CategoryType == CategoryType.Static &&
                     (category.ProductsCount > 0 || onlineCategories.Any(online => online.CategoryPath.Contains(category.CategoryPath)))));
        }

        private CatalogParameterModel[] GetCategoryParameters(int categoryId)
        {
            return _catalog.GetCategoryParameters(categoryId)
                           .Select(p => CatalogParameterModel.Map(p, Request.Params))
                           .ToArray();
        }

        private IEnumerable<CatalogParameterCriteria> GetParametersCriteria(IEnumerable<CatalogParameterModel> parameters)
        {
            return parameters.Where(p => !string.IsNullOrEmpty(p.SelectedValue))
                             .Select(p => new CatalogParameterCriteria
                             {
                                 Name = p.Name,
                                 Value = p.SelectedValue,
                                 Unit = p.Unit
                             });
        }

        private FilterModel CreateFilters(CatalogCategoryInfo category, bool usePostMethod, decimal maxPrice, CatalogQueryModel query, CatalogParameterModel[] parameters)
        {
            // расширенные фильтры доступны только в конкретных категориях 2-го уровня и глубже
            if (!string.IsNullOrEmpty(query.term) || category == null || category.Category.Depth < 2)
            {
                return FilterModel.Map(usePostMethod, query, maxPrice);
            }

            // производители
            var vendors = _catalog.GetFilterMetaData(category.Category.Id);

            return FilterModel.Map(
                usePostMethod,
                query,
                maxPrice,
                vendors.Vendors,
                parameters);

        }

        private IEnumerable<CatalogCategory> GetCategories()
        {
            // рубрикатор (1-й уровень - раздел; 2-й уровень - рубрика; 3-й уровень - подрубрика)
            return _catalog.GetCategories(null, 3, PagingSettings.ByOffset(0, MAX_CATEGORIES_COUNT));
        }

        private IEnumerable<BreadCrumbModel> GetStartBreadCrumbs()
        {
            yield return new BreadCrumbModel
            {
                Text = MAIN_CRUMB_TITLE,
                Url = "/"
            };
            yield return new BreadCrumbModel
            {
                Text = CATALOG_CRUMB_TITLE,
                Url = Url.Action("Index", "Catalog", new { @id = "" })
            };
        }

        private IEnumerable<BreadCrumbModel> GetCategoryBreadCrumbs(CatalogCategoryInfo info, bool sale = false, bool novelty = false)
        {
            var breadCrumbs = GetStartBreadCrumbs();

            if (info != null)
            {
                breadCrumbs = breadCrumbs
                    .Concat(info.PathToCategory
                                .Take(3)
                                .Select(cat => new BreadCrumbModel
                                {
                                    Text = cat.Title,
                                    Url = Url.Action("Index", "Catalog", new { id = cat.Id })
                                }));
            }


            if (sale)
            {
                breadCrumbs = breadCrumbs
                    .Concat(Enumerable.Repeat(new BreadCrumbModel { Text = SALE_CATEGORY_TITLE }, 1));
            }
            else if (novelty)
            {
                breadCrumbs = breadCrumbs
                    .Concat(Enumerable.Repeat(new BreadCrumbModel { Text = NOVELTY_CATEGORY_TITLE }, 1));
            }

            return breadCrumbs;
        }

        private IEnumerable<BreadCrumbModel> GetBankProductsBreadCrumbs()
        {
            var bankProductsBreadCrumbs = Enumerable.Repeat(new BreadCrumbModel { Text = BANK_PORODUCTS_CATEGORY_TITLE }, 1);

            return GetStartBreadCrumbs().Concat(bankProductsBreadCrumbs);
        }

        private static string GetOnlineCategoryPreviewUrl(int categoryId)
        {
            var template = ConfigurationManager.AppSettings["content_online_category_preview_url_template"];

            return string.Format(template, categoryId);
        }

        private static IEnumerable<string> ToKeywords(string phrase, int? maxCountToAnalyze)
        {
            var words = phrase.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                              .Select(w => new string(w.Where(char.IsLetter).ToArray()));

            if (maxCountToAnalyze.HasValue)
            {
                words = words.Take(maxCountToAnalyze.Value);
            }

            return words.Where(w => w.Length > 2 && !StopWords.Contains(w.ToLower()));
        }

        private static string[] GetCategoryKeywords(string categoryName)
        {
            return CategoryKeywords.Concat(Enumerable.Repeat(categoryName, 1))
                                   .Concat(ToKeywords(categoryName, null))
                                   .ToArray();
        }

        private static string[] GetProductKeywords(string productName)
        {
            var keywords = ToKeywords(productName, 5).ToArray();

            return ProductKeywords.Concat(Enumerable.Repeat(string.Join(" ", keywords), 1))
                                  .Concat(keywords)
                                  .ToArray();
        }

        private static string GetBankProductThumbnail(string productId)
        {
            string thumbnail;
            return productId != null && BankProductsThumbnails.TryGetValue(productId, out thumbnail)
                       ? thumbnail
                       : null;
        }

        private static readonly string[] CatalogKeywords = ConfigurationManager.AppSettings["seo_catalog_keywords"].Split(new[] { ',' });
        private static readonly string[] CategoryKeywords = ConfigurationManager.AppSettings["seo_category_keywords"].Split(new[] { ',' });
        private static readonly string[] ProductKeywords = ConfigurationManager.AppSettings["seo_product_keywords"].Split(new[] { ',' });
        private static readonly string[] StopWords = ConfigurationManager.AppSettings["seo_stop_words"].Split(new[] { ',' });

        #endregion
    }
}
