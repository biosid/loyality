using System;
using Vtb24.Site.Services.Infrastructure;

namespace Vtb24.Site.Models.Catalog.Helpers
{
    public static class ProductNoveltyCriteria
    {
         public static bool IsNew(DateTime createDate)
         {
             return createDate >= GetMinNoveltyDate();
         }

        public static DateTime GetMinNoveltyDate()
        {
            var days = AppSettingsHelper.Int("product_novelty_days", 14);
            return DateTime.Now.AddDays(-days);
        }
    }
}