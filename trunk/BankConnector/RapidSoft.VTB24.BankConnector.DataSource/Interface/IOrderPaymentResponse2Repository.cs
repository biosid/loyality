using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidSoft.VTB24.BankConnector.DataSource.Interface
{
    using RapidSoft.VTB24.BankConnector.DataModels;

    public interface IOrderPaymentResponse2Repository : IGenericRepository<OrderPaymentResponse2>
    {
        void DeleteBySessionId(Guid sessionId);
    }
}
