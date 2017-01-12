using System.Linq;
using Vtb24.Arms.AdminServices.CatalogAdminService;
using Vtb24.Arms.AdminServices.GiftShopManagement.Catalog.Models;
using Vtb24.Arms.AdminServices.GiftShopManagement.Catalog.Models.Inputs;
using Vtb24.Arms.AdminServices.GiftShopManagement.Catalog.Models.Outputs;
using Vtb24.Arms.AdminServices.Infrastructure;
using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Catalog
{
    internal class Catalog : ICatalog
    {
        public Catalog(IAdminSecurityService security)
        {
            _security = security;
        }

        private readonly IAdminSecurityService _security;

        public GetCategoriesResult GetCategories(GetCategoriesFilter filter, PagingSettings paging)
        {
            filter = filter ?? new GetCategoriesFilter();

            using (var service = new CatalogAdminServiceClient())
            {
                var parameters = new GetAllSubCategoriesParameters
                {
                    ParentId = filter.ParentCategoryId,
                    CalcTotalCount = true,
                    NestingLevel = filter.Depth,
                    Status = filter.Status.MaybeInvoke(MappingsToService.ToCategoryStatus),
                    Type = filter.Type.MaybeInvoke(MappingsToService.ToCategoryType),
                    IncludeParent = filter.IncludeParent,
                    CountToSkip = paging.Skip,
                    CountToTake = paging.Take,
                    UserId = _security.CurrentUser
                };

                var response = service.GetAllSubCategories(parameters);

                response.AssertSuccess();

                var categories = response.Categories.Select(MappingsFromService.ToCategory).ToArray();

                return new GetCategoriesResult(categories, response.ChildrenCount, response.TotalCount ?? 0, paging);
            }
        }

        public Category GetCategory(int id)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var parameters = new GetAllSubCategoriesParameters
                {
                    ParentId = id,
                    IncludeParent = true,
                    CountToTake = 1,
                    UserId = _security.CurrentUser
                };

                var response = service.GetAllSubCategories(parameters);

                response.AssertSuccess();

                var category = MappingsFromService.ToCategory(response.Categories.FirstOrDefault());

                return category;
            }
        }

        public Category CreateCategory(CreateCategoryOptions options)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var parameters = new CreateCategoryParameters
                {
                    UserId = _security.CurrentUser,
                    Name = options.Title,
                    OnlineCategoryUrl = options.OnlineCategoryUrl,
                    NotifyOrderStatusUrl = options.NotifyOrderStatusUrl,
                    OnlineCategoryPartnerId = options.OnlineCategoryPartnerId,
                    ParentCategoryId = options.ParentId,
                    Type = MappingsToService.ToCategoryType(options.Type),
                    Status = ProductCategoryStatuses.NotActive
                };

                var response = service.CreateCategory(parameters);

                response.AssertSuccess();

                var category = MappingsFromService.ToCategory(response.Category);

                return category;
            }
        }

        public Category UpdateCategory(UpdateCategoryOptions options)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var parameters = new UpdateCategoryParameters
                {
                    UserId = _security.CurrentUser,
                    CategoryId = options.Id,
                    NewName = options.Title,
                    NewOnlineCategoryUrl = options.OnlineCategoryUrl,
                    NewNotifyOrderStatusUrl = options.NotifyOrderStatusUrl,
                    OnlineCategoryPartnerId = options.OnlineCategoryPartnerId,
                    NewStatus = MappingsToService.ToCategoryStatus(options.Status)
                };

                var response = service.UpdateCategory(parameters);

                response.AssertSuccess();

                var category = MappingsFromService.ToCategory(response.Category);

                return category;
            }
        }

        public void ChangeCategoriesStatus(int[] categories, CategoryStatus status)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var mappedStatus = MappingsToService.ToCategoryStatus(status);
                var userId = _security.CurrentUser; 
                var response = service.ChangeCategoriesStatus(userId, categories, mappedStatus);

                response.AssertSuccess();
            }
        }

        public void DeleteCategory(int id)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var userId = _security.CurrentUser; 
                var response = service.DeleteCategory(userId, id);

                response.AssertSuccess();
            }
        }

        public void MoveCategory(MoveCategoryOptions options)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var parameters = new MoveCategoryParameters
                {
                    UserId = _security.CurrentUser, 
                    CategoryId = options.CategoryId,
                    ReferenceCategoryId = options.ReferenceCategoryId,
                    PositionType = MappingsToService.ToMoveOptions(options.MoveOptions)
                };
                var response = service.MoveCategory(parameters);

                response.AssertSuccess();
            }
        }

        public CategoryBinding[] GetCategoryBindings(int supplierId, int[] categoryIds)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var response = service.GetPartnerProductCategoryLinks(_security.CurrentUser, supplierId, categoryIds);

                response.AssertSuccess();

                return response.Links.Select(MappingsFromService.ToCategoryBinding).ToArray();
            }
        }

        public void SetCategoryBinding(CategoryBinding binding)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var parameters = new CreatePartnerProductCateroryLinkParameters
                {
                    UserId = _security.CurrentUser,
                    Link = MappingsToService.ToCategoryBinding(binding)
                };

                var response = service.SetPartnerProductCategoryLink(parameters);

                response.AssertSuccess();
            }
        }

        public int[] GetCategoriesPermissions(int supplierId)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var response = service.GetCategoriesPermissions(_security.CurrentUser, supplierId);

                response.AssertSuccess();

                return response.CategoryIds;
            }
        }

        public void SetCategoriesPermissions(int supplierId, int[] addCategoriesIds, int[] removeCategoriesIds)
        {
            using (var service = new CatalogAdminServiceClient())
            {
                var parameters = new SetCategoriesPermissionsParameters
                {
                    UserId = _security.CurrentUser,
                    PartnerId = supplierId,
                    AddCategoriesId = addCategoriesIds,
                    RemoveCategoriesId = removeCategoriesIds
                };

                var response = service.SetCategoriesPermissions(parameters);

                response.AssertSuccess();
            }
        }
    }
}
