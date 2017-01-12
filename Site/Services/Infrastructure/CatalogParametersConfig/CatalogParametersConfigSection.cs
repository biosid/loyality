using System.Configuration;

namespace Vtb24.Site.Services.Infrastructure.CatalogParametersConfig
{
    public class CatalogParametersConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("parameters")]
        public CatalogParametersCollection ParametersCollection
        {
            get { return (CatalogParametersCollection) base["parameters"]; }
        }
    }
}
