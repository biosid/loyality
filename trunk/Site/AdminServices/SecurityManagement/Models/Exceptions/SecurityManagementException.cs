using System;

namespace Vtb24.Arms.AdminServices.SecurityManagement.Models.Exceptions
{
    public class SecurityManagementException : Exception
    {
        public SecurityManagementException(string message)
            : base(message)
        {
        }
    }
}
