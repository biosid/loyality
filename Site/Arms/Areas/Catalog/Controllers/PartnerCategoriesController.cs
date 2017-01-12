using System;
using System.Collections.Generic;
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
using Vtb24.Arms.Infrastructure;
using Vtb24.Arms.Catalog.Models.PartnerCategories;

namespace Vtb24.Arms.Catalog.Controllers
{
    [Authorize]
    public class PartnerCategoriesController : BaseController
    {
        public PartnerCategoriesController(IGiftShopManagement catalog, IAdminSecurityService security)
        {
            _catalog = catalog;
            _security = security;
        }

        private readonly IGiftShopManagement _catalog;
        private readonly IAdminSecurityService _security;

        [HttpGet]
        public ActionResult Index(int? supplierId)
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Catalog_Login,
                                                          PermissionKeys.Catalog_PartnerCategories);

            var permissions = SupplierCategoriesPermissionsModel.Map(_security);

            var suppliers = _catalog
                .GetUserSuppliersInfo()
                .Where(s => s.Type != SupplierType.Online)
                .OrderBy(p => p.Name)
                .ToArray();

            var model = new SupplierCategoriesModel
            {
                Suppliers = suppliers.Select(SupplierModel.Map).ToArray(),
                Permissions = permissions
            };

            if (suppliers.Length == 0)
            {
                return View("Index", model);
            }

            var supplier = supplierId.HasValue
                              ? suppliers.FirstOrDefault(p => p.Id == supplierId.Value) ?? suppliers.First()
                              : suppliers.First();
            model.SelectedSupplierId = supplier.Id;

            var categories = _catalog.GetCategories(new GetCategoriesFilter(), PagingSettings.ByOffset(0, 500));
            model.Categories = categories.Select(SupplierCategoryModel.Map).ToArray();

            var categoriesBindings = _catalog.GetCategoryBindings(supplier.Id, categories.Select(c => c.Id).ToArray());
            var categoriesAllowed = _catalog.GetCategoriesPermissions(supplier.Id);

            foreach (var category in model.Categories)
            {
                category.SetBindings(categoriesBindings.SingleOrDefault(cb => cb.ProductCategoryId == category.Id));
                category.SupplierHasAccess = categoriesAllowed.Contains(category.Id);
                category.Permissions = permissions;
            }

            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePermissions(int supplierId, int[] categoriesIds)
        {
            var categories = _catalog.GetCategories(new GetCategoriesFilter(), PagingSettings.ByOffset(0, 500));
            var allCategoriesIds = categories.Select(c => c.Id).ToArray();

            var categoriesPermissions = _catalog.GetCategoriesPermissions(supplierId);

            var permissionsToAdd = new List<int>();
            var permissionsToRemove = new List<int>();
            foreach (var id in allCategoriesIds)
            {
                if (categoriesPermissions.Contains(id) && 
                    (categoriesIds == null || !categoriesIds.Contains(id)))
                {
                    permissionsToRemove.Add(id);
                }

                if (!categoriesPermissions.Contains(id) &&
                    categoriesIds != null && categoriesIds.Contains(id))
                {
                    permissionsToAdd.Add(id);
                }
            }

            _catalog.SetCategoriesPermissions(supplierId, permissionsToAdd.ToArray(), permissionsToRemove.ToArray());

            return JsonSuccess();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetSupplierCategories(int supplierId, int productCategoryId, string[] supplierCategories)
        {
            var trimChars = "/\\".ToCharArray();
            supplierCategories = supplierCategories != null
                                     ? supplierCategories.Select(c => c.Trim(trimChars)).ToArray()
                                     : new string[0];

            try
            {
                _catalog.SetCategoryBinding(new CategoryBinding
                {
                    SupplierId = supplierId,
                    ProductCategoryId = productCategoryId,
                    CategoryPaths = supplierCategories.Select(pc =>
                                                              new CategoryPath
                                                              {
                                                                  IncludeSubcategories = true,
                                                                  NamePath = pc
                                                              })
                                                      .ToArray()
                });
            }
            catch (EntityAlreadyExistsException)
            {
                ModelState.AddModelError("", "привязка к данной категории уже существует");
                return JsonErrors("данная категория партнера уже привязана");
            }

            try
            {
                var bindings = _catalog.GetCategoryBindings(supplierId, new[] { productCategoryId })
                                       .First(b => b.ProductCategoryId == productCategoryId)
                                       .CategoryPaths
                                       .Select(p => p.NamePath)
                                       .ToArray();

                return JsonSuccess(new { bindings });
            }
            catch (Exception)
            {
                return JsonSuccess(new { bindings = supplierCategories });
            }
        }
    }
}
