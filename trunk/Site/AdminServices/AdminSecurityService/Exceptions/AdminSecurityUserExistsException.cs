namespace Vtb24.Arms.AdminServices.AdminSecurityService.Exceptions
{
    public class AdminSecurityUserExistsException : AdminSecurityServiceException
    {
        public string Login { get; private set; }

        public AdminSecurityUserExistsException(string login)
            : base(ResultCodes.UserAlreadyExists, "ѕользователь с логином \"" + login + "\" уже существует")
        {
            Login = login;
        }
    }
}
