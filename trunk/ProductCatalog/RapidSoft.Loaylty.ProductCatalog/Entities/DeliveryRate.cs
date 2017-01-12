namespace RapidSoft.Loaylty.ProductCatalog.Entities
{
    public class DeliveryRate
    {
        public int Id { get; set; }

        public int PartnerId { get; set; }

        public int MinWeightGram { get; set; }

        public int MaxWeightGram { get; set; }

        public decimal PriceRur { get; set; }

        public int LocationId { get; set; }
    }
}