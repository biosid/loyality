using System.Collections.Generic;
using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices.ActionsManagement.Actions.Models.Outputs
{
    public class ActionsSearchResult : PagedResult<Action>
    {
        public ActionsSearchResult(IEnumerable<Action> result, int totalCount, PagingSettings paging)
            : base(result, totalCount, paging)
        {
        }
    }
}
