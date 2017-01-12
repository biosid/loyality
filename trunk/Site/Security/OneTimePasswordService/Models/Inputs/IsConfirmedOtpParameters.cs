using FluentValidation;

namespace Vtb24.Site.Security.OneTimePasswordService.Models.Inputs
{
    public class IsConfirmedOtpParameters
    {
        public string OtpToken { get; set; }

        public string OtpType { get; set; }

        public string ExternalId { get; set; }

        public void ValidateAndThrow()
        {
            new InlineValidator<IsConfirmedOtpParameters>
            {
                v=>v.RuleFor(m=>m.OtpToken).NotEmpty().WithMessage("Не задан OTP токен"),
                v=>v.RuleFor(m=>m.OtpType).NotEmpty().WithMessage("Не указан тип одноразового пароля (OtpType)")
            }.ValidateAndThrow(this);
        }
    }
}