namespace RapidSoft.VTB24.BankConnector.API
{
    using System.ServiceModel;

    using RapidSoft.VTB24.BankConnector.API.Entities;

    [ServiceContract]
    public interface IOrderPaymentService
    {
        [OperationContract]
        GenericBankConnectorResponse<bool?> IsCardRegistered(string clientId);

        [OperationContract]
        SimpleBankConnectorResponse RegisterCard(string clientId);

        [OperationContract]
        SimpleBankConnectorResponse UnregisterCard(string clientId);
    }
}
