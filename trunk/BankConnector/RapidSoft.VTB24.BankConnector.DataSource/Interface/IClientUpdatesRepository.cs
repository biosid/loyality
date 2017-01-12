namespace RapidSoft.VTB24.BankConnector.DataSource.Interface
{
    using System;
    using System.Linq;

    using RapidSoft.VTB24.BankConnector.DataModels;

    public interface IClientUpdatesRepository : IGenericRepository<ClientUpdate>
    {
        IQueryable<ClientUpdate> GetBySessionId(Guid sessionId);
    }
}