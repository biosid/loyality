namespace RapidSoft.VTB24.BankConnector.DataSource
{
	using System;

	using DataModels;
	using Interface;
	using Repository;

	public class UnitOfWork : IUnitOfWork
	{
		private bool disposed;

		public UnitOfWork(
            BankConnectorDBContext context,
            GenericRepository<StepsBuffer> stepsBufferRepository,
            GenericRepository<ClientAudienceRelation> clientAudienceRelationRepository,
            GenericRepository<ClientCardRegStatus> clientCardRegStatusRepository,
            GenericRepository<ClientForDeletion> clientForDeletionRepository,
            GenericRepository<ClientForDeletionResponse> clientForDeletionResponseRepository,
            GenericRepository<ClientForBankRegistrationResponse> clientForBankRegistrationResponseRepository,
            IClientPersonalMessageRepository clientPersonalMessageRepository,
            IClientPersonalMessageResponseRepository clientPersonalMessageResponseRepository,
            IPromoActionRepository promoActionRepository,
            IPromoActionResponseRepository promoActionResponseRepository,
            IClientForActivationRepository clientForActivationRepository,
            IClientForRegistrationRepository clientForRegistrationRepository,
            IClientForBankRegistrationRepository clientForBankRegistrationRepository,
            IOrderPaymentResponseRepository orderPaymentResponseRepository,
            IOrderForPaymentRepository orderForPaymentRepository,
            IOrderItemsForPaymentRepository orderItemsForPaymentRepository,
            IOrderPaymentResponse2Repository orderPaymentResponse2Repository,
            IClientForRegistrationResponseRepository clientForRegistrationResponseRepository,
            GenericRepository<Accrual> accrualRepository,
            IClientUpdatesRepository clientUpdatesRepository,
            ILoyaltyClientUpdateRepository loyaltyClientUpdateRepository,
            IProfileCustomFieldsRepository profileCustomFieldsRepository,
            IProfileCustomFieldsValuesRepository profileCustomFieldsValuesRepository,
            IClientLoginBankUpdatesRepository clientLoginBankUpdatesRepository,
            IClientLoginBankUpdatesResponseRepository clientLoginBankUpdatesResponseRepository,
            IClientForBankPwdResetRepository clientForBankPwdResetRepository,
            IClientForBankPwdResetResponseRepository clientForBankPwdResetResponseRepository,
            IUnitellerPaymentsRepository unitellerPaymentsRepository,
            IOrderAttemptsRepository orderAttemptsRepository,
            IBankOfferRepository bankOfferRepository,
            IRegisterBankOffersRepository registerBankOffersRepository,
            IRegisterBankOffersResponseRepository registerBankOffersResponseRepository,
            IBankSmsRepository bankSmsRepository)
		{
			Context = context;

			StepsBufferRepository = stepsBufferRepository;
			ClientAudienceRelationRepository = clientAudienceRelationRepository;
			ClientCardRegStatusRepository = clientCardRegStatusRepository;
			ClientForDeletionRepository = clientForDeletionRepository;
			ClientForDeletionResponseRepository = clientForDeletionResponseRepository;
			ClientForBankRegistrationResponseRepository = clientForBankRegistrationResponseRepository;

			ClientPersonalMessageRepository = clientPersonalMessageRepository;
			ClientPersonalMessageResponseRepository = clientPersonalMessageResponseRepository;
			PromoActionRepository = promoActionRepository;
			PromoActionResponseRepository = promoActionResponseRepository;
			ClientForActivationRepository = clientForActivationRepository;
			ClientForRegistrationRepository = clientForRegistrationRepository;
			ClientForBankRegistrationRepository = clientForBankRegistrationRepository;
			OrderPaymentResponseRepository = orderPaymentResponseRepository;
			OrderForPaymentRepository = orderForPaymentRepository;
            OrderItemsForPaymentRepository = orderItemsForPaymentRepository;
		    OrderPaymentResponse2Repository = orderPaymentResponse2Repository;
			ClientForRegistrationResponseRepository = clientForRegistrationResponseRepository;
			AccrualRepository = accrualRepository;
			ClientUpdatesRepository = clientUpdatesRepository;
			LoyaltyClientUpdateRepository = loyaltyClientUpdateRepository;

		    ProfileCustomFieldsRepository = profileCustomFieldsRepository;
		    ProfileCustomFieldsValuesRepository = profileCustomFieldsValuesRepository;

		    ClientLoginBankUpdatesRepository = clientLoginBankUpdatesRepository;
		    ClientLoginBankUpdatesResponseRepository = clientLoginBankUpdatesResponseRepository;

		    ClientForBankPwdResetRepository = clientForBankPwdResetRepository;
		    ClientForBankPwdResetResponseRepository = clientForBankPwdResetResponseRepository;

		    UnitellerPaymentsRepository = unitellerPaymentsRepository;

		    OrderAttemptsRepository = orderAttemptsRepository;
		    
            BankOffersRepository = bankOfferRepository;

		    RegisterBankOffersRepository = registerBankOffersRepository;
		    RegisterBankOffersResponseRepository = registerBankOffersResponseRepository;

		    BankSmsRepository = bankSmsRepository;
		}

	    public BankConnectorDBContext Context { get; private set; }

		#region Repositories

		public GenericRepository<StepsBuffer> StepsBufferRepository { get; private set; }

		public GenericRepository<ClientAudienceRelation> ClientAudienceRelationRepository { get; private set; }

		public IPromoActionRepository PromoActionRepository { get; private set; }

		public GenericRepository<ClientCardRegStatus> ClientCardRegStatusRepository { get; private set; }

		public IPromoActionResponseRepository PromoActionResponseRepository { get; private set; }

		public IClientForActivationRepository ClientForActivationRepository { get; private set; }

		public GenericRepository<ClientForDeletion> ClientForDeletionRepository { get; private set; }

		public GenericRepository<ClientForDeletionResponse> ClientForDeletionResponseRepository { get; private set; }

		public IClientForRegistrationRepository ClientForRegistrationRepository { get; private set; }

		public IClientForBankRegistrationRepository ClientForBankRegistrationRepository { get; private set; }

		public IClientPersonalMessageRepository ClientPersonalMessageRepository { get; private set; }

		public IClientPersonalMessageResponseRepository ClientPersonalMessageResponseRepository { get; private set; }

		public IOrderPaymentResponseRepository OrderPaymentResponseRepository { get; private set; }

		public IOrderForPaymentRepository OrderForPaymentRepository { get; private set; }

        public IOrderItemsForPaymentRepository OrderItemsForPaymentRepository { get; private set; }

	    public IOrderPaymentResponse2Repository OrderPaymentResponse2Repository { get; private set; }

	    public GenericRepository<ClientForBankRegistrationResponse> ClientForBankRegistrationResponseRepository { get; private set; }

		public IClientForRegistrationResponseRepository ClientForRegistrationResponseRepository { get; private set; }

		public GenericRepository<Accrual> AccrualRepository { get; private set; }

		public IClientUpdatesRepository ClientUpdatesRepository { get; private set; }

		public ILoyaltyClientUpdateRepository LoyaltyClientUpdateRepository { get; private set; }

        public IProfileCustomFieldsRepository ProfileCustomFieldsRepository { get; private set; }

        public IProfileCustomFieldsValuesRepository ProfileCustomFieldsValuesRepository { get; private set; }

	    public IClientLoginBankUpdatesRepository ClientLoginBankUpdatesRepository { get; private set; }

	    public IClientLoginBankUpdatesResponseRepository ClientLoginBankUpdatesResponseRepository { get; private set; }

        public IClientForBankPwdResetRepository ClientForBankPwdResetRepository { get; private set; }

        public IClientForBankPwdResetResponseRepository ClientForBankPwdResetResponseRepository { get; private set; }

        public IUnitellerPaymentsRepository UnitellerPaymentsRepository { get; private set; }

        public IOrderAttemptsRepository OrderAttemptsRepository { get; private set; }
        
        public IBankOfferRepository BankOffersRepository { get; private set; }

        public IRegisterBankOffersRepository RegisterBankOffersRepository { get; private set; }

        public IRegisterBankOffersResponseRepository RegisterBankOffersResponseRepository { get; private set; }

	    public IBankSmsRepository BankSmsRepository { get; private set; }

	    #endregion

		public virtual void Save()
		{
			Context.SaveChanges();
		}

		public virtual void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					Context.Dispose();
				}
			}

			disposed = true;
		}
	}
}
