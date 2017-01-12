namespace Vtb24.Site.Security.Models
{
    /// <summary>
    /// Минимальное представление клиента для слоя сервисов
    /// </summary>
    public class ClientPrincipal 
    {

        public string ClientId { get; set; }

        public string ClientAnonymousId { get; set; }

        public string ClientIp { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsAuthenticated { get { return !string.IsNullOrEmpty(ClientId); } }
    }
}