namespace RapidSoft.VTB24.BankConnector.DataSource.Interface
{
    using System.Collections.Generic;

    using RapidSoft.VTB24.BankConnector.DataModels;

    public interface IPromoActionRepository : IGenericRepository<PromoAction>
    {
        void Add(IEnumerable<PromoAction> promoActions);
    }
}