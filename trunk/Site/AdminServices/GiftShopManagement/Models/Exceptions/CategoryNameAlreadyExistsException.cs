namespace Vtb24.Arms.AdminServices.GiftShopManagement.Models.Exceptions
{
    public class CategoryNameAlreadyExistsException : CatalogManagementServiceException
    {
        public CategoryNameAlreadyExistsException(int resultCode, string codeDescription)
            : base(resultCode, codeDescription)
        {
        }
    }
}
