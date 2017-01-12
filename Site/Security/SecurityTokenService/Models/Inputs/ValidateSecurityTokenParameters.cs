using FluentValidation;

namespace Vtb24.Site.Security.SecurityTokenService.Models.Inputs
{
    public class ValidateSecurityTokenParameters
    {
        public string SecurityToken { get; set; }

        public void ValidateAndThrow()
        {
            new InlineValidator<ValidateSecurityTokenParameters>
            {
                v=>v.RuleFor(m=>m.SecurityToken).NotEmpty().WithMessage("Не задан токен (SecurityToken)")
            }.ValidateAndThrow(this);
        }
    }
}