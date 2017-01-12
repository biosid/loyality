namespace RapidSoft.VTB24.BankConnector.DataSource.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource.Interface;

    public class ClientForBankPwdResetResponseRepository : GenericRepository<ClientForBankPwdResetResponse>, IClientForBankPwdResetResponseRepository
    {
        public ClientForBankPwdResetResponseRepository(BankConnectorDBContext context)
            : base(context)
        {
        }

        public IQueryable<ClientForBankPwdResetResponse> GetBySessionId(Guid sessionId)
        {
            return this.Context.ClientForBankPwdResetResponses.Where(c => c.EtlSessionId == sessionId);
        }

        public void Add(IEnumerable<ClientForBankPwdResetResponse> responses)
        {
            foreach (var response in responses)
            {
                Context.ClientForBankPwdResetResponses.Add(response);
            }
        }
    }
}
