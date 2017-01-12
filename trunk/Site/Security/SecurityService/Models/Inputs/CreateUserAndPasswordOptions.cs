using FluentValidation;

namespace Vtb24.Site.Security.SecurityService.Models.Inputs
{
    public class CreateUserAndPasswordOptions
    {
        public string ClientId { get; set; }

        public string PhoneNumber { get; set; }

        public string Password { get; set; }

        public bool ForcePasswordChange { get; set; }

        internal void ValidateAndThrow()
        {
            new InlineValidator<CreateUserAndPasswordOptions>
            {
                v => v.RuleFor(m => m.ClientId).NotEmpty().WithMessage("Не указан идентификатор клиента"),
                v => v.RuleFor(m => m.PhoneNumber).NotEmpty().WithMessage("Не указан телефон"),
                v => v.RuleFor(m => m.PhoneNumber).Matches(@"7\d{10}").WithMessage("Неверный формат телефона (11 цифр, первая -- 7)"),
                v => v.RuleFor(m => m.Password).NotEmpty().WithMessage("Не указан пароль")
            }
            .ValidateAndThrow(this);
        } 
    }
}