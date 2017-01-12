using FluentValidation;

namespace Vtb24.Site.Security.OneTimePasswordService.Models.Inputs
{
    public class ConfirmOtpParameters
    {
        public string OtpToken { get; set; }

        public string Otp { get; set; }

        public void ValidateAndThrow()
        {
            new InlineValidator<ConfirmOtpParameters>
            {
                v=>v.RuleFor(m=>m.OtpToken).NotEmpty().WithMessage("Не указан OTP токен"),
                v=>v.RuleFor(m=>m.Otp).NotEmpty().WithMessage("Не указан одноразовый пароль")
            }.ValidateAndThrow(this);
        }
    }
}