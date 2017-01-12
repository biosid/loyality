using System.Web.Mvc;
using Vtb24.Arms.AdminServices;
using Vtb24.Arms.Infrastructure;
using Vtb24.Arms.Models;

namespace Vtb24.Arms.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public HomeController(IAdminSecurityService security)
        {
            _security = security;
        }

        private readonly IAdminSecurityService _security;

        [HttpGet]
        public ActionResult Index()
        {
            var model = HomePermissionsModel.Map(_security);

            return View("Index", model);
        }

        [HttpGet]
        public ActionResult AccessDenied()
        {
            return View("AccessDenied");
        }
    }
}
