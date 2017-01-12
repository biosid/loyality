using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Vtb24.Arms.AdminServices;
using Vtb24.Arms.AdminServices.AdminSecurityService.Helpers;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;
using Vtb24.Arms.Infrastructure;
using Vtb24.Arms.Models;

namespace Vtb24.Arms.Security.Controllers
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

            if (permissions.IsGranted(PermissionKeys.Security_Clients))
            {
                yield return new TabModel
                {
                    Title = "Клиенты",
                    Url = Url.Action("Index", "Users"),
                    IsActive = menuId == "users"
                };
            }

            if (permissions.IsGranted(PermissionKeys.Security_CustomFields))
            {
                yield return new TabModel
                {
                    Title = "Дополнительные данные клиентов",
                    Url = Url.Action("Index", "CustomFields"),
                    IsActive = menuId == "custom_fields"
                };
            }

            if (permissions.IsGranted(PermissionKeys.Security_Clients_Feedback))
            {
                yield return new TabModel
                {
                    Title = "Обратная связь с пользователями",
                    Url = Url.Action("Index", "Feedback"),
                    IsActive = menuId == "feedback"
                };
            }
        }
    }
}
