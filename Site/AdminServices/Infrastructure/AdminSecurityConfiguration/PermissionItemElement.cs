using System.Configuration;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;

namespace Vtb24.Arms.AdminServices.Infrastructure.AdminSecurityConfiguration
{
    public class PermissionItemElement : ConfigurationElement
    {
        [ConfigurationProperty("key", IsKey = true, IsRequired = true)]
        public PermissionKeys Key
        {
            get { return (PermissionKeys) base["key"]; }
        }

        [ConfigurationProperty("values")]
        public PermissionValuesCollection ValuesCollection
        {
            get { return (PermissionValuesCollection) base["values"]; }
        }
    }
}
