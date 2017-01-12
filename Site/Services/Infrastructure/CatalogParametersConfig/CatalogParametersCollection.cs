using System.Configuration;

namespace Vtb24.Site.Services.Infrastructure.CatalogParametersConfig
{
    [ConfigurationCollection(typeof(CatalogParameterElement), AddItemName = "parameter")]
    public class CatalogParametersCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new CatalogParameterElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CatalogParameterElement) element).Name;
        }
    }
}
