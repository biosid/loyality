using System.IO;
using Vtb24.Arms.AdminServices.GiftShopManagement.Catalog.Models;
using Vtb24.Arms.AdminServices.GiftShopManagement.Catalog.Models.Inputs;
using Vtb24.Arms.AdminServices.GiftShopManagement.Catalog.Models.Outputs;
using Vtb24.Arms.AdminServices.GiftShopManagement.Delivery.Models.Inputs;
using Vtb24.Arms.AdminServices.GiftShopManagement.Delivery.Models.Outputs;
using Vtb24.Arms.AdminServices.GiftShopManagement.ImportTasks.Models.Outputs;
using Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models;
using Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models.Inputs;
using Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models.Outputs;
using Vtb24.Arms.AdminServices.GiftShopManagement.Partners.Models;
using Vtb24.Arms.AdminServices.GiftShopManagement.Products.Models;
using Vtb24.Arms.AdminServices.GiftShopManagement.Products.Models.Inputs;
using Vtb24.Arms.AdminServices.GiftShopManagement.Products.Models.Outputs;
using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices
{
    public interface IGiftShopManagement
    {
        #region Каталог

        GetCategoriesResult GetCategories(GetCategoriesFilter filter, PagingSettings paging);

        Category GetCategory(int id);

        Category CreateCategory(CreateCategoryOptions options);

        Category UpdateCategory(UpdateCategoryOptions options);

        void ChangeCategoriesStatus(int[] categories, CategoryStatus status);

        void DeleteCategory(int id);

        void MoveCategory(MoveCategoryOptions options);

        CategoryBinding[] GetCategoryBindings(int supplierId, int[] categoryIds);

        void SetCategoryBinding(CategoryBinding binding);

        int[] GetCategoriesPermissions(int supplierId);

        void SetCategoriesPermissions(int supplierId, int[] addCategoriesIds, int[] removeCategoriesIds);

        #endregion


        #region Партнеры

        #region получение краткой информации о партнерах

        PartnerInfo[] GetPartnersInfo();

        SupplierInfo[] GetSuppliersInfo();

        CarrierInfo[] GetCarriersInfo();

        PartnerInfo[] GetUserPartnersInfo();

        SupplierInfo[] GetUserSuppliersInfo();

        CarrierInfo[] GetUserCarriersInfo();

        PartnerInfo GetPartnerInfoById(int id);

        SupplierInfo GetSupplierInfoById(int id);

        CarrierInfo GetCarrierInfoById(int id);

        PartnerInfo GetUserPartnerInfoById(int id);

        SupplierInfo GetUserSupplierInfoById(int id);

        CarrierInfo GetUserCarrierInfoById(int id);

        #endregion

        #region получение полной информации о партнерах

        Partner[] GetPartners();

        Supplier[] GetSuppliers();

        Carrier[] GetCarriers();

        Partner GetPartnerById(int id);

        Supplier GetSupplierById(int id);

        Carrier GetCarrierById(int id);

        #endregion

        #region создание/обновление партнеров

        int CreateSupplier(Supplier supplier);

        int CreateCarrier(Carrier carrier);

        void UpdateSupplier(Supplier supplier);

        void UpdateCarrier(Carrier carrier);

        #endregion

        #region загрузка матрицы стоимости доставки

        void ImportDeliveryRatesFromHttp(int partnerId, string fileUrl);

        #endregion

        #endregion


        #region Доставка
        
        DeliveryRatesImportTaskResult GetDeliveryRatesImportsHistory(int partnerId, PagingSettings paging);

        PartnerLocationsBindingsResult GetPartnerLocationsBindings(GetPartnerLocationsOptions options,
                                                                   PagingSettings paging);

        PartnerLocationsHistoryResult GetDeliveryLocationsHistory(int partnerId, PagingSettings paging);

        void SetPartnerLocationBinding(int bindingId, string kladr);

        void ResetPartnerLocationBinding(int bindingId);
        
        #endregion


        #region Продукты

        void ImportProductsFromYml(int supplierId, string fullPathName);
        
        ProductsSearchResult SearchProducts(ProductsSearchCriteria criteria, PagingSettings paging);

        string CreateProduct(Product product);

        void UpdateProduct(Product product);

        void ModerateProducts(ModerateProductsOptions options);

        void DeleteProducts(string[] ids);

        void MoveProducts(MoveProductsOptions options);

        void ActivateProducts(ActivateProductsOptions options);

        void SetProductsSegments(SetProductsSegmentsOptions options);

        void RecommendProducts(RecommendProductsOptions options);

        #endregion


        #region Заказы

        Order GetOrder(int id);

        OrdersSearchResult SearchOrders(OrdersSearchCriteria criteria, PagingSettings paging);

        void ChangeOrderStatus(ChangeOrderStatusOptions options);

        OrderStatusHistoryRecord[] GetOrderStatusHistory(int id);

        void ExportOrdersHistoryPage(OrdersExportOptions options, TextWriter writer, int page, out int totalPages);

        #endregion


        #region Загрузки

        ProductImportTaskResult GetProductsImportsHistory(int supplierId, PagingSettings paging);

        #endregion
    }
}
