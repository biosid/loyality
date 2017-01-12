using Vtb24.Site.Security;
using Vtb24.Site.Security.SecurityTokenService.Models.Inputs;
using Vtb24.Site.Security.SecurityTokenService.Models.Outputs;
using Vtb24.Site.SecurityWebServices.SecurityToken;

namespace Vtb24.Site.SecurityWebServices
{
    public class SecurityTokenWebApi : ISecurityTokenWebApi
    {
        public SecurityTokenWebApi(ISecurityTokenService tokens)
        {
            _tokens = tokens;
        }

        private readonly ISecurityTokenService _tokens;

        public ValidateSecurityTokenResult Validate(ValidateSecurityTokenParameters parameters)
        {
            return _tokens.Validate(parameters);
        }

        public string Echo(string message)
        {
            return string.Format("Echo: {0}", message);
        }
    }
}
