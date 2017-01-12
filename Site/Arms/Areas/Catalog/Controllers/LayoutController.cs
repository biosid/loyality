using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Vtb24.Arms.AdminServices;
using Vtb24.Arms.AdminServices.AdminSecurityService.Helpers;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;
using Vtb24.Arms.Infrastructure;
using Vtb24.Arms.Models;

namespace Vtb24.Arms.Catalog.Controllers
{
    public class LayoutController : BaseController
    {
        public LayoutController(IAdminSecurityService security)
        {
            _security = security;
        }

        private readonly IAdminSecurityService _security;

        [ChildActionOnly]
        public ActionResult Tabs(string menuId)
        {
            var model = GetTabs(menuId).ToArray();

            return View("_Tabs", model);
        }

        private IEnumerable<TabModel> GetTabs(string menuId)
        {
            var permissions = _security.CurrentPermissions;

            if (permissions.IsAllGranted(PermissionKeys.Catalog_Categories, PermissionKeys.Catalog_Categories_Manage))
            {
                yield return new TabModel
                {
                    Title = "Редактор категорий",
                    Url = Url.Action("Index", "Categories"),
                    IsActive = menuId == "categories"
                };
            }

            if (permissions.IsGranted(PermissionKeys.Catalog_PartnerCategories))
            {
                yield return new TabModel
                {
                    Title = "Привязка категорий",
                    Url = Url.Action("Index", "PartnerCategories"),
                    IsActive = menuId == "partner_categories"
                };
            }

            if (permissions.IsGranted(PermissionKeys.Catalog_Gifts))
            {
                yield return new TabModel
                {
                    Title = "Вознаграждения",
                    Url = Url.Action("Index", "Gifts"),
                    IsActive = menuId == "gifts"
                };
            }

            if (permissions.IsGranted(PermissionKeys.Catalog_Orders))
            {
                yield return new TabModel
                {
                    Title = "Заказы",
                    Url = Url.Action("Index", "Orders"),
                    IsActive = menuId == "orders"
                };
            }

            if (permissions.IsGranted(PermissionKeys.Catalog_Partners))
            {
                yield return new TabModel
                {
                    Title = "Поставщики",
                    Url = Url.Action("Index", "Suppliers"),
                    IsActive = menuId == "suppliers"
                };
                yield return new TabModel
                {
                    Title = "Курьеры",
                    Url = Url.Action("Index", "Carriers"),
                    IsActive = menuId == "carriers"
                };
            }

            if (permissions.IsGranted(PermissionKeys.Catalog_Partners_DeliveryMatrix))
            {
                yield return new TabModel
                {
                    Title = "Доставка",
                    Url = Url.Action("Index", "Delivery"),
                    IsActive = menuId == "delivery"
                };
            }
        }
    }
}
