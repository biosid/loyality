namespace Vtb24.Site.Security.SecurityService.Models.Exceptions
{
    public class UserNotFoundException : SecurityServiceException
    {
        public UserNotFoundException(string login) : base(CreateMessage(login))
        {
        }

        public UserNotFoundException(int userId) : base(CreateMessage(userId))
        {
        }

        private static string CreateMessage(string login)
        {
            return string.Format("Ошибка безопасности. Пользователь {0} не найден", login);
        }

        private static string CreateMessage(int userId)
        {
            return string.Format("Ошибка безопасности. Пользователь c Id {0} не найден", userId);
        }
    }
}