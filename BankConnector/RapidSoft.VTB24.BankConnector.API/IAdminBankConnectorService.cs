namespace RapidSoft.VTB24.BankConnector.API
{
    using System.ServiceModel;

    using RapidSoft.Loaylty.Monitoring;

    [ServiceContract(Name = "AdminBankConnector", Namespace = Globals.DefaultServiceNamespace)]
    public interface IAdminBankConnectorService : IAdminClientManagementService, ISupportService
    {
    }
}
