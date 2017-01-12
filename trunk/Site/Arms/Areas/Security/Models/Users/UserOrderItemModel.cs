using Vtb24.Site.Services.GiftShop.Orders.Models;

namespace Vtb24.Arms.Security.Models.Users
{
    public class UserOrderItemModel
    {
        public string SupplierProductId { get; set; }

        public string Title { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal BonusPrice { get; set; }

        public decimal QuantityPrice { get; set; }

        public decimal QuantityBonusPrice { get; set; }

        public static UserOrderItemModel Map(GiftShopOrderItem item)
        {
            return new UserOrderItemModel
            {
                SupplierProductId = item.ProductId,
                Title = item.Title,
                Price = item.Price,
                BonusPrice = item.BonusPrice,
                QuantityPrice = item.QuantityPrice,
                QuantityBonusPrice = item.QuantityBonusPrice,
                Quantity = item.Quantity,
            };
        }

    }
}