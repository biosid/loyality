using FluentValidation;

namespace Vtb24.Site.Security.SecurityService.Models.Inputs
{
    public class ChangePasswordOptions
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public string NewPassword { get; set; }

        public void ValidateAndThrow()
        {
            new InlineValidator<ChangePasswordOptions>
            {
                v=>v.RuleFor(m=>m.Login).NotEmpty().WithMessage("Не передан логин"),
                v=>v.RuleFor(m=>m.Password).NotEmpty().WithMessage("Не передан пароль"),
                v=>v.RuleFor(m=>m.NewPassword).NotEmpty().WithMessage("Не передан новый пароль"),
            }.ValidateAndThrow(this);
        }
    }
}