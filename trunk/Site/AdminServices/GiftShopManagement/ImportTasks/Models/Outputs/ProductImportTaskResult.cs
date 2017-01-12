using System.Collections.Generic;
using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.ImportTasks.Models.Outputs
{
    public class ProductImportTaskResult : PagedResult<ProductImportTask>
    {
        public ProductImportTaskResult(IEnumerable<ProductImportTask> result, int totalCount, PagingSettings paging)
            : base(result, totalCount, paging)
        {
        }
    }
}
