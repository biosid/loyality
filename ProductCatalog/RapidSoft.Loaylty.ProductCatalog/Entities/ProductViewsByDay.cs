namespace RapidSoft.Loaylty.ProductCatalog.Entities
{
    using System;

    public class ProductViewsByDay
    {
        public DateTime ViewsDate { get; set; }

        public string ProductId { get; set; }

        public int ViewsCount { get; set; }
    }
}