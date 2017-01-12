namespace Vtb24.Site.Security.SecurityService.Models.Exceptions
{
    public class UserLockedOutException : SecurityServiceException
    {
        public UserLockedOutException(int intervalInMinutes) 
            : base("Превышено допустимое количество попыток ввода пароля.")
        {
            IntervalInMinutes = intervalInMinutes;
        }

        public int IntervalInMinutes { get; set; }
    }
}