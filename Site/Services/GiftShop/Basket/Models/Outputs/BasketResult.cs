using System.Collections.Generic;
using Vtb24.Site.Services.GiftShop.Model;
using Vtb24.Site.Services.Models;

namespace Vtb24.Site.Services.GiftShop.Basket.Models.Outputs
{
    public class BasketResult : PagedResult<ReservedProductItem>
    {
        public BasketResult(IEnumerable<ReservedProductItem> result, int totalCount, PagingSettings paging) 
        : base(result, totalCount, paging)
        {
        }

        public decimal TotalPrice { get; set; }
    }
}