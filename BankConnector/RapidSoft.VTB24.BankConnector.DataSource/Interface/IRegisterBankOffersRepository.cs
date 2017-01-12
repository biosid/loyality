namespace RapidSoft.VTB24.BankConnector.DataSource.Interface
{
    using System;
    using System.Linq;
    using DataModels;

    public interface IRegisterBankOffersRepository
    {
        IQueryable<RegisterBankOffer> GetBySessionId(Guid sessionId);

        IQueryable<RegisterBankOffer> GetUnprocessedBySessionId(Guid sessionId, IRegisterBankOffersResponseRepository responses);
    }
}
