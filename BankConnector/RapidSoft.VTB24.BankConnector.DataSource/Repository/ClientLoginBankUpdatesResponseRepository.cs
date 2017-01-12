using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidSoft.VTB24.BankConnector.DataSource.Repository
{
    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource.Interface;

    public class ClientLoginBankUpdatesResponseRepository: GenericRepository<ClientLoginBankUpdatesResponse>, IClientLoginBankUpdatesResponseRepository

    {
        public ClientLoginBankUpdatesResponseRepository(BankConnectorDBContext context)
            : base(context)
        {
        }

        public IQueryable<ClientLoginBankUpdatesResponse> GetBySessionId(Guid sessionId)
        {
            var target = (from c in this.Context.ClientLoginBankUpdatesResponses
                          where c.EtlSessionId == sessionId
                          select c);
            return target;
        }
    }
}
