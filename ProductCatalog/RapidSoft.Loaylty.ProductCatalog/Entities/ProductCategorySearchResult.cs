namespace RapidSoft.Loaylty.ProductCatalog.Entities
{
    using System.Collections.Generic;
    using System.Linq;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    public class ProductCategorySearchResult
    {
        public int? TotalCount { get; set; }

        public int ChildrenCount { get; set; }

        public ProductCategory[] Categories { get; set; }

        public static ProductCategorySearchResult Build(IEnumerable<ProductCategory> categories, int? totalCount, int childrenCount)
        {
            var asArray = categories.ToArray();
            return new ProductCategorySearchResult
                       {
                           Categories = asArray,
                           TotalCount = totalCount,
                           ChildrenCount = childrenCount
                       };
        }
    }
}