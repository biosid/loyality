using System.Configuration;

namespace Vtb24.Arms.AdminServices.Infrastructure.AdminSecurityConfiguration
{
    [ConfigurationCollection(typeof(PermissionValueElement), AddItemName = "add")]
    public class PermissionValuesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PermissionValueElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PermissionValueElement)element).Value;
        }
    }
}
