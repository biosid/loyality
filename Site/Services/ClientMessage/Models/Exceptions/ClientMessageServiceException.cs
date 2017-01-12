using Vtb24.Site.Services.Models.Exceptions;

namespace Vtb24.Site.Services.ClientMessage.Models.Exceptions
{
    public class ClientMessageServiceException : ComponentException
    {
        public ClientMessageServiceException(int resultCode, string resultDescription)
            : base("Публичные сервисы для клиентских сообщений", resultCode, resultDescription)
        {
        }
    }
}
