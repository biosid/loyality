namespace RapidSoft.VTB24.BankConnector.DataSource.Repository
{
    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource.Interface;

    public class PromoActionResponseRepository : GenericRepository<PromoActionResponse>, IPromoActionResponseRepository
    {
        public PromoActionResponseRepository(BankConnectorDBContext context)
            : base(context)
        {
        }
    }
}