using Vtb24.Site.Services.VtbBankConnector.Models.Outputs;

namespace Vtb24.Site.Services.VtbBankConnector
{
    internal class MappingsFromService
    {
        public static CardRegistrationParameters ToCardRegistrationParameters(BankConnectorService.CardRegistrationParameters original)
        {
            return new CardRegistrationParameters
            {
                ShopId = original.ShopId,
                OrderId = original.OrderId,
                CustomerId = original.CustomerId,
                Subtotal = original.Sum,
                Signature = original.Signature
            };
        }
    }
}
