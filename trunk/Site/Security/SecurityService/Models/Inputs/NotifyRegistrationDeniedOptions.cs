using FluentValidation;
using Vtb24.Site.Security.Models;

namespace Vtb24.Site.Security.SecurityService.Models.Inputs
{
    public class NotifyRegistrationDeniedOptions
    {
        public string PhoneNumber { get; set; }

        public string DeniedMessage { get; set; }

        public BankSmsType DeniedSmsType { get; set; }

        internal void ValidateAndThrow()
        {
            new InlineValidator<NotifyRegistrationDeniedOptions>
            {
                v => v.RuleFor(m => m.PhoneNumber).NotEmpty().WithMessage("Не указан телефон"),
                v => v.RuleFor(m => m.PhoneNumber).Matches(@"7\d{10}").WithMessage("Неверный формат телефона (11 цифр, первая -- 7)"),
                v => v.RuleFor(m => m.DeniedMessage).NotEmpty().WithMessage("Не указано сообщение")
            }
                .ValidateAndThrow(this);
        }
    }
}
