using Vtb24.Site.Services.Models.Exceptions;

namespace Vtb24.Site.Services.BankProducts.Models.Exceptions
{
    public class BankProductsServiceException : ComponentException
    {
        public BankProductsServiceException(int resultCode, string codeDescription)
            : base("Продукты банка", resultCode, codeDescription)
        {
        }
    }
}
