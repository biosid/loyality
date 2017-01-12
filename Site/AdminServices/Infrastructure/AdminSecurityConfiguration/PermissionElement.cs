using System.Configuration;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;

namespace Vtb24.Arms.AdminServices.Infrastructure.AdminSecurityConfiguration
{
    public class PermissionElement : ConfigurationElement
    {
        [ConfigurationProperty("key", IsKey = true, IsRequired = true)]
        public PermissionKeys Key
        {
            get { return (PermissionKeys) base["key"]; }
        }

        [ConfigurationProperty("ad_attr_name", IsRequired = true)]
        public string AdAttributeName
        {
            get { return (string) base["ad_attr_name"]; }
        }
    }
}
