using System.Collections.Generic;
using Vtb24.Site.Services.Models;

namespace Vtb24.Site.Services.Processing.Models
{
    public class ProcessingOperationsHistoryResult : PagedResult<ProcessingOperationInfo>
    {
        public ProcessingOperationsHistoryResult(IEnumerable<ProcessingOperationInfo> result, decimal outcome, decimal income, int totalCount, PagingSettings paging)
            : base(result, totalCount, paging)
        {
            TotalOutcome = outcome;
            TotalIncome = income;
        }

        public decimal TotalOutcome { get; set; }
        public decimal TotalIncome { get; set; }
    }
}
