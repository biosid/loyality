using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Vtb24.Arms.AdminServices;
using Vtb24.Arms.AdminServices.AdminSecurityService.Helpers;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;
using Vtb24.Arms.Infrastructure;
using Vtb24.Arms.Models;

namespace Vtb24.Arms.Site.Controllers
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

            if (permissions.IsGranted(PermissionKeys.Site_Page))
                yield return new TabModel
                {
                    Title = "Страницы",
                    Url = Url.Action("Index", "PlainPages"),
                    IsActive = menuId == "pages"
                };

            if (permissions.IsGranted(PermissionKeys.Site_News))
                yield return new TabModel
                {
                    Title = "Новости",
                    Url = Url.Action("Index", "News"),
                    IsActive = menuId == "news"
                };

            if (permissions.IsGranted(PermissionKeys.Site_Page))
                yield return new TabModel
                {
                    Title = "Оферты",
                    Url = Url.Action("Index", "OfferPages"),
                    IsActive = menuId == "offers"
                };

            yield return new TabModel
            {
                Title = "Файлы",
                Url = Url.Action("Index", "Files"),
                IsActive = menuId == "files"
            };
        }
    }
}
