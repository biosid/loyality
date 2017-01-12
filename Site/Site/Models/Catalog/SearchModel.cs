using System.Web.Mvc;

namespace Vtb24.Site.Models.Catalog
{
    public class SearchModel : BaseCatalogListingModel
    {
        public SelectListItem[] Categories { get; set; }
    }
}