using System;
using System.Linq;
using RapidSoft.VTB24.BankConnector.DataModels;
using RapidSoft.VTB24.BankConnector.DataSource.Interface;

namespace RapidSoft.VTB24.BankConnector.DataSource.Repository
{
    public class OrderPaymentResponse2Repository : GenericRepository<OrderPaymentResponse2>,
                                                  IOrderPaymentResponse2Repository
    {
        public OrderPaymentResponse2Repository(BankConnectorDBContext context)
            : base(context)
        {
        }

        public void DeleteBySessionId(Guid sessionId)
        {
            var target = (from c in Context.OrderPaymentResponse2
                          where (c.EtlSessionId == sessionId)
                          select c).ToList();
            target.ForEach(x => Context.OrderPaymentResponse2.Remove(x));
        }
    }
}