namespace Vtb24.OnlineCategories.Client.Models
{
    public class NotifyOrderStatusRequest
    {
        public string UserTicket { get; set; }

        public string OrderId { get; set; }

        public decimal TotalCost { get; set; }

        public OrderStatus Status { get; set; }

        public string InternalStatus { get; set; }

        public string StatusDescription { get; set; }
    }
}
