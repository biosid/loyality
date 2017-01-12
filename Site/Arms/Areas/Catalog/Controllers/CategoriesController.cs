using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Vtb24.Arms.AdminServices;
using Vtb24.Arms.AdminServices.AdminSecurityService.Helpers;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;
using Vtb24.Arms.AdminServices.GiftShopManagement.Catalog.Models;
using Vtb24.Arms.AdminServices.GiftShopManagement.Catalog.Models.Inputs;
using Vtb24.Arms.AdminServices.GiftShopManagement.Models.Exceptions;
using Vtb24.Arms.AdminServices.GiftShopManagement.Partners.Models;
using Vtb24.Arms.AdminServices.Models;
using Vtb24.Arms.Catalog.Models.Categories;
using Vtb24.Arms.Catalog.Models.Shared;
using Vtb24.Arms.Catalog.Models.Shared.Helpers;
using Vtb24.Arms.Infrastructure;

namespace Vtb24.Arms.Catalog.Controllers
{
    [Authorize]
    public class CategoriesController : BaseController
    {
        public CategoriesController(IGiftShopManagement catalog, IAdminSecurityService security)
        {
            _catalog = catalog;
            _security = security;
        }

        private readonly IGiftShopManagement _catalog;
        private readonly IAdminSecurityService _security;

        [HttpGet]
        public ActionResult Index()
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Catalog_Login,
                                                          PermissionKeys.Catalog_Categories,
                                                          PermissionKeys.Catalog_Categories_Manage);

            var categories = _catalog.GetCategories(new GetCategoriesFilter(), PagingSettings.ByOffset(0, 500));

            var permissions = CategoriesPermissionsModel.Map(_security);

            var model = new CategoriesModel
            {
                Categories = categories.Select(c => CategoryModel.Map(c, permissions)).ToArray(),
                CategoryItems = CategoryItemModel.Map(categories, null),
                Permissions = permissions
            };

            if (permissions.Manage)
            {
                var onlinePartners = _catalog.GetSuppliersInfo()
                                             .Where(s => s.Type == SupplierType.Online)
                                             .Select(p => new SelectListItem
                                             {
                                                 Text = p.Name,
                                                 Value = p.Id.ToString("d"),
                                                 Selected = false
                                             })
                                             .ToArray();

                model.Create = new CreateCategoryModel
                {
                    OnlinePartners = onlinePartners,
                    RootCategoryItem = model.CategoryItems.GetRootCategoryModel(m => "#" + m.Id)
                };
                model.UpdateOnline = new UpdateOnlineCategoryModel
                {
                    OnlinePartners = onlinePartners
                };
                model.Move = new MoveCategoryModel();
            }

            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            _catalog.DeleteCategory(id);
            return new EmptyResult();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Status(int[] ids, bool enable)
        {
            _catalog.ChangeCategoriesStatus(ids, enable ? CategoryStatus.Enabled : CategoryStatus.Disabled);
            return JsonSuccess();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Rename(int id, [RegularExpression("[^/]", ErrorMessage = "Недопустимый символ \"/\" в имени категории")] string title)
        {
            if (!ModelState.IsValid)
            {
                return JsonErrors();
            }

            var category = _catalog.GetCategory(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            var options = new UpdateCategoryOptions
            {
                // важное
                Id = category.Id,
                Title = title,
                // не важное
                OnlineCategoryUrl = category.OnlineCategoryUrl,
                NotifyOrderStatusUrl = category.NotifyOrderStatusUrl,
                OnlineCategoryPartnerId = category.OnlineCategoryPartnerId,
                Status = category.Status
            };

            try
            {
                _catalog.UpdateCategory(options);
                return JsonSuccess();
            }
            catch (CategoryNameAlreadyExistsException)
            {
                var message = category.ParentId == null
                    ? string.Format("Корневая категория с именем \"{0}\" уже существует", title)
                    : string.Format("Подкатегория с именем \"{0}\" уже существует", title);
                ModelState.AddModelError("Title", message);
                return JsonErrors();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateCategoryModel model)
        {
            if (model.Type == CreateCategoryModel.Types.Static)
            {
                ModelState.Remove("OnlineCategoryUrl");
                ModelState.Remove("NotifyOrderStatusUrl");
                ModelState.Remove("OnlineCategoryPartnerId");
            }

            if (ModelState.IsValid)
            {
                var options = new CreateCategoryOptions
                {
                    OnlineCategoryUrl = model.OnlineCategoryUrl,
                    NotifyOrderStatusUrl = model.NotifyOrderStatusUrl,
                    OnlineCategoryPartnerId = model.OnlineCategoryPartnerId,
                    ParentId = model.ParentId,
                    Title = model.Title,
                    Type = model.Type == CreateCategoryModel.Types.Static ? CategoryType.Static : CategoryType.Online
                };

                try
                {
                    var category = _catalog.CreateCategory(options);

                    var permissions = CategoriesPermissionsModel.Map(_security);

                    var categoryModel = CategoryModel.Map(category, permissions);

                    var html = RenderPartialViewToString("_CategoryRow", categoryModel);
                    return JsonSuccess(html);
                }
                catch(CategoryNameAlreadyExistsException)
                {
                    var message = model.ParentId == null
                        ? string.Format("Корневая категория с именем \"{0}\" уже существует", model.Title) 
                        : string.Format("Подкатегория с именем \"{0}\" уже существует", model.Title);

                    ModelState.AddModelError("Title", message);
                    return JsonErrors();
                }
            }

            return JsonErrors();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateOnline(UpdateOnlineCategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonErrors();
            }

            var category = _catalog.GetCategory(model.Id);

            if (category == null)
            {
                return HttpNotFound();
            }

            var options = new UpdateCategoryOptions
            {
                // важное
                Id = category.Id,
                OnlineCategoryUrl = model.OnlineCategoryUrl,
                NotifyOrderStatusUrl = model.NotifyOrderStatusUrl,
                OnlineCategoryPartnerId = model.OnlineCategoryPartnerId,
                // не важное
                Status = category.Status,
                Title = category.Title
            };
            _catalog.UpdateCategory(options);
            return JsonSuccess();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reorder(OrderCategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonErrors();                
            }

            _catalog.MoveCategory(model.Map());

            return JsonSuccess();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Move(MoveCategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonErrors();
            }

            _catalog.MoveCategory(model.Map());

            var permissions = CategoriesPermissionsModel.Map(_security);

            var branch = _catalog.GetCategories
                (
                    new GetCategoriesFilter
                    {
                        ParentCategoryId = model.Id,
                        IncludeParent = true
                    },
                    PagingSettings.ByOffset(0, 100)
                )
                .Select(c => CategoryModel.Map(c, permissions)).ToArray();

            var branchMarkup = RenderPartialViewToString("_Branch", branch);

            return JsonSuccess(branchMarkup);

        }

    }
}
