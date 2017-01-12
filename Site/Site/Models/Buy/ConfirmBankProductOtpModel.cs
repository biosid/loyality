using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Vtb24.Site.Models.Otp;

namespace Vtb24.Site.Models.Buy
{
    [Bind(Exclude = "DisplayResendOtp")]
    public class ConfirmBankProductOtpModel : IOtpModel
    {
        public int OrderDraftId { get; set; }

        public string OtpToken { get; set; }

        [Required(ErrorMessage = "Введите код подтверждения")]
        public string Otp { get; set; }

        public DateTime ExpirationTimeUtc { get; set; }

        public int ExpiresInSeconds
        {
            get
            {
                var sec = (int)(ExpirationTimeUtc - DateTime.UtcNow).TotalSeconds;
                return sec > 0 ? sec : 0;
            }
        }

        public bool DisplayResendOtp
        {
            get
            {
                return ExpiresInSeconds <= 0;
            }
        }

        public bool DisableButton { get; set; }
    }
}
