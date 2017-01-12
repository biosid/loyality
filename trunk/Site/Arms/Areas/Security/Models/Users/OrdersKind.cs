using System.ComponentModel;
using System.Linq;
using Vtb24.Site.Services.GiftShop.Orders.Models;

namespace Vtb24.Arms.Security.Models.Users
{
    public enum OrdersKind
    {
        // ReSharper disable InconsistentNaming

        [Description("Заказы в процессе")]
        inprocess = 0,

        [Description("История заказов")]
        history,

        [Description("Все заказы")]
        all

        // ReSharper restore InconsistentNaming
    }

    internal static class OrdersKindExtensions
    {
        private static readonly OrderStatus[] InprocessStatuses = new[]
        {
            OrderStatus.Processing, OrderStatus.DeliveryWaiting, OrderStatus.Delivery
        };

        private static readonly OrderStatus[] HistoryStatuses = new[]
        {
            OrderStatus.Delivered, OrderStatus.NotDelivered, OrderStatus.Cancelled
        };

        public static OrderStatus[] Map(this OrdersKind kind)
        {
            switch (kind)
            {
                case OrdersKind.inprocess:
                    return InprocessStatuses;

                case OrdersKind.history:
                    return HistoryStatuses;

                case OrdersKind.all:
                    return InprocessStatuses.Concat(HistoryStatuses).ToArray();

                default:
                    return null;
            }
        }
    }
}
