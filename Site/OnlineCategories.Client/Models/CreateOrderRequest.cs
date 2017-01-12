namespace Vtb24.OnlineCategories.Client.Models
{
    public class CreateOrderRequest
    {
        public string UserTicket { get; set; }

        public string OrderId { get; set; }

        public decimal TotalCost { get; set; }

        public string InternalStatus { get; set; }

        public string ItemId { get; set; }

        public string ItemName { get; set; }

        public int ItemQuantity { get; set; }

        public decimal ItemPrice { get; set; }

        public int ItemBonusPrice { get; set; }

        public int ItemWeight { get; set; }

        public string ItemComment { get; set; }
    }
}