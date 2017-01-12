using System.Collections.Generic;
using Vtb24.Site.Security.Models;
using Vtb24.Site.Security.SecurityService.Models;
using Vtb24.Site.Security.SecurityService.Models.Inputs;
using Vtb24.Site.Security.SecurityService.Models.Outputs;

namespace Vtb24.Site.Security
{
    public interface ISecurityService
    {
        bool OtpViaEmailEnabled { get; }

        ClientPrincipal GetPrincipal();

        User GetUserByPhoneNumber(string phone);

        User[] GetUsersByClientId(string clientId, SecurityPagingSettings paging = null);

        bool Login(string login, string password);

        bool ChangePassword(ChangePasswordOptions options);

        void ResetPassword(ResetPasswordOptions options);

        SendChangePasswordOtpResult SendChangePasswordOtp(SendChangePasswordOtpOptions options);

        void ChangePasswordWithOtp(ChangePasswordWithOtpOptions options);

        void Logout();

        void CreateUser(CreateClientAccountOptions options);

        void CreateUserAndPassword(CreateUserAndPasswordOptions options);

        void DeleteUser(string login);

        void DisableUser(string login, bool disable);

        IDictionary<string, User> BatchResolveUsersByClientId(string[] clientIds);

        IDictionary<string, User> BatchResolveUsersByPhone(string[] clientPhones);

        void ChangeUserPhoneNumber(ChangeUserPhoneNumberOptions options);

        PhoneNumberChangeHistory GetChangeUserPhoneNumberHistory(int userId, SecurityPagingSettings paging = null);

        void NotifyRegistrationDenied(NotifyRegistrationDeniedOptions options);
    }
}
