using System.Collections.Generic;
using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices.AdminSecurityService.Models.Output
{
    public class GetUsersResult : PagedResult<AdminUser>
    {
        public GetUsersResult(IEnumerable<AdminUser> result, int totalCount, PagingSettings paging)
            : base(result, totalCount, paging)
        {
        }
    }
}
