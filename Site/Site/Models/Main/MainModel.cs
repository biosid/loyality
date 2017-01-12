using System.Collections.Generic;
using Vtb24.Site.Models.Catalog;

namespace Vtb24.Site.Models.Main
{
    public class MainModel
    {
        public const int RECOMENDED_PRODUCTS_COUNT = 6;

        public const int RECOMMENDED_PRODUCT_TITLE = 60;

        public bool IsAuthenticated { get; set; }

		public bool ShowBonusBackBanner { get; set; }

        public decimal? Balance { get; set; }

        public string PoliteName { get; set; }

        public List<object> RecomendedProducts { get; set; }

        public IEnumerable<object> PopularProductsByView { get; set; }

        public IEnumerable<object> PopularProductsByOrder { get; set; }

        public IEnumerable<object> PopularProductsByWish { get; set; }

        public IEnumerable<NewsMessageListModel> News { get; set; }
    }
}