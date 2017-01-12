namespace Vtb24.Site.Security.OneTimePasswordService.Models.Exceptions
{
    public class OtpAllowedAttemptsExceeded : OneTimePasswordServiceException
    {
        public int AttemptsToConfirm { get; private set; }

        internal OtpAllowedAttemptsExceeded(OtpToken token) : base(CreateMessage(token))
        {
            AttemptsToConfirm = token.AttemptsToConfirm;
        }

        private static string CreateMessage(OtpToken token)
        {
            return string.Format("Превышено кол-во попыток подтверждения OTP ({0})", token.AttemptsToConfirm);
        }
    }
}