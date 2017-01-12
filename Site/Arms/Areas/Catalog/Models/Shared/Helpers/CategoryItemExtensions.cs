using System;
using System.Collections.Generic;
using System.Linq;

namespace Vtb24.Arms.Catalog.Models.Shared.Helpers
{
    public static class CategoryItemExtensions
    {
        public static CategoriesLevelModel GetRootCategoriesModel(this IEnumerable<CategoryItemModel> categoryItems, Func<CategoryItemModel, string> toActionUrl)
        {
            return new CategoriesLevelModel
            {
                CategoryItems = categoryItems.Where(c => !c.ParentId.HasValue).ToArray(),
                ToActionUrl = toActionUrl
            };
        }

        public static CategoriesLevelModel GetRootCategoryModel(this IEnumerable<CategoryItemModel> categoryItems, Func<CategoryItemModel, string> toActionUrl)
        {
            var first = categoryItems.FirstOrDefault(c => !c.ParentId.HasValue);

            return new CategoriesLevelModel
            {
                CategoryItems = new[]
                {
                    first != null
                        ? first.Parent
                        : new CategoryItemModel
                        {
                            Id = -1,
                            Title = "Нет",
                            Depth = 0,
                            IsPermissinsGranted = true,
                            Children = new CategoryItemModel[0]
                        }
                },
                ToActionUrl = toActionUrl
            };
        }
    }
}
