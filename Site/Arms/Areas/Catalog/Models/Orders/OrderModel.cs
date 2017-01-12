using System;
using System.Linq;
using Vtb24.Arms.AdminServices.Infrastructure;
using Vtb24.Arms.Catalog.Models.Orders.Helpers;
using Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models;

namespace Vtb24.Arms.Catalog.Models.Orders
{
    public class OrderModel
    {
        public int Id { get; set; }

        public DateTime OrderTime { get; set; }

        public OrderStatuses? Status { get; set; }

        public OrderPaymentStatuses? ProductPaymentStatus { get; set; }

        public OrderPaymentStatuses? DeliveryPaymentStatus { get; set; }

        public OrderItemModel[] Items { get; set; }

        public decimal TotalPrice { get; set; }

        public string Delivery { get; set; }

        public static OrderModel Map(Order original)
        {
            return new OrderModel
            {
                Id = original.Id,
                OrderTime = original.CreateDate,
                Status = original.Status.Map(),
                ProductPaymentStatus = original.ProductPaymentStatus.Map(),
                DeliveryPaymentStatus = original.DeliveryPaymentStatus.Map(),
                Items = original.Items.Select(OrderItemModel.Map).ToArray(),
                TotalPrice = original.IsBankProductOrder()
                                 ? original.TotalBonusPrice * OrderHelpers.BANK_PRODUCTS_PRICE_RATE
                                 : original.TotalPrice,
                Delivery = DeliveryFormatter.Map(original.Delivery)
            };
        }
    }
}
