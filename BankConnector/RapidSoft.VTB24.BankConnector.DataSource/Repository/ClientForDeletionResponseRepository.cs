using RapidSoft.VTB24.BankConnector.DataModels;

namespace RapidSoft.VTB24.BankConnector.DataSource.Repository
{
	public class ClientForDeletionResponseRepository : GenericRepository<ClientForDeletion>
	{
		public ClientForDeletionResponseRepository(BankConnectorDBContext context)
			: base(context)
		{

		}
	}
}
