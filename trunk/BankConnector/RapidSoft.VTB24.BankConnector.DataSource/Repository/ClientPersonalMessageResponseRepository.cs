namespace RapidSoft.VTB24.BankConnector.DataSource.Repository
{
    using System;
    using System.Linq;

    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource.Interface;

    public class ClientPersonalMessageResponseRepository : GenericRepository<ClientPersonalMessageResponse>,
                                                           IClientPersonalMessageResponseRepository
    {
        public ClientPersonalMessageResponseRepository(BankConnectorDBContext context)
            : base(context)
        {
        }

        public IQueryable<ClientPersonalMessageResponse> GetBySessionId(Guid sessionId)
        {
            var target = (from c in this.Context.ClientPersonalMessageResponses
                          where c.SessionId == sessionId
                          select c);
            return target;
        }

        public void DeleteBySessionId(Guid sessionId)
        {
            var target = (from c in this.Context.ClientPersonalMessageResponses
                          where (c.SessionId  == sessionId)
                          select c.ClientId).ToList();
            target.ForEach(x => this.Delete(y => y.ClientId == x));
        }
    }
}