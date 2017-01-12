using Vtb24.Site.Content.Models;
using Vtb24.Site.Content.News.Models;
using Vtb24.Site.Content.News.Models.Outputs;

namespace Vtb24.Site.Content.News
{
    public interface INews
    {
        GetNewsMessagesResult GetNewsMessages(string[] segments, PagingSettings paging);

        NewsMessage GetById(long id);
    }
}
