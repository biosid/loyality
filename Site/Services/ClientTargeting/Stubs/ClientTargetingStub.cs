using Vtb24.Site.Services.ClientTargeting.Models;

namespace Vtb24.Site.Services.ClientTargeting.Stubs
{
    public class ClientTargetingStub : IClientTargeting
    {
        public ClientGroup[] GetClientGroups(string clientId)
        {
            return new[] { new ClientGroup
            {
                Id = "1",
                IsSegment = false,
                Name = "1"
            }};
        }
    }
}