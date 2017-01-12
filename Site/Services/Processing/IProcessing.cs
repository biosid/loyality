using Vtb24.Site.Services.Models;
using Vtb24.Site.Services.Processing.Models;
using Vtb24.Site.Services.Processing.Models.Inputs;

namespace Vtb24.Site.Services.Processing
{
    public interface IProcessing
    {
        decimal GetBalance(GetBalanceParameters parameters);

        ProcessingOperationsHistoryResult GetOperationsHistory(GetOperationHistoryParameters parameters, PagingSettings paging);
    }
}
