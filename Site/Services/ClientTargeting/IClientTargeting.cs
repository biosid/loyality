using Vtb24.Site.Services.ClientTargeting.Models;

namespace Vtb24.Site.Services.ClientTargeting
{
    public interface IClientTargeting
    {
        ClientGroup[] GetClientGroups(string clientId);
    }
}
