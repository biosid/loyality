using System.Configuration;

namespace Vtb24.Arms.AdminServices.Infrastructure.AdminSecurityConfiguration
{
    [ConfigurationCollection(typeof(PermissionNodeElement), AddItemName = "node")]
    public class PermissionNodesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PermissionNodeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PermissionNodeElement)element).TargetKey;
        }
    }
}
