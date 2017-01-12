namespace RapidSoft.VTB24.BankConnector.DataSource.Repository
{
    using System;
    using System.Linq;

    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource.Interface;

    public class ClientUpdatesRepository : GenericRepository<ClientUpdate>, IClientUpdatesRepository
    {
        public ClientUpdatesRepository(BankConnectorDBContext context)
            : base(context)
        {
        }

        #region IClientUpdatesRepository Members

        public IQueryable<ClientUpdate> GetBySessionId(Guid sessionId)
        {
            var result = from c in this.Context.ClientUpdates where c.EtlSessionId == sessionId select c;
            return result;
        }

        #endregion
    }
}