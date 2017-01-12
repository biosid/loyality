using FluentValidation;

namespace Vtb24.Site.Security.SecurityService.Models.Inputs
{
    public class ChangePasswordWithOtpOptions
    {
        public string Login { get; set; }

        public string OtpToken { get; set; }

        public string NewPassword { get; set; }

        public void ValidateAndThrow()
        {
            new InlineValidator<ChangePasswordWithOtpOptions>
            {
                v=>v.RuleFor(m=>Login).NotEmpty().WithMessage("Не указан логин"),
                v=>v.RuleFor(m=>OtpToken).NotEmpty().WithMessage("Не указан OTP токен"),
                v=>v.RuleFor(m=>NewPassword).NotEmpty().WithMessage("Не указан новый пароль"),
            }.ValidateAndThrow(this);
        }
    }
}