using System.Configuration;

namespace Vtb24.Arms.AdminServices.Infrastructure.AdminSecurityConfiguration
{
    [ConfigurationCollection(typeof(PermissionElement), AddItemName = "permission")]
    public class PermissionsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PermissionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PermissionElement) element).Key;
        }
    }
}
