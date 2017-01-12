using RapidSoft.VTB24.BankConnector.DataModels;

namespace RapidSoft.VTB24.BankConnector.DataSource.Repository
{
	public class ClientForDeletionRepository:GenericRepository<ClientForDeletion>
	{
        public ClientForDeletionRepository(BankConnectorDBContext context)
            : base(context)
        {

        }
	}
}
