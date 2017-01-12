using RapidSoft.VTB24.BankConnector.DataModels;

namespace RapidSoft.VTB24.BankConnector.DataSource.Repository
{
	public class AccrualRepository:GenericRepository<Accrual>
	{
		public AccrualRepository(BankConnectorDBContext context)
			: base(context)
		{

		}
	}
}
