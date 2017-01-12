using System;

namespace Vtb24.OnlineCategories.Client.Models
{
    public enum OrderStatus
    {
        CanceledByPartner,
        DeliveryWaiting,
        Delivery,
        Delivered,
        DeliveredWithDelay,
        NotDelivered
    }

    internal static class OrderStatusExtensions
    {
        public static sbyte Map(this OrderStatus original)
        {
            switch (original)
            {
                case OrderStatus.CanceledByPartner:
                    return 20;
                case OrderStatus.DeliveryWaiting:
                    return 30;
                case OrderStatus.Delivery:
                    return 40;
                case OrderStatus.Delivered:
                    return 50;
                case OrderStatus.DeliveredWithDelay:
                    return 51;
                case OrderStatus.NotDelivered:
                    return 60;

                default:
                    throw new NotSupportedException(string.Format("статус заказа \"{0}\" не поддерживается", original));
            }
        }
    }
}
