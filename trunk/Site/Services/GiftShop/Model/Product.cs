using System;

namespace Vtb24.Site.Services.GiftShop.Model
{
    public class Product
    {
        public string Id { get; set; }

        public int PartnerId { get; set; }

        public int CategoryId { get; set; }

        public string CategoryTitle { get; set; }

        public string Title { get; set; }

        public string Thumbnail { get; set; }

        public string Vendor { get; set; }

        public string VendorCode { get; set; }

        public bool HasDiscount { get; set; }

        public DateTime AddedToCatalogDate { get; set; }

        //TODO: remove
        public decimal PriceRur { get; set; }

        public decimal Price { get; set; }

        public decimal PriceWithoutDiscount { get; set; }

	    public bool IsDeliveredByEmail { get; set; }
    }
}
