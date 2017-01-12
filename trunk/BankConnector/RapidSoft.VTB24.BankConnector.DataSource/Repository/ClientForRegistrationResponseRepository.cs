namespace RapidSoft.VTB24.BankConnector.DataSource.Repository
{
    using System;
    using System.Linq;

    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource.Interface;

    public class ClientForRegistrationResponseRepository : GenericRepository<ClientForRegistrationResponse>, 
        IClientForRegistrationResponseRepository
    {
        public ClientForRegistrationResponseRepository(BankConnectorDBContext context)
            : base(context)
        {
        }

        #region IClientForRegistrationResponseRepository Members

        public IQueryable<ClientForRegistrationResponse> GetBySessionId(Guid sessionId)
        {
            var result = from c in this.Context.ClientForRegistrationResponses where c.SessionId == sessionId select c;
            return result;
        }

        #endregion
    }
}