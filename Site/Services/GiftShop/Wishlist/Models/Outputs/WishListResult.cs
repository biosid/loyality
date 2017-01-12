using System.Collections.Generic;
using Vtb24.Site.Services.GiftShop.Model;
using Vtb24.Site.Services.Models;

namespace Vtb24.Site.Services.GiftShop.Wishlist.Models.Outputs
{
    public class WishListResult : PagedResult<ReservedProductItem>
    {
        public WishListResult(IEnumerable<ReservedProductItem> result, int totalCount, PagingSettings paging) 
        : base(result, totalCount, paging)
        {
        }
    }
}