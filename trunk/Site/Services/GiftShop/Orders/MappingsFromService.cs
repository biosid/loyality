using Vtb24.Site.Services.GiftShop.Orders.Models;
using Vtb24.Site.Services.Infrastructure;
using Vtb24.Site.Services.ProductCatalogOrderService;
using DeliveryAddress = Vtb24.Site.Services.GiftShop.Orders.Models.DeliveryAddress;
using DeliveryVariant = Vtb24.Site.Services.GiftShop.Orders.Models.DeliveryVariant;
using LastDeliveryAddress = Vtb24.Site.Services.GiftShop.Orders.Models.LastDeliveryAddress;
using PickupVariant = Vtb24.Site.Services.GiftShop.Orders.Models.PickupVariant;

namespace Vtb24.Site.Services.GiftShop.Orders
{
    internal static class MappingsFromService
    {
        public static DeliveryType ToDeliveryType(DeliveryTypes original)
        {
            switch (original)
            {
                case DeliveryTypes.Delivery:
                    return DeliveryType.Delivery;
                case DeliveryTypes.Pickup:
                    return DeliveryType.Pickup;
				case  DeliveryTypes.Email:
					return DeliveryType.Email;
            }

            return DeliveryType.Unknown;
        }

        public static string ToRecipientPhone(PhoneNumber original)
        {
            if (original == null)
            {
                return null;
            }

            return original.CountryCode + original.CityCode + original.LocalNumber;
        }

        public static DeliveryVariantsLocation ToDeliveryVariantsLocation(VariantsLocation original)
        {
            if (original == null)
            {
                return null;
            }

            return new DeliveryVariantsLocation
            {
                Name = original.LocationName,
                KladrCode = original.KladrCode,
                PostCode = original.PostCode,
                ExternalLocationId = original.ExternalLocationId
            };
        }

        public static DeliveryAddress ToDeliveryAddress(ProductCatalogOrderService.DeliveryAddress original)
        {
            if (original == null)
            {
                return null;
            }

            return new DeliveryAddress
            {
                AddressText = original.AddressText,
                PostCode = original.PostCode,
                Region = original.RegionTitle,
                District = original.DistrictTitle,
                City = original.CityTitle,
                Town = original.TownTitle,
                Street = original.StreetTitle,
                House = original.House,
                Flat = original.Flat
            };
        }

        public static LastDeliveryAddress ToLastDeliveryAddress(ProductCatalogOrderService.LastDeliveryAddress original)
        {
            if (original == null)
            {
                return null;
            }

            return new LastDeliveryAddress
            {
                Location = ToDeliveryVariantsLocation(original.DeliveryVariantsLocation),
                Address = ToDeliveryAddress(original.DeliveryAddress)
            };
        }

        public static DeliveryPickupPoint ToDeliveryPickupPoint(PickupPoint1 original)
        {
            if (original == null)
            {
                return null;
            }

            return new DeliveryPickupPoint
            {
                Name = original.Name,
                ExternalPickupPointId = original.ExternalPickupPointId,
                ExternalDeliveryVariantId = original.ExternalDeliveryVariantId,
                Address = original.Address,
                Phones = original.Phones,
                OperatingHours = original.OperatingHours,
                Description = original.Description
            };
        }

        public static DeliveryContact ToDeliveryContact(Contact original)
        {
            if (original == null)
            {
                return null;
            }

            return new DeliveryContact
            {
                Email = original.Email,
                Phone = ToRecipientPhone(original.Phone),
                FirstName = original.FirstName,
                MiddleName = original.MiddleName,
                LastName = original.LastName
            };
        }

        public static OrderDelivery ToOrderDelivery(DeliveryInfo original)
        {
            if (original == null)
            {
                return null;
            }

            return new OrderDelivery
            {
                Type = ToDeliveryType(original.DeliveryType),
                
                ExternalDeliveryVariantId = original.ExternalDeliveryVariantId,
                DeliveryVariantName = original.DeliveryVariantName,
                DeliveryVariantDescription = original.DeliveryVariantDescription,
                DeliveryVariantsLocation = ToDeliveryVariantsLocation(original.DeliveryVariantsLocation),
                Address = ToDeliveryAddress(original.Address),
                PickupPoint = ToDeliveryPickupPoint(original.PickupPoint),
                Contact = ToDeliveryContact(original.Contact),
                Comment = original.Comment,
                DeliveryDate = original.DeliveryDate,
                DeliveryTimeFrom = original.DeliveryTimeFrom,
                DeliveryTimeTo = original.DeliveryTimeTo,
                AdditionalText = original.AddText,
            };
        }

        public static GiftShopOrderItem ToGiftShopOrderItem(OrderItem original)
        {
            if (original == null)
            {
                return null;
            }

            return new GiftShopOrderItem
            {
                BasketId = original.BasketItemId,
                Quantity = original.Amount,
                Price = original.PriceRur,
                BonusPrice = original.PriceBonus,
                QuantityPrice = original.AmountPriceRur,
                QuantityBonusPrice = original.AmountPriceBonus,
                Title = original.Product.Name,
                ProductId = original.Product.ProductId,
                Article = original.Product.PartnerProductId
            };
        }

        public static GiftShopOrder ToGiftShopOrder(Order original)
        {
            if (original == null)
            {
                return null;
            }

            return new GiftShopOrder
            {
                Id = original.Id,
                ExternalId = original.ExternalOrderId,
                PartnerId = original.PartnerId,
                CreateDate = original.InsertedDate,
                Items = original.Items.MaybeSelect(ToGiftShopOrderItem).MaybeToArray(),
                ItemsPrice = original.BonusItemsCost,
                DeliveryPrice = original.BonusDeliveryCost,
                TotalPrice = original.BonusTotalCost,
                TotalPriceRur = original.TotalCost,
                ItemsAdvance = original.ItemsAdvance,
                DeliveryAdvance = original.DeliveryAdvance,
                TotalAdvance = original.TotalAdvance,
                Status = ToOrderStatus(original.PublicStatus),
                StatusDescription = original.OrderStatusDescription,
                StatusChangeDate = original.StatusChangedDate,
                Delivery = ToOrderDelivery(original.DeliveryInfo),
                DeliveryInstructions = original.DeliveryInstructions
            };
        }

        public static OrderStatus ToOrderStatus(PublicOrderStatuses original)
        {
            switch (original)
            {
                case PublicOrderStatuses.Registration:
                    return OrderStatus.Registration;
                case PublicOrderStatuses.Processing:
                    return OrderStatus.Processing;
                case PublicOrderStatuses.Delivery:
                    return OrderStatus.Delivery;
                case PublicOrderStatuses.DeliveryWaiting:
                    return OrderStatus.DeliveryWaiting;

                case PublicOrderStatuses.NotDelivered:
                    return OrderStatus.NotDelivered;
                case PublicOrderStatuses.Delivered:
                    return OrderStatus.Delivered;
                case PublicOrderStatuses.Cancelled:
                    return OrderStatus.Cancelled;

                default:
                    return OrderStatus.Unknown;
            }

        }

        public static DeliveryLocationInfo ToDeliveryLocationInfo(VariantsLocation original)
        {
            if (original == null)
            {
                return null;
            }

            return new DeliveryLocationInfo
            {
                KladrCode = original.KladrCode,
                PostCode = original.PostCode
            };
        }

        public static PickupVariant ToPickupVariant(ProductCatalogOrderService.PickupVariant original)
        {
            if (original == null)
            {
                return null;
            }

            return new PickupVariant
            {
                PickupPoint = ToDeliveryPickupPoint(original.PickupPoint),
                CostRur = original.DeliveryCost,
                CostBonus = original.BonusDeliveryCost
            };
        }

        public static DeliveryVariant ToDeliveryVariant(ProductCatalogOrderService.DeliveryVariant original)
        {
            if (original == null)
            {
                return null;
            }

            return new DeliveryVariant
            {
                Type = ToDeliveryType(original.DeliveryType),
                Name = original.DeliveryVariantName,
                ExternalId = original.ExternalDeliveryVariantId,
                Description = original.Description,
                CostRur = original.DeliveryCost,
                CostBonus = original.BonusDeliveryCost,
                PickupPoints = original.PickupPoints.MaybeSelect(ToPickupVariant).MaybeToArray()
            };
        }

        public static DeliveryVariantsGroup ToDeliveryVariantsGroup(DeliveryGroup original)
        {
            if (original == null)
            {
                return null;
            }

            return new DeliveryVariantsGroup
            {
                Name = original.GroupName,
                Variants = original.DeliveryVariants.MaybeSelect(ToDeliveryVariant).MaybeToArray()
            };
        }
    }
}
