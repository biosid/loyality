namespace Vtb24.Arms.AdminServices.GiftShopManagement.Models.Exceptions
{
    public class PartnerNameAlreadyExistsException : CatalogManagementServiceException
    {
        public PartnerNameAlreadyExistsException(int resultCode, string codeDescription)
            : base(resultCode, codeDescription)
        {
        }
    }
}
