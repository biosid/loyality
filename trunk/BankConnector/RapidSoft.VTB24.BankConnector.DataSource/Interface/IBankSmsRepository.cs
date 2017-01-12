using RapidSoft.VTB24.BankConnector.DataModels;

namespace RapidSoft.VTB24.BankConnector.DataSource.Interface
{
    public interface IBankSmsRepository
    {
        void Add(BankSms sms);
    }
}
