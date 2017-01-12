namespace RapidSoft.VTB24.BankConnector.DataSource.Interface
{
    using System;
    using System.Linq;

    using RapidSoft.VTB24.BankConnector.DataModels;

    public interface IClientPersonalMessageRepository : IGenericRepository<ClientPersonalMessage>
    {
        IQueryable<ClientPersonalMessage> GetBySessionId(Guid sessionId);

        void DeleteBySessionId(Guid sessionId);        
    }
}