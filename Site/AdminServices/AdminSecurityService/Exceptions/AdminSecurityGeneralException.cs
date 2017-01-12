namespace Vtb24.Arms.AdminServices.AdminSecurityService.Exceptions
{
    public class AdminSecurityGeneralException : AdminSecurityServiceException
    {
        public AdminSecurityGeneralException(string reason)
            : base(ResultCodes.General, "Общая ошибка: " + reason)
        {
        }
    }
}
