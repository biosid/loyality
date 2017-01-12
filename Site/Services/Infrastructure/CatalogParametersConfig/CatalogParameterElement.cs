using System.Configuration;

namespace Vtb24.Site.Services.Infrastructure.CatalogParametersConfig
{
    public class CatalogParameterElement : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return ((string) base["name"]); }
        }

        [ConfigurationProperty("url_argument", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string UrlArgument
        {
            get { return (string) base["url_argument"]; }
        }

        [ConfigurationProperty("unit", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string Unit
        {
            get { return (string) base["unit"]; }
        }
    }
}
