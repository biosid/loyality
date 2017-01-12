using System.Linq;
using Vtb24.Site.Services.GiftShop.Orders.Models;

namespace Vtb24.Site.Models.Buy
{
    public class DeliveryGroupModel
    {
        public string Name { get; set; }

        public DeliveryVariantModel[] Variants { get; set; }


        public static DeliveryGroupModel Map(DeliveryVariantsGroup original)
        {
            return new DeliveryGroupModel()
            {
                Name = original.Name,
                Variants = original.Variants.Select(DeliveryVariantModel.Map).ToArray()
            };
        }
    }
}