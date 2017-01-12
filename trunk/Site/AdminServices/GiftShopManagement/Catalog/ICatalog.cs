using Vtb24.Arms.AdminServices.GiftShopManagement.Catalog.Models;
using Vtb24.Arms.AdminServices.GiftShopManagement.Catalog.Models.Inputs;
using Vtb24.Arms.AdminServices.GiftShopManagement.Catalog.Models.Outputs;
using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Catalog
{
    internal interface ICatalog
    {
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
    }
}
