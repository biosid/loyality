using Vtb24.Site.Services.Models;
using Vtb24.Site.Services.Models.Exceptions;

namespace Vtb24.Site.Services.GiftShop.Wishlist.Models.Exceptions
{
    public class WishListServiceException : ComponentException
    {
        public WishListServiceException(int resultCode, string codeDescription)
            : base("Виш-лист", resultCode, codeDescription)
        {
        }
    }
}