using System.Configuration;

namespace Vtb24.Arms.AdminServices.Infrastructure.PartnerSettingsConfiguration
{
    public class PartnerSettingsConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("settings")]
        public PartnerSettingsCollection SettingsCollection
        {
            get { return (PartnerSettingsCollection) base["settings"]; }
        }
    }
}
