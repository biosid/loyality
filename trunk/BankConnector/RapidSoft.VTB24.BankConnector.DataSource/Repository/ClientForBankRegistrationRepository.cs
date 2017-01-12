namespace RapidSoft.VTB24.BankConnector.DataSource.Repository
{
    using System;
    using System.Linq;

    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource.Interface;

    public class ClientForBankRegistrationRepository : GenericRepository<ClientForBankRegistration>, IClientForBankRegistrationRepository
    {
        public ClientForBankRegistrationRepository(BankConnectorDBContext context)
            : base(context)
        {
        }

        public IQueryable<ClientForBankRegistration> GetBySessionId(Guid sessionId)
        {
            return
                Context.ClientForBankRegistrations.Where(c => c.SessionId == sessionId)
                       .Where(c => c.ErrorStatus == null);
        }
    }
}
