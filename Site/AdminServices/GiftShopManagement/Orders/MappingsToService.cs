using System;
using Vtb24.Arms.AdminServices.CatalogAdminService;
using Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Orders
{
    internal static class MappingsToService
    {
        public static OrderStatuses ToOrderStatus(OrderStatus original)
        {
            switch (original)
            {
                case OrderStatus.Draft:
                    return OrderStatuses.Draft;
                case OrderStatus.Registration:
                    return OrderStatuses.Registration;
                case OrderStatus.Processing:
                    return OrderStatuses.Processing;
                case OrderStatus.CancelledByPartner:
                    return OrderStatuses.CancelledByPartner;
                case OrderStatus.DeliveryWaiting:
                    return OrderStatuses.DeliveryWaiting;
                case OrderStatus.Delivery:
                    return OrderStatuses.Delivery;
                case OrderStatus.Delivered:
                    return OrderStatuses.Delivered;
                case OrderStatus.DeliveredWithDelay:
                    return OrderStatuses.DeliveredWithDelay;
                case OrderStatus.NotDelivered:
                    return OrderStatuses.NotDelivered;
            }

            throw new InvalidOperationException("Неизвестный статус заказа");
        }

        public static OrderDeliveryPaymentStatus ToOrderDeliveryPaymentStatus(OrderPaymentStatus original)
        {
            switch (original)
            {
                case OrderPaymentStatus.Yes:
                    return OrderDeliveryPaymentStatus.Yes;
                case OrderPaymentStatus.No:
                    return OrderDeliveryPaymentStatus.No;
                case OrderPaymentStatus.Error:
                    return OrderDeliveryPaymentStatus.Error;
                case OrderPaymentStatus.BankCancelled:
                    return OrderDeliveryPaymentStatus.BankCancelled;
            }

            throw new InvalidOperationException("Неизвестный статус оплаты доставки");
        }

        public static OrderPaymentStatuses ToOrderPaymentStatus(OrderPaymentStatus original)
        {
            switch (original)
            {
                case OrderPaymentStatus.Yes:
                    return OrderPaymentStatuses.Yes;
                case OrderPaymentStatus.No:
                    return OrderPaymentStatuses.No;
                case OrderPaymentStatus.Error:
                    return OrderPaymentStatuses.Error;
                case OrderPaymentStatus.BankCancelled:
                    return OrderPaymentStatuses.BankCancelled;
            }

            throw new InvalidOperationException("Неизвестный статус оплаты вознаграждений");
        }
    }
}
