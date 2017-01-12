using System.Collections.Generic;
using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices.AdminSecurityService.Models.Output
{
    public class GetGroupsResult : PagedResult<AdminGroup>
    {
        public GetGroupsResult(IEnumerable<AdminGroup> result, int totalCount, PagingSettings paging)
            : base(result, totalCount, paging)
        {
        }
    }
}
