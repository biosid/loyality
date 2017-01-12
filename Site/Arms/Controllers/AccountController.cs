using System.Web.Mvc;
using System.Web.Security;
using Vtb24.Arms.AdminServices;
using Vtb24.Arms.Infrastructure;
using Vtb24.Arms.Models;

namespace Vtb24.Arms.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IAdminSecurityService security)
        {
            _security = security;
        }

        private readonly IAdminSecurityService _security;

        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToLocal(returnUrl);
            }

            var model = new LoginModel
            {
                ReturnUrl = returnUrl
            };
            return View("Login", model);
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (_security.Login(model.Login, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.Login, true);

                    return RedirectToLocal(model.ReturnUrl);
                }

                ModelState.AddModelError("", "Логин или пароль неверен, попробуйте ввести ещё раз");
            }

            return View("Login", model);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login");
        }
    }
}
