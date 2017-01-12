namespace Vtb24.Arms.AdminServices.AdminVtbBankConnector.Models
{
    public class AdminVtbBankConnectorCustomFieldAlreadyExists : AdminVtbBankConnectorServiceException
    {
        public AdminVtbBankConnectorCustomFieldAlreadyExists(int resultCode, string codeDescription)
            : base(resultCode, codeDescription)
        {
        }
    }
}
