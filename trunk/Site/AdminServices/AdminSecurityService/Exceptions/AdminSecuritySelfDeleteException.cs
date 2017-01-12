namespace Vtb24.Arms.AdminServices.AdminSecurityService.Exceptions
{
    public class AdminSecuritySelfDeleteException : AdminSecurityServiceException
    {
        public AdminSecuritySelfDeleteException()
            : base(ResultCodes.SelfDeleteIsAttempted, "попытка удалить собственную учётную запись", null)
        {
        }
    }
}
