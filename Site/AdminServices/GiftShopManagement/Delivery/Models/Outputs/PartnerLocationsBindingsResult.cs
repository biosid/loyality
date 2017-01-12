using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Delivery.Models.Outputs
{
    public class PartnerLocationsBindingsResult : PagedResult<PartnerLocation>
    {
        public PartnerLocationsBindingsResult(IEnumerable<PartnerLocation> result, int totalCount, PagingSettings paging)
            : base(result, totalCount, paging)
        {
        }
    }
}
