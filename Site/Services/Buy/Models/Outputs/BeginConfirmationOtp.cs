using System;

namespace Vtb24.Site.Services.Buy.Models.Outputs
{
    public class BeginConfirmationOtp
    {
        public int OrderId { get; set; }

        public string OtpToken { get; set; }

        public DateTime CreationTimeUtc { get; set; }

        public DateTime ExpirationTimeUtc { get; set; }
    }
}