using System.IO;
using Vtb24.Arms.AdminServices.GiftShopManagement.Catalog;
using Vtb24.Arms.AdminServices.GiftShopManagement.Catalog.Models;
using Vtb24.Arms.AdminServices.GiftShopManagement.Catalog.Models.Inputs;
using Vtb24.Arms.AdminServices.GiftShopManagement.Catalog.Models.Outputs;
using Vtb24.Arms.AdminServices.GiftShopManagement.Delivery;
using Vtb24.Arms.AdminServices.GiftShopManagement.Delivery.Models.Inputs;
using Vtb24.Arms.AdminServices.GiftShopManagement.Delivery.Models.Outputs;
using Vtb24.Arms.AdminServices.GiftShopManagement.ImportTasks;
using Vtb24.Arms.AdminServices.GiftShopManagement.ImportTasks.Models.Outputs;
using Vtb24.Arms.AdminServices.GiftShopManagement.Orders;
using Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models;
using Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models.Inputs;
using Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models.Outputs;
using Vtb24.Arms.AdminServices.GiftShopManagement.Partners;
using Vtb24.Arms.AdminServices.GiftShopManagement.Partners.Models;
using Vtb24.Arms.AdminServices.GiftShopManagement.Products;
using Vtb24.Arms.AdminServices.GiftShopManagement.Products.Models;
using Vtb24.Arms.AdminServices.GiftShopManagement.Products.Models.Inputs;
using Vtb24.Arms.AdminServices.GiftShopManagement.Products.Models.Outputs;
using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices.GiftShopManagement
{
    public class GiftShopManagement : IGiftShopManagement
    {
        public GiftShopManagement(IAdminSecurityService security)
        {
            _catalog = new Catalog.Catalog(security);
            _partners = new Partners.Partners(security);
            _products = new Products.Products(security);
            _orders = new Orders.Orders(security);
            _imports = new ImportTasks.ImportTasks(security);
            _delivery = new Delivery.Delivery(security);
            _ordersExport = new OrdersExport(_orders);
        }

        private readonly ICatalog _catalog;
        private readonly IPartners _partners;
        private readonly IProducts _products;
        private readonly IOrders _orders;
        private readonly IOrdersExport _ordersExport;
        private readonly IImportTasks _imports;
        private readonly IDelivery _delivery;

        #region Каталог

        public GetCategoriesResult GetCategories(GetCategoriesFilter filter, PagingSettings paging)
        {
            return _catalog.GetCategories(filter, paging);
        }

        public Category GetCategory(int id)
        {
            return _catalog.GetCategory(id);
        }

        public Category CreateCategory(CreateCategoryOptions options)
        {
            return _catalog.CreateCategory(options);
        }

        public Category UpdateCategory(UpdateCategoryOptions options)
        {
            return _catalog.UpdateCategory(options);
        }

        public void ChangeCategoriesStatus(int[] categories, CategoryStatus status)
        {
            _catalog.ChangeCategoriesStatus(categories, status);
        }

        public void DeleteCategory(int id)
        {
            _catalog.DeleteCategory(id);
        }

        public void MoveCategory(MoveCategoryOptions options)
        {
            _catalog.MoveCategory(options);
        }

        public CategoryBinding[] GetCategoryBindings(int supplierId, int[] categoryIds)
        {
            return _catalog.GetCategoryBindings(supplierId, categoryIds);
        }

        public void SetCategoryBinding(CategoryBinding binding)
        {
            _catalog.SetCategoryBinding(binding);
        }

        public int[] GetCategoriesPermissions(int supplierId)
        {
            return _catalog.GetCategoriesPermissions(supplierId);
        }

        public void SetCategoriesPermissions(int supplierId, int[] addCategoriesIds, int[] removeCategoriesIds)
        {
            _catalog.SetCategoriesPermissions(supplierId, addCategoriesIds, removeCategoriesIds);
        }

        #endregion

        #region Продукты

        public void ImportProductsFromYml(int supplierId, string fullPathName)
        {
            _products.ImportProductsFromYml(supplierId, fullPathName);
        }

        public ProductsSearchResult SearchProducts(ProductsSearchCriteria criteria, PagingSettings paging)
        {
            return _products.SearchProducts(criteria, paging);
        }

        public string CreateProduct(Product product)
        {
            return _products.CreateProduct(product);
        }

        public void UpdateProduct(Product product)
        {
            _products.UpdateProduct(product);
        }

        public void ModerateProducts(ModerateProductsOptions options)
        {
            _products.ModerateProducts(options);
        }

        public void DeleteProducts(string[] ids)
        {
            _products.DeleteProducts(ids);
        }

        public void MoveProducts(MoveProductsOptions options)
        {
            _products.MoveProducts(options);
        }

        public void ActivateProducts(ActivateProductsOptions options)
        {
            _products.ActivateProducts(options);
        }

        public void SetProductsSegments(SetProductsSegmentsOptions options)
        {
            _products.SetProductsSegments(options);
        }

        public void RecommendProducts(RecommendProductsOptions options)
        {
            _products.RecommendProducts(options);
        }

        #endregion

        #region Доставка

        public DeliveryRatesImportTaskResult GetDeliveryRatesImportsHistory(int partnerId, PagingSettings paging)
        {
            return _delivery.GetDeliveryRatesImportsHistory(partnerId, paging);
        }

        public PartnerLocationsBindingsResult GetPartnerLocationsBindings(GetPartnerLocationsOptions options,
                                                                          PagingSettings paging)
        {
            return _delivery.GetDeliveryLocations(options, paging);
        }

        public PartnerLocationsHistoryResult GetDeliveryLocationsHistory(int partnerId, PagingSettings paging)
        {
            return _delivery.GetDeliveryLocationsHistory(partnerId, paging);
        }

        public void SetPartnerLocationBinding(int bindingId, string kladr)
        {
            _delivery.SetDeliveryLocationKladr(bindingId, kladr);
        }

        public void ResetPartnerLocationBinding(int bindingId)
        {
            _delivery.ResetDeliveryLocationKladr(bindingId);
        }

        #endregion

        #region Партнёры

        #region получение краткой информации о партнерах

        public PartnerInfo[] GetPartnersInfo()
        {
            return _partners.GetPartnersInfo();
        }

        public SupplierInfo[] GetSuppliersInfo()
        {
            return _partners.GetSuppliersInfo();
        }

        public CarrierInfo[] GetCarriersInfo()
        {
            return _partners.GetCarriersInfo();
        }

        public PartnerInfo[] GetUserPartnersInfo()
        {
            return _partners.GetUserPartnersInfo();
        }

        public SupplierInfo[] GetUserSuppliersInfo()
        {
            return _partners.GetUserSuppliersInfo();
        }

        public CarrierInfo[] GetUserCarriersInfo()
        {
            return _partners.GetUserCarriersInfo();
        }

        public PartnerInfo GetPartnerInfoById(int id)
        {
            return _partners.GetPartnerInfoById(id);
        }

        public SupplierInfo GetSupplierInfoById(int id)
        {
            return _partners.GetSupplierInfoById(id);
        }

        public CarrierInfo GetCarrierInfoById(int id)
        {
            return _partners.GetCarrierInfoById(id);
        }

        public PartnerInfo GetUserPartnerInfoById(int id)
        {
            return _partners.GetUserPartnerInfoById(id);
        }

        public SupplierInfo GetUserSupplierInfoById(int id)
        {
            return _partners.GetUserSupplierInfoById(id);
        }

        public CarrierInfo GetUserCarrierInfoById(int id)
        {
            return _partners.GetUserCarrierInfoById(id);
        }

        #endregion

        #region получение полной информации о партнерах

        public Partner[] GetPartners()
        {
            return _partners.GetPartners();
        }

        public Supplier[] GetSuppliers()
        {
            return _partners.GetSuppliers();
        }

        public Carrier[] GetCarriers()
        {
            return _partners.GetCarriers();
        }

        public Partner GetPartnerById(int id)
        {
            return _partners.GetPartnerById(id);
        }

        public Supplier GetSupplierById(int id)
        {
            return _partners.GetSupplierById(id);
        }

        public Carrier GetCarrierById(int id)
        {
            return _partners.GetCarrierById(id);
        }

        #endregion

        #region создание/обновление партнеров

        public int CreateSupplier(Supplier supplier)
        {
            return _partners.CreateSupplier(supplier);
        }

        public int CreateCarrier(Carrier carrier)
        {
            return _partners.CreateCarrier(carrier);
        }

        public void UpdateSupplier(Supplier supplier)
        {
            _partners.UpdateSupplier(supplier);
        }

        public void UpdateCarrier(Carrier carrier)
        {
            _partners.UpdateCarrier(carrier);
        }

        #endregion

        #region загрузка матрицы стоимости доставки

        public void ImportDeliveryRatesFromHttp(int partnerId, string fileUrl)
        {
            _partners.ImportDeliveryRatesFromHttp(partnerId, fileUrl);
        }

        #endregion

        #endregion

        #region Заказы

        public Order GetOrder(int id)
        {
            return _orders.GetOrder(id);
        }

        public OrdersSearchResult SearchOrders(OrdersSearchCriteria criteria, PagingSettings paging)
        {
            return _orders.SearchOrders(criteria, paging);
        }

        public void ChangeOrderStatus(ChangeOrderStatusOptions options)
        {
            _orders.ChangeOrderStatus(options);
        }

        public OrderStatusHistoryRecord[] GetOrderStatusHistory(int id)
        {
            return _orders.GetOrderStatusHistory(id);
        }

        public void ExportOrdersHistoryPage(OrdersExportOptions options, TextWriter writer, int page, out int totalPages)
        {
            _ordersExport.ExportOrdersHistoryPage(options, writer, page, out totalPages);
        }

        #endregion

        #region Загрузки

        public ProductImportTaskResult GetProductsImportsHistory(int supplierId, PagingSettings paging)
        {
            return _imports.GetProductsImportsHistory(supplierId, paging);
        }

        #endregion
    }
}
