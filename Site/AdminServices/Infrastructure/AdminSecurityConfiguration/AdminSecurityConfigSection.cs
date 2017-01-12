using System.Configuration;

namespace Vtb24.Arms.AdminServices.Infrastructure.AdminSecurityConfiguration
{
    public class AdminSecurityConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("permissions")]
        public PermissionsCollection PermissionsCollection
        {
            get { return (PermissionsCollection) base["permissions"]; }
        }

        [ConfigurationProperty("nodes")]
        public PermissionNodesCollection NodesCollection
        {
            get { return (PermissionNodesCollection) base["nodes"]; }
        }
    }
}
