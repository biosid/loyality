namespace RapidSoft.Loaylty.PartnersConnector.Services.Providers
{
    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.PartnersConnector.Interfaces;
    using RapidSoft.Loaylty.ProductCatalog.WsClients;
    using RapidSoft.VTB24.Site.PublicProfileWebApi;
    using RapidSoft.VTB24.Site.SecurityTokenWebApi;

    public class SecurityWebServicesProvider : ISecurityWebServicesProvider
    {
        private readonly ILog log = LogManager.GetLogger(typeof (SecurityWebServicesProvider));

        public GetPublicProfileResult GetUserProfile(string shopId, string userTicket)
        {
            log.InfoFormat("Получение профиля пользователя по userTicket = \"{0}\"", userTicket);

            var parameters = new GetPublicProfileParameters { ShopId = shopId, SecurityToken = userTicket };
            var retVal =
                WebClientCaller.CallService<PublicProfileWebApiClient, GetPublicProfileResult>(cl => cl.Get(parameters));
            log.Info("Полученный ответ на получение профиля пользователя: " + retVal.Serialize());
            return retVal;
        }

        public ValidateSecurityTokenResult GetClientId(string userTicket)
        {
            log.InfoFormat("Получение идентификатора пользователя по userTicket = \"{0}\"", userTicket);

            var parameters = new ValidateSecurityTokenParameters { SecurityToken = userTicket };
            var retVal =
                WebClientCaller.CallService<SecurityTokenWebApiClient, ValidateSecurityTokenResult>(
                    cl => cl.Validate(parameters));
            log.Info("Полученный ответ на получение идентификатора пользователя: " + retVal.Serialize());
            return retVal;
        }
    }
}