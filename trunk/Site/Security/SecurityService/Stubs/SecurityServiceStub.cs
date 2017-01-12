using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using Vtb24.Site.Security.Models;
using Vtb24.Site.Security.SecurityService.Models;
using Vtb24.Site.Security.SecurityService.Models.Exceptions;
using Vtb24.Site.Security.SecurityService.Models.Inputs;
using Vtb24.Site.Security.SecurityService.Models.Outputs;

namespace Vtb24.Site.Security.SecurityService.Stubs
{
    public class SecurityServiceStub : ISecurityService
    {
        public const string STUB_MESSAGE = "Сервис безопасности работает на заглушках";

        public bool OtpViaEmailEnabled { get { return false; } }

        public ClientPrincipal GetPrincipal()
        {
            // залогинен
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var login = HttpContext.Current.User.Identity.Name;
                if (SecurityServiceStubData.Accounts.ContainsKey(login))
                {
                    return SecurityServiceStubData.Accounts[login];
                }
                throw new UserNotFoundException(login);
            }
            // не залогинен
            return new ClientPrincipal
            {
                ClientId = null,
                ClientAnonymousId = HttpContext.Current.Request.AnonymousID,
                ClientIp = "127.0.0.1"
            };
        }

        public User GetUserByPhoneNumber (string phone)
        {
            throw new NotImplementedException(STUB_MESSAGE);
        }

        public User[] GetUsersByClientId(string clientId, SecurityPagingSettings paging = null)
        {
            throw new NotImplementedException(STUB_MESSAGE);
        }

        public bool Login(string login, string password)
        {
            if (SecurityServiceStubData.Accounts.ContainsKey(login) || password == "deleted")
            {
                if (password == "locked")
                {
                    throw new UserLockedOutException(30);
                }

                if (password == "disabled")
                {
                    throw new UserDisabledException( new User () );    
                }

                if (password == "tmp")
                {
                    throw new PasswordNotSetException();
                }

                FormsAuthentication.SetAuthCookie(login, true);
                return true;
            }

            return false;
        }

        public bool ChangePassword(ChangePasswordOptions options)
        {
            return true;
        }

        public void ResetPassword(ResetPasswordOptions options)
        {
            throw new NotImplementedException(STUB_MESSAGE);
        }

        public SendChangePasswordOtpResult SendChangePasswordOtp(SendChangePasswordOtpOptions options)
        {
            options.ValidateAndThrow();

            return new SendChangePasswordOtpResult
            {
                CreationTimeUtc = DateTime.UtcNow,
                ExpirationTimeUtc = DateTime.UtcNow.AddMinutes(5),
                OtpToken = Guid.NewGuid().ToString("N")
            };
        }

        public SendChangePasswordOtpResult ResendChangePasswordOtp(string login, string otpToken)
        {
            return new SendChangePasswordOtpResult
            {
                CreationTimeUtc = DateTime.UtcNow,
                ExpirationTimeUtc = DateTime.UtcNow.AddMinutes(5),
                OtpToken = Guid.NewGuid().ToString("N")
            };
        }

        public void ChangePasswordWithOtp(ChangePasswordWithOtpOptions options)
        {
        }

        public void CreateUser(CreateClientAccountOptions options)
        {
            options.ValidateAndThrow();
        }

        public void CreateUserAndPassword(CreateUserAndPasswordOptions options)
        {
            throw new NotImplementedException(STUB_MESSAGE);
        }

        public void DeleteUser(string login)
        {
        }

        public void Logout()
        {
            FormsAuthentication.SignOut();
        }


        public void DisableUser(string login, bool disable)
        {
            throw new NotImplementedException(STUB_MESSAGE);
        }

        public IDictionary<string, User> BatchResolveUsersByClientId(string[] clientIds)
        {
            throw new NotImplementedException(STUB_MESSAGE);
        }

        public IDictionary<string, User> BatchResolveUsersByPhone(string[] clientPhones)
        {
            throw new NotImplementedException(STUB_MESSAGE);
        }

        public void ChangeUserPhoneNumber(ChangeUserPhoneNumberOptions options)
        {
            throw new NotImplementedException(STUB_MESSAGE);
        }

        public PhoneNumberChangeHistory GetChangeUserPhoneNumberHistory(int userId, SecurityPagingSettings paging = null)
        {
            throw new NotImplementedException(STUB_MESSAGE);
        }

        public void NotifyRegistrationDenied(NotifyRegistrationDeniedOptions options)
        {
            throw new NotImplementedException();
        }
    }
}