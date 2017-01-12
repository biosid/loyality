using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vtb24.Arms.AdminSecurity.Helpers;
using Vtb24.Arms.AdminSecurity.Models.Shared;
using Vtb24.Arms.AdminSecurity.Models.Users;
using Vtb24.Arms.AdminServices;
using Vtb24.Arms.AdminServices.AdminSecurityService.Exceptions;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;
using Vtb24.Arms.AdminServices.Models;
using Vtb24.Arms.Infrastructure;

namespace Vtb24.Arms.AdminSecurity.Controllers
{
    public class UsersController : BaseController
    {
        private const int PAGE_SIZE = 30;

        public UsersController(IAdminSecurityService security, IGiftShopManagement catalog)
        {
            _security = security;
            _catalog = catalog;
        }

        private readonly IAdminSecurityService _security;
        private readonly IGiftShopManagement _catalog;

        [HttpGet]
        public ActionResult Index(UsersQueryModel query)
        {
            var paging = PagingSettings.ByPage(query.page ?? 1, PAGE_SIZE);

            var users = _security.GetUsers(paging);

            var model = new UsersModel
            {
                Users = users.Select(UserModel.Map).ToArray(),
                Query = query,
                TotalPages = users.TotalPages
            };

            var selfModel = model.Users.FirstOrDefault(u => u.Login == _security.CurrentUser);
            if (selfModel != null)
            {
                selfModel.IsDeleteDenied = true;
                selfModel.DenyDeleteReason = "Нельзя удалить собственную учётную запись";
            }

            return View("Index", model);
        }


        [HttpGet]
        public ActionResult Create(string query)
        {
            var model = UserEditModel.Create(query, _security.PermissionNodes);

            Hydrate(model);

            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserEditModel model)
        {
            model.Permissions = PermissionsEditModel.Map(Request.Form, _security.PermissionNodes, null);

            if (!ModelState.IsValid)
            {
                Hydrate(model);
                return View("Edit", model);
            }

            try
            {
                _security.CreateUser(new AdminUser
                {
                    Login = model.Login,
                    Groups = model.Groups ?? new string[0],
                    Permissions = model.Permissions.Permissions
                }, model.Password);
            }
            catch (AdminSecurityUserExistsException e)
            {
                ModelState.AddModelError("Login", "Пользователь c логином \"" + e.Login + "\" уже существует");
                Hydrate(model);
                return View("Edit", model);
            }
            catch (AdminSecuritySetPasswordException)
            {
                ModelState.AddModelError("Password", "Пароль не должен содержать логин");
                Hydrate(model);
                return View("Edit", model);
            }

            return RedirectToAction("Index", "Users", model.UsersQueryModel);
        }

        [HttpGet]
        public ActionResult Edit(string login, string query)
        {
            var user = _security.GetUserByLogin(login);
            if (user == null)
                throw new HttpException(404, "Пользователь c логином \"" + login + "\" не найден");

            var model = UserEditModel.Map(user, query, _security.PermissionNodes);

            Hydrate(model);

            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserEditModel model)
        {
            var inheritedPermissions = _security.GetInheritedPermissionsByLogin(model.Login);

            model.Permissions = PermissionsEditModel.Map(Request.Form, _security.PermissionNodes, inheritedPermissions);

            ModelState.Remove("Password");

            if (!ModelState.IsValid)
            {
                Hydrate(model);
                return View("Edit", model);
            }

            try
            {
                _security.UpdateUser(new AdminUser
                {
                    Login = model.Login,
                    Groups = model.Groups ?? new string[0],
                    Permissions = model.Permissions.Permissions
                });
            }
            catch (AdminSecurityUserNotFoundException e)
            {
                throw new HttpException(404, "Пользователь c логином \"" + e.Login + "\" не найден");
            }
            catch (AdminSecuritySelfEditException)
            {
                ModelState.AddModelError("", "При редактировании собственной учётной записи нельзя удалить право на редактирование учётных записей.");
                Hydrate(model);
                return View("Edit", model);
            }

            return RedirectToAction("Index", "Users", model.UsersQueryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetUserPasswordModel model)
        {
            try
            {
                _security.ResetUserPassword(model.Name, model.Password);
            }
            catch (AdminSecuritySetPasswordException)
            {
                ModelState.AddModelError("Password", "Пароль не должен содержать логин");
            }

            return ModelState.IsValid
                       ? JsonSuccess()
                       : JsonErrors();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string login)
        {
            _security.DeleteUser(login);

            return JsonSuccess();
        }

        private void Hydrate(UserEditModel model)
        {
            model.AllGroups = _security.GetGroupNames();
            model.Permissions.Hydrate(_catalog);
            model.ResetPassword = new ResetUserPasswordModel
            {
                Name = model.Login
            };
        }
    }
}
