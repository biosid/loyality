using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vtb24.Arms.AdminSecurity.Helpers;
using Vtb24.Arms.AdminSecurity.Models.Groups;
using Vtb24.Arms.AdminSecurity.Models.Shared;
using Vtb24.Arms.AdminServices;
using Vtb24.Arms.AdminServices.AdminSecurityService.Exceptions;
using Vtb24.Arms.AdminServices.AdminSecurityService.Helpers;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;
using Vtb24.Arms.AdminServices.Models;
using Vtb24.Arms.Infrastructure;

namespace Vtb24.Arms.AdminSecurity.Controllers
{
    public class GroupsController : BaseController
    {
        private const int PAGE_SIZE = 30;

        public GroupsController(IAdminSecurityService security, IGiftShopManagement catalog)
        {
            _security = security;
            _catalog = catalog;
        }

        private readonly IAdminSecurityService _security;
        private readonly IGiftShopManagement _catalog;

        [HttpGet]
        public ActionResult Index(GroupsQueryModel query)
        {
            var paging = PagingSettings.ByPage(query.page ?? 1, PAGE_SIZE);

            var groups = _security.GetGroups(paging);

            var model = new GroupsModel
            {
                Groups = groups.Select(GroupModel.Map).ToArray(),
                Query = query,
                TotalPages = groups.TotalPages
            };

            // если у текущего пользователя право на редактирование учётных записей унаследовано от единственной группы,
            // то необходимо запретить удаленние данной группы
            if (!_security.GetPermissionsByLogin(_security.CurrentUser).IsGranted(PermissionKeys.AdminSecurity_All))
            {
                var adminSecurityInheritedFrom = _security.GetInheritedPermissionsByLogin(_security.CurrentUser)
                                                          .WhereGranted(PermissionKeys.AdminSecurity_All)
                                                          .ToArray();
                if (adminSecurityInheritedFrom.Length == 1)
                {
                    var groupModel = model.Groups.FirstOrDefault(g => g.Name == adminSecurityInheritedFrom[0].Name);
                    if (groupModel != null)
                    {
                        groupModel.IsDeleteDenied = true;
                        groupModel.DenyDeleteReason = "Нельзя удалить группу, так как собственная учётная запись потерят право на редактирование учётных записей.";
                    }
                }
            }

            return View("Index", model);
        }

        [HttpGet]
        public ActionResult Create(string query)
        {
            var model = GroupEditModel.Create(query, _security.PermissionNodes);

            Hydrate(model);

            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GroupEditModel model)
        {
            model.Permissions = PermissionsEditModel.Map(Request.Form, _security.PermissionNodes, null);

            if (!ModelState.IsValid)
            {
                Hydrate(model);
                return View("Edit", model);
            }

            try
            {
                _security.CreateGroup(new AdminGroup
                {
                    Name = model.Name,
                    Users = model.Users ?? new string[0],
                    Permissions = model.Permissions.Permissions
                });
            }
            catch (AdminSecurityGroupExistsException e)
            {
                ModelState.AddModelError("Name", "Группа с именем \"" + e.Name + "\" уже существует");
                Hydrate(model);
                return View("Edit", model);
            }

            return RedirectToAction("Index", "Groups", model.GroupsQueryModel);
        }

        [HttpGet]
        public ActionResult Edit(string name, string query)
        {
            var group = _security.GetGroupByName(name);
            if (group == null)
                throw new HttpException(404, "Группа с именем \"" + name + "\" не найдена");

            var model = GroupEditModel.Map(group, query, _security.PermissionNodes);

            Hydrate(model);

            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GroupEditModel model)
        {
            model.Permissions = PermissionsEditModel.Map(Request.Form, _security.PermissionNodes, null);

            if (!ModelState.IsValid)
            {
                Hydrate(model);
                return View("Edit", model);
            }

            try
            {
                _security.UpdateGroup(new AdminGroup
                {
                    Name = model.Name,
                    Users = model.Users ?? new string[0],
                    Permissions = model.Permissions.Permissions
                });
            }
            catch (AdminSecurityGroupNotFoundException e)
            {
                throw new HttpException(404, "Группа с именем \"" + e.Name + "\" не найдена");
            }
            catch (AdminSecuritySelfEditException)
            {
                ModelState.AddModelError("", "Невозможно сохранить изменения, так как собственная учётная запись потерят право на редактирование учётных записей.");
                Hydrate(model);
                return View("Edit", model);
            }

            return RedirectToAction("Index", "Groups", model.GroupsQueryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string name)
        {
            _security.DeleteGroup(name);

            return JsonSuccess();
        }

        private void Hydrate(GroupEditModel model)
        {
            model.AllUsers = _security.GetUserNames();
            model.Permissions.Hydrate(_catalog);
        }
    }
}
