using Vtb24.Site.Security.Models;

namespace Vtb24.Site.Security
{
    public interface IBankSmsService
    {
        void Send(BankSmsType type, string phone, string password);
    }
}
