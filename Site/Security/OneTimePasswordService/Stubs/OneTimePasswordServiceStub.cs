using System;
using Vtb24.Site.Security.OneTimePasswordService.Models;
using Vtb24.Site.Security.OneTimePasswordService.Models.Inputs;
using Vtb24.Site.Security.OneTimePasswordService.Models.Outputs;

namespace Vtb24.Site.Security.OneTimePasswordService.Stubs
{
    public class OneTimePasswordServiceStub : IOneTimePasswordService
    {
        public SendOtpResult Send(SendOtpParameters parameters)
        {
            return new SendOtpResult
            {
                OtpToken = Guid.NewGuid().ToString("N"),
                AttemptsRemaining = 5,
                CreationTimeUtc = DateTime.UtcNow,
                ExpirationTimeUtc = DateTime.UtcNow.AddMinutes(5)
            };
        }

        public RenewOtpResult Renew(RenewOtpParameters parameters)
        {
            return new RenewOtpResult
            {
                OtpToken = Guid.NewGuid().ToString("N"),
                AttemptsRemaining = 5,
                CreationTimeUtc = DateTime.UtcNow,
                ExpirationTimeUtc = DateTime.UtcNow.AddMinutes(5)
            };
        }

        public ConfirmOtpResult Confirm(ConfirmOtpParameters parameters)
        {
            return new ConfirmOtpResult
            {
                Confirmed = true
            };
        }

        public bool IsConfirmed(IsConfirmedOtpParameters parameters)
        {
            return true;
        }

        public OtpDeliveryMeans? GetDeliveryMeans(GetDeliveryMeansParameters parameters)
        {
            return OtpDeliveryMeans.Sms;
        }
    }
}