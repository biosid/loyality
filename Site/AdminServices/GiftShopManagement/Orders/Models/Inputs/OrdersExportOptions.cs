namespace Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models.Inputs
{
    public class OrdersExportOptions
    {
        public delegate string MapOrderStatusDelegate(OrderStatus status);

        public delegate string MapOrderPaymentStatusDelegate(OrderPaymentStatus paymentStatus);

        public delegate string MapOrderDeliveryDelegate(OrderDelivery delivery);

        public MapOrderStatusDelegate MapOrderStatus { get; set; }

        public MapOrderPaymentStatusDelegate MapOrderPaymentStatus { get; set; }

        public MapOrderDeliveryDelegate MapOrderDelivery { get; set; }

        public OrdersSearchCriteria SearchCriteria { get; set; }
    }
}
