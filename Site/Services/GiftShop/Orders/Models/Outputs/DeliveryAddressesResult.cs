using System.Collections.Generic;
using Vtb24.Site.Services.Models;

namespace Vtb24.Site.Services.GiftShop.Orders.Models.Outputs
{
    public class DeliveryAddressesResult : PagedResult<LastDeliveryAddress>
    {
        public DeliveryAddressesResult(IEnumerable<LastDeliveryAddress> result, int totalCount, PagingSettings paging)
            : base(result, totalCount, paging)
        {
        }
    }
}
