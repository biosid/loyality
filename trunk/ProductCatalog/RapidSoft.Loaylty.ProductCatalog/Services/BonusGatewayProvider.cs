namespace RapidSoft.Loaylty.ProductCatalog.Services
{
    using System;
    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.BonusGateway.BonusGateway;
    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.ProductCatalog.API.OutputResults;
    using RapidSoft.Loaylty.ProductCatalog.Interfaces;
    using RapidSoft.Loaylty.ProductCatalog.Settings;

    public class BonusGatewayProvider : IBonusGatewayProvider
    {
        private readonly ILog log = LogManager.GetLogger(typeof(BonusGatewayProvider));

        public RollbackPointsResponse CancelPayment(string paymentRequestId)
        {
            log.DebugFormat("Отмена платежа с requestId = \"{0}\"", paymentRequestId);

            var requestId = "cancel_" + paymentRequestId;

            var parameters = new RollbackPointsRequest
            {
                OriginalRequestId = paymentRequestId,
                RequestDateTime = DateTime.Now,
                RequestId = requestId,
                PartnerId = ApiSettings.BonusGatewayPartnerId,
                PosId = ApiSettings.BonusGatewayPosId,
                TerminalId = ApiSettings.BonusGatewayTerminalId
            };

            try
            {
                var retVal =
                    WebClientCaller.CallService<BonusGatewayClient, RollbackPointsResponse>(
                        cl => cl.RollbackPoints(parameters));

                log.Debug("Полученный ответ на отмену платежа: " + retVal.Serialize());
                return retVal;
            }
            catch (Exception e)
            {
                log.Error("Ошибка отмены платежа", e);
                return new RollbackPointsResponse
                {
                    Status = (int)ResultCodes.UNKNOWN_ERROR,
                    Error = "Ошибка отмены платежа"
                };
            }
        }
    }
}
