using System;
using RapidSoft.VTB24.BankConnector.DataModels;

namespace RapidSoft.VTB24.BankConnector.DataSource.Interface
{
    public interface IOrderForPaymentRepository : IGenericRepository<OrderForPayment>
    {
        void DeleteBySessionId(Guid sessionId);
    }
}