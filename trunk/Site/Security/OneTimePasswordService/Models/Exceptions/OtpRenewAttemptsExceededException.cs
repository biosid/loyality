namespace Vtb24.Site.Security.OneTimePasswordService.Models.Exceptions
{
    public class OtpRenewAttemptsExceededException : OneTimePasswordServiceException
    {
        public int AllowedAttempts { get; private set; }

        internal OtpRenewAttemptsExceededException(int attempts) : base(CreateMessage(attempts))
        {
            AllowedAttempts = attempts;
        }

        private static string CreateMessage(int attempts)
        {
            return string.Format(
                "Превышено допустимое количество попыток пересылки OTP ({0})",
                attempts
            );
        }
    }
}