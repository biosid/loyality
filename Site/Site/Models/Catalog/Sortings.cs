using System.ComponentModel;
using Vtb24.Site.Services.GiftShop.Catalog.Models;

namespace Vtb24.Site.Models.Catalog
{
    public enum Sortings
    {
        // ReSharper disable InconsistentNaming

        [Description("�� ������������")]
        popularity_desc = 0,

        [Description("���� �� �����������")]
        price_asc = 1,

        [Description("���� �� ��������")]
        price_desc = 2,

        [Description("������ �� ��������")]
        discount_desc = 3

        // ReSharper restore InconsistentNaming
    }

    public static class SortingsExtensions
    {
        public static CatalogSort Map(this Sortings sort)
        {
            switch (sort)
            {
                case Sortings.price_desc:
                    return CatalogSort.PriceDesc;
                case Sortings.price_asc:
                    return CatalogSort.PriceAsc;
                case Sortings.popularity_desc:
                    return CatalogSort.PopularityMostViewedDesc;       
                case Sortings.discount_desc:
                    return CatalogSort.DiscountDesc;
            }
            return CatalogSort.NameAsc;
        }
    }
}