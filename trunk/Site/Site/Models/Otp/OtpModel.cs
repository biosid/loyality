using System;
using System.ComponentModel.DataAnnotations;

namespace Vtb24.Site.Models.Otp
{
    public interface IOtpModel
    {
        string OtpToken { get; set; }
        string Otp { get; set; }
        DateTime ExpirationTimeUtc { get; set; }
        int ExpiresInSeconds { get; }
        bool DisplayResendOtp { get; }
    }
}