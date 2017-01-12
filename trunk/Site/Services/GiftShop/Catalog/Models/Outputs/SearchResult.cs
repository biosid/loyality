using System.Collections.Generic;
using Vtb24.Site.Services.Models;

namespace Vtb24.Site.Services.GiftShop.Catalog.Models.Outputs
{
    public class SearchResult : PagedResult<CatalogProduct>
    {
        public SearchResult(IEnumerable<CatalogProduct> result, int totalCount, decimal maxPrice, PagingSettings paging) 
        : base(result, totalCount, paging)
        {
            MaxPrice = maxPrice;
        }

        public decimal MaxPrice { get; set; }
    }
}