using System.Collections.Generic;
using Vtb24.Site.Services.Models;

namespace Vtb24.Site.Services.BankProducts.Models.Outputs
{
    public class BankProductsResult : PagedResult<BankProduct>
    {
        public BankProductsResult(IEnumerable<BankProduct> result, int totalCount, PagingSettings paging)
            : base(result, totalCount, paging)
        {
        }
    }
}
