using Vtb24.Site.Services.Models;
using Vtb24.Site.Services.Models.Exceptions;

namespace Vtb24.Site.Services.GiftShop.Catalog.Models.Exceptions
{
    public class CatalogServiceException : ComponentException
    {
        public CatalogServiceException(int resultCode, string codeDescription)
            : base("Каталог подарков", resultCode, codeDescription)
        {
        }
    }
}