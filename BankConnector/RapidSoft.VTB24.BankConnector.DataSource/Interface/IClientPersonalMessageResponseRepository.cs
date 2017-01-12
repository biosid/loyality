namespace RapidSoft.VTB24.BankConnector.DataSource.Interface
{
    using System;
    using System.Linq;

    using RapidSoft.VTB24.BankConnector.DataModels;

    public interface IClientPersonalMessageResponseRepository : IGenericRepository<ClientPersonalMessageResponse>
    {
        IQueryable<ClientPersonalMessageResponse> GetBySessionId(Guid sessionId);

        void DeleteBySessionId(Guid sessionId);        
    }
}