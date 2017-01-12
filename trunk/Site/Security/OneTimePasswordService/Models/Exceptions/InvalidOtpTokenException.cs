namespace Vtb24.Site.Security.OneTimePasswordService.Models.Exceptions
{
    public class InvalidOtpTokenException : OneTimePasswordServiceException
    {
        public string OtpToken { get; private set; }


        internal InvalidOtpTokenException(string otpToken) : base(CreateMessage(otpToken))
        {
            OtpToken = otpToken;
        }

        private static string CreateMessage(string otpToken)
        {
            return string.Format("Неправильный токен OTP ({0}).", otpToken);
        }
    }
}