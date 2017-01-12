using System.Linq;
using Vtb24.Arms.AdminServices.CatalogAdminService;
using Vtb24.Arms.AdminServices.GiftShopManagement.Products.Models.Inputs;
using Vtb24.Arms.AdminServices.GiftShopManagement.Products.Models.Outputs;
using Vtb24.Arms.AdminServices.Infrastructure;
using Vtb24.Arms.AdminServices.Models;
using Product = Vtb24.Arms.AdminServices.GiftShopManagement.Products.Models.Product;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Products
{
    public class Products : IProducts
    {
        public Products(IAdminSecurityService security)
        {
            _security = security;
        }

        private readonly IAdminSecurityService _security;

        public ProductsSearchResult SearchProducts(ProductsSearchCriteria criteria, PagingSettings paging)
        {
            var parameters = new AdminSearchProductsParameters
            {
                UserId = _security.CurrentUser,

                // критерий поиска
                SearchTerm = criteria.SearchTerm,
                SortType = MappingsToService.ToProductsSort(criteria.Sorting),
                IncludeSubCategory = criteria.IncludeSubCategories,
                ParentCategories = criteria.CategoryIds.MaybeToArray(),
                PartnerIds = criteria.SupplierIds,
                ProductIds = criteria.ProductIds,
                ModerationStatus = MappingsToService.ToProductModerationStatus(criteria.ModerationStatus),
                IsRecommended = criteria.IsRecommended,

                // пейджинг
                CountToTake = paging.Take,
                CountToSkip = paging.Skip
            };

            using (var service = new CatalogAdminServiceClient())
            {
                var response = service.SearchProducts(parameters);

                response.AssertSuccess();

                var products = response.Products.Select(MappingsFromService.ToProduct).ToArray();
                return new ProductsSearchResult(products, (int?)response.TotalCount ?? products.Count(), paging);
            }
        }

        public string CreateProduct(Product product)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var parameters = new CreateProductParameters
                {
                    UserId = _security.CurrentUser,
                    Name = product.Name,
                    PartnerProductId = product.SupplierProductId,
                    PriceRUR = product.PriceRUR,
                    BasePriceRUR = product.BasePriceRUR,
                    Description = product.Description,
                    Vendor = product.Vendor,
                    Weight = product.Weight.Value, // REVIEW: решить что-то с nullable int весом
					IsDeliveredByEmail = product.IsDeliveredByEmail,
                    Pictures = product.Pictures,
                    Param = product.Parameters.Select(MappingsToService.ToProductParameter).ToArray(),
                    PartnerId = product.SupplierId,
                    CategoryId = product.CategoryId
                };

                var response = service.CreateProduct(parameters);

                response.AssertSuccess();

                return response.Product.ProductId;
            }
        }

        public void UpdateProduct(Product product)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var parameters = new UpdateProductParameters
                {
                    UserId = _security.CurrentUser,
                    ProductId = product.Id,
                    Name = product.Name,
                    PriceRUR = product.PriceRUR,
                    BasePriceRUR = product.BasePriceRUR,
                    Description = product.Description,
                    Vendor = product.Vendor,
                    Weight = product.Weight.Value, // REVIEW: решить что-то с nullable int весом
					IsDeliveredByEmail = product.IsDeliveredByEmail,
                    Pictures = product.Pictures,
                    Param = product.Parameters.Select(MappingsToService.ToProductParameter).ToArray(),
                    PartnerId = product.SupplierId,
                    CategoryId = product.CategoryId
                };

                var response = service.UpdateProduct(parameters);

                response.AssertSuccess();
            }
        }

        public void ImportProductsFromYml(int supplierId, string fullPathName)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var parameters = new ImportProductsFromYmlHttpParameters
                {
                    UserId = _security.CurrentUser,
                    PartnerId = supplierId,
                    FullFilePath = fullPathName
                };
                var response = service.ImportProductsFromYmlHttp(parameters);

                response.AssertSuccess();
            }
        }

        public void ModerateProducts(ModerateProductsOptions options)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var parameters = new ChangeModerationStatusParameters
                {
                    UserId = _security.CurrentUser,
                    ProductIds = options.ProductIds,
                    ProductModerationStatus = MappingsToService.ToProductModerationStatus(options.ModerationStatus)
                };

                var response = service.ChangeProductsModerationStatus(parameters);

                response.AssertSuccess();
            }
        }

        public void DeleteProducts(string[] ids)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var parameters = new DeleteProductParameters
                {
                    UserId = _security.CurrentUser,
                    ProductIds = ids
                };

                var response = service.DeleteProducts(parameters);

                response.AssertSuccess();
            }
        }

        public void MoveProducts(MoveProductsOptions options)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var parameters = new MoveProductsParameters
                {
                    UserId = _security.CurrentUser,
                    ProductIds = options.ProductIds,
                    TargetCategoryId = options.CategoryId
                };
                var response = service.MoveProducts(parameters);

                response.AssertSuccess();
            }
        }

        public void ActivateProducts(ActivateProductsOptions options)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var parameters = new ChangeStatusParameters
                {
                    UserId = _security.CurrentUser,
                    ProductIds = options.ProductIds,
                    ProductStatus = MappingsToService.ToProductStatuses(options.Status)
                };

                var response = service.ChangeProductsStatus(parameters);

                response.AssertSuccess();
            }
        }

        public void SetProductsSegments(SetProductsSegmentsOptions options)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var parameters = new SetProductsTargetAudiencesParameters
                {
                    UserId = _security.CurrentUser,
                    ProductIds = options.ProductIds,
                    TargetAudienceIds = options.Segments
                };

                var response = service.SetProductsTargetAudiences(parameters);

                response.AssertSuccess();
            }
        }

        public void RecommendProducts(RecommendProductsOptions options)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var parameters = new RecommendParameters
                {
                    UserId = _security.CurrentUser,
                    ProductIds = options.ProductIds,
                    IsRecommended = options.IsRecommended
                };

                var response = service.RecommendProducts(parameters);

                response.AssertSuccess();
            }
        }
    }
}
