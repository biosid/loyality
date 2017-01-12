using System.Linq;

namespace Vtb24.Site.Models.Basket
{
    public class BasketModel
    {
        public decimal Balance { get; set; }

        public decimal MaxOrderCost { get; set; }

        public BasketItemModel[] Items { get; set; }

        public decimal? TotalPrice
        { 
            get
            {
                return Items != null && Items.Length > 0 ? Items.Sum(i => i.BasketItemPrice) : (decimal?) null;
            }
        }
    }
}