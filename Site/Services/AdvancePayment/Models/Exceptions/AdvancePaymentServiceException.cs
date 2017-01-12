using Vtb24.Site.Services.Models.Exceptions;

namespace Vtb24.Site.Services.AdvancePayment.Models.Exceptions
{
    public class AdvancePaymentServiceException : ComponentException
    {
        public AdvancePaymentServiceException(int resultCode, string codeDescription)
            : base("Оплата картой", resultCode, codeDescription)
        {
        }
    }
}
