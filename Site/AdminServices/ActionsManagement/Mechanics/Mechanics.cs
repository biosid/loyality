using System.Linq;
using Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models;
using Vtb24.Arms.AdminServices.AdminMechanicsService;
using Vtb24.Arms.AdminServices.Infrastructure;

namespace Vtb24.Arms.AdminServices.ActionsManagement.Mechanics
{
    public class Mechanics : IMechanics
    {
        public Mechanics(IAdminSecurityService security)
        {
            _security = security;
        }

        private readonly IAdminSecurityService _security;

        public Mechanic[] GetAllMechanics()
        {
            using (var service = new AdminMechanicsServiceClient())
            {
                var response = service.GetAllRuleDomains(_security.CurrentUser);

                response.AssertSuccess();

                return response.RuleDomains.Select(MappingsFromService.ToMechanic).ToArray();
            }
        }

        public Mechanic GetMechanic(long id)
        {
            using (var service = new AdminMechanicsServiceClient())
            {
                var response = service.GetRuleDomain(id, _security.CurrentUser);

                response.AssertSuccess();

                return MappingsFromService.ToMechanic(response.RuleDomain);
            }
        }

        public Metadata[] GetMetadataByMechanicId(long id)
        {
            using (var service = new AdminMechanicsServiceClient())
            {
                var response = service.GetMetadataByDomainId(id, _security.CurrentUser);

                response.AssertSuccess();

                return response.Entities.MaybeSelect(MappingsFromService.ToMetadata).MaybeToArray();
            }
        }
    }
}
