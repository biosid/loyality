using System.Collections.Generic;
using Vtb24.Site.Services.Models;

namespace Vtb24.Site.Services.GiftShop.Catalog.Models.Outputs
{
    public class CategoriesResult : PagedResult<CatalogCategory>
    {
        public CategoriesResult(IEnumerable<CatalogCategory> result, int immediateCount, int totalCount, PagingSettings paging)
        : base(result, totalCount, paging)
        {
            ImmediateCount = immediateCount;
        }

        public int ImmediateCount { get; set; }
    }
}