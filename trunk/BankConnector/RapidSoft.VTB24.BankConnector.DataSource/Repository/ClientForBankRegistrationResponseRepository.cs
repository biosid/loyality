namespace RapidSoft.VTB24.BankConnector.DataSource.Repository
{
    using RapidSoft.VTB24.BankConnector.DataModels;

    public class ClientForBankRegistrationResponseRepository : GenericRepository<ClientForBankRegistrationResponse>
    {
        public ClientForBankRegistrationResponseRepository(BankConnectorDBContext context)
            : base(context)
        {

        }
    }
}
