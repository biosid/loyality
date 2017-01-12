namespace RapidSoft.VTB24.BankConnector.API
{
    using System.ServiceModel;

    using RapidSoft.VTB24.BankConnector.API.Entities;

    [ServiceContract]
	public interface IClientManagementService
	{
		[OperationContract]
		SimpleBankConnectorResponse RegisterNewClient(RegisterClientRequest request);

		[OperationContract]
		SimpleBankConnectorResponse BlockClientToDelete(string clientId);

		[OperationContract]
		GenericBankConnectorResponse<bool> IsClientAddedToDetachList(string clientId);
		
		[OperationContract]
		GenericBankConnectorResponse<CardRegistrationParameters> GetCardRegistrationParameters(string clientId);

		[OperationContract]
		GenericBankConnectorResponse<bool> VerifyCardRegistration(string orderId);

		[OperationContract]
		SimpleBankConnectorResponse UpdateClient(UpdateClientRequest request);

        [OperationContract]
        GenericBankConnectorResponse<ClientProfile> GetClientProfile(string clientId);

        [OperationContract]
        SimpleBankConnectorResponse SaveOrderAttempt(string clientId);

        [OperationContract]
        SimpleBankConnectorResponse ClearOrderAttempt(string clientId);
	}
}
