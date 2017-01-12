using Vtb24.Site.Services.Models.Exceptions;

namespace Vtb24.Site.Services.VtbBankConnector.Models.Exceptions
{
    public class VtbBankConnectorServiceException : ComponentException
    {
        public VtbBankConnectorServiceException(int statusCode, string codeDescription)
            : base("Коннектор к банку", statusCode, codeDescription)
        {
        }
    }
}
