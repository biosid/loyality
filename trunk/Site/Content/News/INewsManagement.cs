using Vtb24.Site.Content.Models;
using Vtb24.Site.Content.News.Management.Models.Inputs;
using Vtb24.Site.Content.News.Models;
using Vtb24.Site.Content.News.Models.Outputs;
using Vtb24.Site.Content.Snapshots.Models;

namespace Vtb24.Site.Content.News
{
    public interface INewsManagement
    {
        void Create(UpdateNewsMessageOption option);

        void Edit(long id, UpdateNewsMessageOption option);

        void Delete(long id);
        
        void Publish(long[] ids, bool publish);

        NewsMessage GetById(long id);

        Snapshot<NewsMessage>[] GetAllHistoryById(long id);
        
        Snapshot<NewsMessage> GetFromHistoryBySnapshotId(string id);
        
        GetNewsMessagesResult GetNewsMessages(GetNewsMessagesFilter filter, PagingSettings paging);
    }
}