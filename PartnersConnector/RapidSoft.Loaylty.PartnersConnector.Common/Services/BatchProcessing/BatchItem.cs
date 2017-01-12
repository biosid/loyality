namespace RapidSoft.Loaylty.PartnersConnector.Services.BatchProcessing
{
    using System.Text;

    using AutoMapper;

    using Common.DTO.CommitOrder;

    using Queue.Entities;

    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.ProductCatalog.WsClients.CatalogAdminService;

    using Order = Common.DTO.CommitOrder.Order;

    internal class BatchItem
    {
        private Order order;

        public BatchItem(CommitOrderQueueItem commitOrderQueueItem)
        {
            this.CommitOrderQueueItem = commitOrderQueueItem;
        }

        public CommitOrderQueueItem CommitOrderQueueItem { get; set; }

        public Order Order 
        {
            get
            {
                if (this.order != null)
                {
                    return this.order;
                }

                if (this.CommitOrderQueueItem == null)
                {
                    return null;
                }

                // NOTE: Так как сущность храниться в MS SQL базе в поле с типом xml который не признает utf-8, сериализовано в utf-16 (Encoding.Unicode)
                this.order = this.CommitOrderQueueItem.Order.Deserialize<Order>(Encoding.Unicode);
                return this.order;
            }
        }

        public CommitOrderResult CommitOrderResult { get; set; }

        public PartnerOrderCommitment PartnerOrderCommitment
        {
            get
            {
                return this.CommitOrderResult != null
                           ? Mapper.Map<CommitOrderResult, PartnerOrderCommitment>(this.CommitOrderResult)
                           : null;
            }
        }

        public ChangeOrderStatusResult ChangeOrderStatusResult { get; set; }
    }
}