using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Models.Exceptions
{
    public class CatalogManagementServiceException : ComponentException
    {
        public CatalogManagementServiceException(int resultCode, string codeDescription)
            : base("Каталог вознаграждений", resultCode, codeDescription)
        {
        }
    }
}
