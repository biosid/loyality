using FluentValidation;

namespace Vtb24.Site.Security.SecurityService.Models.Inputs
{
    public class ResetPasswordOptions
    {
        public string Login { get; set; }

        /// <summary>
        /// Шаблон сообщения с новым паролем.
        /// {0} - пароль
        /// </summary>
        public string NotificationMessageTemplate { get; set; }

        internal void ValidateAndThrow()
        {
            new InlineValidator<ResetPasswordOptions>
            {
                v => v.RuleFor(m => m.Login).NotEmpty().WithMessage("Не указан логин пользователя"),
                v => v.RuleFor(m => m.NotificationMessageTemplate).NotEmpty().WithMessage("Не указан шаблон OTP сообщения"),
            }
            .ValidateAndThrow(this);
        } 
    }
}