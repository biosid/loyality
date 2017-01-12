using Vtb24.Arms.AdminServices.TargetingManagement.Models;

namespace Vtb24.Arms.Catalog.Models.Gifts
{
    public class SegmentModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public static SegmentModel Map(Segment original)
        {
            return new SegmentModel
            {
                Id = original.Id,
                Name = original.Name
            };
        }
    }
}
