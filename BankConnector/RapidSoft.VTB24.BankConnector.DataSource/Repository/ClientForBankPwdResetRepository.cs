namespace RapidSoft.VTB24.BankConnector.DataSource.Repository
{
    using System;
    using System.Linq;

    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource.Interface;

    public class ClientForBankPwdResetRepository : GenericRepository<ClientForBankPwdReset>, IClientForBankPwdResetRepository
    {
        public ClientForBankPwdResetRepository(BankConnectorDBContext context)
            : base(context)
        {
        }

        public IQueryable<ClientForBankPwdReset> GetBySessionId(Guid sessionId)
        {
            return this.Context.ClientForBankPwdResets.Where(c => c.EtlSessionId == sessionId);
        }
    }
}
