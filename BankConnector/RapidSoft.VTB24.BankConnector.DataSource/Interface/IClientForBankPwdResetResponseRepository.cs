namespace RapidSoft.VTB24.BankConnector.DataSource.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RapidSoft.VTB24.BankConnector.DataModels;

    public interface IClientForBankPwdResetResponseRepository : IGenericRepository<ClientForBankPwdResetResponse>
    {
        IQueryable<ClientForBankPwdResetResponse> GetBySessionId(Guid sessionId);

        void Add(IEnumerable<ClientForBankPwdResetResponse> responses);
    }
}
