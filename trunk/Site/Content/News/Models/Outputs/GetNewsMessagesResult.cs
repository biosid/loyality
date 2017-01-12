using System.Collections.Generic;
using Vtb24.Site.Content.Models;

namespace Vtb24.Site.Content.News.Models.Outputs
{
    public class GetNewsMessagesResult : PagedResult<NewsMessage>
    {
        public GetNewsMessagesResult(IEnumerable<NewsMessage> result, int totalCount, PagingSettings paging)
            : base(result, totalCount, paging)
        {
        }
    }
}
