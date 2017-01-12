using Vtb24.Arms.AdminServices;
using Vtb24.Arms.AdminServices.AdminSecurityService.Helpers;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;

namespace Vtb24.Arms.Catalog.Models.Partners
{
    public class PartnersPermissionsModel
    {
        public bool Edit { get; set; }

        public static PartnersPermissionsModel Map(IAdminSecurityService security)
        {
            return new PartnersPermissionsModel
            {
                Edit = security.CurrentPermissions.IsGranted(PermissionKeys.Catalog_Partners_Edit)
            };
        }
    }
}
