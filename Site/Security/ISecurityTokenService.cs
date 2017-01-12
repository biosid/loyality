using Vtb24.Site.Security.SecurityTokenService.Models.Inputs;
using Vtb24.Site.Security.SecurityTokenService.Models.Outputs;

namespace Vtb24.Site.Security
{
    public interface ISecurityTokenService
    {
        CreateSecurityTokenResult Create(CreateSecurityTokenParameters parameters);

        ValidateSecurityTokenResult Validate(ValidateSecurityTokenParameters parameters);

        void Invalidate(InvalidateSecurityTokenParameters parameters);
    }
}