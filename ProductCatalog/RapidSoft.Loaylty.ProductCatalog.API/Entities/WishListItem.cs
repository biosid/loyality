namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    using System;

    public class WishListItem
    {
        public string ClientId { get; set; }

        public string ProductId { get; set; }

        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Количество "желаемых" товаров
        /// </summary>
        public int ProductsQuantity { get; set; }
    }
}