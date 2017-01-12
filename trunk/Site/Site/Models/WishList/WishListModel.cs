using Vtb24.Site.Models.Basket;

namespace Vtb24.Site.Models.WishList
{
    public class WishListModel
    {
        public BasketItemModel[] Items { get; set; }
        public decimal Balance { get; set; }
    }
}