using System.Collections.Generic;
using RapidSoft.VTB24.BankConnector.Acquiring.Uniteller.Models.Inputs;
using RapidSoft.VTB24.BankConnector.Acquiring.Uniteller.Models.Outputs;

namespace RapidSoft.VTB24.BankConnector.Acquiring.Uniteller
{
    public interface IUnitellerAcquiring
    {
        string PaymentFormUrl { get; }

        Dictionary<string, string> GetPaymentFormParameters(UnitellerPayParameters payParameters);

        UnitellerPaymentInfo GetPaymentInfo(string shopId, string orderId);

        void ConfirmPayment(string shopId, string billNumber);

        void CancelPayment(string shopId, string billNumber);
    }
}
