namespace Vtb24.Arms.AdminServices.AdminVtbBankConnector.Models
{
    public class AdminVtbBankConnectorPhoneNumberAlreadyUsed : AdminVtbBankConnectorServiceException
    {
        public AdminVtbBankConnectorPhoneNumberAlreadyUsed(int resultCode, string codeDescription)
            : base(resultCode, codeDescription)
        {
        }
    }
}
