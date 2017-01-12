using System.Collections.Generic;
using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Products.Models.Outputs
{
    public class ProductsSearchResult : PagedResult<Product>
    {
        public ProductsSearchResult(IEnumerable<Product> result, int totalCount, PagingSettings paging)
            : base(result, totalCount, paging)
        {
        }
    }
}
