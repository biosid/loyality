namespace RapidSoft.VTB24.BankConnector.API
{
    using System.ServiceModel;

    using RapidSoft.VTB24.BankConnector.API.Entities;

    [ServiceContract]
    public interface IAdminClientManagementService
    {
        [OperationContract]
        GenericBankConnectorResponse<ClientProfileCustomField[]> GetAllProfileCustomFields(string userId);

        [OperationContract]
        SimpleBankConnectorResponse UpdateClientPhoneNumber(UpdateClientPhoneNumberRequest request);

        [OperationContract]
        SimpleBankConnectorResponse UpdateClientEmail(UpdateClientEmailRequest request);

        [OperationContract]
        GenericBankConnectorResponse<int> AppendProfileCustomField(string name, string userId);

        [OperationContract]
        SimpleBankConnectorResponse RemoveProfileCustomField(int id, string userId);

        [OperationContract]
        SimpleBankConnectorResponse RenameProfileCustomField(int id, string name, string userId);
    }
}
