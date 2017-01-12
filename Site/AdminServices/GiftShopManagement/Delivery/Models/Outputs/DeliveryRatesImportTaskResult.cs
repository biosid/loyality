using System.Collections.Generic;
using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Delivery.Models.Outputs
{
    public class DeliveryRatesImportTaskResult : PagedResult<DeliveryRatesImportTask>
    {
        public DeliveryRatesImportTaskResult(IEnumerable<DeliveryRatesImportTask> result, int totalCount, PagingSettings paging)
            : base(result, totalCount, paging)
        {
        }
    }
}
