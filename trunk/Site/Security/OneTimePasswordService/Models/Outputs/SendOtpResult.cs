using System;

namespace Vtb24.Site.Security.OneTimePasswordService.Models.Outputs
{
    public class SendOtpResult
    {
        public string OtpToken { get; set; }

        public DateTime CreationTimeUtc { get; set; }

        public DateTime ExpirationTimeUtc { get; set; }

        public int AttemptsRemaining { get; set; }
    }
}