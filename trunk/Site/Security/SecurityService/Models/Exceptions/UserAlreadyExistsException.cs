namespace Vtb24.Site.Security.SecurityService.Models.Exceptions
{
    public class UserAlreadyExistsException : SecurityServiceException
    {
        public UserAlreadyExistsException(string login) : base(string.Format("Пользователь {0} уже существует", login))
        {
        }
    }
}