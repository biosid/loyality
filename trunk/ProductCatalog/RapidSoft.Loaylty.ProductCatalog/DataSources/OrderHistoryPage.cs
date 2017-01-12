namespace RapidSoft.Loaylty.ProductCatalog.DataSources
{
    using System.Collections.Generic;

    using API.Entities;

    public class OrderHistoryPage : List<OrderHistory>
    {
        public OrderHistoryPage()
        {
        }

        public OrderHistoryPage(int capacity)
            : base(capacity)
        {
        }
        
        public int? TotalCount { get; set; }
    }
}