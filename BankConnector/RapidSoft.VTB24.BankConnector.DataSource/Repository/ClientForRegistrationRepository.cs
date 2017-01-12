using RapidSoft.VTB24.BankConnector.DataModels;
using RapidSoft.VTB24.BankConnector.DataSource.Interface;
using System;
using System.Linq;

namespace RapidSoft.VTB24.BankConnector.DataSource.Repository
{
    

	public class ClientForRegistrationRepository : GenericRepository<ClientForRegistration>,
	                                               IClientForRegistrationRepository
	{
        public ClientForRegistrationRepository(BankConnectorDBContext context)
            : base(context)
		{
		}

	    public void DeleteBySessionId(Guid sessionId)
		{
			var target = (from c in Context.ClientForRegistrations
			              where (c.RequestSessionId == sessionId || c.ResponseSessionId == sessionId)
			              select c.ClientId).ToList();
			target.ForEach(x => Delete(y => y.ClientId == x));
		}
	}
}
