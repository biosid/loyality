using System;
using System.Linq;
using Vtb24.Arms.AdminServices.CatalogAdminService;
using Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models;
using OrderDeliveryAddress = Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models.OrderDeliveryAddress;
using Order = Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models.Order;
using OrderItem = Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models.OrderItem;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Orders
{
    internal static class MappingsFromService
    {
        public static OrderStatus ToOrderStatus(OrderStatuses original)
        {
            switch (original)
            {
                case OrderStatuses.Draft:
                    return OrderStatus.Draft;
                case OrderStatuses.Registration:
                    return OrderStatus.Registration;
                case OrderStatuses.Processing:
                    return OrderStatus.Processing;
                case OrderStatuses.CancelledByPartner:
                    return OrderStatus.CancelledByPartner;
                case OrderStatuses.DeliveryWaiting:
                    return OrderStatus.DeliveryWaiting;
                case OrderStatuses.Delivery:
                    return OrderStatus.Delivery;
                case OrderStatuses.Delivered:
                    return OrderStatus.Delivered;
                case OrderStatuses.DeliveredWithDelay:
                    return OrderStatus.DeliveredWithDelay;
                case OrderStatuses.NotDelivered:
                    return OrderStatus.NotDelivered;
            }

            return OrderStatus.Unknown;
        }

        public static OrderPaymentStatus ToOrderPaymentStatus(OrderDeliveryPaymentStatus original)
        {
            switch (original)
            {
                case OrderDeliveryPaymentStatus.Yes:
                    return OrderPaymentStatus.Yes;
                case OrderDeliveryPaymentStatus.No:
                    return OrderPaymentStatus.No;
                case OrderDeliveryPaymentStatus.Error:
                    return OrderPaymentStatus.Error;
                case OrderDeliveryPaymentStatus.BankCancelled:
                    return OrderPaymentStatus.BankCancelled;
            }

            return OrderPaymentStatus.Unknown;
        }

        public static OrderPaymentStatus ToOrderPaymentStatus(OrderPaymentStatuses original)
        {
            switch (original)
            {
                case OrderPaymentStatuses.Yes:
                    return OrderPaymentStatus.Yes;
                case OrderPaymentStatuses.No:
                    return OrderPaymentStatus.No;
                case OrderPaymentStatuses.Error:
                    return OrderPaymentStatus.Error;
                case OrderPaymentStatuses.BankCancelled:
                    return OrderPaymentStatus.BankCancelled;
            }

            return OrderPaymentStatus.Unknown;
        }

        public static OrderDeliveryType ToOrderDeliveryType(DeliveryTypes original)
        {
            switch (original)
            {
                case DeliveryTypes.Delivery:
                    return OrderDeliveryType.Delivery;
                case DeliveryTypes.Pickup:
                    return OrderDeliveryType.Pickup;
                case DeliveryTypes.Email:
                    return OrderDeliveryType.Email;
            }

            return OrderDeliveryType.Unknown;
        }

        public static string ToPhoneString(PhoneNumber original)
        {
            return original != null
                       ? string.Format("+{0} ({1}) {2}", original.CountryCode, original.CityCode, original.LocalNumber)
                       : null;
        }

        public static OrderDeliveryAddress ToOrderDeliveryAddress(DeliveryAddress original)
        {
            if (original == null)
            {
                return null;
            }

            return new OrderDeliveryAddress
            {
                PostCode = original.PostCode,
                RegionTitle = original.RegionTitle,
                DistrictTitle = original.DistrictTitle,
                CityTitle = original.CityTitle,
                TownTitle = original.TownTitle,
                StreetTitle = original.StreetTitle,
                House = original.House,
                Flat = original.Flat,
            };
        }

        public static OrderDeliveryPickupPoint ToOrderDeliveryPickupPoint(PickupPoint original)
        {
            if (original == null)
            {
                return null;
            }

            return new OrderDeliveryPickupPoint
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

        public static OrderDeliveryContact ToOrderDeliveryContact(Contact original)
        {
            if (original == null)
            {
                return null;
            }

            return new OrderDeliveryContact
            {
                Email = original.Email,
                Phone = ToPhoneString(original.Phone),
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
                Type = ToOrderDeliveryType(original.DeliveryType),
                ExternalDeliveryVariantId = original.ExternalDeliveryVariantId,
                DeliveryVariantName = original.DeliveryVariantName,
                Address = ToOrderDeliveryAddress(original.Address),
                PickupPoint = ToOrderDeliveryPickupPoint(original.PickupPoint),
                Contact = ToOrderDeliveryContact(original.Contact),
                Comment = original.Comment,
                DeliveryDate = original.DeliveryDate,
                DeliveryTimeFrom = original.DeliveryTimeFrom,
                DeliveryTimeTo = original.DeliveryTimeTo,
                AdditionalText = original.AddText,
            };
        }

        public static OrderItem ToOrderItem(CatalogAdminService.OrderItem original)
        {
            if (original == null)
            {
                return null;
            }

            return new OrderItem
            {
                ProductId = original.Product.ProductId,
                BasketId = original.BasketItemId,
                Title = original.Product.Name,
                Price = original.PriceRur,
                BonusPrice = original.PriceBonus,
                Quantity = original.Amount,
                QuantityPrice = original.AmountPriceRur,
                QuantityBonusPrice = original.AmountPriceBonus
            };
        }

        public static Order ToOrder(CatalogAdminService.Order original)
        {
            if (original == null)
            {
                return null;
            }

            return new Order
            {
                Id = original.Id,
                ExternalId = original.ExternalOrderId,
                SupplierId = original.PartnerId,
                CarrierId = original.CarrierId,
                CreateDate = original.InsertedDate,
                StatusChangeDate = original.StatusChangedDate,
                Status = ToOrderStatus(original.Status),
                StatusDescription = original.OrderStatusDescription,
                ProductPaymentStatus = ToOrderPaymentStatus(original.PaymentStatus),
                DeliveryPaymentStatus = ToOrderPaymentStatus(original.DeliveryPaymentStatus),
                ItemsPrice = original.ItemsCost,
                ItemsBonusPrice = original.BonusItemsCost,
                DeliveryPrice = original.DeliveryCost,
                DeliveryBonusPrice = original.BonusDeliveryCost,
                DeliveryAdvance = original.DeliveryAdvance,
                TotalAdvance = original.TotalAdvance,
                TotalPrice = original.TotalCost,
                TotalBonusPrice = original.BonusTotalCost,
                Items = original.Items.Select(ToOrderItem).ToArray(),
                Delivery = ToOrderDelivery(original.DeliveryInfo)
            };
        }

        public static OrderStatusHistoryRecord ToOrderStatusHistoryRecord(OrderHistory original)
        {
            if (original == null)
            {
                return null;
            }

            return new OrderStatusHistoryRecord
            {
                When = original.UpdatedDate,
                Who = original.UpdatedUserId,
                Status = ToOrderStatusHistoryField(original.OldStatus, original.NewStatus, ToOrderStatus),
                StatusDescription = ToOrderStatusHistoryField(original.OldOrderStatusDescription, original.NewOrderStatusDescription),
                ProductPaymentStatus = ToOrderStatusHistoryField(original.OldOrderPaymentStatus, original.NewOrderPaymentStatus, ToOrderPaymentStatus),
                DeliveryPaymentStatus = ToOrderStatusHistoryField(original.OldDeliveryPaymentStatus, original.NewDeliveryPaymentStatus, ToOrderPaymentStatus)
            };
        }

        public static OrderStatusHistoryField<T> ToOrderStatusHistoryField<T>(T oldValue, T newValue)
            where T : class
        {
            return !Equals(oldValue, newValue)
                       ? new OrderStatusHistoryField<T> { OldValue = oldValue, NewValue = newValue }
                       : null;
        }

        public static OrderStatusHistoryField<TMapped?> ToOrderStatusHistoryField<T, TMapped>(T? oldValue, T? newValue, Func<T, TMapped> mapper)
            where T : struct
            where TMapped : struct
        {
            return !Nullable.Equals(oldValue, newValue)
                       ? new OrderStatusHistoryField<TMapped?>
                       {
                           OldValue = oldValue.HasValue ? mapper(oldValue.Value) : (TMapped?) null,
                           NewValue = newValue.HasValue ? mapper(newValue.Value) : (TMapped?) null
                       }
                       : null;
        }
    }
}
