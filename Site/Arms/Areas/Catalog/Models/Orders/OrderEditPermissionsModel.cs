using Vtb24.Arms.AdminServices;
using Vtb24.Arms.AdminServices.AdminSecurityService.Helpers;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;

namespace Vtb24.Arms.Catalog.Models.Orders
{
    public class OrderEditPermissionsModel
    {
        public bool Prices { get; set; }

        public bool Status { get; set; }

        public static OrderEditPermissionsModel Map(IAdminSecurityService security)
        {
            var permissions = security.CurrentPermissions;

            return new OrderEditPermissionsModel
            {
                Prices = permissions.IsGranted(PermissionKeys.Catalog_Orders_Prices),
                Status = permissions.IsGranted(PermissionKeys.Catalog_Orders_Status)
            };
        }
    }
}
