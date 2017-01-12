using System.Collections.Generic;

using RapidSoft.Loaylty.PartnersConnector.Queue.Entities;

namespace RapidSoft.Loaylty.PartnersConnector.Queue.Repository
{
    public interface ICommitOrderQueueItemRepository
    {
        CommitOrderQueueItem Save(CommitOrderQueueItem item);

        IList<CommitOrderQueueItem> Save(IList<CommitOrderQueueItem> items);

        IList<CommitOrderQueueItem> GetByPartnerId(int partnerId, Statuses? status = null);
    }
}