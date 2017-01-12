using System;

namespace Vtb24.Site.Models.Catalog
{
    public class ListBankProductModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Thumbnail { get; set; }

        public DateTime ExpirationDate { get; set; }

        public decimal Price { get; set; }

        public bool CanRedeem { get; set; }

        public string RedeemDenyReason { get; set; }
    }
}
