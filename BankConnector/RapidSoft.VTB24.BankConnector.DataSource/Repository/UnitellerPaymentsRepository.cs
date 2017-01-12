using System;
using System.Linq;
using RapidSoft.VTB24.BankConnector.DataModels;
using RapidSoft.VTB24.BankConnector.DataSource.Interface;

namespace RapidSoft.VTB24.BankConnector.DataSource.Repository
{
    public class UnitellerPaymentsRepository : IUnitellerPaymentsRepository
    {
        private readonly BankConnectorDBContext _context;

        public UnitellerPaymentsRepository(BankConnectorDBContext context)
        {
            _context = context;
        }

        public UnitellerPayment GetByOrderId(int orderId)
        {
            return _context.UnitellerPayments.FirstOrDefault(p => p.OrderId == orderId);
        }

        public void SaveRequest(int orderId, string shopId)
        {
            var payment = _context.UnitellerPayments.FirstOrDefault(p => p.OrderId == orderId);

            if (payment == null)
            {
                payment = new UnitellerPayment
                {
                    OrderId = orderId,
                    ShopId = shopId
                };
                _context.UnitellerPayments.Add(payment);
            }

            payment.FormDate = DateTime.Now;
        }
    }
}
