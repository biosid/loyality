using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices.ActionsManagement.Models
{
    public class ActionsBaseRuleConflictException : ComponentException
    {
        public ActionsBaseRuleConflictException(int resultCode, string codeDescription)
            : base("Механики", resultCode, codeDescription)
        {
        }
    }
}
