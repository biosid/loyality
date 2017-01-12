namespace RapidSoft.VTB24.BankConnector.DataSource
{
    using System;

    using DataModels;
    using Interface;
    using Repository;

    public interface IUnitOfWork : IDisposable
    {
        void Save();

        BankConnectorDBContext Context { get; }

        GenericRepository<StepsBuffer> StepsBufferRepository { get; }

		GenericRepository<ClientAudienceRelation> ClientAudienceRelationRepository { get; }

        GenericRepository<ClientCardRegStatus> ClientCardRegStatusRepository { get; }

        GenericRepository<ClientForDeletion> ClientForDeletionRepository { get; }

		GenericRepository<ClientForDeletionResponse> ClientForDeletionResponseRepository { get; }

        GenericRepository<ClientForBankRegistrationResponse> ClientForBankRegistrationResponseRepository { get; }

        IClientPersonalMessageRepository ClientPersonalMessageRepository{ get; }

        IClientPersonalMessageResponseRepository ClientPersonalMessageResponseRepository{ get; }

        IPromoActionRepository PromoActionRepository { get; }

        IPromoActionResponseRepository PromoActionResponseRepository { get; }

        IClientForActivationRepository ClientForActivationRepository { get; }

        IClientForRegistrationRepository ClientForRegistrationRepository { get; }

        IClientForBankRegistrationRepository ClientForBankRegistrationRepository { get; }

        IOrderPaymentResponseRepository OrderPaymentResponseRepository { get; }

        IOrderForPaymentRepository OrderForPaymentRepository { get; }

        IOrderItemsForPaymentRepository OrderItemsForPaymentRepository { get; }

        IOrderPaymentResponse2Repository OrderPaymentResponse2Repository { get; }

        IClientForRegistrationResponseRepository ClientForRegistrationResponseRepository { get; }

		GenericRepository<Accrual> AccrualRepository { get; }

        IClientUpdatesRepository ClientUpdatesRepository { get; }

        ILoyaltyClientUpdateRepository LoyaltyClientUpdateRepository { get; }

        IProfileCustomFieldsRepository ProfileCustomFieldsRepository { get; }

        IProfileCustomFieldsValuesRepository ProfileCustomFieldsValuesRepository { get; }

        IClientLoginBankUpdatesRepository ClientLoginBankUpdatesRepository { get; }

        IClientLoginBankUpdatesResponseRepository ClientLoginBankUpdatesResponseRepository { get; }

        IClientForBankPwdResetRepository ClientForBankPwdResetRepository { get; }

        IClientForBankPwdResetResponseRepository ClientForBankPwdResetResponseRepository { get; }

        IUnitellerPaymentsRepository UnitellerPaymentsRepository { get; }

        IOrderAttemptsRepository OrderAttemptsRepository { get; }

        IBankOfferRepository BankOffersRepository { get; }

        IRegisterBankOffersRepository RegisterBankOffersRepository { get; }

        IRegisterBankOffersResponseRepository RegisterBankOffersResponseRepository { get; }

        IBankSmsRepository BankSmsRepository { get; }
    }
}
