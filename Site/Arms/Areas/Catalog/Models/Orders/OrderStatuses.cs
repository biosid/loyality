using System;
using System.Collections.Generic;
using System.ComponentModel;
using Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models;

namespace Vtb24.Arms.Catalog.Models.Orders
{
    public enum OrderStatuses
    {
        // ReSharper disable InconsistentNaming

        [Description("Оформление")]
        draft,

        [Description("Регистрация")]
        registration,

        [Description("В обработке")]
        processing,

        [Description("Аннулирован партнёром")]
        cancelled_by_partner,

        [Description("Требует доставки")]
        delivery_waiting,

        [Description("Доставляется")]
        delivery,

        [Description("Доставлен")]
        delivered,

        [Description("Доставлен с задержкой")]
        delivered_with_delay,

        [Description("Не доставлен")]
        not_delivered

        // ReSharper restore InconsistentNaming
    }

    public static class OrderStatusesExtensions
    {
        public static OrderStatus Map(this OrderStatuses? original)
        {
            if (!original.HasValue)
                throw new InvalidOperationException("Неверный статус заказа");
            return original.Value.Map();
        }

        public static OrderStatus Map(this OrderStatuses original)
        {
            switch (original)
            {
                case OrderStatuses.draft:
                    return OrderStatus.Draft;
                case OrderStatuses.registration:
                    return OrderStatus.Registration;
                case OrderStatuses.processing:
                    return OrderStatus.Processing;
                case OrderStatuses.cancelled_by_partner:
                    return OrderStatus.CancelledByPartner;
                case OrderStatuses.delivery_waiting:
                    return OrderStatus.DeliveryWaiting;
                case OrderStatuses.delivery:
                    return OrderStatus.Delivery;
                case OrderStatuses.delivered:
                    return OrderStatus.Delivered;
                case OrderStatuses.delivered_with_delay:
                    return OrderStatus.DeliveredWithDelay;
                case OrderStatuses.not_delivered:
                    return OrderStatus.NotDelivered;
            }
            throw new InvalidOperationException("Неверный статус заказа");
        }

        public static OrderStatuses? Map(this OrderStatus original)
        {
            switch (original)
            {
                case OrderStatus.Draft:
                    return OrderStatuses.draft;
                case OrderStatus.Registration:
                    return OrderStatuses.registration;
                case OrderStatus.Processing:
                    return OrderStatuses.processing;
                case OrderStatus.CancelledByPartner:
                    return OrderStatuses.cancelled_by_partner;
                case OrderStatus.DeliveryWaiting:
                    return OrderStatuses.delivery_waiting;
                case OrderStatus.Delivery:
                    return OrderStatuses.delivery;
                case OrderStatus.Delivered:
                    return OrderStatuses.delivered;
                case OrderStatus.DeliveredWithDelay:
                    return OrderStatuses.delivered_with_delay;
                case OrderStatus.NotDelivered:
                    return OrderStatuses.not_delivered;
            }
            return null;
        }

        public static IEnumerable<OrderStatuses> Map(this IEnumerable<OrderStatus> original)
        {
            using (var enumerator = original.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var status = enumerator.Current.Map();
                    if (status.HasValue)
                        yield return status.Value;
                }
            }
        }
    }
}
