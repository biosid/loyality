using System;
using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices.AdminSecurityService.Exceptions
{
    public class AdminSecurityServiceException : ComponentException
    {
        public AdminSecurityServiceException(ResultCodes resultCode, string codeDescription, Exception innerException = null)
            : base("Безопасность АРМ", (int) resultCode, codeDescription, innerException)
        {
        }
    }
}
