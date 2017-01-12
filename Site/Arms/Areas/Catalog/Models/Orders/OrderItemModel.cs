using Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models;

namespace Vtb24.Arms.Catalog.Models.Orders
{
    public class OrderItemModel
    {
        public string ProductId { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public decimal BonusPrice { get; set; }

        public int Quantity { get; set; }

        public decimal QuantityPrice { get; set; }

        public decimal QuantityBonusPrice { get; set; }

        public static OrderItemModel Map(OrderItem item)
        {
            return new OrderItemModel
            {
                ProductId = item.ProductId,
                Title = item.Title,
                Price = item.Price,
                BonusPrice = item.BonusPrice,
                Quantity = item.Quantity,
                QuantityPrice = item.QuantityPrice,
                QuantityBonusPrice = item.QuantityBonusPrice
            };
        }
    }
}
