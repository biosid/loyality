namespace Vtb24.Site.Services.GiftShop.Orders.Models
{
    public class GiftShopOrderItem
    {
        public string ProductId { get; set; }

        public string Article { get; set; }

        public string Title { get; set; }

        public int Quantity { get; set; }

        public string BasketId { get; set; }

        public decimal Price { get; set; }

        public decimal BonusPrice { get; set; }

        public decimal QuantityPrice { get; set; }

        public decimal QuantityBonusPrice { get; set; }
    }
}
