namespace RapidSoft.Loaylty.PartnersConnector.Services.BatchProcessing
{
    using System;
    using System.Globalization;
    using System.Linq;

    using RapidSoft.Extensions;

    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.PartnersConnector.Interfaces;
    using RapidSoft.Loaylty.PartnersConnector.Queue.Entities;
    using RapidSoft.Loaylty.PartnersConnector.Queue.Repository;
    using RapidSoft.Loaylty.ProductCatalog.WsClients.CatalogAdminService;

    /// <summary>
    /// Операция над пакетом заказов по уведомлению каталога порадков об подтверждении заказа у партнера.
    /// </summary>
    internal class NotifyCatalogAction : IBatchAction
    {
        private readonly ICommitOrderQueueItemRepository _queueItemRepository;
        private readonly ICatalogAdminServiceProvider _catalogProvider;
        private readonly ILog log = LogManager.GetLogger(typeof (NotifyCatalogAction));

        public NotifyCatalogAction(ICatalogAdminServiceProvider catalogProvider, ICommitOrderQueueItemRepository queueItemRepository)
        {
            catalogProvider.ThrowIfNull("catalogProvider");
            queueItemRepository.ThrowIfNull("queueItemRepository");
            this._catalogProvider = catalogProvider;
            this._queueItemRepository = queueItemRepository;
        }

        /// <summary>
        /// Отправка пакета в каталог.
        /// </summary>
        /// <param name="batch">
        /// Пакет жлементов очереди.
        /// Элементы очереди не имеющие ответа от партнера игнорируются.
        /// </param>
        /// <returns>
        /// Обработанный пакет.
        /// В отличие от отправляемого пакета содержит только те элементы очереди, по которым получен ответ от каталога.
        /// </returns>
        public Batch Execute(Batch batch)
        {
            try
            {
                var commitments = batch.GetCommitments().ToArray();

                if (commitments.Length == 0)
                {
                    // NOTE: Возвращаем пустой набор данных
                    return new Batch(batch.PartnerId);
                }

                var partnerOrders = this._catalogProvider.PartnerCommitOrder(batch.PartnerId, commitments);

                if (!partnerOrders.Success)
                {
                    // NOTE: Если получили "общую" ошибку от каталога, то переводим все заказы в ошибку
                    log.Error("Получена ошибку выполнения операции изменения статуса в каталоге: " + partnerOrders.ResultDescription);

                    var queueItems = batch.Select(
                        x =>
                            {
                                var item = x.CommitOrderQueueItem;
                                item.ProcessStatus = Statuses.CatalogPartnerError;
                                item.ProcessStatusDescription = partnerOrders.ResultDescription;
                                return item;
                            }).ToArray();

                    this._queueItemRepository.Save(queueItems);

                    // NOTE: и возвращаем пустой набор данных
                    return new Batch(batch.PartnerId);
                }

                var results = partnerOrders.ChangeExternalOrderStatusResults ?? new ChangeExternalOrderStatusResult[0];

                var retBatch = new Batch(batch.PartnerId);

                foreach (var batchItem in batch)
                {
                    var result = results.SingleOrDefault(x => SelectResult(x, batchItem));

                    if (result == null)
                    {
                        var error = string.Format("Ответ партнера не содержит данных по отправленному заказу {0}", batchItem.Order.OrderId);

                        var queueItem = batchItem.CommitOrderQueueItem;

                        queueItem.ProcessStatus = Statuses.PartnerConnectError;
                        queueItem.ProcessStatusDescription = error;

                        this._queueItemRepository.Save(queueItem);
                    }
                    else
                    {
                        batchItem.ChangeOrderStatusResult = result;
                        retBatch.Add(batchItem);
                    }
                }

                return retBatch;
            }
            catch (Exception ex)
            {
                // NOTE: Если поймали любую ошибку, то переводим все заказы в ошибку
                const string Error = "Ошибка обновления статуса в каталоге";
                log.Error(Error, ex);

                var exception = ex.ToString();

                var queueItems = batch.Select(
                    x =>
                        {
                            var item = x.CommitOrderQueueItem;
                            item.ProcessStatus = Statuses.CatalogPartnerError;
                            item.ProcessStatusDescription = exception;
                            return item;
                        }).ToArray();

                this._queueItemRepository.Save(queueItems);

                // NOTE: и возвращаем пустой набор данных
                return new Batch(batch.PartnerId);
            }
        }

        /// <summary>
        /// Селектор результатов и элементов очереди для сопоставления ответов из каталога и набором элементов очереди.
        /// </summary>
        private static bool SelectResult(ChangeExternalOrderStatusResult x, BatchItem batchItem)
        {
            var byOrderId = x.OrderId.HasValue && (x.OrderId.Value.ToString(CultureInfo.InvariantCulture) == batchItem.Order.OrderId);
            var byExternalOrderId = !x.OrderId.HasValue && (x.ExternalOrderId == batchItem.CommitOrderResult.InternalOrderId);
            return byOrderId || byExternalOrderId;
        }
    }
}