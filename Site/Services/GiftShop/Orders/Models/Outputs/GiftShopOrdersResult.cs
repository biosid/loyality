using System.Collections.Generic;
using Vtb24.Site.Services.Models;

namespace Vtb24.Site.Services.GiftShop.Orders.Models.Outputs
{
    public class GiftShopOrdersResult : PagedResult<GiftShopOrder>
    {
        public GiftShopOrdersResult(IEnumerable<GiftShopOrder> result, int totalCount, PagingSettings paging)
            : base(result, totalCount, paging)
        {
        }
    }
}
