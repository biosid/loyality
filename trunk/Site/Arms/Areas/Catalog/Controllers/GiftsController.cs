using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;
using Vtb24.Arms.AdminServices;
using Vtb24.Arms.AdminServices.AdminSecurityService.Helpers;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;
using Vtb24.Arms.AdminServices.GiftShopManagement.Catalog.Models.Inputs;
using Vtb24.Arms.AdminServices.GiftShopManagement.Models.Exceptions;
using Vtb24.Arms.AdminServices.GiftShopManagement.Partners.Models;
using Vtb24.Arms.AdminServices.GiftShopManagement.Products.Models;
using Vtb24.Arms.AdminServices.GiftShopManagement.Products.Models.Inputs;
using Vtb24.Arms.AdminServices.Infrastructure;
using Vtb24.Arms.AdminServices.Models;
using Vtb24.Arms.Helpers;
using Vtb24.Arms.Infrastructure;
using Vtb24.Arms.Catalog.Models.Gifts;
using Vtb24.Arms.Catalog.Models.Gifts.Helpers;
using Vtb24.Arms.Catalog.Models.PartnerCategories;
using Vtb24.Arms.Catalog.Models.Shared;
using HtmlSanitizer = Vtb24.Site.Services.Infrastructure.HtmlSanitizer;
using HtmlSanitizerPresets = Vtb24.Site.Services.Infrastructure.HtmlSanitizerPresets;
using HtmlSanitizerPresetsExtensions = Vtb24.Site.Services.Infrastructure.HtmlSanitizerPresetsExtensions;

namespace Vtb24.Arms.Catalog.Controllers
{
    [Authorize]
    public class GiftsController : BaseController
    {
        private const int PAGE_SIZE = 50;
        private const int IMPORT_HISTORY_PAGE_SIZE = 30;

        public GiftsController(IGiftShopManagement catalog, ITargetingManagement targeting, IAdminSecurityService security)
        {
            _catalog = catalog;
            _targeting = targeting;
            _security = security;
        }

        private readonly IGiftShopManagement _catalog;
        private readonly ITargetingManagement _targeting;
        private readonly IAdminSecurityService _security;
        private readonly PagingSettings _categoriesPaging = PagingSettings.ByOffset(0, 1000);

        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("List", "Gifts", new GiftsQueryModel());
        }

        [HttpGet]
        public ActionResult List(GiftsQueryModel query)
        {
            var suppliers = _catalog.GetUserSuppliersInfo()
                                    .Where(s => s.Type != SupplierType.Online)
                                    .OrderBy(s => s.Name)
                                    .ToArray();

            SupplierInfo[] selectedSuppliers;
            if (query.supplier.HasValue)
            {
                var supplier = suppliers.FirstOrDefault(s => s.Id == query.supplier);

                // если поставщик не существует - 404
                if (supplier == null)
                    return HttpNotFound();

                selectedSuppliers = new[] { supplier };
            }
            else
                selectedSuppliers = suppliers;

            var categories = _catalog.GetCategories(new GetCategoriesFilter(), _categoriesPaging);
            var categoriesPermissions = new HashSet<int>(selectedSuppliers.SelectMany(p => _catalog.GetCategoriesPermissions(p.Id)));

            var categoryItemModels = CategoryItemModel.Map(categories, categoriesPermissions);
            var categoryItemModel = categoryItemModels.FirstOrDefault(cm => cm.Id == query.category);

            var disallowCreate = false;
            string disallowCreateMessage = null;
            if (!query.supplier.HasValue)
            {
                disallowCreate = true;
                disallowCreateMessage = "Выберите поставщика, чтобы добавить вознаграждение";
            }
            else if (categoryItemModel == null)
            {
                disallowCreate = true;
                disallowCreateMessage = "Выберите категорию, чтобы добавить вознаграждение";
            }
            else if (!categoryItemModel.IsPermissinsGranted)
            {
                disallowCreate = true;
                disallowCreateMessage = "Недостаточно полномочий для добавления вознаграждения в данную категорию";
            }

            var criteria = new ProductsSearchCriteria
            {
                SearchTerm = query.term,
                CategoryIds = query.category.HasValue ? new[] { query.category.Value } : null,
                IncludeSubCategories = true,
                SupplierIds = selectedSuppliers.Select(p => p.Id).ToArray(),
                ModerationStatus = query.moderation.Map(),
                IsRecommended = query.recommended_only ? true : (bool?) null,
                Sorting = query.sort.Map()
            };

            var paging = PagingSettings.ByPage(query.page ?? 1, PAGE_SIZE);
            var products = _catalog.SearchProducts(criteria, paging);

            var permissions = GiftsPermissionsModel.Map(_security);

            var model = new GiftsModel
            {
                Suppliers = suppliers.Select(SupplierModel.Map).ToArray(),
                Query = query,
                CategoryItems = categoryItemModels,
                BreadCrumbs = new BreadCrumbsModel
                {
                    BreadCrumbs = GetBreadCrumbs(categoryItemModel).Reverse().ToArray(),
                    Query = query
                },
                Gifts = products.Select(GiftModel.Map).ToArray(),
                DisallowCreate = disallowCreate,
                DisallowCreateMessage = disallowCreateMessage,
                TotalPages = products.TotalPages,
                Segments = _targeting.GetSegments()
                                     .Select(SegmentModel.Map)
                                     .ToArray(),
                Permissions = permissions
            };

            return View("List", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Moderate(ModerateModel model)
        {
            var options = new ModerateProductsOptions
            {
                ProductIds = model.Ids,
                ModerationStatus = model.MapStatus()
            };
            _catalog.ModerateProducts(options);

            return JsonSuccess();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string[] ids)
        {
            _catalog.DeleteProducts(ids);

            return JsonSuccess();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Move(MoveModel model)
        {
            var options = new MoveProductsOptions
            {
                CategoryId = model.CategoryId,
                ProductIds = model.Ids
            };
            _catalog.MoveProducts(options);

            return JsonSuccess();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Activate(ActivateModel model)
        {
            var options = new ActivateProductsOptions
            {
                ProductIds = model.Ids,
                Status = model.MapStatus()
            };
            _catalog.ActivateProducts(options);

            return JsonSuccess();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetSegments(SetSegmentsModel model)
        {
            var options = new SetProductsSegmentsOptions
            {
                ProductIds = model.Ids,
                Segments = model.Segments ?? new string[0]
            };
            _catalog.SetProductsSegments(options);

            return JsonSuccess();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Recommend(RecommendModel model)
        {
            var options = new RecommendProductsOptions
            {
                ProductIds = model.Ids,
                IsRecommended = model.IsRecommended
            };
            _catalog.RecommendProducts(options);

            return JsonSuccess();
        }

        [HttpGet]
        public ActionResult Import(int? supplierId, string query, int page = 1)
        {
            var suppliers = _catalog.GetUserSuppliersInfo();

            var supplier = supplierId.HasValue
                              ? suppliers.FirstOrDefault(p => p.Id == supplierId.Value) ?? suppliers.First()
                              : suppliers.First();

            var history = _catalog.GetProductsImportsHistory(supplier.Id, PagingSettings.ByPage(page, IMPORT_HISTORY_PAGE_SIZE));

            var model = new GiftsImportModel
            {
                SupplierId = supplier.Id,
                SupplierName = supplier.Name,
                HistoryRows = history.Select(GiftsImportHistoryRowModel.Map)
                                     .ToArray(),
                TotalPages = history.TotalPages,
                query = query,
                page = page
            };

            return View("Import/Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Import(int supplierId, HttpPostedFileBase file, string query)
        {
            file.ImportGifts(_catalog, supplierId);
            return RedirectToAction("Import", "Gifts", new { supplierId, query });
        }

        [HttpGet]
        public ActionResult Create(int categoryId, int supplierId, string query)
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Catalog_Login,
                                                          PermissionKeys.Catalog_Gifts,
                                                          PermissionKeys.Catalog_Gifts_Edit);

            var supplier = _catalog.GetUserSupplierInfoById(supplierId);

            if (supplier == null)
                throw new InvalidOperationException(string.Format("Поставщик не найден: {0}", supplierId));

            var permissions = _catalog.GetCategoriesPermissions(supplierId);
            if (!permissions.Contains(categoryId))
                throw new SecurityException(string.Format("Категория не найдена: {0}", categoryId));

            var category = _catalog.GetCategory(categoryId);
            if (category == null)
                throw new InvalidOperationException(string.Format("Категория не найдена: {0}", categoryId));

            var model = new GiftEditModel
            {
                SupplierName = supplier.Name,
                SupplierId = supplier.Id,
                IsNewProduct = true,
                CategoryId = categoryId,
                CategoryPath = category.CategoryPath,
                query = query
            };

            return View("Edit/Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GiftEditModel model)
        {
            AssertUploadedImagesSize(model.Pictures);

            PrevalidateModel(model);

            if (ModelState.IsValid)
            {
                try
                {
                    var product = new Product
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Parameters = MaybeLinqExtensions.MaybeToArray(model
                                .Parameters
                                .MaybeWhere(p => !string.IsNullOrEmpty(p.Value))
                                .MaybeSelect(p => new Product.Parameter
                                {
                                    Name = p.Name,
                                    Unit = p.Unit,
                                    Value = p.Value
                                })),
                        Pictures = model
                            .PictureUrls.Where(p => !string.IsNullOrWhiteSpace(p))
                            .ToArray(),
                        PriceRUR = model.PriceRUR.HasValue ? model.PriceRUR.Value : 0,
                        BasePriceRUR = model.BasePriceRUR,
                        Vendor = model.Vendor,
                        Weight = model.Weight.HasValue ? model.Weight.Value : 0,
						IsDeliveredByEmail = model.IsDeliveredByEmail,
                        SupplierId = model.SupplierId,
                        CategoryId = model.CategoryId,
                        SupplierProductId = model.SupplierProductId
                    };
                    var id = _catalog.CreateProduct(product);

                    // досохраняем картинки
                    if (model.Pictures.Any(f => f != null))
                    {
                        var picturesFilesUrls = model.Pictures
                            .Where(f => f != null)
                            .Select(f => f.SaveGiftImage(id)).ToArray();

                        product.Id = id;
                        product.Pictures = product.Pictures.Concat(picturesFilesUrls).ToArray();
                        _catalog.UpdateProduct(product);
                    }

                    return RedirectToAction("List", "Gifts", model.GiftsQueryModel);
                }
                catch (EntityAlreadyExistsException)
                {
                    ModelState.AddModelError("SupplierProductId", "Вознаграждение с таким артикулом уже существует. Укажите уникальный артикул.");
                }
            }

            model.PictureUrls = MaybeLinqExtensions.MaybeToArray(model
                                    .PictureUrls
                                    .MaybeWhere(p => !string.IsNullOrEmpty(p)));

            return View("Edit/Index", model);
        }

        [HttpGet]
        public ActionResult Edit(string id, string query)
        {
            var product = GetProductById(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            var supplier = _catalog.GetUserSupplierInfoById(product.SupplierId);
            if (supplier == null)
                throw new InvalidOperationException(string.Format("Поставщик не найден: {0}", product.SupplierId));

            var model = new GiftEditModel
            {
                Id = product.Id,
                CategoryId = product.CategoryId,
                CategoryPath = product.CategoryPath,
                Description = SanitizeProductDescription(product.Description),
                Name = product.Name,
                SupplierId = product.SupplierId,
                SupplierName = supplier.Name,
                SupplierProductId = product.SupplierProductId,
                PriceRUR = product.PriceRUR,
                BasePriceRUR = product.BasePriceRurDate.HasValue ? null : product.BasePriceRUR,
                Vendor = product.Vendor,
                Weight = product.Weight,
				IsDeliveredByEmail = product.IsDeliveredByEmail,
                PictureUrls = product.Pictures,
                Parameters = MaybeLinqExtensions.MaybeToArray(product
                        .Parameters
                        .MaybeSelect(p => new GiftEditModel.Parameter
                        {
                            Name = p.Name,
                            Unit = p.Unit,
                            Value = p.Value
                        })),
                Status = product.Status.Map(),
                Segments = product.Segments != null && product.Segments.Length > 0
                               ? string.Join(", ", product.Segments)
                               : string.Empty,
                ModerationStatus = ProductModerationStatusMapper.Map(product.ModerationStatus),
                IsRecommended = product.IsRecommended,
                query = query,
                IsNewProduct = false
            };

            return View("Edit/Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GiftEditModel model)
        {
            AssertUploadedImagesSize(model.Pictures);

            PrevalidateModel(model);

            if (ModelState.IsValid)
            {
                var picturesFilesUrls = model.Pictures
                                             .Where(f => f != null)
                                             .Select(f => f.SaveGiftImage(model.Id))
                                             .ToArray();
                
                var pictures = model
                    .PictureUrls.Where(p => !string.IsNullOrWhiteSpace(p))
                    .Concat(picturesFilesUrls)
                    .ToArray();

                var product = GetProductById(model.Id);

                if (product == null)
                {
                    return HttpNotFound();
                }

                var deletedPicturesUrls = product.Pictures.Except(pictures);

                ClearDeletedPictures(product.Id, deletedPicturesUrls);

                _catalog.UpdateProduct(new Product
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = SanitizeProductDescription(model.Description),
                    Parameters = MaybeLinqExtensions.MaybeToArray(model
                            .Parameters
                            .MaybeWhere(p => !string.IsNullOrEmpty(p.Value))
                            .MaybeSelect(p => new Product.Parameter
                            {
                                Name = p.Name,
                                Unit = p.Unit,
                                Value = p.Value
                            })),
                    Pictures = pictures,
                    PriceRUR = model.PriceRUR.HasValue ? model.PriceRUR.Value : 0,
                    BasePriceRUR = model.BasePriceRUR.HasValue ? model.BasePriceRUR.Value : (decimal?)null,
                    Vendor = model.Vendor,
                    Weight = model.Weight.HasValue ? model.Weight.Value : 0,
					IsDeliveredByEmail = model.IsDeliveredByEmail,
                    SupplierId = model.SupplierId,
                    CategoryId = model.CategoryId
                });

                return RedirectToAction("List", "Gifts", model.GiftsQueryModel);
            }

            model.PictureUrls = MaybeLinqExtensions.MaybeToArray(model
                                    .PictureUrls
                                    .MaybeWhere(p => !string.IsNullOrEmpty(p)));

            model.Pictures = MaybeLinqExtensions.MaybeToArray(model.Pictures.MaybeWhere(p => p != null));

            return View("Edit/Index", model);
        }

        private void ClearDeletedPictures(string productId, IEnumerable<string> deletedPicturesUrls)
        {
            foreach (var deletedPicture in deletedPicturesUrls)
            {
                PartnerHelpers.DeleteGiftImage(productId, deletedPicture);
            }
        }

        #region Приватные методы

        private void PrevalidateModel(GiftEditModel model)
        {
            // убираем ошибки валидации веса, если проставлена доставка по e-mail
            if (model.IsDeliveredByEmail)
            {
                ModelState.RemoveErrors("Weight");
            }
        }

        private Product GetProductById(string id)
        {
            var searchResult = _catalog.SearchProducts(new ProductsSearchCriteria
            {
                IncludeSubCategories = true,
                ProductIds = new[] { id }
            }, PagingSettings.ByOffset(0, 1));

            return searchResult.SingleOrDefault();
        }

        private string SanitizeProductDescription(string description)
        {
            var whitelist = HtmlSanitizerPresetsExtensions.GetWhitelist(HtmlSanitizerPresets.ProductInfo);
            return HtmlSanitizer.SanitizeHtml(description, whitelist);
        }

        private static IEnumerable<CategoryItemModel> GetBreadCrumbs(CategoryItemModel categoryItemModel)
        {
            while (categoryItemModel != null && categoryItemModel.Parent != null)
            {
                yield return categoryItemModel;
                categoryItemModel = categoryItemModel.Parent;
            }
        }

        private void AssertUploadedImagesSize(params HttpPostedFileBase[] uploads)
        {
            var maxPictureSizeMb = Int32.Parse(ConfigurationManager.AppSettings["content_files_max_upload_image_size_mb"]);
            var bigPictures = uploads.Where(p => p != null && p.ContentLength >= maxPictureSizeMb * 1024 * 1024).ToArray();
            foreach (var bigPicture in bigPictures)
            {
                ModelState.AddModelError(
                    "",
                    string.Format("Изображением {0} превышен максимальный допустимый размер для загрузки {1} МБ",
                        bigPicture.FileName, maxPictureSizeMb)
                    );
            }
        }

        #endregion
    }
}
