using FluentValidation;

namespace Vtb24.Site.SecurityWebServices.Security.Models.Inputs
{
    public class ResetUserPasswordOptions
    {
        public string Login { get; set; }

        public void ValidateAndThrow()
        {
            new InlineValidator<ResetUserPasswordOptions>
            {
                v => v.RuleFor(m => m.Login).NotEmpty().WithMessage("Не передан логин"),
            }.ValidateAndThrow(this);
        }
    }
}
