using Vtb24.Site.Services.AdvancePayment.Models.Outputs;

namespace Vtb24.Site.Services.AdvancePayment
{
    internal static class MappingsFromService
    {
        public static PaymentFormParameters ToPaymentFormParameters(BankConnectorPaymentService.PaymentFormParameters original)
        {
            return new PaymentFormParameters
            {
                Url = original.Url,
                Method = original.Method,
                Parameters = original.Parameters
            };
        }
    }
}
