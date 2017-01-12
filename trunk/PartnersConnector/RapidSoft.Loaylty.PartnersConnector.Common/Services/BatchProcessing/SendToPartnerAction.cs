namespace RapidSoft.Loaylty.PartnersConnector.Services.BatchProcessing
{
    using System;
    using System.Linq;
    using System.Text;

    using Common.DTO.CommitOrder;

    using Loyalty.Security;

    using RapidSoft.Extensions;

    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.PartnersConnector.Queue.Entities;
    using RapidSoft.Loaylty.PartnersConnector.Queue.Repository;
    
    /// <summary>
    /// Операция над пакетом заказов по отправки заказов клиенту с получением пакета успешно отправленных заказов.
    /// </summary>
    internal class SendToPartnerAction : IBatchAction
    {
        private readonly ICommitOrderQueueItemRepository queueItemRepository;
        private readonly ITextMessageDispatcher messageDispatcher;
        
        private readonly Uri uri;

        private string requestDataXml;

        private string responseDataXml;

        private readonly ILog log = LogManager.GetLogger(typeof (SendToPartnerAction));

        public SendToPartnerAction(Uri uri, ITextMessageDispatcher messageDispatcher, ICommitOrderQueueItemRepository queueItemRepository)
        {
            uri.ThrowIfNull("uri");
            messageDispatcher.ThrowIfNull("messageDispatcher");
            queueItemRepository.ThrowIfNull("queueItemRepository");
            this.uri = uri;
            this.messageDispatcher = messageDispatcher;
            this.queueItemRepository = queueItemRepository;
        }

        /// <summary>
        /// Отправка пакета партнеру.
        /// </summary>
        /// <param name="batch">
        /// Отправляемый пакет.
        /// </param>
        /// <returns>
        /// Обработанный пакет.
        /// В отличие от отправляемого пакета содержит только те элементы очереди, по которым получен ответ от партнера.
        /// </returns>
        public Batch Execute(Batch batch)
        {
            try
            {
                var orders = batch.GetOrders().ToArray();

                var message = new CommitOrdersMessage { Orders = orders };

                this.requestDataXml = message.Serialize(Encoding.UTF8);
                log.InfoFormat("Отправка запроса {0} в систему партнера, body: {1}", this.uri, this.requestDataXml);
                this.responseDataXml = this.messageDispatcher.Send(this.uri, this.requestDataXml);
                log.InfoFormat("Полученный ответ: {0}", this.responseDataXml);
                var commitResult = this.responseDataXml.Deserialize<CommitOrdersResult>(Encoding.UTF8);

                var ordersResult = commitResult.Orders ?? new CommitOrderResult[0];

                var retBatch = new Batch(batch.PartnerId);

                foreach (var batchItem in batch)
                {
                    var result = ordersResult.SingleOrDefault(x => x.OrderId == batchItem.Order.OrderId);

                    if (result == null)
                    {
                        var error = string.Format("Ответ партнера не содержит данных по отправленному заказу {0}", batchItem.Order.OrderId);

                        var queueItem = batchItem.CommitOrderQueueItem;

                        queueItem.ProcessStatus = Statuses.PartnerConnectError;
                        queueItem.ProcessStatusDescription = error;

                        this.queueItemRepository.Save(queueItem);
                    }
                    else
                    {
                        batchItem.CommitOrderResult = result;
                        retBatch.Add(batchItem);
                    }
                }

                return retBatch;
            }
            catch (Exception ex)
            {
                // NOTE: Если поймали любую ошибку, то переводим все заказы в ошибку
                var error = string.Format(
                    "Ошибка взаимодействия с партнером. Url {0}, Запрос: {1}, Ответ: {2}",
                    this.uri,
                    this.requestDataXml,
                    this.responseDataXml);
                log.Error(error, ex);

                var exception = ex.ToString();

                var queueItems = batch.Select(
                    x =>
                    {
                        var item = x.CommitOrderQueueItem;
                        item.ProcessStatus = Statuses.CatalogPartnerError;
                        item.ProcessStatusDescription = exception;
                        return item;
                    }).ToArray();

                this.queueItemRepository.Save(queueItems);

                // NOTE: и возвращаем пустой набор данных
                return new Batch(batch.PartnerId);
            }
        }
    }
}