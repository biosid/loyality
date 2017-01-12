using System;

namespace Vtb24.Arms.AdminServices.AdminSecurityService.Exceptions
{
    public class AdminSecuritySetPasswordException : AdminSecurityServiceException
    {
        public AdminSecuritySetPasswordException(Exception innerException = null)
            : base(ResultCodes.SetPasswordError, "������ ��� ������� ���������� ������ ������������", innerException)
        {
        }
    }
}
