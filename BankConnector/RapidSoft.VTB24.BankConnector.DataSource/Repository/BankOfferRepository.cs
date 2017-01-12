using System;
using System.Collections.Generic;
using System.Linq;
using RapidSoft.VTB24.BankConnector.DataModels;
using RapidSoft.VTB24.BankConnector.DataSource.Interface;

namespace RapidSoft.VTB24.BankConnector.DataSource.Repository
{
    public class BankOfferRepository : IBankOfferRepository
    {
        private readonly BankConnectorDBContext _context;

        public BankOfferRepository(BankConnectorDBContext context)
        {
            _context = context;
        }

        public void Add(IEnumerable<BankOffer> bankOffers)
        {
            foreach (var bankOffer in bankOffers)
            {
                _context.BankOffers.Add(bankOffer);
            }
        }

	    public void BulkAdd(IEnumerable<BankOffer> bankOffers)
	    {
		    _context.BulkInsertAll(bankOffers.ToArray());
	    }

        public List<BankOffer> GetOffers(string id, string clientId, DateTime? expirationDate, int skipCount, int takeCount, bool countTotal, out int total)
        {
            total = -1;

            var query = QueryOffers(id, clientId, expirationDate);

            if (countTotal)
            {
                total = query.Count();
            }

            return query.OrderBy(bo => bo.Id)
                        .Skip(skipCount)
                        .Take(takeCount)
                        .ToList();
        }

        public void DisableOffer(string offerId)
        {
            var offer = QueryOffers(offerId, null, null).SingleOrDefault();
            if (offer != null)
            {
                offer.Status = BankOfferStatus.Inactive;
            }
        }

        private IQueryable<BankOffer> QueryOffers(string id, string clientId, DateTime? expirationDate)
        {
            IQueryable<BankOffer> query = _context.BankOffers.Where(bo => bo.Status == BankOfferStatus.Active);

            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(bo => bo.Id == id);
            }

            if (!string.IsNullOrWhiteSpace(clientId))
            {
                query = query.Where(bo => bo.ClientId == clientId);
            }

            if (expirationDate.HasValue)
            {
                var date = expirationDate.Value.Date;
                query = query.Where(bo => bo.ExpirationDate >= date);
            }

            return query;
        }
    }
}
