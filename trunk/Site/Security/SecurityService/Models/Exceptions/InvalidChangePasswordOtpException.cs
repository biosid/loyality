namespace Vtb24.Site.Security.SecurityService.Models.Exceptions
{
    public class InvalidChangePasswordOtpException : SecurityServiceException
    {
        public InvalidChangePasswordOtpException(string login) : base(string.Format("Неверный одноразовый пароль для сброса пароля пользователя {0}", login))
        {
        }
    }
}