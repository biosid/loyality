namespace RapidSoft.Loaylty.ProductCatalog.Services
{
    using System;
    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.ProductCatalog.API.OutputResults;
    using RapidSoft.Loaylty.ProductCatalog.Interfaces;
    using RapidSoft.VTB24.BankConnector.WsClients.PaymentService;

    public class AdvancePaymentProvider : IAdvancePaymentProvider
    {
        private readonly ILog log = LogManager.GetLogger(typeof(AdvancePaymentProvider));

        public SimpleBankConnectorResponse ConfirmPayment(int orderId)
        {
            try
            {
                log.DebugFormat("Подтверждение платежа картой по заказу {0}", orderId);

                using (var service = new PaymentServiceClient())
                {
                    var response = service.ConfirmPayment(orderId);

                    log.DebugFormat("Получен ответ на подтверждения платежа картой по заказу {0}: {1}", orderId, response.Serialize());

                    return response;
                }
            }
            catch (Exception e)
            {
                log.Error("Ошибка подтверждения платежа", e);

                return new SimpleBankConnectorResponse
                {
                    Success = false,
                    ResultCode = ResultCodes.UNKNOWN_ERROR,
                    Error = "Ошибка подтверждения платежа"
                };
            }
        }

        public SimpleBankConnectorResponse CancelPayment(int orderId)
        {
            try
            {
                log.DebugFormat("Отмена платежа картой по заказу {0}", orderId);

                using (var service = new PaymentServiceClient())
                {
                    var response = service.CancelPayment(orderId);

                    log.DebugFormat("Получен ответ на отмену платежа картой по заказу {0}: {1}", orderId, response.Serialize());

                    return response;
                }
            }
            catch (Exception e)
            {
                log.Error("Ошибка отмены платежа", e);

                return new SimpleBankConnectorResponse
                {
                    Success = false,
                    ResultCode = ResultCodes.UNKNOWN_ERROR,
                    Error = "Ошибка отмены платежа"
                };
            }
        }
    }
}
