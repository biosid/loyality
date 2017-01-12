using Vtb24.Arms.Catalog.Models.Shared;

namespace Vtb24.Arms.Catalog.Models.Categories
{
    public class CategoriesModel
    {
        public CategoryModel[] Categories;

        public CategoryItemModel[] CategoryItems { get; set; }

        public CategoriesPermissionsModel Permissions { get; set; }

        public CreateCategoryModel Create { get; set; }

        public UpdateOnlineCategoryModel UpdateOnline { get; set; }

        public MoveCategoryModel Move { get; set; }
    }
}