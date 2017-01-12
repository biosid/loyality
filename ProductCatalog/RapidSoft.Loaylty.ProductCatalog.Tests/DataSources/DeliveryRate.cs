namespace RapidSoft.Loaylty.ProductCatalog.Tests.DataSources
{
    public class DeliveryRate
    {
        public DeliveryRate()
        {
            this.Status = 1;
        }

        private string locationName;

        public int Id { get; set; }

        public int PartnerId { get; set; }

        public string Kladr { get; set; }

        public int LocationId { get; set; }

        public string LocationName
        {
            get { return this.locationName ?? this.Kladr; }
            set { this.locationName = value; }
        }

        public int MinWeightGram { get; set; }

        public int MaxWeightGram { get; set; }

        public decimal PriceRUR { get; set; }

        public string ExternalLocationId { get; set; }

        public int Status { get; set; }
    }
}