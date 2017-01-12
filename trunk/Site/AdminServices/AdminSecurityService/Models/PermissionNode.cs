using System.Collections.Generic;

namespace Vtb24.Arms.AdminServices.AdminSecurityService.Models
{
    public class PermissionNode
    {
        public string Description { get; set; }

        public PermissionsNodeTypes Type { get; set; }

        public string ListDescription { get; set; }

        public string ListWildcardDescription { get; set; }

        public PermissionKeys TargetKey { get; set; }

        public Dictionary<PermissionKeys, string[]> AdditionalValues { get; set; }

        public PermissionNode[] Children { get; set; }
    }
}
