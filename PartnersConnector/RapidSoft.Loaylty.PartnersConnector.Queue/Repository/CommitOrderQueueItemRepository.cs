using System.Collections.Generic;
using System.Data;
using System.Linq;

using RapidSoft.Loaylty.PartnersConnector.Queue.Entities;

namespace RapidSoft.Loaylty.PartnersConnector.Queue.Repository
{
    using RapidSoft.Extensions;

    public class CommitOrderQueueItemRepository : ICommitOrderQueueItemRepository
    {
        public CommitOrderQueueItem Save(CommitOrderQueueItem item)
        {
            item.ThrowIfNull("item");

            using (var ctx = new QueueContext())
            {
                this.InternalSave(item, ctx);

                ctx.SaveChanges();

                return item;
            }
        }

        public IList<CommitOrderQueueItem> Save(IList<CommitOrderQueueItem> items)
        {
            items.ThrowIfNull("items");

            using (var ctx = new QueueContext())
            {
                foreach (var item in items)
                {
                    this.InternalSave(item, ctx);
                }

                ctx.SaveChanges();

                return items;
            }
        }

        public IList<CommitOrderQueueItem> GetByPartnerId(int partnerId, Statuses? status = null)
        {
            using (var ctx = new QueueContext())
            {
                var query = ctx.CommitOrderQueueItems.Where(x => x.PartnerId == partnerId);

                if (status.HasValue)
                {
                    query = query.Where(x => x.ProcessStatus == status.Value);
                }

                var retVal = query.ToList();

                return retVal;
            }
        }
        
        private void InternalSave(CommitOrderQueueItem item, QueueContext ctx)
        {
            if (item.Id == default(long))
            {
                ctx.CommitOrderQueueItems.Add(item);
            }
            else
            {
                ctx.CommitOrderQueueItems.Attach(item);
                ctx.Entry(item).State = EntityState.Modified;
            }
        }
    }
}