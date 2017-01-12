namespace Vtb24.Site.Services.GiftShop.Wishlist.Models.Exceptions
{
    public class WishListItemNotFoundException : WishListServiceException
    {
        public WishListItemNotFoundException(int resultCode, string codeDescription)
            : base(resultCode, codeDescription)
        {
        }
    }
}
