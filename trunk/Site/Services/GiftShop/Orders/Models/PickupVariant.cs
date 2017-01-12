namespace Vtb24.Site.Services.GiftShop.Orders.Models
{
    public class PickupVariant
    {
        public DeliveryPickupPoint PickupPoint { get; set; }

        public decimal CostRur { get; set; }

        public decimal CostBonus { get; set; }
    }
}
