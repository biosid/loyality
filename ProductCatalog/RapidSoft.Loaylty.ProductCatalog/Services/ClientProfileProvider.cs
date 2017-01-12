namespace RapidSoft.Loaylty.ProductCatalog.Services
{
    using API.OutputResults;

    using ClientProfile.ClientProfileService;

    using Interfaces;
    
    public enum ClientProfileStatuses
    {
        /// <summary>
        /// The active.
        /// </summary>
        Active = 1
    }

    public class ClientProfileProvider : IClientProfileProvider
    {
        public GetClientProfileFullResponseTypeClientProfile GetClientProfile(string clientId)
        {
            var request = new GetClientProfileFullRequestType()
            {
                ClientId = clientId
            };
            
            var response =
                WebClientCaller.CallService<ClientProfileServiceClient, GetClientProfileFullResponseType>(
                    service => service.GetClientProfileFull(request));

            return response.StatusCode != ResultCodes.SUCCESS ? null : response.ClientProfile;
        }
    }
}