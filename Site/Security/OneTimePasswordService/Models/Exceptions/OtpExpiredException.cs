using System;

namespace Vtb24.Site.Security.OneTimePasswordService.Models.Exceptions
{
    public class OtpExpiredException : OneTimePasswordServiceException
    {
        public DateTime CreationTimeUtc { get; private set; }

        public DateTime ExpirationTimeUtc { get; private set; }

        internal OtpExpiredException(OtpToken token) : base(CreateMessage(token))
        {
            CreationTimeUtc = token.CreationTimeUtc;
            ExpirationTimeUtc = token.ExpirationTimeUtc;
        }

        private static string CreateMessage(OtpToken token)
        {
            return string.Format("Время действия OTP истекло ({0})", token.ExpirationTimeUtc);
        }
    }
}