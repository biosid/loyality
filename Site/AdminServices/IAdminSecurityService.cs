using Vtb24.Arms.AdminServices.AdminSecurityService;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models.Output;
using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices
{
    public interface IAdminSecurityService
    {
        bool IsAuthenticated { get; }

        string CurrentUser { get; }

        IPermissionsSource CurrentPermissions { get; }

        /// <summary>
        /// Проверяет существование учётных данных
        /// </summary>
        bool Login(string login, string password);

        /// <summary>
        /// Проверяет, существует ли пользователь с логином CurrentUser.
        /// Если пользователь не аутентифицирован, возращает true.
        /// </summary>
        bool VerifyCurrentUser();

        PermissionNode[] PermissionNodes { get; }

        string[] GetGroupNames();

        string[] GetUserNames();

        AdminGroup GetGroupByName(string name);

        AdminUser GetUserByLogin(string login);

        IPermissionsSource GetPermissionsByLogin(string login);

        IPermissionsSource[] GetInheritedPermissionsByLogin(string login);

        GetGroupsResult GetGroups(PagingSettings paging);

        GetUsersResult GetUsers(PagingSettings paging);

        void CreateGroup(AdminGroup group);

        void UpdateGroup(AdminGroup group);

        void DeleteGroup(string name);

        void CreateUser(AdminUser user, string password);

        void UpdateUser(AdminUser user);

        void DeleteUser(string login);

        void ResetUserPassword(string name, string password);
    }
}