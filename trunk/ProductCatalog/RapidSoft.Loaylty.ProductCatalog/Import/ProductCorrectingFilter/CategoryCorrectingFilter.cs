namespace RapidSoft.Loaylty.ProductCatalog.Import.ProductCorrectingFilter
{
    using System.Collections.Generic;
    using System.Linq;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.Entities;

    internal class CategoryCorrectingFilter : IProductCorrectingFilter
    {
        private readonly IList<CategoryMappingProjection> catMappings;

        public CategoryCorrectingFilter(IList<CategoryMappingProjection> catMappings)
        {
            this.catMappings = catMappings;
        }

        public Result Execute(Product product)
        {
            var concretCatMaps = this.catMappings.Where(x => x.PartnerCategoryId == product.PartnerCategoryId).ToArray();

            if (concretCatMaps.Length > 1)
            {
                const string MessFormat = "Для продукта {0} найдено несколько маппингов для партнерской категории (категория продукта из файла {1})";
                return new Result(string.Format(MessFormat, product.PartnerProductId, product.PartnerCategoryId));
            }

            var concretCatMap = concretCatMaps.SingleOrDefault();

            if (concretCatMap == null)
            {
                const string MessFormat = "Для продукта {0} не найдена партнерская категория (категория продукта из файла {1})";
                return new Result(string.Format(MessFormat, product.PartnerProductId, product.PartnerCategoryId));
            }

            if (!concretCatMap.ProductCategoryId.HasValue)
            {
                const string MessFormat = "Для продукта {0} не найден маппинг партнерской категории (категория продукта из файла {1})";
                return new Result(string.Format(MessFormat, product.PartnerProductId, product.PartnerCategoryId));
            }

            var homeCategoryId = concretCatMap.ProductCategoryId.Value;

            product.CategoryId = homeCategoryId;

            return new Result(product);
        }
    }
}