using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Vtb24.Arms.AdminServices;
using Vtb24.Arms.AdminServices.AdminSecurityService.Helpers;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;
using Vtb24.Arms.Infrastructure;
using Vtb24.Arms.Models;

namespace Vtb24.Arms.AdminSecurity.Controllers
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
            if (!_security.CurrentPermissions.IsGranted(PermissionKeys.AdminSecurity_All))
                yield break;

            yield return new TabModel
            {
                Title = "Группы",
                Url = Url.Action("Index", "Groups"),
                IsActive = menuId == "groups"
            };
            yield return new TabModel
            {
                Title = "Пользователи",
                Url = Url.Action("Index", "Users"),
                IsActive = menuId == "users"
            };
        }
    }
}
