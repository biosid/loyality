using Vtb24.Site.Services.BankConnectorService;
using Vtb24.Site.Services.BankProducts.Models;

namespace Vtb24.Site.Services.BankProducts
{
    internal static class MappingsFromService
    {
        public static BankProduct ToBankProduct(BankOffer original)
        {
            if (original == null)
            {
                return null;
            }

            return new BankProduct
            {
                Id = original.Id,
                Description = original.Description,
                Cost = original.BonusCost,
                ExpirationDate = original.ExpirationDate,
                ProductId = original.OfferId
            };
        }
    }
}
