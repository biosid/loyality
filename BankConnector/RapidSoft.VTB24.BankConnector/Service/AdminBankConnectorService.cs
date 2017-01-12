namespace RapidSoft.VTB24.BankConnector.Service
{
    using RapidSoft.Loaylty.Logging.Wcf;
    using RapidSoft.Loaylty.Monitoring;
    using RapidSoft.VTB24.BankConnector.API;
    using RapidSoft.VTB24.BankConnector.API.Entities;

    [LoggingBehavior]
    public class AdminBankConnectorService : SupportService, IAdminBankConnectorService
    {
        private readonly IAdminClientManagementService adminClientManagement;

        public AdminBankConnectorService(IAdminClientManagementService adminClientManagement)
        {
            this.adminClientManagement = adminClientManagement;
        }

        public GenericBankConnectorResponse<ClientProfileCustomField[]> GetAllProfileCustomFields(string userId)
        {
            return this.adminClientManagement.GetAllProfileCustomFields(userId);
        }

        public SimpleBankConnectorResponse UpdateClientPhoneNumber(UpdateClientPhoneNumberRequest request)
        {
            return this.adminClientManagement.UpdateClientPhoneNumber(request);
        }

        public SimpleBankConnectorResponse UpdateClientEmail(UpdateClientEmailRequest request)
        {
            return this.adminClientManagement.UpdateClientEmail(request);
        }

        public GenericBankConnectorResponse<int> AppendProfileCustomField(string name, string userId)
        {
            return this.adminClientManagement.AppendProfileCustomField(name, userId);
        }

        public SimpleBankConnectorResponse RemoveProfileCustomField(int id, string userId)
        {
            return this.adminClientManagement.RemoveProfileCustomField(id, userId);
        }

        public SimpleBankConnectorResponse RenameProfileCustomField(int id, string name, string userId)
        {
            return this.adminClientManagement.RenameProfileCustomField(id, name, userId);
        }
    }
}
