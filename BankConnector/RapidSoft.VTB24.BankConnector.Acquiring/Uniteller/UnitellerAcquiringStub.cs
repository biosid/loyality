namespace RapidSoft.VTB24.BankConnector.Acquiring.Uniteller
{
    using System;
    using System.Collections.Generic;
    using RapidSoft.VTB24.BankConnector.Acquiring.Uniteller.Models.Inputs;
    using RapidSoft.VTB24.BankConnector.Acquiring.Uniteller.Models.Outputs;

    public class UnitellerAcquiringStub : IUnitellerAcquiring
    {
        public string PaymentFormUrl { get { return "https://example.com/pay"; } }

        public Dictionary<string, string> GetPaymentFormParameters(UnitellerPayParameters payParameters)
        {
            throw new NotImplementedException();
        }

        public UnitellerPaymentInfo GetPaymentInfo(string shopId, string orderId)
        {
            throw new NotImplementedException();
        }

        public void ConfirmPayment(string shopId, string billNumber)
        {
            throw new NotImplementedException();
        }

        public void CancelPayment(string shopId, string billNumber)
        {
            throw new NotImplementedException();
        }
    }
}
