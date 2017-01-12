namespace RapidSoft.VTB24.BankConnector.DataSource.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DataModels;
    using Interface;

    public class RegisterBankOffersResponseRepository : IRegisterBankOffersResponseRepository
    {
        private readonly BankConnectorDBContext _context;

        public RegisterBankOffersResponseRepository(BankConnectorDBContext context)
        {
            _context = context;
        }

        public IQueryable<RegisterBankOffersResponse> GetBySessionId(Guid sessionId)
        {
            return _context.RegisterBankOffersResponses.Where(rbor => rbor.EtlSessionId.Value == sessionId);
        }

        public void Add(IEnumerable<RegisterBankOffersResponse> responses)
        {
            foreach (var response in responses)
            {
                _context.RegisterBankOffersResponses.Add(response);
            }
        }

	    public void BulkAdd(IEnumerable<RegisterBankOffersResponse> responses)
	    {
		    _context.BulkInsertAll(responses.ToArray());
	    }
    }
}
