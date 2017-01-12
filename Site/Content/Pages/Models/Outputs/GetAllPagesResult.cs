using System.Collections.Generic;
using Vtb24.Site.Content.Models;

namespace Vtb24.Site.Content.Pages.Models.Outputs
{
    public class GetAllPagesResult : PagedResult<Page>
    {
        public GetAllPagesResult(IEnumerable<Page> result, int totalCount, PagingSettings paging)
            : base(result, totalCount, paging)
        {
        }
    }
}
