using System.Globalization;
using Vtb24.Site.Services.ClientTargeting.Models;
using Vtb24.Site.Services.ClientTargeting.Models.Exceptions;
using Vtb24.Site.Services.Infrastructure;
using Vtb24.Site.Services.TargetAudienceService;

namespace Vtb24.Site.Services.ClientTargeting
{
    public class ClientTargeting : IClientTargeting
    {
        public ClientGroup[] GetClientGroups(string clientId)
        {
            if (string.IsNullOrWhiteSpace(clientId))
            {
                return null;
            }

            using (var service = new TargetAudienceServiceClient())
            {
                var response = service.GetClientTargetAudiences(clientId);

                if (!response.Success)
                {
                    throw new PromoActionServiceException(response.ResultCode, response.ResultDescription);
                }

                return response.ClientTargetAudiences.MaybeSelect(ta => new ClientGroup
                {
                    Id = ta.Id.ToString(CultureInfo.InvariantCulture),
                    IsSegment = ta.IsSegment,
                    Name = ta.Name
                }).MaybeToArray();
            }
        }
    }
}