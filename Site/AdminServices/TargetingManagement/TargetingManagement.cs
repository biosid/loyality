using System.Linq;
using Vtb24.Arms.AdminServices.Infrastructure;
using Vtb24.Arms.AdminServices.TargetAudienceService;
using Segment = Vtb24.Arms.AdminServices.TargetingManagement.Models.Segment;

namespace Vtb24.Arms.AdminServices.TargetingManagement
{
    public class TargetingManagement : ITargetingManagement
    {
        public Segment[] GetSegments()
        {
            using (var service = new TargetAudienceServiceClient())
            {
                var response = service.GetTargetAudiences(true);

                response.AssertSuccess();

                return response.TargetAudiences.Select(MappingsFromService.ToSegment).ToArray();
            }
        }
    }
}
