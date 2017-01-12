namespace Vtb24.Site.Security.SecurityService.Models.Exceptions
{
    public class UserDisabledException : SecurityServiceException
    {
        public UserDisabledException(User account) : base(CreateMessage(account))
        {
        }

        private static string CreateMessage(User user)
        {
            return string.Format(
                "Ошибка безопасности. Учётная запись пользователя {0} с Id {1} заблокирована",
                user.PhoneNumber,
                user.Id
                );
        }
    }
}