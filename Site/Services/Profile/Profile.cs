using System;
using System.Linq;
using Vtb24.Site.Services.ClientProfileService;
using Vtb24.Site.Services.Infrastructure;
using Vtb24.Site.Services.Profile.Models;
using Vtb24.Site.Services.Profile.Models.Exceptions;
using Vtb24.Site.Services.Profile.Models.Inputs;

namespace Vtb24.Site.Services.Profile
{
    public class Profile : IProfile
    {
        public ClientProfile GetProfile(GetProfileParameters parameters)
        {
            parameters.ValidateAndThrow();
            
            using (var service = new ClientProfileServiceClient())
            {
                var options = new GetClientProfileFullRequestType
                {
                    ClientId = parameters.ClientId
                };
                var response = service.GetClientProfileFull(options);

                AssertResponse(response.StatusCode, response.Error);

                var profile = response.ClientProfile;
                return MappingsFromService.ToClientProfile(profile);
            }
        }

        public ClientStatus GetStatus(GetProfileParameters parameters)
        {
            parameters.ValidateAndThrow();

            using (var service = new ClientProfileServiceClient())
            {
                var options = new GetClientProfileFullRequestType
                {
                    ClientId = parameters.ClientId
                };
                var response = service.GetClientProfileFull(options);

                AssertResponse(response.StatusCode, response.Error);

                var profile = response.ClientProfile;
                return MappingsFromService.ToClientStatus(profile.ClientStatus);
            }
        }

        public string GetEmail(GetProfileParameters parameters)
        {
            parameters.ValidateAndThrow();

            using (var service = new ClientProfileServiceClient())
            {
                var options = new GetClientProfileFullRequestType
                {
                    ClientId = parameters.ClientId
                };
                var response = service.GetClientProfileFull(options);

                AssertResponse(response.StatusCode, response.Error);

                return response.ClientProfile != null
                           ? response.ClientProfile.Email
                           : null;
            }
        }

        public void SetLocation(SetLocationParameters parameters)
        {
            parameters.ValidateAndThrow();

            using (var service = new ClientProfileServiceClient())
            {
                var options = new SetClientLocationRequestType
                {
                    ClientId = parameters.ClientId,
                    ClientLocationKladr = parameters.LocationKladr,
                    ClientLocationName = parameters.LocationTitle
                };
                var response = service.SetClientLocation(options);

                AssertResponse(response.StatusCode, response.Error);
            }           
        }

        private static void AssertResponse(int code, string message)
        {
            if (code == 0)
            {
                return;
            }

            throw new ClientProfileException(code, message);
        }
    }
}
