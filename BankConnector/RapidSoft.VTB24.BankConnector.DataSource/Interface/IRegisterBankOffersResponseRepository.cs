namespace RapidSoft.VTB24.BankConnector.DataSource.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DataModels;

    public interface IRegisterBankOffersResponseRepository
    {
        IQueryable<RegisterBankOffersResponse> GetBySessionId(Guid sessionId);

        void Add(IEnumerable<RegisterBankOffersResponse> responses);

	    void BulkAdd(IEnumerable<RegisterBankOffersResponse> responses);
    }
}
