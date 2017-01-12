namespace RapidSoft.VTB24.BankConnector.API
{
	using System.ServiceModel;

	using RapidSoft.Loaylty.Monitoring;

	[ServiceContract(Name = "BankConnector", Namespace = Globals.DefaultServiceNamespace)]
    public interface IBankConnectorService : IOrderPaymentService, IClientManagementService, IBankOffersService, IBankSmsService, ISupportService
	{

	}
}
