using Vtb24.Arms.AdminServices;
using Vtb24.Arms.AdminServices.AdminSecurityService.Helpers;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;

namespace Vtb24.Arms.Security.Models.Users
{
    public class UserEditPermissionsModel
    {
        public bool Deactivate { get; set; }

        public bool PasswordReset { get; set; }

        public bool SiteAccess { get; set; }

        public bool ChangePhone { get; set; }

        public static UserEditPermissionsModel Map(IAdminSecurityService security)
        {
            var permissions = security.CurrentPermissions;

            return new UserEditPermissionsModel
            {
                Deactivate = permissions.IsGranted(PermissionKeys.Security_Clients_Deactivate),
                PasswordReset = permissions.IsGranted(PermissionKeys.Security_Clients_ResetPassword),
                SiteAccess = permissions.IsGranted(PermissionKeys.Security_Clients_SiteAccess),
                ChangePhone = permissions.IsGranted(PermissionKeys.Security_Clients_ChangePhone)
            };
        }
    }
}
