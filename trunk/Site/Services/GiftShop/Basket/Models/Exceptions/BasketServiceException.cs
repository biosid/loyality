using Vtb24.Site.Services.Models.Exceptions;

namespace Vtb24.Site.Services.GiftShop.Basket.Models.Exceptions
{
    public class BasketServiceException : ComponentException
    {
        public BasketServiceException(int resultCode, string codeDescription)
            : base("Корзина", resultCode, codeDescription)
        {
        }
    }
}