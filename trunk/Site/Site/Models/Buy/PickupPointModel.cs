using Vtb24.Site.Services.GiftShop.Orders.Models;

namespace Vtb24.Site.Models.Buy
{
    public class PickupPointModel
    {
        public string PickupPointId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string[] Phones { get; set; }

        public string[] OperationHours { get; set; }

        public string Description { get; set; }

        public decimal DeliveryPriceRur { get; set; }

        public int DeliveryPriceBonus { get; set; }

        public static PickupPointModel Map(PickupVariant original)
        {
            return Map(original.PickupPoint, original.CostRur, original.CostBonus);
        }

        public static PickupPointModel Map(DeliveryPickupPoint original, decimal priceRur, decimal priceBonus)
        {
            return new PickupPointModel
            {
                PickupPointId = original.ExternalPickupPointId,
                Name = original.Name,
                Address = original.Address,
                Phones = original.Phones,
                OperationHours = original.OperatingHours,
                Description = original.Description,
                DeliveryPriceRur = priceRur,
                DeliveryPriceBonus = (int) priceBonus
            };
        }
    }
}