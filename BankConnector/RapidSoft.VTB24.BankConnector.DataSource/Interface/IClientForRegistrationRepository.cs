namespace RapidSoft.VTB24.BankConnector.DataSource.Interface
{
	using System;

	using RapidSoft.VTB24.BankConnector.DataModels;

    public interface IClientForRegistrationRepository : IGenericRepository<ClientForRegistration>
	{
        void DeleteBySessionId(Guid sessionId);

		//void UpdateClientsStatus(List<ClientForRegistration> partialClients, Guid responseSessionId);
	}
}
