using Vtb24.Site.Services.AdvancePayment.Models.Outputs;

namespace Vtb24.Site.Services
{
    public interface IAdvancePaymentService
    {
        PaymentFormParameters GetPaymentFormParameters(string clientId, int orderId, decimal amount, string returnUrlSuccess, string returnUrlFail);

        bool IsPaymentAuthorized(int orderId);
    }
}
