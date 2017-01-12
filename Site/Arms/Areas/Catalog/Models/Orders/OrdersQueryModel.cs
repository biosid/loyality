using System;
using System.Web.Routing;
using Vtb24.Arms.Helpers;

namespace Vtb24.Arms.Catalog.Models.Orders
{
    public class OrdersQueryModel
    {
        // ReSharper disable InconsistentNaming

        public DateTime? from { get; set; }

        public DateTime? to { get; set; }

        public int? partner { get; set; }

        public bool issupplier { get; set; }

        public int? id { get; set; }

        public OrderStatuses[] status { get; set; }

        public OrderPaymentStatuses[] productpayment { get; set; }

        public OrderPaymentStatuses[] deliverypayment { get; set; }

        public int? page { get; set; }

        // ReSharper restore InconsistentNaming

        public string ToQuery()
        {
            return QueryHelper.ToQuery(this);
        }

        public RouteValueDictionary ToRouteValue()
        {
            return QueryHelper.ToRouteValue(this);
        }

        public OrdersQueryModel MixQuery(string query, bool overwrite = false)
        {
            return QueryHelper.MixQueryTo(this, query, overwrite);
        }
    }
}