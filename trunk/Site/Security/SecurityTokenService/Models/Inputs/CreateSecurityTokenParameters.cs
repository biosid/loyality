using FluentValidation;

namespace Vtb24.Site.Security.SecurityTokenService.Models.Inputs
{
    public class CreateSecurityTokenParameters
    {
        public string PrincipalId { get; set; }

        public string ExternalId { get; set; }

        public int? CategoryId { get; set; }

        public void ValidateAndThrow()
        {
            new InlineValidator<CreateSecurityTokenParameters>
            {
                v=>v.RuleFor(m=>m.PrincipalId).NotEmpty().WithMessage("Не указан идентификатор учетной записи (PrincipalId)")
            }.ValidateAndThrow(this);
        }
    }
}