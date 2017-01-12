using Vtb24.Arms.AdminServices.TargetingManagement.Models;

namespace Vtb24.Arms.AdminServices.TargetingManagement.Stubs
{
    public class TargetingManagementStub : ITargetingManagement
    {
        public Segment[] GetSegments()
        {
            return new[]
            {
                new Segment { Id = "Standard", Name = "Standard" },
                new Segment { Id = "VIP", Name = "VIP" }
            };
        }
    }
}
