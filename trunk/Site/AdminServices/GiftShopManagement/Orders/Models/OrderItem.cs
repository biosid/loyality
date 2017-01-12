namespace Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models
{
    public class OrderItem
    {
        public string ProductId { get; set; }

        public string BasketId { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public decimal BonusPrice { get; set; }

        public int Quantity { get; set; }

        public decimal QuantityPrice { get; set; }

        public decimal QuantityBonusPrice { get; set; }
    }
}
