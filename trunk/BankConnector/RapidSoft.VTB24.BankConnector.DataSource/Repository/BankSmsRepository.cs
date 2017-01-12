using RapidSoft.VTB24.BankConnector.DataModels;
using RapidSoft.VTB24.BankConnector.DataSource.Interface;

namespace RapidSoft.VTB24.BankConnector.DataSource.Repository
{
    public class BankSmsRepository : IBankSmsRepository
    {
        private readonly BankConnectorDBContext _context;

        public BankSmsRepository(BankConnectorDBContext context)
        {
            _context = context;
        }

        public void Add(BankSms sms)
        {
            _context.BankSms.Add(sms);
        }
    }
}
