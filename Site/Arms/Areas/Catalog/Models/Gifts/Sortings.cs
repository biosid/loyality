using System.ComponentModel;
using Vtb24.Arms.AdminServices.GiftShopManagement.Products.Models;

namespace Vtb24.Arms.Catalog.Models.Gifts
{
    public enum Sortings
    {
// ReSharper disable InconsistentNaming
        [Description("Цена по возрастанию")]
        price_asc_name_asc,

        [Description("Цена по убыванию")]
        price_desc_name_asc
// ReSharper restore InconsistentNaming
    }

    public static class SortingsExtensions
    {
        public static ProductsSort Map(this Sortings? original)
        {
            switch (original)
            {
                case Sortings.price_asc_name_asc:
                    return ProductsSort.ByPriceAscByNameAsc;
                case Sortings.price_desc_name_asc:
                    return ProductsSort.ByPriceDescByNameAsc;
            }
            return ProductsSort.ByPriceAscByNameAsc;
        }
    }
}
