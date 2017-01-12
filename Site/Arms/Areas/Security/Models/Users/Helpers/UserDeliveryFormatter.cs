using System.Linq;
using Vtb24.Site.Services.GiftShop.Orders.Models;
using OrderDelivery = Vtb24.Site.Services.GiftShop.Orders.Models.OrderDelivery;

namespace Vtb24.Arms.Security.Models.Users.Helpers
{
    internal static class UserDeliveryFormatter
    {
        public static string Map(OrderDelivery delivery)
        {
            if (delivery == null)
            {
                return "- Параметры доставки не указаны -";
            }

            if (delivery.Type == DeliveryType.Email)
            {
                return MapEmailDelivery(delivery);
            }

            var isPickup = delivery.Type == DeliveryType.Pickup;
            var address = isPickup ? MapPickup(delivery.PickupPoint) : MapAddress(delivery.Address);

            var result = string.Format("{0}. {1}", delivery.DeliveryVariantName ?? "Курьерская доставка", address);

            return result;
        }

        private static string MapEmailDelivery(OrderDelivery delivery)
        {
            var result = string.Format("Доставка по email. {0}", delivery.Contact.Email);

            return result;
        }

        private static string MapPickup(DeliveryPickupPoint pickup)
        {
            if (pickup == null)
            {
                return "- ПВЗ не указан -";
            }

            return string.Format("{0}", pickup.Address);
        }

        public static string MapAddress(DeliveryAddress address)
        {
            if (address == null)
            {
                return "- Адрес не указан -";
            }

            return string.Join(", ",
                               new[]
                               {
                                   address.PostCode,
                                   address.Region,
                                   address.District,
                                   address.City,
                                   address.Town,
                                   address.Street,
                                   address.House,
                                   address.Flat
                               }.Where(part => !string.IsNullOrEmpty(part)));
        }
    }
}
