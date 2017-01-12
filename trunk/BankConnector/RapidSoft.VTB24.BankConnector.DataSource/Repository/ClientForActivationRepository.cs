using System;
using System.Collections.Generic;
using System.Linq;
using RapidSoft.VTB24.BankConnector.DataModels;
using RapidSoft.VTB24.BankConnector.DataSource.Interface;

namespace RapidSoft.VTB24.BankConnector.DataSource.Repository
{
	public class ClientForActivationRepository : GenericRepository<ClientForActivation>, IClientForActivationRepository
	{
        public ClientForActivationRepository(BankConnectorDBContext context)
            : base(context)
		{
		}

		public IQueryable<ClientForActivation> GetUniqueClientIdBySession(Guid sessionId)
		{
		    var otherEtlSession = from c in Context.ClientForActivations
		                          where c.EtlSessionId != sessionId && c.Status == (int)ActivateClientStatus.Success
		                          select c;

            // NOTE: VTBPLK-1743: Берем только тех клиентов у которых не проставлен ид сессии удалающей клиента.
		    var result = from c in Context.ClientForActivations
		                 join s in otherEtlSession on c.ClientId equals s.ClientId into lj
		                 from j in lj.DefaultIfEmpty()
		                 where c.EtlSessionId == sessionId && j.EtlSessionId == null && c.DeletionEtlSessionId == null
		                 select c;
			return result;
		}

		public IQueryable<ClientForActivation> GetOtherEtlSessionsClientRecords(Guid sessionId)
		{
            var otherEtlSession = from c in Context.ClientForActivations where c.EtlSessionId != sessionId && c.Status == (int)ActivateClientStatus.Success select c;

			return otherEtlSession;
		}

		public void DeleteBySessionId(Guid sessionId)
		{
			var target = (from c in Context.ClientForActivations
			              where c.EtlSessionId == sessionId
			              select new { c.ClientId, c.EtlSessionId }).ToList();

			target.ForEach(x => Delete(y => y.EtlSessionId == x.EtlSessionId && y.ClientId == x.ClientId));
		}

		public void DeleteByClientId(string clientId)
		{
			var target = (from c in Context.ClientForActivations
			              where c.ClientId == clientId
						  select new { c.ClientId, c.EtlSessionId }).ToList();
			target.ForEach(x => Delete(y => y.EtlSessionId == x.EtlSessionId && y.ClientId == x.ClientId));
		}

		public IQueryable<ClientForActivation> GetBySessionId(Guid sessionId)
		{
			var target = (from c in Context.ClientForActivations where c.EtlSessionId == sessionId select c);
			return target;
		}

		public IQueryable<ClientForActivation> GetByClientId(string clientId)
		{
			var target = (from c in Context.ClientForActivations where c.ClientId == clientId select c);
			return target;
		}
	}
}
