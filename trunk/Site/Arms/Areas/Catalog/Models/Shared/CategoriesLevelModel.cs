using System;

namespace Vtb24.Arms.Catalog.Models.Shared
{
    public class CategoriesLevelModel
    {
        public CategoryItemModel[] CategoryItems { get; set; }

        public Func<CategoryItemModel, string> ToActionUrl { get; set; }
    }
}
