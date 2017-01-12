namespace RapidSoft.Loaylty.PartnersConnector.Interfaces
{
    using VTB24.Site.PublicProfileWebApi;
    using VTB24.Site.SecurityTokenWebApi;

    public interface ISecurityWebServicesProvider
    {
        GetPublicProfileResult GetUserProfile(string shopId, string userTicket);

        ValidateSecurityTokenResult GetClientId(string userTicket);
    }
}