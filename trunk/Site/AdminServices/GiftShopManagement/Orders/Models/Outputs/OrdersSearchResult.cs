using System.Collections.Generic;
using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models.Outputs
{
    public class OrdersSearchResult: PagedResult<Order>
    {
        public OrdersSearchResult(IEnumerable<Order> result, int totalCount, PagingSettings paging)
            : base(result, totalCount, paging)
        {
        }
    }
}