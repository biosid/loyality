using Vtb24.Arms.AdminServices;
using Vtb24.Arms.AdminServices.AdminSecurityService.Helpers;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;

namespace Vtb24.Arms.Catalog.Models.Categories
{
    public class CategoriesPermissionsModel
    {
        public bool Manage { get; set; }

        public static CategoriesPermissionsModel Map(IAdminSecurityService security)
        {
            return new CategoriesPermissionsModel
            {
                Manage = security.CurrentPermissions.IsGranted(PermissionKeys.Catalog_Categories_Manage)
            };
        }
    }
}
