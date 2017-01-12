using System;
using Rapidsoft.Loyalty.NotificationSystem.ClientProfileService;

namespace Rapidsoft.Loyalty.NotificationSystem.Services
{
    public class Profile : IProfile
    {
        public string GetProfileEmail(string clientId)
        {
            using (var service = new ClientProfileServiceClient())
            {
                var options = new GetClientProfileFullRequestType
                {
                    ClientId = clientId
                };
                var response = service.GetClientProfileFull(options);

                AssertResponse(response.StatusCode, response.Error);

                var profile = response.ClientProfile;
                return profile.Email;
            }
        }

        private static void AssertResponse(int code, string message)
        {
            if (code == 0)
            {
                return;
            }

            throw new Exception(message);
        }
    }
}
