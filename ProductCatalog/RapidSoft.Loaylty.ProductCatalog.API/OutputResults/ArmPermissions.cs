namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    public class ArmPermissions
    {
        // Категории
        public const string ProductCategoriesView = "ProductCategories_View";
        public const string ProductCategoriesManage = "ProductCategories_Manage";
        public const string ProductCategoryLinksVew = "ProductCategoryLinks_Vew";
        public const string ProductCategoryLinksSetPermissions = "ProductCategoryLinks_SetPermissions";
        public const string ProductCategoryLinksMapping = "ProductCategoryLinks_Mapping";

        // Подарки
        public const string ProductsImportCatalog = "Products_ImportCatalog";
        public const string ProductsModerate = "Products_Moderate";
        public const string ProductsDelete = "Products_Delete";
        public const string ProductsActivateDeactivate = "Products_ActivateDeactivate";
        public const string ProductsChangeProductCategory = "Products_ChangeProductCategory";
        public const string ProductsAssignAudience = "Products_AssignAudience";
        public const string ProductsView = "Products_View";
        public const string ProductsCreateUpdate = "Products_CreateUpdate";
        
        // Заказы
        public const string OrdersViewList = "Orders_ViewList";
        public const string OrdersViewOrder = "Orders_ViewOrder";
        public const string OrdersViewOrderPrices = "Orders_ViewOrderPrices";
        public const string OrdersChangeOrderStatus = "Orders_ChangeOrderStatus";
        
        // Партнёры
        public const string PartnersVew = "Partners_Vew";
        public const string PartnersCreateUpdateDelete = "Partners_CreateUpdateDelete";
        public const string PartnersDownloadDeliveryMatrix = "Partners_DownloadDeliveryMatrix";
        public const string PartnersUploadDeliveryMatrix = "Partners_UploadDeliveryMatrix";
        
        // Промоакции
        public const string PromoVewList = "Promo_VewList";
        public const string PromoVewPromo = "Promo_VewPromo";
        public const string PromoCreateUpdateDelete = "Promo_CreateUpdateDelete";
    }
}