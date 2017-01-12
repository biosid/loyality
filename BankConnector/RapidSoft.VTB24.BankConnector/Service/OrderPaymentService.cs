namespace RapidSoft.VTB24.BankConnector.Service
{
    using System.Linq;
    using RapidSoft.VTB24.BankConnector.Acquiring;
    using RapidSoft.VTB24.BankConnector.API;
    using RapidSoft.VTB24.BankConnector.API.Entities;
    using RapidSoft.VTB24.BankConnector.API.Exceptions;
    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource;
    
    public class OrderPaymentService : IOrderPaymentService
    {
        private readonly IUnitOfWork uow;

	    private readonly IUnitellerProvider unitellerProvider;

        public OrderPaymentService(IUnitOfWork uow, IUnitellerProvider unitellerProvider)
        {
            this.uow = uow;
	        this.unitellerProvider = unitellerProvider;
        }

        #region IOrderPaymentService Members

        public GenericBankConnectorResponse<bool?> IsCardRegistered(string clientId)
        {
            return new GenericBankConnectorResponse<bool?>(
                    this.uow.ClientCardRegStatusRepository.GetAll().Any(x => x.ClientId == clientId));
        }

        public SimpleBankConnectorResponse RegisterCard(string clientId)
        {
            var repository = this.uow.ClientCardRegStatusRepository;

            if (repository.GetAll().Any(x => x.ClientId == clientId))
            {
                throw new CardRegistrationException();
            }

            // unitellerProvider.RegisterCard(clientId);
            repository.Add(new ClientCardRegStatus
            {
                ClientId = clientId,
                IsCardRegistered = true
            });

            this.uow.Save();

            return new SimpleBankConnectorResponse();
        }

        public SimpleBankConnectorResponse UnregisterCard(string clientId)
        {
            var repository = this.uow.ClientCardRegStatusRepository;

            if (repository.GetAll().Any(x => x.ClientId == clientId))
            {
                throw new CardRegistrationException();
            }

            repository.Delete(x => x.ClientId == clientId);
            this.uow.Save();

            return new SimpleBankConnectorResponse();
        }

        #endregion
    }
}