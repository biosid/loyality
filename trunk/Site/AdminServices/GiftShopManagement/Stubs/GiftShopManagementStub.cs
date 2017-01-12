using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using Vtb24.Arms.AdminServices.Infrastructure;
using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Stubs
{
    public class GiftShopManagementStub : IGiftShopManagement
    {
        private int _lastCategoryId;

        public GetCategoriesResult GetCategories(GetCategoriesFilter filter, PagingSettings paging)
        {
            IEnumerable<Category> categories;

            int? level;
            int immediate;
            if (!filter.ParentCategoryId.HasValue)
            {
                level = filter.Depth;
                immediate = Data.CategoryRoots.Count;
                categories = Data.Categories.ToArray();
            }
            else
            {
                var parent = Data.Categories.FirstOrDefault(c => c.Id == filter.ParentCategoryId);
                if (parent == null)
                {
                    return null;
                }

                level = filter.Depth.HasValue ? parent.Depth + filter.Depth : null;
                immediate = parent.CountedSubCategories ?? 0;
                categories = Data.Categories
                    .SkipWhile(c => c != parent).Skip(1)
                    .TakeWhile(c => c.Depth > parent.Depth)
                    .ToArray();
            }

            var total = categories.Count();

            //  вложенность
            if (level.HasValue)
            {
                categories = categories.Where(c => c.Depth <= level);
            }

            // пейджинг
            categories = categories.MaybeSkip(paging.Skip).MaybeTake(paging.Take);

            var result = categories.ToArray();

            return new GetCategoriesResult(result, immediate, total, paging);
        }

        public Category GetCategory(int id)
        {
            var cat = Data.CategoryRoots.FirstOrDefault(c => c.Id == id);
            return cat;
        }

        public Category CreateCategory(CreateCategoryOptions options)
        {
            var cat = new Category
            {
                Id = _lastCategoryId++,
                ParentId = options.ParentId,
                Title = options.Title,
                Type = options.Type,
                OnlineCategoryUrl = options.OnlineCategoryUrl,
                Status = CategoryStatus.Disabled
            };

            MoveCategory(cat, options.ParentId);

            Data.Categories.Add(cat);

            return cat;
        }

        public Category UpdateCategory(UpdateCategoryOptions options)
        {
            var cat = GetCategory(options.Id);

            cat.Title = options.Title;
            cat.OnlineCategoryUrl = options.OnlineCategoryUrl;
            cat.Status = options.Status;

            return cat;
        }

        public void ChangeCategoriesStatus(int[] categories, CategoryStatus status)
        {
            var cats = Data.Categories.Where(c => categories.Contains(c.Id)).ToArray();
            foreach (var category in cats)
            {
                category.Status = status;
            }
        }

        public void DeleteCategory(int id)
        {
            var cat = GetCategory(id);
            
            if (cat.ParentId.HasValue)
            {
                var parent = GetCategory(cat.ParentId.Value);
                parent.SubCategories.Remove(cat);
            }
            else
            {
                Data.CategoryRoots.Remove(cat);
            }

            Data.Categories.Remove(cat);
        }

        public void MoveCategory(MoveCategoryOptions options)
        {
            throw new NotImplementedException();
        }

        public CategoryBinding[] GetCategoryBindings(int supplierId, int[] categoryIds)
        {
            throw new NotImplementedException();
        }

        public void SetCategoryBinding(CategoryBinding binding)
        {
            throw new NotImplementedException();
        }

        public int[] GetCategoriesPermissions(int supplierId)
        {
            throw new NotImplementedException();
        }

        public void SetCategoriesPermissions(int supplierId, int[] addCategoriesIds, int[] removeCategoriesIds)
        {
            throw new NotImplementedException();
        }

        public PartnerInfo[] GetPartnersInfo()
        {
            throw new NotImplementedException();
        }

        public SupplierInfo[] GetSuppliersInfo()
        {
            throw new NotImplementedException();
        }

        public CarrierInfo[] GetCarriersInfo()
        {
            throw new NotImplementedException();
        }

        public PartnerInfo[] GetUserPartnersInfo()
        {
            throw new NotImplementedException();
        }

        public SupplierInfo[] GetUserSuppliersInfo()
        {
            throw new NotImplementedException();
        }

        public CarrierInfo[] GetUserCarriersInfo()
        {
            throw new NotImplementedException();
        }

        public PartnerInfo GetPartnerInfoById(int id)
        {
            throw new NotImplementedException();
        }

        public SupplierInfo GetSupplierInfoById(int id)
        {
            throw new NotImplementedException();
        }

        public CarrierInfo GetCarrierInfoById(int id)
        {
            throw new NotImplementedException();
        }

        public PartnerInfo GetUserPartnerInfoById(int id)
        {
            throw new NotImplementedException();
        }

        public SupplierInfo GetUserSupplierInfoById(int id)
        {
            throw new NotImplementedException();
        }

        public CarrierInfo GetUserCarrierInfoById(int id)
        {
            throw new NotImplementedException();
        }

        public Partner[] GetPartners()
        {
            throw new NotImplementedException();
        }

        public Supplier[] GetSuppliers()
        {
            throw new NotImplementedException();
        }

        public Carrier[] GetCarriers()
        {
            throw new NotImplementedException();
        }

        public Partner GetPartnerById(int id)
        {
            throw new NotImplementedException();
        }

        public Supplier GetSupplierById(int id)
        {
            throw new NotImplementedException();
        }

        public Carrier GetCarrierById(int id)
        {
            throw new NotImplementedException();
        }

        public int CreateSupplier(Supplier supplier)
        {
            throw new NotImplementedException();
        }

        public int CreateCarrier(Carrier carrier)
        {
            throw new NotImplementedException();
        }

        public void UpdateSupplier(Supplier supplier)
        {
            throw new NotImplementedException();
        }

        public void UpdateCarrier(Carrier carrier)
        {
            throw new NotImplementedException();
        }

        public void ImportDeliveryRatesFromHttp(int partnerId, string fileUrl)
        {
            throw new NotImplementedException();
        }

        public PartnerLocationsBindingsResult GetPartnerLocationsBindings(GetPartnerLocationsOptions options,
                                                                          PagingSettings paging)
        {
            throw new NotImplementedException();
        }

        public PartnerLocationsHistoryResult GetDeliveryLocationsHistory(int partnerId, PagingSettings paging)
        {
            throw new NotImplementedException();
        }

        public void SetPartnerLocationBinding(int bindingId, string kladr)
        {
            throw new NotImplementedException();
        }

        public void ResetPartnerLocationBinding(int bindingId)
        {
            throw new NotImplementedException();
        }

        public void ImportProductsFromYml(int supplierId, string fullPathName)
        {
            throw new NotImplementedException();
        }

        public ProductsSearchResult SearchProducts(ProductsSearchCriteria criteria, PagingSettings paging)
        {
            throw new NotImplementedException();
        }

        public string CreateProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public void UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public void ModerateProducts(ModerateProductsOptions options)
        {
            throw new NotImplementedException();
        }

        public void DeleteProducts(string[] ids)
        {
            throw new NotImplementedException();
        }

        public void MoveProducts(MoveProductsOptions options)
        {
            throw new NotImplementedException();
        }

        public void ActivateProducts(ActivateProductsOptions options)
        {
            throw new NotImplementedException();
        }

        public void SetProductsSegments(SetProductsSegmentsOptions options)
        {
            throw new NotImplementedException();
        }

        public void RecommendProducts(RecommendProductsOptions options)
        {
            throw new NotImplementedException();
        }

        public void ExportOrdersHistoryPage(OrdersExportOptions options, TextWriter writer, int page, out int totalPages)
        {
            throw new NotImplementedException();
        }

        public ProductImportTaskResult GetProductsImportsHistory(int supplierId, PagingSettings paging)
        {
            throw new NotImplementedException();
        }

        public DeliveryRatesImportTaskResult GetDeliveryRatesImportsHistory(int partnerId, PagingSettings paging)
        {
            throw new NotImplementedException();
        }

        public Order GetOrder(int id)
        {
            throw new NotImplementedException();
        }

        public OrdersSearchResult SearchOrders(OrdersSearchCriteria criteria, PagingSettings paging)
        {
            throw new NotImplementedException();
        }

        public void ChangeOrderStatus(ChangeOrderStatusOptions options)
        {
            throw new NotImplementedException();
        }

        public OrderStatusHistoryRecord[] GetOrderStatusHistory(int id)
        {
            throw new NotImplementedException();
        }

        private void MoveCategory(Category cat, int? parentId)
        {
            if (parentId.HasValue && !cat.ParentId.HasValue)
            {
                Data.CategoryRoots.Remove(cat);
            }

            if (cat.ParentId.HasValue)
            {
                var p = cat;
                while (p.ParentId.HasValue)
                {
                    p = GetCategory(p.ParentId.Value);
                    p.CountedSubCategories = (p.CountedSubCategories ?? 0) - 1;
                }
            }

            cat.ParentId = parentId;

            if (parentId.HasValue)
            {
                var parent = GetCategory(parentId.Value);
                cat.CategoryPath = parent.CategoryPath + "/" + cat.Title;
                parent.SubCategories.Add(cat);

                var p = cat;
                while (p.ParentId.HasValue)
                {
                    p = GetCategory(p.ParentId.Value);
                    p.CountedSubCategories = (p.CountedSubCategories ?? 0) + 1;
                }
            } 
            else
            {
                cat.CategoryPath = cat.Title;
                Data.CategoryRoots.Add(cat);
            }
        }
    }
}