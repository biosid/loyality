using System.Configuration;

namespace Vtb24.Arms.AdminServices.Infrastructure.PartnerSettingsConfiguration
{
    [ConfigurationCollection(typeof (PartnerSettingsElement), AddItemName = "setting")]
    public class PartnerSettingsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PartnerSettingsElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PartnerSettingsElement) element).Name;
        }
    }
}
