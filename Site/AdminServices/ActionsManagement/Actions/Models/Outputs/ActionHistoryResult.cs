using System.Collections.Generic;
using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices.ActionsManagement.Actions.Models.Outputs
{
    public class ActionHistoryResult : PagedResult<HistoryRecord>
    {
        public ActionHistoryResult(IEnumerable<HistoryRecord> result, int totalCount, PagingSettings paging)
            : base(result, totalCount, paging)
        {
            
        }
    }
}
