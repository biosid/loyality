using FluentValidation;

namespace Vtb24.Site.Security.SecurityTokenService.Models.Inputs
{
    public class InvalidateSecurityTokenParameters
    {
        public string PrincipalId { get; set; }

        public void ValidateAndThrow()
        {
            new InlineValidator<InvalidateSecurityTokenParameters>
            {
                v=>v.RuleFor(m=>m.PrincipalId).NotEmpty().WithMessage("Не указан идентификатор учетной записи (PrincipalId)")
            }.ValidateAndThrow(this);
        }
    }
}