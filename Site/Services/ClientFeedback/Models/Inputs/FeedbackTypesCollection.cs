using System.Configuration;

namespace Vtb24.Site.Services.ClientFeedback.Models.Inputs
{
    [ConfigurationCollection(typeof(FeedbackTypeElement), AddItemName = "type")]
    public class FeedbackTypesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new FeedbackTypeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FeedbackTypeElement) element).Name;
        }
    }
}