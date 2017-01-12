using System.Collections.Generic;
using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Catalog.Models.Outputs
{
    public class GetCategoriesResult : PagedResult<Category>
    {
        public GetCategoriesResult(IEnumerable<Category> result, int immediateCount, int totalCount, PagingSettings paging)
        : base(result, totalCount, paging)
        {
            ImmediateCount = immediateCount;
        }

        public int ImmediateCount { get; set; }
    }
}