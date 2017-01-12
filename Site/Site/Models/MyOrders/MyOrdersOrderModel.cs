using System;
using System.Globalization;
using System.Linq;
using Vtb24.Site.Models.MyOrders.Helpers;
using Vtb24.Site.Services.GiftShop.Orders.Models;

namespace Vtb24.Site.Models.MyOrders
{
    public class MyOrdersOrderModel
    {
        public DateTime Date { get; set; }
        
        public string OrderId { get; set; }

        public MyOrderItemModel[] Items { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; }

        public static MyOrdersOrderModel Map(GiftShopOrder original)
        {
            var order = new MyOrdersOrderModel
            {
                OrderId = original.Id.ToString(CultureInfo.InvariantCulture),
                Date = original.CreateDate,

                Items = original.Items.Select(MyOrderItemModel.Map).ToArray(),
                TotalPrice = original.TotalPrice,

                Status = OrderStatusTextHelper.GetStatusText(original.Status, original.PartnerId),
            };

            return order;
        }
    }
}