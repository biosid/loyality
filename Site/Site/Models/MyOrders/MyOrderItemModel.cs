using Vtb24.Site.Services.GiftShop.Orders.Models;

namespace Vtb24.Site.Models.MyOrders
{
    public class MyOrderItemModel
    {
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal ItemPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public static MyOrderItemModel Map(GiftShopOrderItem original)
        {
            return new MyOrderItemModel
            {
                ProductId = original.ProductId,
                ProductName = original.Title,
                Quantity = original.Quantity,
                ItemPrice = original.BonusPrice,
                TotalPrice = original.QuantityBonusPrice
            };
        }
    }
}