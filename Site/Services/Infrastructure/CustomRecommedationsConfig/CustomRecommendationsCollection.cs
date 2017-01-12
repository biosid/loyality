using System.Configuration;

namespace Vtb24.Site.Services.Infrastructure.CustomRecommedationsConfig
{
    [ConfigurationCollection(typeof(CustomRecommendationElement), AddItemName = "item")]
    public class CustomRecommendationsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new CustomRecommendationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CustomRecommendationElement)element).Id;
        }
    }
}
