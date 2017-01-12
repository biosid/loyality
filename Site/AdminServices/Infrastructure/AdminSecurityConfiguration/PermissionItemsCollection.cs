using System.Configuration;

namespace Vtb24.Arms.AdminServices.Infrastructure.AdminSecurityConfiguration
{
    [ConfigurationCollection(typeof(PermissionItemElement), AddItemName = "item")]
    public class PermissionItemsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PermissionItemElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PermissionItemElement)element).Key;
        }
    }
}
