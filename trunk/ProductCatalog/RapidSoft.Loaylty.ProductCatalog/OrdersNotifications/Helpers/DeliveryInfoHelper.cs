namespace RapidSoft.Loaylty.ProductCatalog.OrdersNotifications.Helpers
{
    using System;
    using System.Collections.Generic;    
    using System.Text;

    using API.Entities;

    internal static class DeliveryInfoHelper
    {
        private const string Separator = ", ";

        public static string ExtractEmailAddress(this DeliveryInfo deliveryInfo)
        {
            return deliveryInfo != null && deliveryInfo.Contact != null
                       ? deliveryInfo.Contact.Email
                       : null;
        }

        public static string ExtractDeliveryAddress(this DeliveryInfo deliveryInfo)
        {
            if (deliveryInfo == null)
            {
                return null;
            }

            var sb = new StringBuilder(deliveryInfo.DeliveryVariantName + ". ");

            switch (deliveryInfo.DeliveryType)
            {
                case DeliveryTypes.Delivery:
                    sb.Append(string.Join(Separator, GetAddressParts(deliveryInfo.Address)));
                    break;

                case DeliveryTypes.Pickup:
                    sb.Append(string.Join(Separator, GetAddressParts(deliveryInfo.PickupPoint)));
                    break;

                case DeliveryTypes.Email:
                    sb.Append(ExtractEmailAddress(deliveryInfo));
                    break;

                default:
                    throw new Exception(string.Format("DeliveryType {0} not supported", deliveryInfo.DeliveryType));
            }

            return sb.ToString();
        }

        private static IEnumerable<string> GetAddressParts(PickupPoint pickupPoint)
        {
            if (!string.IsNullOrEmpty(pickupPoint.Address))
            {
                yield return pickupPoint.Address;
            }
        }

        private static IEnumerable<string> GetAddressParts(DeliveryAddress address)
        {
            if (!string.IsNullOrEmpty(address.PostCode))
            {
                yield return address.PostCode;
            }

            var city = GetCity(address);
            if (!string.IsNullOrEmpty(city))
            {
                yield return city;
            }

            var streetAddress = string.Join(Separator, GetStreetAddressParts(address));
            if (!string.IsNullOrEmpty(streetAddress))
            {
                yield return streetAddress;
            }
        }

        private static IEnumerable<string> GetStreetAddressParts(DeliveryAddress address)
        {
            if (!string.IsNullOrEmpty(address.StreetTitle))
            {
                yield return address.StreetTitle;
            }

            if (!string.IsNullOrEmpty(address.House))
            {
                yield return address.House;
            }

            if (!string.IsNullOrEmpty(address.Flat))
            {
                yield return "кв. " + address.Flat;
            }
        }

        private static string GetCity(DeliveryAddress deliveryInfo)
        {
            return !string.IsNullOrEmpty(deliveryInfo.TownTitle)
                       ? deliveryInfo.TownTitle
                       : !string.IsNullOrEmpty(deliveryInfo.CityTitle)
                             ? deliveryInfo.CityTitle
                             : !string.IsNullOrEmpty(deliveryInfo.RegionTitle)
                                   ? deliveryInfo.RegionTitle
                                   : null;
        }
    }
}
