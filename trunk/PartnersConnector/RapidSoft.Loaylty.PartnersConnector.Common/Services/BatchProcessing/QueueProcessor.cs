namespace RapidSoft.Loaylty.PartnersConnector.Services.BatchProcessing
{
    using System;
    using System.Configuration;
    using System.Linq;

    using AutoMapper;

    using Common.DTO.CommitOrder;

    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.PartnersConnector.Interfaces;
    using RapidSoft.Loaylty.PartnersConnector.Queue.Entities;
    using RapidSoft.Loaylty.PartnersConnector.Queue.Repository;
    using RapidSoft.Loaylty.PartnersConnector.Services.Providers;
    using RapidSoft.Loaylty.ProductCatalog.WsClients.CatalogAdminService;
    using RapidSoft.Loyalty.Security;

    using CheckOrderResult = RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities.CheckOrderResult;

    public class QueueProcessor : IQueueProcessor
    {
        private readonly ICatalogAdminServiceProvider catalogProvider;
        private readonly ITextMessageDispatcher messageDispatcher;
        private readonly ICommitOrderQueueItemRepository queueItemRepository;
        private readonly ILog log = LogManager.GetLogger(typeof (QueueProcessor));

        static QueueProcessor()
        {
            Mapper.CreateMap<CommitOrderResult, PartnerOrderCommitment>()
                  .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => int.Parse(src.OrderId)));

            Mapper.CreateMap<Common.DTO.CheckOrder.CheckOrderResult, CheckOrderResult>();
        }

        public QueueProcessor(
            ICatalogAdminServiceProvider catalogProvider = null,
            ITextMessageDispatcher messageDispatcher = null,
            ICommitOrderQueueItemRepository queueItemRepository = null)
        {
            this.catalogProvider = catalogProvider ?? new CatalogAdminServiceProvider();
            this.queueItemRepository = queueItemRepository ?? new CommitOrderQueueItemRepository();

            if (messageDispatcher == null)
            {
                var thumbprint = ConfigurationManager.AppSettings["PartnersConnectorThumbprint"];
                log.InfoFormat("Отпечаток сертификата: {0}", thumbprint);

                var x509Certificate = new StoreCertificateProvider().GetByThumbprint(thumbprint);
                log.InfoFormat("Сертификат: \r\n{0}", x509Certificate);

                this.messageDispatcher = new TextOverSslMessageDispatcher(x509Certificate);
            }
            else
            {
                this.messageDispatcher = messageDispatcher;
            }
        }

        public void Execute(int partnerId)
        {
            try
            {
                var connection = this.catalogProvider.GetPartnerSettings(partnerId);

                if (connection == null)
                {
                    var error = string.Format(
                        "Для партнера с идентификатором {0} не заданы настройки подключения", partnerId);
                    log.ErrorFormat(error);
                    throw new ApplicationException(error);
                }

                var uri = new Uri(connection.BatchOrderConfirmation);

                var queueItems = this.queueItemRepository.GetByPartnerId(partnerId, Statuses.NotProcessed);

                if (queueItems == null || queueItems.Count == 0)
                {
                    // NOTE: Нету заказов.
                    return;
                }

                var commitOrderItems = queueItems.Select(x => new BatchItem(x));

                var batch = new Batch(partnerId, commitOrderItems);

                var normalizeAction = new NormalizeAction(this.catalogProvider, this.queueItemRepository);
                batch = normalizeAction.Execute(batch);

                var sendAction = new SendToPartnerAction(uri, this.messageDispatcher, this.queueItemRepository);
                batch = sendAction.Execute(batch);

                var notifyCatalogAction = new NotifyCatalogAction(this.catalogProvider, this.queueItemRepository);
                batch = notifyCatalogAction.Execute(batch);

                var markAsCompliteAction = new MarkAsCompliteAction(this.queueItemRepository);
                markAsCompliteAction.Execute(batch);
            }
            catch (Exception ex)
            {
                var error = "Ошибка обработки очереди для партнера: " + partnerId;
                log.Error(error, ex);
            }
        }
    }
}