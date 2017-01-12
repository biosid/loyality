using FluentValidation;

namespace Vtb24.Site.Security.SecurityService.Models.Inputs
{
    public class SendChangePasswordOtpOptions
    {
        public string Login { get; set; }

        public string Email { get; set; }

        public bool ForceSms { get; set; }

        public string SmsOtpMessageTemplate { get; set; }

        public string EmailOtpMessageTemplate { get; set; }

        internal void ValidateAndThrow()
        {
            new InlineValidator<SendChangePasswordOtpOptions>
            {
                v => v.RuleFor(m => m.Login).NotEmpty().WithMessage("Не указан логин пользователя"),
                v => v.RuleFor(m => m.SmsOtpMessageTemplate).NotEmpty().WithMessage("Не указан шаблон OTP СМС-сообщения"),
                v => v.RuleFor(m => m.EmailOtpMessageTemplate).NotEmpty().WithMessage("Не указан шаблон OTP Email-сообщения"),
            }
            .ValidateAndThrow(this);
        }
    }
}
