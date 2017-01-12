using System.Configuration;
using System.Linq;
using Vtb24.Arms.AdminServices.AdminSecurityService.Helpers;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;
using Vtb24.Arms.AdminServices.SecurityManagement.Models;
using Vtb24.Site.Security;
using Vtb24.Site.Security.Models;
using Vtb24.Site.Security.SecurityService.Models.Inputs;
using Vtb24.Site.Security.SecurityService.Models.Outputs;
using Vtb24.Site.Services.ClientTargeting;
using Vtb24.Site.Services.MyInfoService;
using Vtb24.Site.Services.MyInfoService.Models;
using Vtb24.Site.Services.MyInfoService.Models.Exceptions;
using Vtb24.Site.Services.Processing;
using Vtb24.Site.Services.Processing.Models.Exceptions;
using Vtb24.Site.Services.Processing.Models.Inputs;
using Vtb24.Site.Services.Profile.Models;
using Vtb24.Site.Services.VtbBankConnector;

namespace Vtb24.Arms.AdminServices.SecurityManagement
{
    public class SecurityManagement : ISecurityManagement
    {
        public SecurityManagement(IAdminSecurityService adminSecurity, ISecurityService security, IClientTargeting targeting, IProcessing processing, IVtbBankConnectorService bankConnector, IMyInfoServiceInternal info)
        {
            _adminSecurity = adminSecurity;
            _security = security;
            _targeting = targeting;
            _processing = processing;
            _bankConnector = bankConnector;
            _info = info;
        }

        private readonly IAdminSecurityService _adminSecurity;
        private readonly ISecurityService _security;
        private readonly IClientTargeting _targeting;
        private readonly IProcessing _processing;
        private readonly IVtbBankConnectorService _bankConnector;
        private readonly IMyInfoServiceInternal _info;

        public string GetClientIdByLogin(string login)
        {
            _adminSecurity.CurrentPermissions.AssertAllGranted(PermissionKeys.Security_Login,
                                                               PermissionKeys.Security_Clients);

            var user = _security.GetUserByPhoneNumber(login);

            return user != null ? user.ClientId : null;
        }

        public string GetLoginByClientId(string clientId)
        {
            _adminSecurity.CurrentPermissions.AssertAllGranted(PermissionKeys.Security_Login,
                                                               PermissionKeys.Security_Clients);

            var users = _security.GetUsersByClientId(clientId);

            return users != null && users.Any() ? users.First().PhoneNumber : null;
        }

        public Client SearchClient(string login)
        {
            _adminSecurity.CurrentPermissions.AssertAllGranted(PermissionKeys.Security_Login,
                                                               PermissionKeys.Security_Clients);

            var user = _security.GetUserByPhoneNumber(login);

            if (user == null)
                return null;

            MyInfo info = null;
            try
            {
                info = _info.GetMyInfo(user.ClientId);
            }
            catch (MyInfoException e)
            {
                // TODO: залогировать
            }

            var groups = info != null
                                ? _targeting.GetClientGroups(user.ClientId)
                                : null;

            var client = new Client
            {
                SecurityUserId = user.Id,
                Login = user.PhoneNumber,
                ClientId = user.ClientId,
                IsUserDisabled = user.IsDisabled,
                IsPasswordSet = user.IsPasswordSet,
                RegistrationDate = user.RegistrationDate,
                Groups = groups
            };

            if (info != null)
            {
                client.ProfileStatus = info.Status != ClientStatus.Blocked && info.Status != ClientStatus.Deleted
                                           ? ClientProfileStatus.Normal
                                           : ClientProfileStatus.PendingDelete;
                client.FirstName = info.FirstName;
                client.LastName = info.LastName;
                client.MiddleName = info.MiddleName;
                client.Status = info.Status;
                client.BirthDate = info.BirthDate;
                client.Gender = info.Gender;
                client.Email = info.Email;
                client.LocationTitle = info.LocationTitle;
                client.CustomFields = info.CustomFields;

                try
                {
                    client.Balance = _processing.GetBalance(new GetBalanceParameters { ClientId = user.ClientId });
                }
                catch (ProcessingException e)
                {
                    // TODO: залогировать
                    client.Balance = null;
                }
            }
            else
            {
                client.ProfileStatus = ClientProfileStatus.NotFound;
            }

            return client;
        }

        public void ResetPassword(string login)
        {
            _adminSecurity.CurrentPermissions.AssertAllGranted(PermissionKeys.Security_Login,
                                                               PermissionKeys.Security_Clients,
                                                               PermissionKeys.Security_Clients_ResetPassword);

            var notificationTemplate = ConfigurationManager.AppSettings["security_reset_pwd_notification"];

            if (string.IsNullOrWhiteSpace(notificationTemplate))
            {
                throw new ConfigurationErrorsException("Не задан шаблон сообщения о сбросе пароля (appSettings/security_reset_pwd_notification)");
            }

            var options = new ResetPasswordOptions
            {
                Login = login,
                NotificationMessageTemplate = notificationTemplate
            };
            _security.ResetPassword(options);
        }

        public void DisableUser(string login, bool disable)
        {
            _adminSecurity.CurrentPermissions.AssertAllGranted(PermissionKeys.Security_Login,
                                                               PermissionKeys.Security_Clients,
                                                               PermissionKeys.Security_Clients_SiteAccess);

            _security.DisableUser(login, disable);
        }

        public void DisableProfile(string login)
        {
            _adminSecurity.CurrentPermissions.AssertAllGranted(PermissionKeys.Security_Login,
                                                               PermissionKeys.Security_Clients,
                                                               PermissionKeys.Security_Clients_Deactivate);

            DisableUser(login, true);

            var user = _security.GetUserByPhoneNumber(login);

            _bankConnector.BlockClientToDelete(user.ClientId);
        }

        public bool IsUserOnBlocking(string login)
        {
            _adminSecurity.CurrentPermissions.AssertAllGranted(PermissionKeys.Security_Login,
                                                               PermissionKeys.Security_Clients);

            var user = _security.GetUserByPhoneNumber(login);

            return _bankConnector.IsClientOnBlocking(user.ClientId);
        }

        public PhoneNumberChangeHistory GetLoginChangeHistory(int userId, SecurityPagingSettings paging = null)
        {
            _adminSecurity.CurrentPermissions.AssertAllGranted(PermissionKeys.Security_Login,
                                                               PermissionKeys.Security_Clients);

            return _security.GetChangeUserPhoneNumberHistory(userId, paging);
        }
    }
}
