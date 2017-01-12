using System;

namespace Vtb24.Site.Security.SecurityService.Models.Outputs
{
    public class SendResetPasswordOtpResult
    {
        public string OtpToken { get; set; }

        public DateTime CreationTimeUtc { get; set; }

        public DateTime ExpirationTimeUtc { get; set; }
    }
}