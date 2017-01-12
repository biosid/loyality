using System.Collections.Generic;
using System.ServiceModel;
using Vtb24.Site.Security.SecurityService.Models;
using Vtb24.Site.Security.SecurityService.Models.Inputs;
using Vtb24.Site.SecurityWebServices.Security.Models.Inputs;
using Vtb24.Site.SecurityWebServices.Security.Models.Outputs;

namespace Vtb24.Site.SecurityWebServices.Security
{
    [ServiceContract]
    public interface ISecurityWebApi
    {
        [OperationContract]
        void CreateUser(CreateUserOptions options);

        [OperationContract]
        void DenyRegistrationRequest(DenyRegistrationRequestOptions options);

        [OperationContract]
        void CreateUserAndPassword(CreateUserAndPasswordOptions options);

        [OperationContract]
        void DeleteUser(string login);

        [OperationContract]
        void DisableUser(string login);

        [OperationContract]
        void EnableUser(string login);

        [OperationContract]
        IDictionary<string, User> BatchResolveUsersByClientId(string[] clientIds);

        [OperationContract]
        IDictionary<string, User> BatchResolveUsersByPhone(string[] clientPhones);

        [OperationContract]
        ChangeUserPhoneNumberResult ChangeUserPhoneNumber(ChangePhoneNumberOptions options);

        [OperationContract]
        ResetUserPasswordResult ResetUserPassword(ResetUserPasswordOptions options);

        [OperationContract]
        string Echo(string message);
    }
}
