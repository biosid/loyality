using System.Web.Mvc;

namespace Vtb24.Arms.Catalog.Models.Orders
{
    public class OrdersModel
    {
        public OrdersQueryModel Query { get; set; }

        public SelectListItem[] Suppliers { get; set; }

        public SelectListItem[] Carriers { get; set; }

        public OrderModel[] Orders { get; set; }

        public int TotalPages { get; set; }
    }
}
