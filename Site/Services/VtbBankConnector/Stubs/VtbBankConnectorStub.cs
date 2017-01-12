using Vtb24.Site.Services.VtbBankConnector.Models.Inputs;
using Vtb24.Site.Services.VtbBankConnector.Models.Outputs;

namespace Vtb24.Site.Services.VtbBankConnector.Stubs
{
    public class VtbBankConnectorStub : IVtbBankConnectorService
    {
        public bool IsCardRegistered(string clientId)
        {
            return true;
        }

        public void RegisterCard(string clientId)
        {
        }

        public void RegisterClient(RegisterClientParams parameters)
        {
        }

        public void BlockClientToDelete(string clientId)
        {
        }

        public CardRegistrationParameters GetCardRegistrationParameters(string clientId)
        {
            return new CardRegistrationParameters
            {
                ShopId = "1",
                OrderId = "1",
                CustomerId = clientId,
                Subtotal = "1",
                Signature = "1"
            };
        }

        public bool VerifyCardRegistration(string orderId)
        {
            return true;
        }

        public bool IsClientOnBlocking(string clientId)
        {
            return false;
        }

        public void UpdateClientEmail(string clientId, string email)
        {
        }
    }
}
