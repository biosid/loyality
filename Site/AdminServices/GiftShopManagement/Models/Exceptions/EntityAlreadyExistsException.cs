namespace Vtb24.Arms.AdminServices.GiftShopManagement.Models.Exceptions
{
    public class EntityAlreadyExistsException : CatalogManagementServiceException
    {
        public EntityAlreadyExistsException(int resultCode, string codeDescription)
            : base(resultCode, codeDescription)
        {
        }
    }
}
