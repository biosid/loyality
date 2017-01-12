using System;
using System.Linq;

namespace RapidSoft.VTB24.BankConnector.DataSource.Interface
{
	using RapidSoft.VTB24.BankConnector.DataModels;

    public interface IClientForActivationRepository : IGenericRepository<ClientForActivation>
	{
		IQueryable<ClientForActivation> GetUniqueClientIdBySession(Guid sessionId);

		IQueryable<ClientForActivation> GetOtherEtlSessionsClientRecords(Guid sessionId);

		void DeleteBySessionId(Guid sessionId);

		void DeleteByClientId(string clientId);
	}
}
