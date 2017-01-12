using Vtb24.Site.Services.GiftShop.Orders.Models;
using Vtb24.Site.Services.Infrastructure;

namespace Vtb24.Site.Models.Buy
{
    public class DeliveryVariantModel
    {
        public bool IsPickup { get; set; }

		public bool IsEmail { get; set; }

		public int DeliveryType { get; set; }

        public PickupPointModel[] PickupPoints { get; set; }
        
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal DeliveryPriceRur { get; set; }

        public int DeliveryPriceBonus { get; set; }


        public static DeliveryVariantModel Map(DeliveryVariant original)
        {
            return new DeliveryVariantModel
            {
                Id = original.ExternalId,
                Name = original.Name,
                IsPickup = original.Type == Services.GiftShop.Orders.Models.DeliveryType.Pickup,
				IsEmail = original.Type == Services.GiftShop.Orders.Models.DeliveryType.Email,
				DeliveryType = (int)original.Type,
                DeliveryPriceRur = original.CostRur,
                DeliveryPriceBonus = (int)original.CostBonus,
                PickupPoints = original.PickupPoints.MaybeSelect(PickupPointModel.Map).MaybeToArray(),
                Description = original.Description
            };
        }
    }
}