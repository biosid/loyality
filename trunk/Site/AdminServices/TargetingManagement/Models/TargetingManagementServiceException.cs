using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices.TargetingManagement.Models
{
    public class TargetingManagementServiceException : ComponentException
    {
        public TargetingManagementServiceException(int resultCode, string codeDescription)
            : base("Целевые аудитории", resultCode, codeDescription)
        {
        }
    }
}
