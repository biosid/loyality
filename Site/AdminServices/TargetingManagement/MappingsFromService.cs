using Vtb24.Arms.AdminServices.TargetingManagement.Models;

namespace Vtb24.Arms.AdminServices.TargetingManagement
{
    internal static class MappingsFromService
    {
        public static Segment ToSegment(TargetAudienceService.TargetAudience original)
        {
            return new Segment
            {
                Id = original.Id,
                Name = original.Name
            };
        }
    }
}
