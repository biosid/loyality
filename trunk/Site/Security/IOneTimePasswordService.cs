using Vtb24.Site.Security.OneTimePasswordService.Models;
using Vtb24.Site.Security.OneTimePasswordService.Models.Inputs;
using Vtb24.Site.Security.OneTimePasswordService.Models.Outputs;

namespace Vtb24.Site.Security
{
    public interface IOneTimePasswordService
    {
        SendOtpResult Send(SendOtpParameters parameters);

        RenewOtpResult Renew(RenewOtpParameters parameters);

        ConfirmOtpResult Confirm(ConfirmOtpParameters parameters);

        bool IsConfirmed(IsConfirmedOtpParameters parameters);

        OtpDeliveryMeans? GetDeliveryMeans(GetDeliveryMeansParameters parameters);
    }
}