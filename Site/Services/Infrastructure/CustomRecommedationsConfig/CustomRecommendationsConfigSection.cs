using System.Configuration;

namespace Vtb24.Site.Services.Infrastructure.CustomRecommedationsConfig
{
    public class CustomRecommendationsConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("items")]
        public CustomRecommendationsCollection ItemsCollection
        {
            get { return (CustomRecommendationsCollection)base["items"]; }
        }
    }
}
