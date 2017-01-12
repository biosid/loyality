using FluentValidation;
using Vtb24.Site.Security.Models;

namespace Vtb24.Site.Security.SecurityService.Models.Inputs
{
    public class CreateClientAccountOptions
    {
        public string ClientId { get; set; }

        public string PhoneNumber { get; set; }

        /// <summary>
        /// Шаблон сообщения об успешной регистрации.
        /// {0} - логин
        /// {1} - пароль
        /// </summary>
        public string WelcomeMessageTemplate { get; set; }

        public BankSmsType WelcomeSmsType { get; set; }

        internal void ValidateAndThrow()
        {
            new InlineValidator<CreateClientAccountOptions>
            {
                v => v.RuleFor(m => m.ClientId).NotEmpty().WithMessage("Не указан идентификатор клиента"),
                v => v.RuleFor(m => m.PhoneNumber).NotEmpty().WithMessage("Не указан телефон"),
                v => v.RuleFor(m => m.PhoneNumber).Matches(@"7\d{10}").WithMessage("Неверный формат телефона (11 цифр, первая -- 7)"),
                v => v.RuleFor(m => m.WelcomeMessageTemplate).NotEmpty().WithMessage("Не указан шаблон сообщения")
            }
            .ValidateAndThrow(this);
        }
    }
}
