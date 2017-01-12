using Vtb24.Arms.AdminServices;
using Vtb24.Arms.AdminServices.AdminSecurityService.Helpers;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;

namespace Vtb24.Arms.Actions.Models
{
    public class ActionsPermissionsModel
    {
        public bool Edit { get; set; }

        public static ActionsPermissionsModel Map(IAdminSecurityService security)
        {
            return new ActionsPermissionsModel
            {
                Edit = security.CurrentPermissions.IsGranted(PermissionKeys.Actions_Edit)
            };
        }
    }
}
