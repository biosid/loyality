using RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities;

namespace RapidSoft.Loaylty.PartnersConnector.Services.Providers
{
    using System;
    using System.Globalization;

    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.BonusGateway.BonusGateway;
    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.PartnersConnector.Interfaces;
    using RapidSoft.Loaylty.PartnersConnector.Interfaces.Settings;
    using RapidSoft.Loaylty.ProductCatalog.WsClients;

    public class BonusGatewayProvider : IBonusGatewayProvider
    {
        private readonly ILog log = LogManager.GetLogger(typeof (BonusGatewayProvider));

        public RollbackPointsResponse CancelPayment(int partnerId, string orderId)
        {
            log.InfoFormat("Отмена платежа по partnerId = \"{0}\" и orderId = \"{1}\"", partnerId, orderId);

            var requestId = Guid.NewGuid().ToString();

            var parameters = new RollbackPointsRequest
                                 {
                                     PartnerId = partnerId.ToString(CultureInfo.InvariantCulture),
                                     OriginalRequestId = orderId,
                                     RequestDateTime = DateTime.Now,
                                     RequestId = requestId,
									 PosId = PartnerConnectionsConfig.PosId,
									 TerminalId = PartnerConnectionsConfig.TerminalId
                                 };

            try
            {
                var retVal =
                    WebClientCaller.CallService<BonusGatewayClient, RollbackPointsResponse>(
                        cl => cl.RollbackPoints(parameters));

                log.Info("Полученный ответ на отмену платежа: " + retVal.Serialize());
                return retVal;
            }
            catch (Exception e)
            {
                log.Error("Ошибка отмены оплаты", e);
                return new RollbackPointsResponse
                {
                    Status = (int)ResultCodes.UnknowCancelPaymentError,
                    Error = "Ошибка отмены оплаты"
                };
            }
        }
    }
}