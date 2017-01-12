using Vtb24.Arms.AdminServices;
using Vtb24.Arms.AdminServices.AdminSecurityService.Helpers;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;

namespace Vtb24.Arms.Models
{
    public class HomePermissionsModel
    {
        public string CatalogControllerName { get; set; }

        public string ActionsControllerName { get; set; }

        public string SecurityControllerName { get; set; }

        public string SiteControllerName { get; set; }

        public string ArmSecurityControllerName { get; set; }

        public static HomePermissionsModel Map(IAdminSecurityService security)
        {
            var permissions = security.CurrentPermissions;

            var model = new HomePermissionsModel();

            if (permissions.IsGranted(PermissionKeys.Catalog_Login))
            {
                if (permissions.IsAllGranted(PermissionKeys.Catalog_Categories, PermissionKeys.Catalog_Categories_Manage))
                {
                    model.CatalogControllerName = "Categories";
                }
                else if (permissions.IsGranted(PermissionKeys.Catalog_PartnerCategories))
                {
                    model.CatalogControllerName = "PartnerCategories";
                }
                else if (permissions.IsAllGranted(PermissionKeys.Catalog_Gifts, PermissionKeys.Catalog_Gifts_Edit))
                {
                    model.CatalogControllerName = "Gifts";
                }
                else if (permissions.IsGranted(PermissionKeys.Catalog_Orders))
                {
                    model.CatalogControllerName = "Orders";
                }
                else if (permissions.IsGranted(PermissionKeys.Catalog_Partners))
                {
                    model.CatalogControllerName = "Suppliers";
                }
                else if (permissions.IsGranted(PermissionKeys.Catalog_Partners_DeliveryMatrix))
                {
                    model.CatalogControllerName = "Delivery";
                }
            }

            if (permissions.IsAllGranted(PermissionKeys.Actions_Login, PermissionKeys.Actions))
            {
                model.ActionsControllerName = "Actions";
            }

            if (permissions.IsGranted(PermissionKeys.Site_Login))
            {
                if (permissions.IsGranted(PermissionKeys.Site_Page))
                {
                    model.SiteControllerName = "PlainPages";
                }
                else if (permissions.IsGranted(PermissionKeys.Site_News))
                {
                    model.SiteControllerName = "News";
                }
                else
                {
                    model.SiteControllerName = "Files";
                }
            }

            if (permissions.IsGranted(PermissionKeys.Security_Login))
            {
                if (permissions.IsGranted(PermissionKeys.Security_Clients))
                {
                    model.SecurityControllerName = "Users";
                }
                else if (permissions.IsGranted(PermissionKeys.Security_CustomFields))
                {
                    model.SecurityControllerName = "CustomFields";
                }
                else if (permissions.IsGranted(PermissionKeys.Security_Clients_Feedback))
                {
                    model.SecurityControllerName = "Feedback";
                }
            }

            if (permissions.IsGranted(PermissionKeys.AdminSecurity_All))
            {
                model.ArmSecurityControllerName = "Groups";
            }

            return model;
        }
    }
}
