using Vtb24.Arms.AdminServices.SecurityManagement.Models;
using Vtb24.Site.Security.Models;
using Vtb24.Site.Security.SecurityService.Models.Outputs;

namespace Vtb24.Arms.AdminServices
{
    public interface ISecurityManagement
    {
        string GetClientIdByLogin(string login);

        string GetLoginByClientId(string clientId);

        Client SearchClient(string login);

        void DisableUser(string login, bool disable);

        void DisableProfile(string login);

        void ResetPassword(string login);

        bool IsUserOnBlocking(string login);

        PhoneNumberChangeHistory GetLoginChangeHistory(int userId, SecurityPagingSettings paging = null);
    }
}
