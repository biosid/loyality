namespace Vtb24.Arms.AdminServices.AdminSecurityService.Exceptions
{
    public class AdminSecuritySelfEditException : AdminSecurityServiceException
    {
        public AdminSecuritySelfEditException()
            : base(ResultCodes.SelfEditIsInvalid, "ошибка при редактировании собственной учётной записи", null)
        {
        }
    }
}
