namespace Vtb24.Arms.AdminServices.GiftShopManagement.Models.Exceptions
{
    public class EntityNotFoundException : CatalogManagementServiceException
    {
        public EntityNotFoundException(int resultCode, string codeDescription)
            : base(resultCode, codeDescription)
        {
        }
    }
}
