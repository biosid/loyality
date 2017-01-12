namespace RapidSoft.Loaylty.ProductCatalog.Interfaces
{
    using ClientProfile.ClientProfileService;

    public interface IClientProfileProvider
    {
        GetClientProfileFullResponseTypeClientProfile GetClientProfile(string clientId); 
    }
}
