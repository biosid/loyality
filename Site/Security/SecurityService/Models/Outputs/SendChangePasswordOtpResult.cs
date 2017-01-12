using System;
using Vtb24.Site.Security.OneTimePasswordService.Models;

namespace Vtb24.Site.Security.SecurityService.Models.Outputs
{
    public class SendChangePasswordOtpResult
    {
        public string OtpToken { get; set; }

        public DateTime CreationTimeUtc { get; set; }

        public DateTime ExpirationTimeUtc { get; set; }

        public OtpDeliveryMeans DeliveryMeans { get; set; }
    }
}