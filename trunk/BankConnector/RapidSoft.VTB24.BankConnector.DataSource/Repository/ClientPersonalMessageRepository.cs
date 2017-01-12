namespace RapidSoft.VTB24.BankConnector.DataSource.Repository
{
    using System;
    using System.Linq;

    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource.Interface;

    public class ClientPersonalMessageRepository : GenericRepository<ClientPersonalMessage>,
                                                   IClientPersonalMessageRepository
    {
        public ClientPersonalMessageRepository(BankConnectorDBContext context)
            : base(context)
        {
        }

        public IQueryable<ClientPersonalMessage> GetBySessionId(Guid sessionId)
        {
            var target = (from c in this.Context.ClientPersonalMessages
                          where c.SessionId == sessionId
                          select c);
            return target;
        }

        public void DeleteBySessionId(Guid sessionId)
        {
            var target = (from c in this.Context.ClientForRegistrations
                          where (c.RequestSessionId == sessionId || c.ResponseSessionId == sessionId)
                          select c.ClientId).ToList();
            target.ForEach(x => this.Delete(y => y.ClientId == x));
        }
    }
}