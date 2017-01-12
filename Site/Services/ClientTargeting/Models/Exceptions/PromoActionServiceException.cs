using Vtb24.Site.Services.Models.Exceptions;

namespace Vtb24.Site.Services.ClientTargeting.Models.Exceptions
{
    public class PromoActionServiceException : ComponentException
    {
        public PromoActionServiceException(int errorCode, string codeDescription)
            : base("Промо-акции", errorCode, codeDescription)
        {
        }
    }
}