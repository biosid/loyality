using System.Configuration;
using Vtb24.Site.Services.GiftShop.Catalog.Models;

namespace Vtb24.Site.Services.Infrastructure.CustomRecommedationsConfig
{
    public class CustomRecommendationElement : ConfigurationElement
    {
        [ConfigurationProperty("id", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Id
        {
            get { return ((string)base["id"]); }
        }

        [ConfigurationProperty("name", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Name
        {
            get { return ((string)base["name"]); }
        }

        [ConfigurationProperty("kind", DefaultValue = CustomRecommendationKind.Popular, IsKey = false, IsRequired = true)]
        public CustomRecommendationKind Kind
        {
            get { return ((CustomRecommendationKind)base["kind"]); }
        }

        [ConfigurationProperty("url", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Url
        {
            get { return (string)base["url"]; }
        }

        [ConfigurationProperty("image_url", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string ImageUrl
        {
            get { return (string)base["image_url"]; }
        }

        [ConfigurationProperty("price_text", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string PriceText
        {
            get { return (string)base["price_text"]; }
        }

        [ConfigurationProperty("probability_percentage", DefaultValue = 0, IsKey = false, IsRequired = true)]
        public int ProbabilityPercentage
        {
            get { return (int)base["probability_percentage"]; }
        }

        [ConfigurationProperty("min_balance", DefaultValue = 0, IsKey = false, IsRequired = false)]
        public int MinBalance
        {
            get { return (int)base["min_balance"]; }
        }
    }
}
