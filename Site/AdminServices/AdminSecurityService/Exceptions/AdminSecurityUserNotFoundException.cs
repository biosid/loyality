namespace Vtb24.Arms.AdminServices.AdminSecurityService.Exceptions
{
    public class AdminSecurityUserNotFoundException : AdminSecurityServiceException
    {
        public string Login { get; private set; }

        public AdminSecurityUserNotFoundException(string login)
            : base(ResultCodes.UserNotFound, "Пользователь с логином \"" + login + "\" не найден")
        {
            Login = login;
        }
    }
}
