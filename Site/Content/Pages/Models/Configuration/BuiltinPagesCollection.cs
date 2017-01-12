using System.Configuration;

namespace Vtb24.Site.Content.Pages.Models.Configuration
{
    [ConfigurationCollection(typeof(BuiltinPageElement), AddItemName = "page")]
    public class BuiltinPagesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new BuiltinPageElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((BuiltinPageElement)element).Url;
        }
    }
}
