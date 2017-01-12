using System.Configuration;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;

namespace Vtb24.Arms.AdminServices.Infrastructure.AdminSecurityConfiguration
{
    public class PermissionNodeElement : ConfigurationElement
    {
        [ConfigurationProperty("target_key", IsKey = true, IsRequired = true)]
        public PermissionKeys TargetKey
        {
            get { return (PermissionKeys) base["target_key"]; }
        }

        [ConfigurationProperty("description", IsRequired = true)]
        public string Description
        {
            get { return (string) base["description"]; }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public PermissionsNodeTypes Type
        {
            get { return (PermissionsNodeTypes) base["type"]; }
        }

        [ConfigurationProperty("list_description")]
        public string ListDescription
        {
            get { return (string)base["list_description"]; }
        }

        [ConfigurationProperty("list_wildcard_description")]
        public string ListWildcardDescription
        {
            get { return (string)base["list_wildcard_description"]; }
        }

        [ConfigurationProperty("additional_items")]
        public PermissionItemsCollection AdditionalItemsCollection
        {
            get { return (PermissionItemsCollection) base["additional_items"]; }
        }

        [ConfigurationProperty("children")]
        public PermissionNodesCollection ChildrenCollection
        {
            get { return (PermissionNodesCollection) base["children"]; }
        }
    }
}
