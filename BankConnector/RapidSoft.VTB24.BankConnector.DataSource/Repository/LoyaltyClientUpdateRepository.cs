namespace RapidSoft.VTB24.BankConnector.DataSource.Repository
{
    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource.Interface;

    public class LoyaltyClientUpdateRepository : GenericRepository<LoyaltyClientUpdate>, ILoyaltyClientUpdateRepository
    {
        public LoyaltyClientUpdateRepository(BankConnectorDBContext context)
            : base(context)
        {
        }
    }
}