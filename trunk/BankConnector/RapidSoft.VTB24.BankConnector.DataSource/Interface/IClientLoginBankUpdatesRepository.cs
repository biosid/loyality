namespace RapidSoft.VTB24.BankConnector.DataSource.Interface
{
    using System;
    using System.Linq;

    using RapidSoft.VTB24.BankConnector.DataModels;

    public interface IClientLoginBankUpdatesRepository : IGenericRepository<ClientLoginBankUpdate>
    {
        IQueryable<ClientLoginBankUpdate> GetBySessionId(Guid sessionId);
    }
}