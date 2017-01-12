namespace Vtb24.Site.Services.GiftShop.Catalog.Models
{
    public class CustomRecommendation
    {
        public CustomRecommendationKind Kind { get; set; }

        public int ProbabilityPercentage { get; set; }

        public int MinBalance { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string ImageUrl { get; set; }

        public string PriceText { get; set; }

    }
}
