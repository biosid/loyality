namespace Vtb24.Site.Services.GiftShop.Orders.Models
{
    public class DeliveryVariant
    {
        public DeliveryType Type { get; set; }

        public string Name { get; set; }

        public string ExternalId { get; set; }

        public string Description { get; set; }

        public decimal CostRur { get; set; }

        public decimal CostBonus { get; set; }

        public PickupVariant[] PickupPoints { get; set; }
    }
}
