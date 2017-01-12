using Vtb24.Arms.AdminServices.AdminBankConnectorService;
using Vtb24.Arms.AdminServices.AdminVtbBankConnector.Models;

namespace Vtb24.Arms.AdminServices.AdminVtbBankConnector
{
    internal static class MappingsFromService
    {
        public static CustomField ToCustomField(ClientProfileCustomField original)
        {
            return new CustomField
            {
                Id = original.Id,
                Name = original.Name
            };
        }
    }
}
