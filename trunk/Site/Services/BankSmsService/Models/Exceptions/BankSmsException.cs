using Vtb24.Site.Services.Models.Exceptions;

namespace Vtb24.Site.Services.BankSmsService.Models.Exceptions
{
    public class BankSmsException : ComponentException
    {
        public BankSmsException(int statusCode, string codeDescription)
            : base("СМС через Банк", statusCode, codeDescription)
        {
        }
    }
}
