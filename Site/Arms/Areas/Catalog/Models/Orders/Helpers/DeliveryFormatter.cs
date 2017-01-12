using System.Linq;
using Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models;

namespace Vtb24.Arms.Catalog.Models.Orders.Helpers
{
    internal class DeliveryFormatter
    {
        public static string Map(OrderDelivery delivery)
        {
            if (delivery == null)
            {
                return "- Параметры доставки не указаны -";
            }

            if (delivery.Type == OrderDeliveryType.Email)
            {
                return MapEmailDelivery(delivery);
            }

            var isPickup = delivery.Type == OrderDeliveryType.Pickup;
            var address = isPickup ? MapPickup(delivery.PickupPoint) : MapAddress(delivery.Address);

            var result = string.Format("{0}. {1}", delivery.DeliveryVariantName ?? "Курьерская доставка", address);

            return result;
        }

        private static string MapEmailDelivery(OrderDelivery delivery)
        {
            var result = string.Format("Доставка по email. {0}", delivery.Contact.Email);

            return result;
        }

        private static string MapPickup(OrderDeliveryPickupPoint pickup)
        {
            if (pickup == null)
            {
                return "- ПВЗ не указан -";
            }

            return string.Format("{0}", pickup.Address);
        }

        public static string MapAddress(OrderDeliveryAddress address)
        {
            if (address == null)
            {
                return "- Адрес не указан -";
            }

            return string.Join(", ",
                               new[]
                               {
                                   address.PostCode,
                                   address.RegionTitle,
                                   address.DistrictTitle,
                                   address.CityTitle,
                                   address.TownTitle,
                                   address.StreetTitle,
                                   address.House,
                                   address.Flat
                               }.Where(part => !string.IsNullOrEmpty(part)));
        }
    }
}
