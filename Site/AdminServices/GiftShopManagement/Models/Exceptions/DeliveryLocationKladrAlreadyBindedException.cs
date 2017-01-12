namespace Vtb24.Arms.AdminServices.GiftShopManagement.Models.Exceptions
{
    public class DeliveryLocationKladrAlreadyBindedException: CatalogManagementServiceException
    {
        public string Text { get; set; }

        public DeliveryLocationKladrAlreadyBindedException(int resultCode, string codeDescription)
            : base(resultCode, codeDescription)
        {
            Text = codeDescription;
        }
    }
}
