namespace RapidSoft.VTB24.BankConnector.DataSource.Repository
{
    using System;
    using System.Linq;

    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource.Interface;

    public class ClientLoginBankUpdatesRepository : GenericRepository<ClientLoginBankUpdate>, IClientLoginBankUpdatesRepository

    {
        public ClientLoginBankUpdatesRepository(BankConnectorDBContext context)
            : base(context)
        {
        }

        public IQueryable<ClientLoginBankUpdate> GetBySessionId(Guid sessionId)
        {
            var target = (from c in this.Context.ClientLoginBankUpdates
                          where c.EtlSessionId == sessionId
                          select c);
            return target;
        }
    }
}
