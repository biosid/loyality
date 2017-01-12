using System;

namespace Vtb24.Site.Services.BankProducts.Models
{
    public class BankProduct
    {
        public string Id { get; set; }

        public string ProductId { get; set; }

        public string Description { get; set; }

        public DateTime ExpirationDate { get; set; }

        public decimal Cost { get; set; }
    }
}
