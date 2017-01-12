using System.Collections.Generic;

namespace Vtb24.Site.Models.MyOrders
{
    public class MyOrdersModel
    {
        public List<MyOrdersOrderModel> Orders { get; set; }

        public OrdersKind Kind { get; set; }

        public int TotalPages { get; set; }

        public int Page { get; set; }
    }
}
