namespace Vtb24.Arms.AdminServices.AdminVtbBankConnector.Models
{
    public class AdminVtbBankConnectorUserNotFound : AdminVtbBankConnectorServiceException
    {
        public AdminVtbBankConnectorUserNotFound(int resultCode, string codeDescription)
            : base(resultCode, codeDescription)
        {
        }
    }
}
