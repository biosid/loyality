namespace RapidSoft.Loaylty.PartnersConnector.Services.BatchProcessing
{
    using System.Collections.Generic;
    using System.Linq;

    using ProductCatalog.WsClients.CatalogAdminService;

    using Queue.Entities;

    using Order = Common.DTO.CommitOrder.Order;

    internal class Batch : List<BatchItem>
    {
        public int PartnerId { get; private set; }

        public Batch(int partnerId, IEnumerable<BatchItem> collection)
            : base(collection)
        {
            this.PartnerId = partnerId;
        }

        public Batch(int partnerId, IEnumerable<CommitOrderQueueItem> queueItems)
            : base(queueItems.Select(x => new BatchItem(x)))
        {
            this.PartnerId = partnerId;
        }

        public Batch(int partnerId)
        {
            this.PartnerId = partnerId;
        }

        public IEnumerable<Order> GetOrders()
        {
            return this.Select(x => x.Order);
        }

        public IEnumerable<PartnerOrderCommitment> GetCommitments()
        {
            return this.Where(x => x.CommitOrderResult != null).Select(x => x.PartnerOrderCommitment);
        }
    }
}