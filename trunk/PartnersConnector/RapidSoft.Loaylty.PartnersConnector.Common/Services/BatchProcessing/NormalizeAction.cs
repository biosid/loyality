using System.Linq;

using RapidSoft.Extensions;

using RapidSoft.Loaylty.PartnersConnector.Interfaces;
using RapidSoft.Loaylty.PartnersConnector.Queue.Entities;
using RapidSoft.Loaylty.PartnersConnector.Queue.Repository;

namespace RapidSoft.Loaylty.PartnersConnector.Services.BatchProcessing
{
    using RapidSoft.Loaylty.ProductCatalog.WsClients.CatalogAdminService;

    internal class NormalizeAction : IBatchAction
    {
        private readonly ICommitOrderQueueItemRepository _queueItemRepository;
        private readonly ICatalogAdminServiceProvider _catalogProvider;

        public NormalizeAction(ICatalogAdminServiceProvider catalogProvider, ICommitOrderQueueItemRepository queueItemRepository)
        {
            catalogProvider.ThrowIfNull("catalogProvider");
            queueItemRepository.ThrowIfNull("queueItemRepository");
            this._catalogProvider = catalogProvider;
            this._queueItemRepository = queueItemRepository;
        }

        public Batch Execute(Batch batch)
        {
            var groupBy = batch.GroupBy(key => key.Order.OrderId);

            var retBatch = new Batch(batch.PartnerId);

            foreach (var group in groupBy)
            {
                if (group.Count() > 1)
                {
                    const string Error = "Заказ был несколько раз поставлен в очередь, что не допустимо";
                    var commitment = new PartnerOrderCommitment
                                         {
                                             IsConfirmed = false,
                                             OrderId = int.Parse(group.Key),
                                             Reason = Error
                                         };
                    this._catalogProvider.PartnerCommitOrder(batch.PartnerId, new[] { commitment });
                    var queueItems = group.Select(
                        x =>
                            {
                                var item = x.CommitOrderQueueItem;
                                item.ProcessStatus = Statuses.ConnectorError;
                                item.ProcessStatusDescription = Error;
                                return item;
                            }).ToArray();

                    this._queueItemRepository.Save(queueItems);
                }
                else
                {
                    retBatch.Add(group.First());
                }
            }

            return retBatch;
        }
    }
}