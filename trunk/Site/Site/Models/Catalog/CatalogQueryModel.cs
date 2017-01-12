using System.Web.Mvc;
using Vtb24.Site.Infrastructure;

namespace Vtb24.Site.Models.Catalog
{
    public class CatalogQueryModel : BaseQueryModel
    {
        public CatalogQueryModel()
        {
            sort = Sortings.popularity_desc;
        }

        // ReSharper disable InconsistentNaming

        [AllowHtml]
        public string term { get; set; }
        
        public Sortings sort { get; set; }

        public decimal? min_price { get; set; }

        public decimal? max_price { get; set; }

        public string[] vendor { get; set; }

        public int? category { get; set; }

        public bool is_new { get; set; }

        public bool sale { get; set; }

        // ReSharper restore InconsistentNaming
    }
}
