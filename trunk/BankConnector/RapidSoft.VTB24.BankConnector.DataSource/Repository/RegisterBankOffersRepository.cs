namespace RapidSoft.VTB24.BankConnector.DataSource.Repository
{
    using System;
    using System.Linq;

    using DataModels;
    using Interface;

    public class RegisterBankOffersRepository : IRegisterBankOffersRepository
    {
        private readonly BankConnectorDBContext _context;

        public RegisterBankOffersRepository(BankConnectorDBContext context)
        {
            _context = context;
        }

        public IQueryable<RegisterBankOffer> GetBySessionId(Guid sessionId)
        {
            return _context.RegisterBankOffers.Where(rbo => rbo.EtlSessionId == sessionId);
        }

        public IQueryable<RegisterBankOffer> GetUnprocessedBySessionId(Guid sessionId, IRegisterBankOffersResponseRepository responses)
        {
            return GetBySessionId(sessionId).GroupJoin(responses.GetBySessionId(sessionId),
                                                       rbo => rbo.PartnerOrderNum,
                                                       rbor => rbor.PartnerOrderNum,
                                                       (rbo, rbor) => new { rbo, rbor = rbor.FirstOrDefault() })
                                            .Where(bo => bo.rbor == null)
                                            .Select(bo => bo.rbo);
        }
    }
}
