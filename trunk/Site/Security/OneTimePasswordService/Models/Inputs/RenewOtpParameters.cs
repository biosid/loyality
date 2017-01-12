using FluentValidation;

namespace Vtb24.Site.Security.OneTimePasswordService.Models.Inputs
{
    public class RenewOtpParameters
    {
        public string OtpToken { get; set; }

        public void ValidateAndThrow()
        {
            new InlineValidator<RenewOtpParameters>
            {
                v=>v.RuleFor(m=>m.OtpToken).NotEmpty().WithMessage("Не указан токен одноразового пароля (OtpToken)")
            }.ValidateAndThrow(this);
        }
    }
}