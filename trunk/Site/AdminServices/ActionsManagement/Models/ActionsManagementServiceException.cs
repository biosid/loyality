using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices.ActionsManagement.Models
{
    public class ActionsManagementServiceException : ComponentException
    {
        public ActionsManagementServiceException(int resultCode, string codeDescription)
            : base("Механики", resultCode, codeDescription)
        {
        }
    }
}
