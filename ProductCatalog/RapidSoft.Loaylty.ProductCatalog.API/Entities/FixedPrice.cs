namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    using System;

    public class FixedPrice
    {
        public FixedPrice()
        {
        }

        public FixedPrice(Product product)
        {
            PriceRUR = product.PriceRUR;
        }

        /// <summary>
        /// Зафиксированная цена в рублях
        /// </summary>
        public decimal PriceRUR { get; set; }

        /// <summary>
        /// Дата фиксации цены
        /// </summary>
        public DateTime FixDate { get; set; }
    }
}