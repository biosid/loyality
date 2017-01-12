namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    public class OrderStatusWorkFlowItem
    {
        public OrderStatuses FromStatus { get; set; }

        public OrderStatuses ToStatus { get; set; }
    }
}