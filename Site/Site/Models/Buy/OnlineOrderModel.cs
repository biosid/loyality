namespace Vtb24.Site.Models.Buy
{
    public class OnlineOrderModel
    {
        public int ShopId { get; set; }

        public string OrderId { get; set; }

        public decimal MaxDiscount { get; set; }

        public string UserTicket { get; set; }

        public string Signature { get; set; }
    }
}