using System;
using System.Linq;
using RapidSoft.VTB24.BankConnector.DataModels;
using RapidSoft.VTB24.BankConnector.DataSource.Interface;

namespace RapidSoft.VTB24.BankConnector.DataSource.Repository
{
    public class OrderItemsForPaymentRepository : GenericRepository<OrderItemsForPayment>,
                                             IOrderItemsForPaymentRepository
    {
        public OrderItemsForPaymentRepository(BankConnectorDBContext context)
            : base(context)
        {
        }

        public void DeleteBySessionId(Guid sessionId)
        {
            var target = (from c in Context.OrderItemsForPayments
                          where (c.EtlSessionId == sessionId)
                          select c).ToList();
            target.ForEach(x => Context.OrderItemsForPayments.Remove(x));
        }
    }
}