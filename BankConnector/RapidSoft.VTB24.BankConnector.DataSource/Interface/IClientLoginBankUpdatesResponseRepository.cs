namespace RapidSoft.VTB24.BankConnector.DataSource.Interface
{
    using System;
    using System.Linq;

    using RapidSoft.VTB24.BankConnector.DataModels;

    public interface IClientLoginBankUpdatesResponseRepository : IGenericRepository<ClientLoginBankUpdatesResponse>
    {
        IQueryable<ClientLoginBankUpdatesResponse> GetBySessionId(Guid sessionId);
    }
}