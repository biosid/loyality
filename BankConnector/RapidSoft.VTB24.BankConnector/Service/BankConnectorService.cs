namespace RapidSoft.VTB24.BankConnector.Service
{
    using RapidSoft.Loaylty.Logging.Wcf;
    using RapidSoft.Loaylty.Monitoring;
    using RapidSoft.VTB24.BankConnector.API;
    using RapidSoft.VTB24.BankConnector.API.Entities;

    [LoggingBehavior]
    public class BankConnectorService : SupportService, IBankConnectorService
    {
        private readonly IOrderPaymentService orderPayment;
        private readonly IClientManagementService clientManagement;
        private readonly IBankOffersService bankOffersService;
        private readonly IBankSmsService bankSmsService;

        public BankConnectorService(IClientManagementService clientManagement, IOrderPaymentService orderPayment, IBankOffersService bankOffersService, IBankSmsService bankSmsService)
        {
            this.clientManagement = clientManagement;
            this.orderPayment = orderPayment;
            this.bankOffersService = bankOffersService;
            this.bankSmsService = bankSmsService;
        }

        #region IBankConnectorService Members

        public GenericBankConnectorResponse<bool?> IsCardRegistered(string clientId)
        {
            return this.orderPayment.IsCardRegistered(clientId);
        }

        public SimpleBankConnectorResponse RegisterCard(string clientId)
        {
            return this.orderPayment.RegisterCard(clientId);
        }

        public SimpleBankConnectorResponse UnregisterCard(string clientId)
        {
            return this.orderPayment.UnregisterCard(clientId);
        }

        public SimpleBankConnectorResponse RegisterNewClient(RegisterClientRequest request)
        {
            return this.clientManagement.RegisterNewClient(request);
        }

        public SimpleBankConnectorResponse BlockClientToDelete(string clientId)
        {
            return this.clientManagement.BlockClientToDelete(clientId);
        }

	    public GenericBankConnectorResponse<bool> IsClientAddedToDetachList(string clientId)
	    {
		    return this.clientManagement.IsClientAddedToDetachList(clientId);
	    }

	    public GenericBankConnectorResponse<CardRegistrationParameters> GetCardRegistrationParameters(string clientId)
        {
            return this.clientManagement.GetCardRegistrationParameters(clientId);
        }

        public GenericBankConnectorResponse<bool> VerifyCardRegistration(string orderId)
        {
            return this.clientManagement.VerifyCardRegistration(orderId);
        }

        public SimpleBankConnectorResponse UpdateClient(UpdateClientRequest request)
        {
            return this.clientManagement.UpdateClient(request);
        }

        public GenericBankConnectorResponse<ClientProfile> GetClientProfile(string clientId)
        {
            return this.clientManagement.GetClientProfile(clientId);
        }

        public SimpleBankConnectorResponse SaveOrderAttempt(string clientId)
        {
            return this.clientManagement.SaveOrderAttempt(clientId);
        }

        public SimpleBankConnectorResponse ClearOrderAttempt(string clientId)
        {
            return this.clientManagement.ClearOrderAttempt(clientId);
        }

        public GenericBankConnectorResponse<BankOffersServiceResponse> GetBankOffers(BankOffersServiceParameter parameter)
        {
            return this.bankOffersService.GetBankOffers(parameter);
        }

        public SimpleBankConnectorResponse DisableOffer(string offerId)
        {
            return this.bankOffersService.DisableOffer(offerId);
        }

        public SimpleBankConnectorResponse EnqueueSms(BankSmsType type, string phone, string password)
        {
            return this.bankSmsService.EnqueueSms(type, phone, password);
        }

        #endregion
    }
}