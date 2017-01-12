namespace RapidSoft.Loaylty.ProductCatalog.Entities
{
    public class PartnerDeliveryRate
    {
        public int PartnerId { get; set; }

        public int? CarrierId { get; set; }

        public string Kladr { get; set; }

        public int MinWeightGram { get; set; }

        public int MaxWeightGram { get; set; }

        public decimal PriceRur { get; set; }

        public string ExternalLocationId { get; set; }

        public int Priority { get; set; }

        public int Type { get; set; }
    }
}
