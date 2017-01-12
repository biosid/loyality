using System.Collections.Generic;

namespace Vtb24.Site.Models.Buy
{
    public class BuyResponseModel
    {
        public int OrderId { get; set; }
        public bool Success { get; set; }

        public bool NotifyPriceChange { get; set; }

        public decimal Price { get; set; }

        public IDictionary<string, string> Errors { get; set; }
        
    }
}