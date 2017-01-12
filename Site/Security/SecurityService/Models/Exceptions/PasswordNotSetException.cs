namespace Vtb24.Site.Security.SecurityService.Models.Exceptions
{
    public class PasswordNotSetException : SecurityServiceException
    {
        public PasswordNotSetException() : base("Необходимо сменить временный пароль")
        {
        }
    }
}