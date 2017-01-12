using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Vtb24.Site.Infrastructure;
using Vtb24.Site.Models.Otp;

namespace Vtb24.Site.Models.Account
{
    public class ResetPasswordModel : IOtpModel
    {
        [Mask("+7 (999) 999-9999", ErrorMessage = "Номер телефона должен быть в формате +7 (xxx) xxx-xxxx")]
        public string Phone { get; set; }

        public string OtpToken { get; set; }

        [Required(ErrorMessage = "Введите код подтверждения")]
        public string Otp { get; set; }

        [Required(ErrorMessage = "Укажите E-mail")]
        [StringLength(250, ErrorMessage = "Превышена допустимая длина E-mail (250 символов)")]
        [SimpleEmailAddressAttribute(ErrorMessage = "Неверный формат E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Повторно введите E-mail")]
        [StringLength(250, ErrorMessage = "Превышена допустимая длина E-mail (250 символов)")]
        [SimpleEmailAddress(ErrorMessage = "Неверный формат E-mail")]
        [System.ComponentModel.DataAnnotations.Compare("Email", ErrorMessage = "Введенные адреса E-mail не совпадают")]
        public string ConfirmEmail { get; set; }

        public DateTime ExpirationTimeUtc { get; set; }

        public bool IsEmailRequired { get; set; }

        public string Hint { get; set; }

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

        [AllowHtml]
        [Required(ErrorMessage = "Введите новый пароль")]
        [RegularExpressionAttribute(Constants.PASSWORD_REGEX, ErrorMessage = Constants.PASSWORD_ERROR_MESSAGE)]
        public string NewPassword { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "Подтвердите пароль")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "Введённые пароли не совпадают")]
        public string ConfirmNewPassword { get; set; } 
    }
}