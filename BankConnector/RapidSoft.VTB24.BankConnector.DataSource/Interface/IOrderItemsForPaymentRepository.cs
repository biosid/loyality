using System;
using RapidSoft.VTB24.BankConnector.DataModels;

namespace RapidSoft.VTB24.BankConnector.DataSource.Interface
{
    public interface IOrderItemsForPaymentRepository : IGenericRepository<OrderItemsForPayment>
    {
        void DeleteBySessionId(Guid sessionId);
    }
}