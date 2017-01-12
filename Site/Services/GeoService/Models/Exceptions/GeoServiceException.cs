using Vtb24.Site.Services.Models;
using Vtb24.Site.Services.Models.Exceptions;

namespace Vtb24.Site.Services.GeoService.Models.Exceptions
{
    public class GeoServiceException : ComponentException
    {
        public GeoServiceException(int resultCode, string codeDescription)
            : base("ГеоБаза", resultCode, codeDescription) { }
    }
}