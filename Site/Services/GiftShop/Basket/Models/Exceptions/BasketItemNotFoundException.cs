namespace Vtb24.Site.Services.GiftShop.Basket.Models.Exceptions
{
    public class BasketItemNotFoundException : BasketServiceException
    {
        public BasketItemNotFoundException(int resultCode, string codeDescription) : base(resultCode, codeDescription)
        {
        }
    }
}