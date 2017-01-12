using System.Configuration;

namespace Vtb24.Site.Services.ClientFeedback.Models.Inputs
{
    public class FeedbackConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("types")]
        public FeedbackTypesCollection TypesCollection
        {
            get { return ((FeedbackTypesCollection) base["types"]); }
        }
    }
}