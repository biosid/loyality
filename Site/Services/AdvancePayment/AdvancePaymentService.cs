using Vtb24.Site.Services.AdvancePayment.Models.Exceptions;
using Vtb24.Site.Services.BankConnectorPaymentService;
using PaymentFormParameters = Vtb24.Site.Services.AdvancePayment.Models.Outputs.PaymentFormParameters;

namespace Vtb24.Site.Services.AdvancePayment
{
    public class AdvancePaymentService : IAdvancePaymentService
    {
        public PaymentFormParameters GetPaymentFormParameters(string clientId, int orderId, decimal amount, string returnUrlSuccess, string returnUrlFail)
        {
            var parameters = new PaymentFormRequest
            {
                ClientId = clientId,
                OrderId = orderId,
                Amount = amount,
                ReturnUrlSuccess = returnUrlSuccess,
                ReturnUrlFail = returnUrlFail
            };

            using (var service = new PaymentServiceClient())
            {
                var response = service.GetPaymentFormParameters(parameters);

                AssertResponse(response.ResultCode, response.Error);

                return MappingsFromService.ToPaymentFormParameters(response.Result);
            }
        }

        public bool IsPaymentAuthorized(int orderId)
        {
            using (var service = new PaymentServiceClient())
            {
                var response = service.IsPaymentAuthorized(orderId);

                AssertResponse(response.ResultCode, response.Error);

                return response.Result;
            }
        }

        private static void AssertResponse(int code, string message)
        {
            switch (code)
            {
                case 0:
                    return;
            }

            throw new AdvancePaymentServiceException(code, message);
        }
    }
}
