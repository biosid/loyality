using Vtb24.Arms.AdminServices;
using Vtb24.Arms.AdminServices.AdminSecurityService.Helpers;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;

namespace Vtb24.Arms.Catalog.Models.PartnerCategories
{
    public class SupplierCategoriesPermissionsModel
    {
        public bool Access { get; set; }

        public bool Binding { get; set; }

        public static SupplierCategoriesPermissionsModel Map(IAdminSecurityService security)
        {
            var allow = security.CurrentPermissions.IsGranted(PermissionKeys.Catalog_PartnerCategories);

            return new SupplierCategoriesPermissionsModel
            {
                Access = allow,
                Binding = allow
            };
        }
    }
}
