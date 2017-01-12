using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices.AdminVtbBankConnector.Models
{
    public class AdminVtbBankConnectorServiceException : ComponentException
    {
        public AdminVtbBankConnectorServiceException(int resultCode, string codeDescription)
            : base("Админ-коннектор к банку", resultCode, codeDescription)
        {
        }
    }
}
