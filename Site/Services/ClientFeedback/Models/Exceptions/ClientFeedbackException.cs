using Vtb24.Site.Services.Models.Exceptions;

namespace Vtb24.Site.Services.ClientFeedback.Models.Exceptions
{
    public class ClientFeedbackException : ComponentException
    {
        public ClientFeedbackException(int resultCode, string codeDescription)
            : base("Публичные сервисы для обратной связи ", resultCode, codeDescription)
        {
        }
    }
}