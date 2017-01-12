using System;
using Vtb24.Site.Services.GiftShop.Orders.Models;
using Vtb24.Site.Services.GiftShop.Orders.Models.Inputs;
using Vtb24.Site.Services.ProductCatalogOrderService;
using DeliveryAddress = Vtb24.Site.Services.GiftShop.Orders.Models.DeliveryAddress;

namespace Vtb24.Site.Services.GiftShop.Orders
{
    internal static class MappingsToService
    {
        public static Location ToLocation(DeliveryLocationInfo original)
        {
            if (original == null)
            {
                return null;
            }

            return new Location
            {
                KladrCode = original.KladrCode,
                PostCode = original.PostCode
            };
        }

        public static ProductCatalogOrderService.DeliveryAddress ToDeliveryAddress(DeliveryAddress original)
        {
            if (original == null)
            {
                return null;
            }

            return new ProductCatalogOrderService.DeliveryAddress
            {
                PostCode = original.PostCode,
                RegionTitle = original.Region,
                DistrictTitle = original.District,
                CityTitle = original.City,
                TownTitle = original.Town,
                StreetTitle = original.Street,
                House = original.House,
                Flat = original.Flat
            };
        }

        public static PhoneNumber ToPhoneNumber(string original)
        {
            if (string.IsNullOrWhiteSpace(original))
            {
                return null;
            }

            return new PhoneNumber
            {
                LocalNumber = original.Substring(4),
                CityCode = original.Substring(1, 3),
                CountryCode = original.Substring(0, 1)
            };
        }

        public static Contact ToContact(DeliveryContact original)
        {
            if (original == null)
            {
                return null;
            }

            return new Contact
            {
                Email = original.Email,
                Phone = ToPhoneNumber(original.Phone),
                FirstName = original.FirstName,
                LastName = original.LastName,
                MiddleName = original.MiddleName
            };
        }

        public static DeliveryDto ToDeliveryDto(OrderDeliveryParameters original)
        {
            if (original == null)
            {
                return null;
            }

            var result = new DeliveryDto
            {
                ExternalDeliveryVariantId = original.ExternalDeliveryVariantId,
                DeliveryVariantLocation = ToLocation(original.DeliveryVariantLocation),
                Contact = ToContact(original.Contact),
                Comment = original.Comment
            };

            switch (original.Type)
            {
                case DeliveryType.Delivery:
                    result.DeliveryType = DeliveryTypes.Delivery;
                    result.Address = ToDeliveryAddress(original.Address);
                    break;

                case DeliveryType.Pickup:
                    result.DeliveryType = DeliveryTypes.Pickup;
                    result.PickupPoint = new PickupPoint
                    {
                        ExternalPickupPointId = original.ExternalPickupPointId
                    };
                    break;
				case DeliveryType.Email:
					result.DeliveryType = DeliveryTypes.Email;
					break;

                default:
                    throw new NotSupportedException(string.Format("delivery type not supported: {0}", original.Type));
            }

            return result;
        }

        public static PublicOrderStatuses ToPublicOrderStatus(OrderStatus original)
        {
            switch (original)
            {
                case OrderStatus.Registration:
                    return PublicOrderStatuses.Registration;
                case OrderStatus.Processing:
                    return PublicOrderStatuses.Processing;
                case OrderStatus.Delivery:
                    return PublicOrderStatuses.Delivery;
                case OrderStatus.DeliveryWaiting:
                    return PublicOrderStatuses.DeliveryWaiting;

                case OrderStatus.NotDelivered:
                    return PublicOrderStatuses.NotDelivered;
                case OrderStatus.Delivered:
                    return PublicOrderStatuses.Delivered;
                case OrderStatus.Cancelled:
                    return PublicOrderStatuses.Cancelled;

                default:
                    throw new Exception("unknown public order status");
            }
        }

        public static OrderStatuses ToOrderStatuses(OrderStatus original)
        {
            switch (original)
            {
                case OrderStatus.Registration:
                    return OrderStatuses.Registration;
                case OrderStatus.Processing:
                    return OrderStatuses.Processing;
                case OrderStatus.Cancelled:
                    return OrderStatuses.CancelledByPartner;
                case OrderStatus.DeliveryWaiting:
                    return OrderStatuses.DeliveryWaiting;
                case OrderStatus.Delivery:
                    return OrderStatuses.Delivery;
                case OrderStatus.Delivered:
                    return OrderStatuses.Delivered;
                case OrderStatus.NotDelivered:
                    return OrderStatuses.NotDelivered;

                default:
                    throw new Exception("unknown order status");
            }
        }
    }
}
