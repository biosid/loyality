using System;

namespace RapidSoft.VTB24.BankConnector.DataSource.Interface
{
    using System.Collections.Generic;
    using RapidSoft.VTB24.BankConnector.DataModels;

    public interface IBankOfferRepository
    {
        void Add(IEnumerable<BankOffer> bankOffers);

	    void BulkAdd(IEnumerable<BankOffer> bankOffers);

        List<BankOffer> GetOffers(string id, string clientId, DateTime? expirationDate, int skipCount, int takeCount, bool countTotal, out int total);

        void DisableOffer(string offerId);
    }
}
