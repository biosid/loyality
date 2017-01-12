using System;

namespace Vtb24.Site.Security.OneTimePasswordService.Models.Outputs
{
    public class ConfirmOtpResult
    {
        public bool Confirmed { get; set; }

        public DateTime CreationTimeUtc { get; set; }

        public DateTime ExpirationTimeUtc { get; set; }

        public int AttemptsRemaining { get; set; }

        public static implicit operator bool(ConfirmOtpResult r)
        {
            return r.Confirmed;
        }
    }
}