using System.Linq;

using RapidSoft.Extensions;

using RapidSoft.Loaylty.PartnersConnector.Queue.Entities;
using RapidSoft.Loaylty.PartnersConnector.Queue.Repository;

namespace RapidSoft.Loaylty.PartnersConnector.Services.BatchProcessing
{
    internal class MarkAsCompliteAction : IBatchAction
    {
        private readonly ICommitOrderQueueItemRepository _queueItemRepository;

        public MarkAsCompliteAction(ICommitOrderQueueItemRepository queueItemRepository)
        {
            queueItemRepository.ThrowIfNull("queueItemRepository");
            this._queueItemRepository = queueItemRepository;
        }

        public Batch Execute(Batch batch)
        {
            var queueItems = batch.Select(GetUpdatedItem).ToList();

            if (queueItems.Count == 0)
            {
                return new Batch(batch.PartnerId);
            }

            this._queueItemRepository.Save(queueItems);

            // NOTE: Всегда возвращаем пустой пакет, так как это последний шаг.
            return new Batch(batch.PartnerId);
        }

        public static CommitOrderQueueItem GetUpdatedItem(BatchItem i)
        {
            string resultDescription;

            if (i.ChangeOrderStatusResult == null)
            {
                resultDescription = "Не известен";
            }
            else
            {
                resultDescription = i.ChangeOrderStatusResult.Success ? "Успешно" : "Не успешно";
            }

            var item = i.CommitOrderQueueItem;

            item.ProcessStatus = Statuses.Processed;
            item.ProcessStatusDescription = resultDescription;

            return item;
        }
    }
}