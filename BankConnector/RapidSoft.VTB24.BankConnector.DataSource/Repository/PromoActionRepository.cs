using System.Collections.Generic;

using RapidSoft.VTB24.BankConnector.DataModels;
using RapidSoft.VTB24.BankConnector.DataSource.Interface;

namespace RapidSoft.VTB24.BankConnector.DataSource.Repository
{
    public class PromoActionRepository : GenericRepository<PromoAction>, IPromoActionRepository
    {
        public PromoActionRepository(BankConnectorDBContext context)
            : base(context)
        {
        }

        public void Add(IEnumerable<PromoAction> promoActions)
        {
            foreach (var promoAction in promoActions)
            {
                this.Add(promoAction);
            }
        }
    }
}