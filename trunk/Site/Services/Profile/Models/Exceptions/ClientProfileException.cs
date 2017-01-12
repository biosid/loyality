using Vtb24.Site.Services.Models.Exceptions;

namespace Vtb24.Site.Services.Profile.Models.Exceptions
{
    public class ClientProfileException : ComponentException
    {
        public ClientProfileException(int statusCode, string codeDescription)
            : base("Профиль клиента", statusCode, codeDescription)
        {
        }
    }
}