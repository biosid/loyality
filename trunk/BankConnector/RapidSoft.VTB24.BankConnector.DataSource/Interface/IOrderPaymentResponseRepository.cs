using System;
using RapidSoft.VTB24.BankConnector.DataModels;

namespace RapidSoft.VTB24.BankConnector.DataSource.Interface
{
    public interface IOrderPaymentResponseRepository : IGenericRepository<OrderPaymentResponse>
    {
        void DeleteBySessionId(Guid sessionId);
    }
}