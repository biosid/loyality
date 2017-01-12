using Vtb24.Arms.AdminServices;
using Vtb24.Arms.AdminServices.AdminSecurityService.Helpers;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;

namespace Vtb24.Arms.Catalog.Models.Gifts
{
    public class GiftsPermissionsModel
    {
        public bool Edit { get; set; }

        public bool Import { get; set; }

        public bool Moderate { get; set; }

        public bool Delete { get; set; }

        public bool Activate { get; set; }

        public bool Move { get; set; }

        public bool SetSegments { get; set; }

        public bool Recommend { get; set; }

        public bool MassActions
        {
            get { return Moderate || Delete || Activate || Move || SetSegments || Recommend; }
        }

        public static GiftsPermissionsModel Map(IAdminSecurityService security)
        {
            var permissions = security.CurrentPermissions;

            return new GiftsPermissionsModel
            {
                Edit = permissions.IsGranted(PermissionKeys.Catalog_Gifts_Edit),
                Import = permissions.IsGranted(PermissionKeys.Catalog_Gifts_Import),
                Moderate = permissions.IsGranted(PermissionKeys.Catalog_Gifts_Moderate),
                Delete = permissions.IsGranted(PermissionKeys.Catalog_Gifts_Delete),
                Activate = permissions.IsGranted(PermissionKeys.Catalog_Gifts_Activate),
                Move = permissions.IsGranted(PermissionKeys.Catalog_Gifts_Move),
                SetSegments = permissions.IsGranted(PermissionKeys.Catalog_Gifts_SetSegments),
                Recommend = permissions.IsGranted(PermissionKeys.Catalog_Gifts_Recommend)
            };
        }
    }
}
