using System;

namespace Vtb24.Site.Services.GiftShop.Model
{
    public class ReservedProductItem
    {
        public string Id { get; set; }

        public int? ReservedProductGroupId { get; set; }

        public Product Product { get; set; }

        public ProductStatus ProductStatus { get; set; }

        public int Quantity { get; set; }

        public decimal TotalQuantityPriceRur { get; set; }

        public decimal TotalQuantityPrice { get; set; }

        public decimal ItemPrice { get; set; }

        public DateTime AddedDate { get; set; }
    }
}
