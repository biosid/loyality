namespace RapidSoft.VTB24.BankConnector.Service
{
    using System;
    using System.Collections.Generic;

    using API;
    using API.Entities;
    using DataModels;
    using DataSource;

    public class BankOffersService : IBankOffersService
    {
        private readonly IUnitOfWork uow;

        public BankOffersService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        #region IBankOffersService members

        public GenericBankConnectorResponse<BankOffersServiceResponse> GetBankOffers(
            BankOffersServiceParameter parameter)
        {
            try
            {
                int total;
                List<BankOffer> offers = uow.BankOffersRepository.GetOffers(parameter.Id, parameter.ClientId, parameter.ExpirationDate, parameter.SkipCount, parameter.TakeCount, parameter.CountTotal, out total);

                return new GenericBankConnectorResponse<BankOffersServiceResponse>(new BankOffersServiceResponse(total, offers));
            }
            catch (Exception error)
            {
                return new GenericBankConnectorResponse<BankOffersServiceResponse>(error);
            }
        }

        public SimpleBankConnectorResponse DisableOffer(string offerId)
        {
            try
            {
                uow.BankOffersRepository.DisableOffer(offerId);
                uow.Save();
            }
            catch (Exception error)
            {
                return new SimpleBankConnectorResponse(error);
            }

            return new SimpleBankConnectorResponse();
        }

        #endregion
    }
}
