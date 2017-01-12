namespace RapidSoft.VTB24.BankConnector.Acquiring
{
	public interface IUnitellerProvider
	{
		void Pay(string shopId, decimal sum, string client, string orderId);

		void RegisterCard(string client);
	}
}
