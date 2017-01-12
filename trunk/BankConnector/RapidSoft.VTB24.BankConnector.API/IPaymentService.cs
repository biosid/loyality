namespace RapidSoft.VTB24.BankConnector.API
{
    using System.ServiceModel;
    using RapidSoft.VTB24.BankConnector.API.Entities;

    [ServiceContract]
    public interface IPaymentService
    {
        [OperationContract]
        GenericBankConnectorResponse<PaymentFormParameters> GetPaymentFormParameters(PaymentFormRequest request);

        [OperationContract]
        GenericBankConnectorResponse<bool> IsPaymentAuthorized(int orderId);
            
        [OperationContract]
        SimpleBankConnectorResponse ConfirmPayment(int orderId);

        [OperationContract]
        SimpleBankConnectorResponse CancelPayment(int orderId);

        [OperationContract]
        GenericBankConnectorResponse<PaymentInfo> GetPaymentByOrderId(int orderId);
    }
}
