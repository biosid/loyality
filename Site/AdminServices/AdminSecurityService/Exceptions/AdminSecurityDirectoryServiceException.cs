using System;

namespace Vtb24.Arms.AdminServices.AdminSecurityService.Exceptions
{
    public class AdminSecurityDirectoryServiceException : AdminSecurityServiceException
    {
        public AdminSecurityDirectoryServiceException(string operationDescription, Exception innerException)
            : base(ResultCodes.ActiveDirectoryError, operationDescription + ": ������ ��� ��������� � ActiveDirectory", innerException)
        {
        }
    }
}
