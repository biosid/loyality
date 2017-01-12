namespace RapidSoft.Loaylty.ProductCatalog.Import
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RapidSoft.Etl.Logging;
    using RapidSoft.Etl.Runtime;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories;
    using RapidSoft.Loaylty.ProductCatalog.Interfaces;
    using RapidSoft.Loaylty.ProductCatalog.Services;
    using RapidSoft.Loaylty.ProductCatalog.Settings;

    public class KladrCodeChecker
    {
        private const int InvalidKladrCodeStatus = -9;

        private readonly IDeliveryRatesRepository deliveryRatesRepository;
        private readonly IGeoPointProvider geoPointProvider;

        public KladrCodeChecker(IDeliveryRatesRepository deliveryRatesRepository = null, IGeoPointProvider geoPointProvider = null)
        {
            this.deliveryRatesRepository = deliveryRatesRepository ?? new DeliveryRatesRepository();
            this.geoPointProvider = geoPointProvider ?? new GeoPointProvider();
        }

        public static void CheckKladrByGeobase(EtlContext context, IEtlLogger logger)
        {
            var checker = new KladrCodeChecker();
            checker.Execute(context, logger);
        }

        public void Execute(EtlContext context, IEtlLogger logger)
        {
            var batchSize = ApiSettings.BatchSizeValidatingKladrByGeobase;
            var processed = 0;
            var totalNotExists = new List<string>();

            var page = this.deliveryRatesRepository.GetKladrCodesFromBuffer(context.EtlSessionId, processed, batchSize);

            while (page.Count > 0)
            {
                var exists = this.geoPointProvider.GetExistKladrCodes(page);

                var notExists = page.Except(exists).ToList();

                if (notExists.Count > 0)
                {
                    this.deliveryRatesRepository.SetDeliveryBufferStatusByKladr(context.EtlSessionId, notExists, InvalidKladrCodeStatus);
                    totalNotExists.AddRange(notExists);
                }

                processed += page.Count;
                page = this.deliveryRatesRepository.GetKladrCodesFromBuffer(context.EtlSessionId, processed, batchSize);
            }

            if (totalNotExists.Count > 0)
            {
                LogNotExistKladrCodes(context, logger, totalNotExists);
            }
        }

        private void LogNotExistKladrCodes(EtlContext context, IEtlLogger logger, List<string> notExistKladrCodes)
        {
            const string MessageFormat = "По геобазе не найдено {0} КЛАДР: {1}";
            var message = string.Format(MessageFormat, notExistKladrCodes.Count, string.Join(",", notExistKladrCodes));

            var now = DateTime.Now;
            var etlMessage = new EtlMessage
                                 {
                                     EtlPackageId = context.EtlPackageId,
                                     EtlSessionId = context.EtlSessionId,
                                     MessageType = EtlMessageType.Warning,
                                     LogDateTime = now,
                                     LogUtcDateTime = now.ToUniversalTime(),
                                     Text = message
                                 };
            logger.LogEtlMessage(etlMessage);
        }
    }
}