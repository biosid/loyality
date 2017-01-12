namespace Vtb24.Site.Services.GiftShop.Wishlist.Models.Exceptions
{
    public class WishListItemQuantityOverflowException : WishListServiceException
    {
        public WishListItemQuantityOverflowException(int resultCode, string codeDescription)
            : base(resultCode, codeDescription)
        {
        }
    }
}
