using System.Configuration;

namespace Vtb24.Arms.AdminServices.Infrastructure.AdminSecurityConfiguration
{
    public class PermissionValueElement : ConfigurationElement
    {
        [ConfigurationProperty("value", IsKey = true)]
        public string Value
        {
            get { return (string) base["value"]; }
        }
    }
}
