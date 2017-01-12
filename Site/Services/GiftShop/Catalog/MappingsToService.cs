using System;
using Vtb24.Site.Services.GiftShop.Catalog.Models;
using Vtb24.Site.Services.GiftShop.Catalog.Models.Inputs;
using Vtb24.Site.Services.ProductCatalogSearcherService;

namespace Vtb24.Site.Services.GiftShop.Catalog
{
    internal static class MappingsToService
    {
        public static SortTypes ToSortTypes(CatalogSort original)
        {
            switch (original)
            {
                case CatalogSort.NameAsc:
                    return SortTypes.ByNameAsc;
                case CatalogSort.NameDesc:
                    return SortTypes.ByNameDesc;
                case CatalogSort.PriceAsc:
                    return SortTypes.ByPriceAscByNameAsc;
                case CatalogSort.PriceDesc:
                    return SortTypes.ByPriceDescByNameAsc;
                case CatalogSort.PopularityMostViewedDesc:
                    return SortTypes.ByPopularityDesc;
                case CatalogSort.DiscountDesc:
                    return SortTypes.ByDiscountDesc;
                default:
                    return SortTypes.ByNameAsc;
            }
        }

        public static PopularProductTypes ToPopularProductTypes(ProductPopularityType original)
        {
            switch (original)
            {
                case ProductPopularityType.MostOrdered:
                    return PopularProductTypes.MostOrdered;
                case ProductPopularityType.MostViewed:
                    return PopularProductTypes.MostViewed;
                case ProductPopularityType.MostWished:
                    return PopularProductTypes.MostWished;
                default:
                    return PopularProductTypes.MostOrdered;
            }
        }

        public static ProductParam ToProductParam(CatalogParameterCriteria original)
        {
            return new ProductParam
            {
                Name = original.Name,
                Value = original.Value
            };
        }

        public static CategoryProductParamsParameter ToCategoryProductParamsParameter(CatalogParameterDefinition original)
        {
            return new CategoryProductParamsParameter
            {
                Name = original.Name,
                Unit = original.Unit
            };
        }
    }
}
