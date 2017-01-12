using System.ServiceModel;
using Vtb24.Site.Security.SecurityTokenService.Models.Inputs;
using Vtb24.Site.Security.SecurityTokenService.Models.Outputs;

namespace Vtb24.Site.SecurityWebServices.SecurityToken
{
    [ServiceContract]
    public interface ISecurityTokenWebApi
    {
        [OperationContract]
        ValidateSecurityTokenResult Validate(ValidateSecurityTokenParameters parameters);

        [OperationContract]
        string Echo(string message);
    }
}
