namespace Vtb24.Arms.AdminServices.AdminSecurityService.Exceptions
{
    public class AdminSecuritySelfEditException : AdminSecurityServiceException
    {
        public AdminSecuritySelfEditException()
            : base(ResultCodes.SelfEditIsInvalid, "������ ��� �������������� ����������� ������� ������", null)
        {
        }
    }
}
