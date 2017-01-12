namespace Vtb24.Arms.AdminServices.AdminSecurityService.Models
{
    public enum PermissionKeys
    {
        // ReSharper disable InconsistentNaming

        Catalog_Login,

        Catalog_Categories,
        Catalog_Categories_Manage,

        Catalog_PartnerIds,

        Catalog_PartnerCategories,

        Catalog_Gifts,
        Catalog_Gifts_Edit,
        Catalog_Gifts_Import,
        Catalog_Gifts_Moderate,
        Catalog_Gifts_Delete,
        Catalog_Gifts_Activate,
        Catalog_Gifts_Move,
        Catalog_Gifts_SetSegments,
        Catalog_Gifts_Recommend,

        Catalog_Orders,
        Catalog_Orders_Prices,
        Catalog_Orders_Status,

        Catalog_Partners,
        Catalog_Partners_Edit,
        Catalog_Partners_DeliveryMatrix,

        Actions_Login,

        Actions,
        Actions_Edit,

        Site_Login,

        Site_Page,
        Site_News,

        Security_Login,

        Security_Clients,
        Security_Clients_Deactivate,
        Security_Clients_ResetPassword,
        Security_Clients_SiteAccess,
        Security_Clients_ChangePhone,
        Security_Clients_Feedback,

        Security_CustomFields,

        AdminSecurity_All

        // ReSharper restore InconsistentNaming
    }
}
