using System.Configuration;

namespace Vtb24.Site.Content.Pages.Models.Configuration
{
    public class BuiltinPageElement : ConfigurationElement
    {
        [ConfigurationProperty("url", DefaultValue = "/", IsKey = true, IsRequired = true)]
        public string Url
        {
            get { return (string) base["url"]; }
        }

        [ConfigurationProperty("title", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Title
        {
            get { return (string) base["title"]; }
        }

        [ConfigurationProperty("keywords", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string Keywords
        {
            get { return (string)base["keywords"]; }
        }

        [ConfigurationProperty("description", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string Description
        {
            get { return (string)base["description"]; }
        }
    }
}
