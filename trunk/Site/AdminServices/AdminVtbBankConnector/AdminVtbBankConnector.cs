using System.Linq;
using Vtb24.Arms.AdminServices.AdminBankConnectorService;
using Vtb24.Arms.AdminServices.AdminVtbBankConnector.Models;
using Vtb24.Arms.AdminServices.Infrastructure;

namespace Vtb24.Arms.AdminServices.AdminVtbBankConnector
{
    public class AdminVtbBankConnector : IAdminVtbBankConnector
    {
        public AdminVtbBankConnector(IAdminSecurityService security)
        {
            _security = security;
        }

        private readonly IAdminSecurityService _security;

        public void ChangeClientPhoneNumber(string clientId, string phoneNumber)
        {
            using (var service = new AdminBankConnectorClient())
            {
                var request = new UpdateClientPhoneNumberRequest
                {
                    ClientId = clientId,
                    PhoneNumber = phoneNumber,
                    UserId = _security.CurrentUser
                };

                var response = service.UpdateClientPhoneNumber(request);

                response.AssertSuccess();
            }
        }

        public void ChangeClientEmail(string clientId, string email)
        {
            using (var service = new AdminBankConnectorClient())
            {
                var request = new UpdateClientEmailRequest
                {
                    ClientId = clientId,
                    Email = email,
                    UserId = _security.CurrentUser
                };

                var response = service.UpdateClientEmail(request);

                response.AssertSuccess();
            }
        }

        public CustomField[] GetAllCustomFields()
        {
            using (var service = new AdminBankConnectorClient())
            {
                var response = service.GetAllProfileCustomFields(_security.CurrentUser);

                response.AssertSuccess();

                return response.Result.Select(MappingsFromService.ToCustomField).ToArray();
            }
        }

        public int AppendCustomField(string name)
        {
            using (var service = new AdminBankConnectorClient())
            {
                var response = service.AppendProfileCustomField(name, _security.CurrentUser);

                response.AssertSuccess();

                return response.Result;
            }
        }

        public void RemoveCustomField(int id)
        {
            using (var service = new AdminBankConnectorClient())
            {
                var response = service.RemoveProfileCustomField(id, _security.CurrentUser);

                response.AssertSuccess();
            }
        }

        public void RenameCustomField(int id, string name)
        {
            using (var service = new AdminBankConnectorClient())
            {
                var response = service.RenameProfileCustomField(id, name, _security.CurrentUser);

                response.AssertSuccess();
            }
        }
    }
}
